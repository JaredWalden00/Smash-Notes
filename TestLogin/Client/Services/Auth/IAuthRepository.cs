using System.Threading.Tasks;
using TestLogin.Shared;

namespace TestLogin.Client.Services.Auth
{
    interface IAuthRepository
    {
        public event Func<Task> LoggedIn;
        public event Func<Task> LoggedOut;
        Task<ServiceResponse<string>> Register(string username, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task OnLoggedOut();
    }
}
