using System;
using System.Collections.Generic;

namespace  ChatApp.responses
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken{get;set;}
        public bool IsValid { get; set; }
        public string UserKey{get;set;}
        public IEnumerable<string> Errors { get; set; }
    }

    public class InnerToken{
        public string Token;
        public string RefreshToken;
    }
}