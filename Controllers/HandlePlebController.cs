using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ChatApp.responses;
using ChatApp.services;
using System.Security.Claims;

namespace ChatApp.Controllers{

    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class Pleb : Controller{

        private readonly IHubContext<PlebChat> _hubContext;
        private Iuser UserService;
        public Pleb(IHubContext<PlebChat> p ,Iuser u){
               UserService =u;
               _hubContext = p;
        }



        //EndPoint to send images
        [Route("sendimage")]
        public async Task<IActionResult> SendImage([FromForm] MediaUserResponse res ){
           
            string fromEmail =   HttpContext.User?.FindFirst(ClaimTypes.Email)?.Value;
            var obj = await UserService.StoreMessageChat(fromEmail,res.to,res);
            if(!obj.IsSuccess){
              Console.WriteLine(obj.Error); // change it to log later
               return StatusCode(500);
            }
            await _hubContext.Clients.User(fromEmail).SendAsync("imagerec",obj.FilePath);           
            return Ok(new {FilePath = obj.FilePath});
        } 

    }
}