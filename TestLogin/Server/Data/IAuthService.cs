using TestLogin.Shared;

namespace TestLogin.Server.Data
{
	public interface IAuthService
	{
		Task<ServiceResponse<int>> Register(User user, string password);
		Task<ServiceResponse<string>> Login(string username, string password);
		Task<bool> UserExists(string username);
	}
}
