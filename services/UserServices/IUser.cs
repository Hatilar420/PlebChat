using System;
using System.Collections.Generic;
using ChatApp.Models;
using System.Threading.Tasks;

namespace ChatApp.services{

     public interface Iuser
     { 
           public IEnumerable<string> GetOnline();
           public Task<innerOnlineResponse> SetOnline(string Email);

           public Task<innerOnlineResponse> SetOffline(string Email);

           public Task StoreMessageChat(string From_Email,string To_Email,string message);
          
     }

}