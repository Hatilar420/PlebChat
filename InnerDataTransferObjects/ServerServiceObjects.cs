using System.Collections.Generic;
namespace ChatApp.InnerDataTransferObjects{

    public class CreateServerObject
    {
        public bool IsSuccess {get;set;} //If the Creation of server is Is Succesfull
        public IEnumerable<string> Errors{get;set;} //To mark all the errors
        public string ServerKey{get;set;}
        public string ServerName{get;set;}
    }

    public class GetServerObject{
        public bool IsSuccess {get;set;}
        public IEnumerable<string> Errors {get;set;}
        public string ServerKey {get;set;}
        public string ServerName {get;set;}
        public IEnumerable<string> UserKeys {get;set;}
        public IEnumerable<string> MessageKeys {get;set;}


    }

}