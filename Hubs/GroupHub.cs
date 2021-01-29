using System;
using System.Threading.Tasks;
using ChatApp.services;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ChatApp{

    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class GroupHub:Hub{

        private IGroup _ServerService;
        private Iuser _UserService;
        private ChatContext _Context;

        public GroupHub(IGroup s,Iuser u,ChatContext context){
            _ServerService = s;
            _UserService = u;
            _Context = context;
        }


        //Place The user into groups when he is connected
        public override async Task OnConnectedAsync()
        {    
            string fromUserKey =  Context.User?.FindFirst("UserKey")?.Value; // To get the user key from the JWT token
            List<string> server_keys =  _UserService.GetUserServerKeys(fromUserKey).ToList();
            List<string> TextServerKeys =  new List<string>();
            foreach(string keys in server_keys){
               var response =  await _ServerService.GetServerAsync(keys);
               if(response.IsSuccess){
                   TextServerKeys.AddRange(response.MessageKeys);
               }
            }
            //Join the Text servers
            foreach(string textServerKeys in  TextServerKeys){
                await Groups.AddToGroupAsync(Context.ConnectionId,textServerKeys);
                await Clients.Group(textServerKeys).SendAsync("TextServerConnected",$"{fromUserKey}");
            }
        }
        
               


    }

}