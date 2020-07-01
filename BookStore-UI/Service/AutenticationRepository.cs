using Blazored.LocalStorage;
using BookStore_UI.Contract;
using BookStore_UI.Modals;
using BookStore_UI.Providers;
using BookStore_UI.Static;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI.Service
{
    public class AutenticationRepository : IAutenticationRepository
    {
        private readonly IHttpClientFactory client;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AutenticationRepository(IHttpClientFactory _client, ILocalStorageService _localStorage,
            AuthenticationStateProvider _authenticationStateProvider)
        {
            client = _client;
            localStorage = _localStorage;
            authenticationStateProvider = _authenticationStateProvider;
        }

        public async Task<bool> Login(LoginModel usr)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, EndPoints.LoginEndPoint);

            request.Content = new StringContent(JsonConvert.SerializeObject(usr), Encoding.UTF8, "application/json");

            var cliente = client.CreateClient();
            HttpResponseMessage response = await cliente.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(content);


            //store token 
            await localStorage.SetItemAsync("authToken", token.Token);

            // change the authentication state of the aplication
            await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedIn();

            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Token);

            //return true 
            return true;
        }

        public async Task LogOut()
        {
            await localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedOut();
        }






        public async Task<bool> Register(RegistrationModel usr)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, EndPoints.RegistrationEndPoint);

            request.Content = new StringContent(JsonConvert.SerializeObject(usr), Encoding.UTF8, "application/json");

            var cliente = client.CreateClient();
            HttpResponseMessage response = await cliente.SendAsync(request);

            return response.IsSuccessStatusCode;

        }
    }
}
