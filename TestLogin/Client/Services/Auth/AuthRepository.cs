using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using TestLogin.Shared;
using Newtonsoft.Json;

namespace TestLogin.Client.Services.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
		public AuthRepository(HttpClient http, IConfiguration configuration)
		{
			_configuration = configuration;
			_http = http;
		}

        public event Func<Task> LoggedIn;
        public event Func<Task> LoggedOut;

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            UserLoginDto userLoginDto = new UserLoginDto();
            userLoginDto.Username = username;
            userLoginDto.Password = password;

            var response = await _http.PostAsJsonAsync("Auth/Login", userLoginDto);
			var content = await response.Content.ReadAsStringAsync();

			ServiceResponse<string> serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(content);
            await LoggedIn?.Invoke();
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> Register(string username, string password)
        {
            var data = new { Username = username, Password = password };

            var response = await _http.PostAsJsonAsync("Auth/Register", data);

            var serviceResponse = new ServiceResponse<string>();
            if (response.IsSuccessStatusCode)
            {
                // Successful login, read the content (token) from the response
                var token = await response.Content.ReadAsStringAsync();
                serviceResponse.Data = token;
                serviceResponse.Success = true;
            }
            else
            {
                // Handle the error response
                serviceResponse.Success = false;
                serviceResponse.Message = "Registration failed. Please check your credentials.";
            }
          
            return serviceResponse;
        }

        public async Task OnLoggedOut()
        {
            await LoggedOut.Invoke();
        }
    }
}
