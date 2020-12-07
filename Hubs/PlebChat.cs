using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ChatApp
{

    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class PlebChat:Hub
    {
        public PlebChat(){
          
        }

        public async Task SendPrivate(string user,string message){
            Console.WriteLine(Context.UserIdentifier);
            Console.WriteLine(user);
            Console.WriteLine(message);
            await Clients.User(user).SendAsync("RecievePrivate",message);
        }
        
        public override async Task OnConnectedAsync()
        {
            await Clients.User(Context.UserIdentifier).SendAsync("bruhh","Hello");
        }


    }
    
}