
using System.Collections.Generic;

namespace ChatApp.responses{

    public class CreatServerResponse{
        public string ServerKey{get;set;}
        public string ServerName{get;set;}
        public IEnumerable<string> errors{get;set;}
    }

    public class CreateMessageChannelResponse{
        public string MessageKey{get;set;}
        public string MessageChannelName{get;set;}
        public IEnumerable<string> errors{get;set;}
    }

}