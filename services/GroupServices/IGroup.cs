using System.Threading.Tasks;

namespace ChatApp.services{

    public interface IGroup{
         public Task CreateServerAsync(string ServerName,string AdminEmail);
         public Task CreateMessageGroupAsync(string ChannelName,string serverKey);
        public Task JoinServerAsync(string userEmail ,string ServerKey);

    }

}