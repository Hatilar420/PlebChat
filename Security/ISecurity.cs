using ChatApp.responses;
using System.Threading.Tasks;

namespace ChatApp.Security{

   public interface ISecurity{
       Task<TokenResponse> Login(string UserName, string PassWord);
       Task<TokenResponse> Register(string email,string UserName, string PassWord);
   }

}