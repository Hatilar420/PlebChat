﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.responses
{
    public class SecurityUserLogIn
    { 
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class SecurityUserRegister
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class GetRefresh{
        public string JwtToken {get;set;}
        public string RefreshToken{get;set;}
    }

}