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
            if(String.IsNullOrWhiteSpace(ServerKey)){
                return BadRequest(new {Errors = "ServerKey cannot be empty"});
            }
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


        //Create Message Channel
        [HttpPost]
        [Route("createmessagechannel")]
        public async Task<IActionResult> CreateMessageChannelAsync([FromBody] CreateMessageRequest request){
                if(string.IsNullOrWhiteSpace(request.server_key) || string.IsNullOrWhiteSpace(request.name)){
                    return BadRequest(new {Error = "server key is not specified"});
                }
                else{
                    CreateMessageChannelObject response = await _ServerService.CreateMessageGroupAsync(request.name,request.server_key);
                    if(response.IsSuccess){
                        return Ok(new CreateMessageChannelResponse{
                            MessageKey = response.MessageChannelKey,
                            MessageChannelName = response.MessageChannelName
                        } );
                    }
                    else{
                        return StatusCode(500,new {Errors = response.Errors});
                    }                  
                }
        }

        
        [HttpGet]
        [Route("getMessageChannel")]
        public async Task<IActionResult> GetMessageAsync([FromQuery] string Key){
            if(String.IsNullOrWhiteSpace(Key)){
                return BadRequest(new {Errors = "message key cannot be empty"});
            }
          GetMessageObject obj =  await _ServerService.GetMessageGroupAsync(Key);
          if(obj.IsSuccess){
              return Ok(new GetMessageResponse{
                  message_key  = obj.MessageKey,
                  name = obj.name,
                  user_keys = obj.UserKeys,
              });
          }
          else{
              return BadRequest(new {Errors = obj.Errors});
          }

        }


        [HttpPost]
        [Route("createserver")]
        public async Task<IActionResult> CreateServerAsync([FromBody]CreateServerRequest Server){
            string UserEmail = HttpContext.User?.FindFirst("Email")?.Value;
            if(String.IsNullOrWhiteSpace(Server.server_name)){
                return BadRequest(new {Error = "server name is invalid"});
            }
            else{
                 var response = await _ServerService.CreateServerAsync(Server.server_name,UserEmail);
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