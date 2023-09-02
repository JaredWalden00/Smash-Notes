using System.Net.Http.Json;
using TestLogin.Client.Services.Blog;
using TestLogin.Shared;
using static System.Net.WebRequestMethods;

namespace TestLogin.Client.Services.Char
{
    public class CharacterService : ICharacterService
    {
        private readonly HttpClient _http;
        public CharacterService(HttpClient http)
        {
            _http = http;
        }
        public async Task AddBlogPostCharacter(int blogId, List<int> characterIds)
        {
            // Create an anonymous type with the required parameters
            var data = new { BlogId = blogId, CharacterIds = characterIds };

            // Send the POST request to the API endpoint
            await _http.PostAsJsonAsync("api/character", data);
        }

        public async Task<List<Character>> GetAllCharacters()
        {
            return await _http.GetFromJsonAsync<List<Character>>("api/character");
        }

        public async Task<ServiceResponse<List<Character>>> GetBlogPostCharactersById(int id)
        {
            var response = await _http.GetAsync($"api/character/blog/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<List<Character>>>();
                if (responseData == null)
                {
                    return new ServiceResponse<List<Character>>
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
                return new ServiceResponse<List<Character>>
                {
                    Success = false,
                    Message = "Failed to retrieve blog posts."
                };
            }
        }

        public async Task<ServiceResponse<List<GetBlogPostDto>>> GetBlogPostsByCharacterId(int characterId)
        {
            var response = await _http.GetAsync($"api/character/{characterId}");
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
    }
}
