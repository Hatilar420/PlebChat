using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Models
{


    //Identity user Extension

    public class ApplicationUser : IdentityUser{
        public bool IsOnline {get;set;} //True if user is connected and will be offline OnDisconnect
 
        public string ConnectedUserId{get;set;} //ConnectionID

        public IEnumerable<ChatMap> chats{get;set;}
        
        public IEnumerable<GroupMap> groups{get;set;}

        public IEnumerable<ServerChannelMap> servers{get;set;}

        public IEnumerable<Media> Medias{get;set;}

        public IEnumerable<RefreshToken> refreshTokens{get;set;}

    }

  
    //Base Class
    public class Base{
        [Key]
      public  string Key{get;set;}
        [Timestamp]
       public byte[] RowVersion{get;set;}
    }

    //To set Many to Many relation between ApplicationUser and ServerChannel
    public class ServerChannelMap:Base{
      public string UserId{get;set;}
      public string ServerChannelKey{get;set;}

      [ForeignKey("UserId")]
      public ApplicationUser user{get;set;}

      [ForeignKey("ServerChannelKey")]
      public ServerChannel server{get;set;}


    }

    //To set Many to many relation between ApplicationUser and MessageChannel
    public class GroupMap:Base{
        public string UserId{get;set;}
        public string MessageChannelKey{get;set;}

        public string ServerName{get;set;}

        [ForeignKey("UserId")]
        public ApplicationUser user{get;set;}

        [ForeignKey("MessageChannelKey")]
        public MessageChannel messageChannel{get;set;}
    }


    //The Message Channel of a server
    public class MessageChannel:Base{

      public string Name {get;set;}

      public string ServerKey{get;set;}

      [ForeignKey("ServerKey")]
      public ServerChannel server{get;set;}
      public IEnumerable<Media> Medias{get;set;}
      public IEnumerable<GroupMap> groupMap{get;set;}
    }

    public class ServerChannel:Base{
      public string Name{get;set;}
      public DateTime CreationTime{get;set;}
      public IEnumerable<MessageChannel> MessageChannels{get;set;}
      public IEnumerable<ServerChannelMap> ServerChannelMaps{get;set;}
    }


    //To set Many to many relation
    public class ChatMap:Base{
      public string UserId {get;set;}

      public string UserId_2 {get;set;}
      public string ChatId{get;set;}

     [ForeignKey("ChatId")]
      public PrivateChat chat{get;set;}

     [ForeignKey("UserId")]
      public ApplicationUser user{get;set;}
    }
      
    // To store chats on the server
    public class PrivateChat:Base{

      
      public IEnumerable<ChatMap> chatMaps{get;set;}      
      public IEnumerable<Media> Media{get;set;}


    }


    //Media Related
    public class Media:Base{


        public long TimeUtc{get;set;} //When was the message send
        public string ChatId{get;set;}  // Get the chat Key from chats
        public string SendFrom{get;set;}   // Gets the user from User
        public string SendFromEmail{get;set;} //Gets the user Email who sent this media
        public string Type{get;set;}      //Get the type of media (eg:- Jpeg, text etc...)
        public string Message{get;set;}    
        public string Image{get;set;}   // Location of image on static server
        public string MessageChannelKey{get;set;} // Key of the message channel to which media is sent to

        [ForeignKey("ChatId")]
        public PrivateChat Chat{get;set;}

        [ForeignKey("SendFrom")]
        public ApplicationUser User{get;set;}

        [ForeignKey("MessageChannelKey")]
        public MessageChannel messageChannel {get;set;}
     
    } 

    //RefreshToken
    public class RefreshToken:Base{

      public string JwtToken{get;set;}  //To get and set the Jwt token it is related to 
      public DateTime CreationTime{get;set;} // Get and set the creation time of token
      public DateTime ExpiryTime{get;set;}  // Get and set the Expiry time of token , It is obviously more than the Jwt token Issued Lifetime
      public bool IsValid{get;set;}   //To check if the Token is Still valid , True if it is valid
                                      // Implement a method to set it to false , if the Refresh token is stolen
      public bool IsUsed {get;set;} // Set it to false if the Refresh token is used

      //Also link it to Application user
      public string UserKey{get;set;}
      
      [ForeignKey("UserKey")]
      public ApplicationUser User {get;set;}

    }


}