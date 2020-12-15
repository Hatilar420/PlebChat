using System;
using System.Collections.Generic;
using ChatApp.Models;
using System.Threading.Tasks;
using ChatApp.responses;

namespace ChatApp.services{

     public interface Iuser
     { 
           public IEnumerable<string> GetOnline();
           public Task<innerOnlineResponse> SetOnline(string Email);

           public Task<innerOnlineResponse> SetOffline(string Email);

           public Task<StoreMessageResponse> StoreMessageChat(string From_Email,string To_Email,MediaUserResponse message);
          
     }

}