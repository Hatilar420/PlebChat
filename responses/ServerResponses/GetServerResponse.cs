using System;
using System.Collections.Generic;

namespace ChatApp.responses{

    public class GeteServerResponse{
        public string server_key{get;set;}
        public string server_name{get;set;}
        public IEnumerable<string> user_keys {get;set;}
        public IEnumerable<string> message_channel_keys{get;set;}
    }

}