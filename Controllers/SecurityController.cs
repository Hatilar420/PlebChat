using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Security;
using System.Threading.Tasks;
using ChatApp.responses;

namespace ChatApp.Controllers{

      [Route("api/")]
      [ApiController]
      public class SecurityController:ControllerBase{
         
         private ISecurity _Secure ;
         public SecurityController(ISecurity s){
           _Secure = s;
         }

         [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SecurityUserRegister s)
        {
            TokenResponse r = await _Secure.Register(s.Email, s.UserName, s.Password);
            if (r.IsValid)
            {
                return Created(@$"api/ContactApi/GetUser/{r.UserKey}", new { token = r.Token });
            }
            return BadRequest(new { errors = r.Errors });
        }

        [HttpPost("signin")]
        public async Task<IActionResult> LogIn([FromBody] SecurityUserLogIn l)
        {
            TokenResponse r = await _Secure.Login(l.UserName, l.Password);
            if(r.IsValid)
            {
                return Ok(new { token = r.Token });
            }
            return Unauthorized(new { Error = r.Errors });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> GetRefresh([FromBody] GetRefresh l){
            return Unauthorized();

        }



      }
}