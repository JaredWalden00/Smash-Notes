using Blazored.LocalStorage;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using TestLogin.Server.Dto;
using TestLogin.Shared;
using TestLogin.Shared.Dto;

namespace TestLogin.Client.Services.Blog
{
    public class BlogService : IBlogService
    {
		private readonly HttpClient _http;
        public event Func<Task>? BlogPostCreated;
        private readonly ILocalStorageService _localStorage;
        public BlogService(ILocalStorageService localStorage, HttpClient http)
		{
			_localStorage = localStorage;
			_http = http;
		}

        public async Task<int> GetStoredBlogPostId()
        {
            int id = await _localStorage.GetItemAsync<int>("blogId");
            await _localStorage.RemoveItemAsync("blogId");
            return id;
        }

        public async Task<ServiceResponse<List<GetBlogPostDto>>> GetAllBlogPosts()
        {
            var fart = _http.DefaultRequestHeaders.Authorization;
            var response = await _http.GetAsync("api/Blog");
            
            if (response.IsSuccessStatusCode)
			{
				var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<List<GetBlogPostDto>>>();
                if (responseData == null)
                {
                    return new ServiceResponse<List<GetBlogPostDto>>
                    {
                        Success = false,
                        Message = "Response data is null after deserialization."
                    };
                }
                return responseData;
			}
			else
			{
				// Handle error response
				return new ServiceResponse<List<GetBlogPostDto>>
				{
					Success = false,
					Message = "Failed to retrieve blog posts."
				};
			}
		}

        public async Task<ServiceResponse<GetBlogPostDto>> CreateNewBlogPost(AddBlogPostDto request)
        {
            var response = await _http.PostAsJsonAsync("api/Blog", request);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<GetBlogPostDto>>();

                if (responseData == null)
                {
                    return new ServiceResponse<GetBlogPostDto>
                    {
                        Success = false,
                        Message = "Response data is null after deserialization."
                    };
                }

                BlogPostCreated?.Invoke();
                return responseData;
            }
            else
            {
                var statusCode = response.StatusCode;
                var errorMessage = $"Failed to create a new blog post. Status code: {statusCode}";

                return new ServiceResponse<GetBlogPostDto>
                {
                    Success = false,
                    Message = errorMessage
                };
            }
        }


        public async Task<ServiceResponse<GetBlogPostDto>> GetBlogPostById(int id)
        {
            var response = await _http.GetAsync($"api/Blog/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
            }

            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<GetBlogPostDto>>();

            if (responseData == null)
            {
                throw new NullReferenceException("Response data is null.");
            }
            return responseData;
        }

        public async Task<ServiceResponse<GetBlogPostDto>> UpdateBlogPost(UpdateBlogPostDto blogPost)
        {
            var response = await _http.PutAsJsonAsync($"api/Blog/{blogPost.Id}", blogPost);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
            }

            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<GetBlogPostDto>>();

            if (responseData == null)
            {
                throw new NullReferenceException("Response data is null.");
            }
            return responseData;
        }
    }
}
