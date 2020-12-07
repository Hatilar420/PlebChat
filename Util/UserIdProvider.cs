using System;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Util{
    public class EmailBasedUserId : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}