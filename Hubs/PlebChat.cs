using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ChatApp.services;

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
            await UserService.StoreMessageChat(Context.UserIdentifier,user,message);
            await Clients.User(user).SendAsync("RecievePrivate",message);
        }
        
        public override async Task OnConnectedAsync()
        {
             string email =  Context.UserIdentifier;
             var b  = await UserService.SetOnline(email);
              
             if(b.Success)
             {
                 var temp = UserService.GetOnline();
                 await  Clients.All.SendAsync("connected",new {Email = temp});
             }
            //await Clients.User(Context.UserIdentifier).SendAsync("bruhh","Hello");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string email =  Context.UserIdentifier;
             var b  = await UserService.SetOffline(email);
             if(b.Success)
             {
                 await Clients.All.SendAsync("disconnected",new {Email = email});
             }
        }


    }
    
}