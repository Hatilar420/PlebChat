using System;
using System.Threading.Tasks;
using ChatApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using ChatApp.InnerDataTransferObjects;

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
                UserkeyList.Add(x.Key);
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

        public async Task CreateMessageGroupAsync(string ChannelName,string serverKey){
           ServerChannel serverChannel =  await _Context.ServerChannels.FindAsync(serverKey);
           if(serverChannel == null){
               return ;
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
            }
            catch(Exception e){
                Console.WriteLine(e); // change it to log later
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

}