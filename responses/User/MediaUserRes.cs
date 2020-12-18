using System;
using Microsoft.AspNetCore.Http;

namespace ChatApp.responses
{
    public class MediaUserResponse{

        public string to{get;set;}
        public string type {get;set;}
        public IFormFile image {get;set;}

        public string message{get;set;}
    }
    
}