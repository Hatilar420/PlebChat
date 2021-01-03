using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatApp.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ChatApp.responses;
using ChatApp.InnerDataTransferObjects;

namespace ChatApp.Controllers{

    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupController: Controller{
        
        private IGroup _ServerService;

        public GroupController(IGroup serverService){
            _ServerService = serverService;
        }



        // To get the server details
        [HttpGet]
        [Route("getserver")]
        public async Task<IActionResult> GetServerAsync([FromQuery] string ServerKey){
          GetServerObject obj =  await _ServerService.GetServerAsync(ServerKey);
          if(obj.IsSuccess){
              return Ok(new GeteServerResponse{
                  server_key  = obj.ServerKey,
                  server_name = obj.ServerName,
                  user_keys = obj.UserKeys,
                  message_channel_keys = obj.MessageKeys
              });
          }
          else{
              return BadRequest(new {Errors = obj.Errors});
          }

        }

        [HttpPost]
        [Route("createserver")]
        public async Task<IActionResult> CreateServerAsync([FromBody]string ServerName){
            string UserEmail = HttpContext.User?.FindFirst("Email")?.Value;
            if(String.IsNullOrWhiteSpace(ServerName)){
                return BadRequest(new {Error = "server name is invalid"});
            }
            else{
                 var response = await _ServerService.CreateServerAsync(ServerName,UserEmail);
                 if(response.IsSuccess){
                     return Ok(new CreatServerResponse{
                         ServerKey = response.ServerKey,
                         ServerName = response.ServerName
                     });
                 }
                 else{
                     return StatusCode(500,new {Errors = response.Errors});
                 }
            }         
        }



    }
}