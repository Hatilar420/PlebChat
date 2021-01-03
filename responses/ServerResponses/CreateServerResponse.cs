
using System.Collections.Generic;

namespace ChatApp.responses{

    public class CreatServerResponse{
        public string ServerKey{get;set;}
        public string ServerName{get;set;}
        public IEnumerable<string> errors{get;set;}
    }

}