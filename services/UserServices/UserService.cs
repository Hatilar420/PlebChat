using System;
using Microsoft.AspNetCore.Identity;
using ChatApp.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;

namespace ChatApp.services{

    public class UserService : Iuser{

         private UserManager<ApplicationUser> _UserManager;
         private ChatContext Context ;
         public UserService(UserManager<ApplicationUser> u,ChatContext c){
              _UserManager = u;
               Context = c;
         }

         //to get a list of  who is online
         public IEnumerable<string> GetOnline(){
             return _UserManager.Users.Where(user => user.IsOnline == true).Select(user => user.Email);
         }

        //To set user offline
        public async Task<innerOnlineResponse> SetOffline(string Email)
        {
            ApplicationUser temp =  await _UserManager.FindByEmailAsync(Email);
            if(temp.IsOnline)
            {
               temp.IsOnline =  false;
               try{
               await _UserManager.UpdateAsync(temp);
               return new innerOnlineResponse{Success = true, Error = ""};
               }
               catch(Exception e){
                   return new innerOnlineResponse{Success = false,Error = e.Message};
               }
            }
            else{
                return new innerOnlineResponse{Success = false,Error = "User was'nt online"};
            }
        }

        //To set user online
        public async Task<innerOnlineResponse> SetOnline(string Email){
               ApplicationUser temp =  await _UserManager.FindByEmailAsync(Email);
               temp.IsOnline =  true;
               try{
               await _UserManager.UpdateAsync(temp);
               return new innerOnlineResponse{Success = true, Error = ""};
               }
               catch(Exception e){
                   return new innerOnlineResponse{Success = false,Error = e.Message};
               }
          }
        

        //To store chats
        public async Task StoreMessageChat(string From_Email,string To_Email,string message){
           ApplicationUser User1 = await _UserManager.FindByEmailAsync(From_Email);
           ApplicationUser User2 = await _UserManager.FindByEmailAsync(To_Email);
           Media media = new Media{
                   Key=Guid.NewGuid().ToString(),
                   TimeUtc = 0,
                   SendFrom = User1.Id,
                   Type = "Text",
                   Message = message
               };
           //Check if the relation exists
           //if not then create a chat
           string Chatid = GetPrivateChatId(User1,User2);
           if(Chatid != null){
               media.ChatId =Chatid;
           }
           else{
              PrivateChat chat  =await MakePrivateChat(User1,User2);
               media.ChatId = chat.Key;
           }
            await Context.Medias.AddAsync(media);
            await Context.SaveChangesAsync();
        }

         //To make a PrivateChat
         //Assuming No chat between User1 and User2 Exist
         private async Task<PrivateChat> MakePrivateChat(ApplicationUser User1 , ApplicationUser User2){
                   PrivateChat Chat = new PrivateChat{
                   Key =  Guid.NewGuid().ToString()
                   };
               ChatMap temp = new ChatMap{
                   Key = Guid.NewGuid().ToString(),
                   UserId =  User1.Id,
                   UserId_2 = User2.Id,
                   ChatId = Chat.Key
               };
               ChatMap temp2 = new ChatMap{
                   Key = Guid.NewGuid().ToString(),
                   UserId = User2.Id,
                   UserId_2 = User1.Id,
                   ChatId = Chat.Key
               };
               try{
                   await Context.Chats.AddAsync(Chat);
                   await Context.ChatMaps.AddAsync(temp);
                   await Context.ChatMaps.AddAsync(temp2);
                   await Context.SaveChangesAsync();
                   return Chat;
               }
               catch(Exception e){
                  Console.WriteLine(e.Message); // change it to log later
                  return null;
               }

         }

        
        //To Check if the relationship exist between two users
        private string GetPrivateChatId(ApplicationUser user1 , ApplicationUser user2){
            try{
            ChatMap ch = Context.ChatMaps.Where(x => ((x.UserId == user1.Id) && (x.UserId_2 == user2.Id))).SingleOrDefault();
            return  ch.ChatId;
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                return null;
            }
        }

    }

    public class innerOnlineResponse{
        public bool Success{get;set;}
        public string Error{get;set;}
    }
}