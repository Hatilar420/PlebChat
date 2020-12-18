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

       public async Task Broadcast(string message){
           await Clients.All.SendAsync("Broadcast",message);
        }

        public async Task SendPrivate(string clientId, string message)
        {
            await Clients.Client(clientId).SendAsync("receive",message);
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("client",Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("OnDisconnected",Context.ConnectionId);
        }


    }
}
