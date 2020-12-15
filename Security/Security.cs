using System;
using Microsoft.AspNetCore.Identity;
using ChatApp.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using ChatApp.responses;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;

namespace ChatApp.Security{
       public class SecurityService:ISecurity{
           
           private UserManager<ApplicationUser> _UserManager;
           private readonly IConfiguration Configuration;
           public SecurityService(UserManager<ApplicationUser> u,IConfiguration c ){
                _UserManager = u; 
                Configuration = c;
           }

           public async Task<TokenResponse> Login(string UserName, string PassWord)
        {
            var u = await _UserManager.FindByNameAsync(UserName);
            var a = await _UserManager.CheckPasswordAsync(u, PassWord);
            if (a)
            {
                var t = Token(UserName, u.Id,u.Email);
                return new TokenResponse { IsValid = true, Token = t , UserKey = u.Id};
            }
            return new TokenResponse { IsValid = false, Errors = new[] { "UserName or password is invalid" } };
        }
         

       public async Task<TokenResponse> Register(string email,string UserName, string PassWord)
        {
            var a = await _UserManager.FindByIdAsync(UserName);
            var b= await _UserManager.FindByEmailAsync(email);
            if(a ==  null && b == null && !String.IsNullOrEmpty(email))
            {
                var u = new ApplicationUser { UserName = UserName, Email = email };
               var r = await _UserManager.CreateAsync(u, PassWord);
               if(!r.Succeeded)
                {
                    return new TokenResponse { IsValid = false, Token = "", Errors = (from i in r.Errors select i.Description)};
                }
               else
                {
                    var token = Token(UserName, u.Id,u.Email);
                    return new TokenResponse { IsValid = true, Token = token,UserKey = u.Id};
                }
            }
            return new TokenResponse { IsValid = false, Errors = new[] {"User already Exist"} };
        }

       

        private string Token(string UserName , string UserKey,string Email)
        {
            
            var token = new JwtSecurityTokenHandler();
            var key = Configuration["SecurityKey"];
            var Des = new SecurityTokenDescriptor()
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, UserName),
                    new Claim("UserKey", UserKey),
                    new Claim(JwtRegisteredClaimNames.Email,Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10)
            };
            var tokenHandler = token.CreateToken(Des);
            return token.WriteToken(tokenHandler).ToString();
        }



       }
}