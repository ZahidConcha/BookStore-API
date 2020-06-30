using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BookStore_API.Contracts;
using BookStore_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> signInManeger;
        private readonly UserManager<IdentityUser> userManeger;
        private readonly ILoggerServices logger;
        private readonly IConfiguration configuration;

        public UsersController(SignInManager<IdentityUser> _signInManeger,UserManager<IdentityUser> _userManeger,
           ILoggerServices _logger,IConfiguration _configuration) 
        {
            signInManeger = _signInManeger;
            userManeger = _userManeger;
            logger = _logger;
            configuration = _configuration;
        }

        


        /// <summary>
        /// User Login Endpoint! 
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var location = GetControllerActionNames();
            try
            {
               
                var userName = userDTO.UserName;
                var password = userDTO.Password;
                logger.LogInfo($"{location}:Login Attempt From User {userName}");
               var result = await signInManeger.PasswordSignInAsync(userName, password, false, false);

                if (result.Succeeded)
                {
                    logger.LogInfo($"{location}:Succesfully Authenticated {userName}");
                    var user = await userManeger.FindByNameAsync(userName);
                    var tokenString = await GenorateJWT(user);
                    return Ok(new { token = tokenString });
                }
                logger.LogInfo($"{location}:Not Authenticated {userName}");
                return Unauthorized(userDTO);
            }
            catch (Exception e)
            {
                return internalError($"{e.Message}-{e.InnerException}");
            }
           
        }






        private async Task<string> GenorateJWT(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> { 
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                
            };
            var roles = await userManeger.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Issuer"],claims,null,expires:DateTime.Now.AddMinutes(5),signingCredentials:credentials);
            return  new JwtSecurityTokenHandler().WriteToken(token);
        }


        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ActionName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }


        private ObjectResult internalError(string message)
        {
            logger.LogError($"{message}");
            return StatusCode(500, "Server error");
        }
    }
}
