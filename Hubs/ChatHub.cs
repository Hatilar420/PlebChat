using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace ChatApp
{
    public class ChatHub : Hub
    {
        public ChatHub()
        { 
        }

        public async Task Broadcast(string Message){
           await Clients.All.SendAsync("Broadcast",Message);
        }

    }
}
