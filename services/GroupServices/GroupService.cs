using System;
using System.Threading.Tasks;
using ChatApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using ChatApp.InnerDataTransferObjects;
using ChatApp.responses;

namespace ChatApp.services{

    public class GroupService:IGroup{
    
        private ChatContext _Context;
        private UserManager<ApplicationUser> _UserManager;
        private readonly IWebHostEnvironment enviroment;
        public GroupService(UserManager<ApplicationUser> u,ChatContext c,IWebHostEnvironment e){
               enviroment = e;
              _UserManager = u;
               _Context = c;
         }


        //to create server
        public async Task<CreateServerObject> CreateServerAsync(string ServerName,string AdminEmail){
            
            //Check if Server Already Exists
           var Server =  _Context.ServerChannels.Select(val => val).Where(pre => pre.Name == ServerName).SingleOrDefault();
           if(Server != null)
           {
               return new CreateServerObject{IsSuccess = false , Errors = new[] {"server already exists"}};
           }
            //Check if the Email Exists;
            ApplicationUser user = await _UserManager.FindByEmailAsync(AdminEmail);
            if(user == null)
            {
                return new CreateServerObject{IsSuccess = false , Errors = new[] {"cannot find user"}};
            }   
            ServerChannel server = new ServerChannel{
                Key = Guid.NewGuid().ToString(),
                Name = ServerName,
                CreationTime = DateTime.UtcNow,
            };
            try{
                await _Context.ServerChannels.AddAsync(server);
                await _Context.SaveChangesAsync();
                await CreateServerMapAsync(server.Key,user.Id);
                return new CreateServerObject{IsSuccess = true , ServerKey = server.Key, ServerName = server.Name};
            }   
            catch(Exception e){
              Console.WriteLine(e); // change it to log later
              return new CreateServerObject{IsSuccess = false , Errors= new[] {e.Message}};
            }     
        } 

        public async Task<GetServerObject> GetServerAsync(string serverKey){
           ServerChannel  server =  await _Context.ServerChannels.FindAsync(serverKey);
           if(server == null){
               return new GetServerObject{IsSuccess = false , Errors = new[] {"server does not exist"}};
           }
           try{
            
            //Get Message Channel Keys
            List<string> MessageChannelKeys = new List<string>();
            IEnumerable<MessageChannel> messageChannels =  await GetMessageChannelAsync(serverKey);
            foreach(MessageChannel x in messageChannels){
                MessageChannelKeys.Add(x.Key);
            }
            
            //Get Server Users
            IEnumerable<ServerChannelMap> maps = _Context.serverChannelMaps.Select(val => val).Where(pre => pre.ServerChannelKey == serverKey);
            List<string> UserkeyList = new List<string>();
             foreach(ServerChannelMap x in maps){
                UserkeyList.Add(x.UserId);
            }
            
            
            return new GetServerObject(){
                IsSuccess = true,
                ServerKey = server.Key,
                ServerName = server.Name,
                UserKeys = UserkeyList,
                MessageKeys = MessageChannelKeys
            };

            
           }catch(Exception e){
                return new GetServerObject{
                    IsSuccess = false, 
                    Errors = new[] {e.Message}
                };
           }

        }

        public async Task<CreateMessageChannelObject> CreateMessageGroupAsync(string ChannelName,string serverKey){
           ServerChannel serverChannel =  await _Context.ServerChannels.FindAsync(serverKey);
           if(serverChannel == null){
               return new CreateMessageChannelObject{
                   IsSuccess = false,
                   Errors = new[] {"Couldn't find the server"}
               } ;
           } 
            MessageChannel messageChannel = new MessageChannel(){
                Key = Guid.NewGuid().ToString(),
                Name = ChannelName,
                ServerKey = serverKey
            };
            try{
                await _Context.messageChannels.AddAsync(messageChannel);
                await _Context.SaveChangesAsync();
                await CreateGroupMapAsync(serverKey,messageChannel.Key);//Make a map to server users to this message channel
                return new CreateMessageChannelObject{
                    IsSuccess = true,
                    Errors = null,
                    MessageChannelKey =  messageChannel.Key,
                    MessageChannelName = messageChannel.Name
                };
            }
            catch(Exception e){
                Console.WriteLine(e); // change it to log later
                return new CreateMessageChannelObject{
                    IsSuccess = false,
                    Errors = new[] {e.Message}  
                };
            }

        }


        //To get a single Message channel inside a server
        public async Task<GetMessageObject> GetMessageGroupAsync(string MessageKey){
           MessageChannel  message =  await _Context.messageChannels.FindAsync(MessageKey);
           if(message == null){
               return new GetMessageObject{IsSuccess = false , Errors = new[] {"message channel does not exist"}};
           }
           try{
            
            //Get MessageGroupUsers Users
            IEnumerable<GroupMap> maps = _Context.GroupMaps.Select(val => val).Where(pre => pre.MessageChannelKey == MessageKey);
            List<string> UserkeyList = new List<string>();
             foreach(GroupMap x in maps){
                UserkeyList.Add(x.UserId);
            }
            
            return new GetMessageObject(){
                IsSuccess = true,
                MessageKey = message.Key,
                name= message.Name,
                UserKeys = UserkeyList,
            };

            
           }catch(Exception e){
                return new GetMessageObject{
                    IsSuccess = false, 
                    Errors = new[] {e.Message}
                };
           }

        }

        //Allow a user to join Server
        public async Task JoinServerAsync(string userEmail ,string ServerKey){
            ApplicationUser user = await _UserManager.FindByEmailAsync(userEmail);
            ServerChannel server = await _Context.ServerChannels.FindAsync(ServerKey);
            if(user == null || server ==  null){
                return;
            }
            try{
                await CreateServerMapAsync(ServerKey,user.Id);
                IEnumerable<MessageChannel> messageChannels = await GetMessageChannelAsync(ServerKey);
                await JoinMessageChannelsAsync(messageChannels,user.Id,server.Name);
            }
            catch(Exception e){
                Console.WriteLine(e) ; //Change it to log later
            }           
        }

        // Create a server map
        private async Task CreateServerMapAsync(string ServerKey,string UserKey){

            ServerChannelMap serverChannelMap =  new ServerChannelMap{
                Key = Guid.NewGuid().ToString(),
                UserId = UserKey,
                ServerChannelKey = ServerKey
            };

            try{
            await _Context.serverChannelMaps.AddAsync(serverChannelMap);
            await _Context.SaveChangesAsync();
            }
            catch(Exception e){
                Console.WriteLine(e.Message); // Change it to log later
            }

        }   

        //Create a GroupMap
        private async Task CreateGroupMapAsync(string ServerKey,string messageChannelKey){
            ServerChannel serverChannel =  await _Context.ServerChannels.FindAsync(ServerKey);
            MessageChannel messageChannel = await _Context.messageChannels.FindAsync(messageChannelKey);
            // Check , that they both exist
            if(messageChannel == null || serverChannel== null){
                return ;
            }
            //Get the users who have joined the server
            IEnumerable<ServerChannelMap> maps = _Context.serverChannelMaps.Select(val => val).Where(pre => pre.ServerChannelKey == ServerKey);
            List<GroupMap> groupMaps =  new List<GroupMap>(); //Map them to the group
            foreach(var m in maps){
                GroupMap gmap = new GroupMap{
                    Key = Guid.NewGuid().ToString(),
                    UserId = m.UserId,
                    MessageChannelKey = messageChannelKey,
                    ServerName = serverChannel.Name,    
                };
                groupMaps.Add(gmap);
            }
            try
            {
            await _Context.GroupMaps.AddRangeAsync(groupMaps);
            await _Context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e); // Change it to log later
            }       
        }


        public async Task<StoreGroupMessageResponse> StoreMessageChatAsync (string FromUserKey , string MessageChannelKey , MediaUserResponse res ){
            try{
                    ApplicationUser User1  = await _UserManager.FindByIdAsync(FromUserKey);
                    if(User1  == null){
                        return new StoreGroupMessageResponse{ IsSuccess = false , Error = "couldnt find the user"};
                     }

                    string mes = string.Empty;
                    string ImageName =  string.Empty;
                    if(string.Equals(res.type , "Text")){
                        mes = res.message;
                    }
                    else if(string.Equals(res.type,"Image")){ // Implement a method to store images
                        /* string name =  await StoreImage(res.image);
                        if(name != null){
                            ImageName = name;
                                }
                            else {return new StoreGroupMessageResponse{IsSuccess = false,Error = "Image Couldn't be stored"};}*/
                        }

                    // Link it to a Message channel or text channel by assigning a key to Messsage channel key
                    Media media =  new Media{
                        Key=Guid.NewGuid().ToString(),
                        TimeUtc = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        SendFrom = User1.Id,
                        Type = res.type,
                        Message = mes,
                        Image = ImageName,
                        SendFromEmail = User1.Email,
                        MessageChannelKey = MessageChannelKey
                        };
                        
                    //Store the messages
                    await _Context.Medias.AddAsync(media);
                    await _Context.SaveChangesAsync();  
                    return new StoreGroupMessageResponse{IsSuccess = true , Error = null,Type=res.type ,FilePath = ImageName }; 

            }
            catch(Exception e){
                    return new StoreGroupMessageResponse {IsSuccess=false , Error = e.Message , Type = res.type , FilePath =  string.Empty};
            }
        }



        //Get messsage channels of a server
        private async Task<IEnumerable<MessageChannel>> GetMessageChannelAsync(string serverKey){
            ServerChannel server = await _Context.ServerChannels.FindAsync(serverKey);
            await _Context.Entry(server).Collection(s => s.MessageChannels).LoadAsync();
            if(server != null){
                return server.MessageChannels;
            }
            return null;
        }



        //Join the messageChannels
        private async Task JoinMessageChannelsAsync(IEnumerable<MessageChannel> messageChannels, string userkey,string serverName){
            List<GroupMap> groupMaps = new List<GroupMap>();
            foreach(var m in messageChannels){
                var gmap = new GroupMap(){
                    UserId = userkey,
                    MessageChannelKey = m.Key,
                    ServerName = serverName
                };
                groupMaps.Add(gmap);
            }  
            try{ 
                await _Context.GroupMaps.AddRangeAsync(groupMaps);
                await _Context.SaveChangesAsync();
            }
            catch(Exception e){
                Console.WriteLine(e); // change it to log later
            }
        }


        


    }


    public class StoreGroupMessageResponse{
        public bool IsSuccess {get;set;}
        public string Error{get;set;}
        public string FilePath{get;set;}
        public string Type{get;set;}
    }

}