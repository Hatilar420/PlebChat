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
        
        public IEnumerable<Media> Medias{get;set;}


    }

    //Base Class
     public class Base{
        [Key]
      public  string Key{get;set;}
        [Timestamp]
       public byte[] RowVersion{get;set;}
    }

    //To set Many to many relation
    public class ChatMap:Base{
      public string UserId {get;set;}
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
        public string Type{get;set;}      //Get the type of media (eg:- Jpeg, text etc...)
        public string Message{get;set;}    
        public string Image{get;set;}   // Location of image on static server

        [ForeignKey("ChatId")]
        public PrivateChat Chat{get;set;}

        [ForeignKey("SendFrom")]
        public ApplicationUser User{get;set;}
     
    } 



}