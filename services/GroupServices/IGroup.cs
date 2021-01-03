using System.Threading.Tasks;
using ChatApp.InnerDataTransferObjects;

namespace ChatApp.services{

    public interface IGroup{
         public Task<CreateServerObject> CreateServerAsync(string ServerName,string AdminEmail);
         public Task<GetServerObject> GetServerAsync(string serverKey);
         public Task CreateMessageGroupAsync(string ChannelName,string serverKey);
         public Task JoinServerAsync(string userEmail ,string ServerKey);

    }

}