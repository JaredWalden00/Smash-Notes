using Microsoft.AspNetCore.Mvc;

namespace TestLogin.Server.CharacterControlService
{
    public interface ICharacterControlService
    {
        Task AddCharactersToBlogPost([FromBody] BlogPostCharacterRequest request);
        Task<List<Character>> GimmeAllTheCharacters();
        Task<ServiceResponse<List<Character>>> GetBlogPostCharacters(int id);
        Task<ServiceResponse<List<GetBlogPostDto>>> GetBlogPostsByCharacterId(int charid);
    }
}
