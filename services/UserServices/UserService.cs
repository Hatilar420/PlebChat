using System;
using Microsoft.AspNetCore.Identity;
using ChatApp.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using ChatApp.responses;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ChatApp.Util;

namespace ChatApp.services{

    public class UserService : Iuser{

         private UserManager<ApplicationUser> _UserManager;
         private readonly IWebHostEnvironment enviroment;
         private ChatContext Context ;
         public UserService(UserManager<ApplicationUser> u,ChatContext c,IWebHostEnvironment e){
               enviroment = e;
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
        public async Task<StoreMessageResponse> StoreMessageChat(string From_Email,string To_Email,MediaUserResponse res){
           ApplicationUser User1 = await _UserManager.FindByEmailAsync(From_Email);
           ApplicationUser User2 = await _UserManager.FindByEmailAsync(To_Email);
           string mes = string.Empty;
           string ImageName= string.Empty;
           if(string.Equals(res.type, "Text")){
                mes = res.message;
           }
           else if(string.Equals(res.type,"Image")){
                string name =  await StoreImage(res.image);
                if(name != null){
                    ImageName = name;
                }
                else {return new StoreMessageResponse{IsSuccess = false,Error = "Image Couldn't be stored"};}
           }

           Media media = new Media{
                   Key=Guid.NewGuid().ToString(),
                   TimeUtc = DateTimeOffset.Now.ToUnixTimeSeconds(),
                   SendFrom = User1.Id,
                   Type = res.type,
                   Message = mes,
                   Image = ImageName
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
            return new StoreMessageResponse{IsSuccess = true , Error = null,Type=res.type ,FilePath = ImageName };
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



        // get the private chat Entity
        private async Task<PrivateChat> GetPrivateChat(ApplicationUser user1 , ApplicationUser user2){
            try{
                string chatId = GetPrivateChatId(user1,user2);
               return await Context.PrivateChats.FindAsync(chatId);
            }
            catch(Exception e){
                   Console.WriteLine(e.Message);
                   return null;
            }
        }


        //Store Image
        private async Task<String> StoreImage(IFormFile image){
            string FilePathName = null;
            try{
            if(image.FileName.Length > 0 && image != null){
                FilePathName = $"{ Guid.NewGuid()}_{ image.FileName}";
                if (!Directory.Exists(enviroment.WebRootPath))
                {
                    Directory.CreateDirectory(enviroment.WebRootPath);
                }
                using (FileStream fs = File.Create(Path.Combine(enviroment.WebRootPath,"Images",$"{FilePathName}")))
                {
                    await image.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
            }
            }
            catch(Exception e){
                Console.WriteLine(e.ToString());
                return null;
            }
            return FilePathName;
        }

        //Get all chat
        public async Task<IEnumerable<Media>> GetChats(string Email1 , string Email2){

            var user1  = await _UserManager.FindByEmailAsync(Email1);
            var user2 =  await _UserManager.FindByEmailAsync(Email2);
            if(user1 == null || user2 == null){
                return null;
            }
            PrivateChat map =   await GetPrivateChat(user1,user2);
            if(map != null){
                try{
                       await Context.Entry(map).Collection(s => s.Media).LoadAsync();
                       List<Media> m = new List<Media>();
                       foreach(var b in map.Media)
                       {
                            m.Add(new Media{Key = b.Key,
                            RowVersion=b.RowVersion,
                            TimeUtc = b.TimeUtc,
                            ChatId = b.ChatId,
                            SendFrom = b.SendFrom,
                            Type = b.Type,
                            Message = b.Message,
                            Image = b.Message});
                       }    // As it is also loading Navigation Properties , so i have to explicitly create a list and the laod all the usefull data
                            // Also It is transferring too much data
                       return m; //Get all the media or the chats
                }
                catch(Exception e){
                    Console.WriteLine(e.Message); //CHANGE IT TO LOG
                    return null;

                }
            }
            else{
                return null;
            }
            throw new NotImplementedException();
        } 

         //Pagination is also Required
         //As all data cannot be transferred at once
         public async Task<PaginatedList <Media>> GetPaginatedList(string Email1, string Email2 ,int Page,int PageItemCount)
         {
             IEnumerable<Media> m = await GetChats(Email1,Email2);
             if(m == null){
                 return null;
             }
             List<Media> Paginated = m.Skip((Page-1)*PageItemCount).Take(PageItemCount).ToList();
             return new PaginatedList<Media>(m.Count(),Page,Paginated,PageItemCount);

         }



    }

  //User Responses
    public class innerOnlineResponse{
        public bool Success{get;set;}
        public string Error{get;set;}
    }

    public class StoreMessageResponse{
        public bool IsSuccess {get;set;}
        public string Error{get;set;}
        public string FilePath{get;set;}
        public string Type{get;set;}
    }


}