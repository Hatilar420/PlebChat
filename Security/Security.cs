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
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Security{
       public class SecurityService:ISecurity{
           
           private UserManager<ApplicationUser> _UserManager;
           private readonly IConfiguration Configuration;
           private readonly TokenValidationParameters _TokenValidParam;
           
           private readonly ChatContext _Context;
           public SecurityService(UserManager<ApplicationUser> u,IConfiguration c,TokenValidationParameters t, ChatContext con ){
                _Context = con;
                _UserManager = u; 
                _TokenValidParam = t;
                Configuration = c;
           }

           public async Task<TokenResponse> Login(string UserName, string PassWord)
        {
            var u = await _UserManager.FindByNameAsync(UserName);
            var a = await _UserManager.CheckPasswordAsync(u, PassWord);
            if (a)
            {
                var t = await GetAuthToken(UserName, u.Id,u.Email);
                return new TokenResponse { IsValid = true, Token = t.Token ,RefreshToken = t.RefreshToken , UserKey = u.Id};
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
                    var token = await GetAuthToken(UserName, u.Id,u.Email);
                    return new TokenResponse { IsValid = true, Token = token.Token,RefreshToken = token.RefreshToken,UserKey = u.Id};
                }
            }
            return new TokenResponse { IsValid = false, Errors = new[] {"User already Exist"} };
        }

        public async Task<TokenResponse> GetRefreshTokenAsync(string Token,string RefreshToken){
           ClaimsPrincipal JwtPrincipal = GetPrincipal(Token);

           if(JwtPrincipal == null){
               return new TokenResponse{IsValid = false , Errors = new[]{"Cannot initialLize token"}};
           }
           string username = JwtPrincipal.Claims.Single(x=> x.Type == "Sub").Value;
           long expiry =  long.Parse(JwtPrincipal.Claims.Single(x=> x.Type == JwtRegisteredClaimNames.Exp).Value);

           string email = JwtPrincipal.Claims.Single(x=> x.Type == "Email").Value;

            if(expiry > DateTimeOffset.Now.ToUnixTimeSeconds()){
                return new TokenResponse{IsValid = false , Errors = new[]{"Token is not yet expired"} };
            }

           string userKey = JwtPrincipal.Claims.Single(x => x.Type == "UserKey").Value;

           RefreshToken refresh = await _Context.refreshTokens.FindAsync(RefreshToken);

           if(refresh == null){
               return new TokenResponse{IsValid=false,Errors = new[]{"RefreshToken is not issued to this user"}};
           }

           if(refresh.ExpiryTime < DateTime.UtcNow){
               return new TokenResponse{IsValid=false,Errors = new[]{"RefreshToken is expired"}};
           }

           if(refresh.IsUsed){
              return new TokenResponse{IsValid=false,Errors = new[]{"RefreshToken is already Used"}};
           }

           if(!refresh.IsValid){
               return new TokenResponse{IsValid=false,Errors = new[]{"RefreshToken is not valid"}};
           }

            if(refresh.UserKey != userKey){
                return new TokenResponse{IsValid = false, Errors = new[]{"Token Does'nt belong to this user"}};
            }
            
            if(refresh.JwtToken != Token){
                return new TokenResponse{IsValid = false, Errors = new[]{"Token Does'nt belong to this user"}};
            }
             refresh.IsValid = false ; refresh.IsUsed = true;
             _Context.refreshTokens.Update(refresh);
             await _Context.SaveChangesAsync();
             InnerToken t = await GetAuthToken(username,userKey,email);
             return new TokenResponse{IsValid = true,Token = t.Token , RefreshToken = t.RefreshToken, UserKey = userKey };
             
        }
        
        //To get Claims from the JwtToken
        //Will be used to Verify if the token should be refreshed
       private ClaimsPrincipal GetPrincipal(string Token){
           JwtSecurityTokenHandler handler =  new JwtSecurityTokenHandler();
           var tem =  _TokenValidParam;
           tem.ValidateLifetime = false;
           try{
               ClaimsPrincipal principal = handler.ValidateToken(Token,tem,out var ValidToken);
              
               if(CheckValidatedToken(ValidToken)){
                   return principal;
               } 
               else{
                   return null;
               }
           }
           catch(Exception e){
               Console.WriteLine(e.Message); // CHANGE IT TO LOG
               return null;
           }
       }

       private bool CheckValidatedToken(SecurityToken token){
           return (token is JwtSecurityToken jwtToken) && jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase);
       }

        private async Task<InnerToken> GetAuthToken(string UserName , string UserKey,string Email)
        {
            
            var token = new JwtSecurityTokenHandler();
            var key = Configuration["SecurityKey"];
            var g = UserKey;
            var Des = new SecurityTokenDescriptor()
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Sub", UserName),
                    new Claim("UserKey", UserKey),    // VERY IMP: JWTREGISTEREDCLAIMNAMES IS GIVING ERROR DONT USE IT
                    new Claim("Email",Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10)
            };
            var tokenHandler = token.CreateToken(Des);

            string GenJwtToken =  token.WriteToken(tokenHandler).ToString();
            
            //Genrate Refresh token
             RefreshToken refresh = new RefreshToken{
                 Key = Guid.NewGuid().ToString(),
                 CreationTime = DateTime.UtcNow,
                 ExpiryTime = DateTime.UtcNow.AddMonths(6),
                 IsValid = true,
                 IsUsed =false,
                 UserKey = g,
                 JwtToken = GenJwtToken 
             };

             await _Context.refreshTokens.AddAsync(refresh);
             await _Context.SaveChangesAsync();
             return new InnerToken{RefreshToken = refresh.Key,Token = GenJwtToken};
        }


       }
}