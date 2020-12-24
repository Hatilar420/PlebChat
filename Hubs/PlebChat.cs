using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ChatApp.services;
using ChatApp.responses;
using ChatApp.Models;
using System.Collections.Generic;

namespace ChatApp
{

    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class PlebChat:Hub
    {
        private Iuser UserService;
        public PlebChat(Iuser u){
          UserService  = u;
        }

        public async Task SendPrivate(string user,string message){
            string email =  Context.UserIdentifier;
            var b = new MediaUserResponse{
                to = user,
                type = "Text",
                message = message
            };
            ApplicationUser u =  await UserService.Getuser(email);
            var obj = await UserService.StoreMessageChat(Context.UserIdentifier,user,b);
            if(obj.IsSuccess) await Clients.User(user).SendAsync("RecievePrivate",u.UserName,b);
            else Console.WriteLine(obj.Error);
        }
        
        public override async Task OnConnectedAsync()
        {
             string email =  Context.UserIdentifier;
             var b  = await UserService.SetOnline(email);
             
             if(b.Success)
             {
                 var temp = UserService.GetOnline();
                 List<ConnectedResponse> li = new List<ConnectedResponse>();
                 foreach(var u in temp){
                    li.Add(new ConnectedResponse{email =  u.Email,ukey=u.Id,username=u.UserName});                    
                 }
                 await  Clients.All.SendAsync("connected",new {users = li});
             }
            //await Clients.User(Context.UserIdentifier).SendAsync("bruhh","Hello");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string email =  Context.UserIdentifier;            
             var b  = await UserService.SetOffline(email);
             if(b.Success)
             {
                 ApplicationUser u = await UserService.Getuser(email);
                 await Clients.All.SendAsync("disconnected",new ConnectedResponse{email=u.Email,ukey=u.Id,username=u.UserName});
             }
        }


    }
    
}