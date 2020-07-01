using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore_UI.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorage;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public ApiAuthenticationStateProvider(ILocalStorageService _localStorage,JwtSecurityTokenHandler _tokenHandler)
        {
            localStorage = _localStorage;
            tokenHandler = _tokenHandler;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var saveToken = await localStorage.GetItemAsync<string>("authToken");
                if (string.IsNullOrWhiteSpace(saveToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                var tokenContent = tokenHandler.ReadJwtToken(saveToken);
                var expiry = tokenContent.ValidTo;
                if (expiry < DateTime.Now)
                {
                    await localStorage.RemoveItemAsync("authToken");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                // get claims from token   // Build authenticated user object 
                var claims = ParseClaims(tokenContent);
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
                //return authanticated person
                return new AuthenticationState(user);
            }
            catch (Exception e)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }


        public async Task LoggedIn()
        {
            var savedToken = await localStorage.GetItemAsync<string>("authToken");
            var tokenContent = tokenHandler.ReadJwtToken(savedToken);
            var claims = ParseClaims(tokenContent);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void LoggedOut()
        {
            var nobody = new ClaimsPrincipal(new ClaimsPrincipal());
            var authState = Task.FromResult(new AuthenticationState(nobody));
             NotifyAuthenticationStateChanged(authState);
        }

        private IList<Claim> ParseClaims (JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }
    }
}
