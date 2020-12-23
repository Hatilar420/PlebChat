using System;
using System.Collections.Generic;
using ChatApp.Models;
using System.Threading.Tasks;
using ChatApp.responses;
using ChatApp.Util;

namespace ChatApp.services{

     public interface Iuser
     { 
           public IEnumerable<ApplicationUser> GetOnline();
           public Task<innerOnlineResponse> SetOnline(string Email);

           public Task<innerOnlineResponse> SetOffline(string Email);

           public Task<StoreMessageResponse> StoreMessageChat(string From_Email,string To_Email,MediaUserResponse message);

           public  Task<IEnumerable<Media>> GetChats(string Email1 , string Email2);

           public Task<ApplicationUser> Getuser(string Email);

           public Task<PaginatedList <Media>> GetPaginatedList(string Email1, string Email2 ,int Page,int PageItemCount);
          
     }

}