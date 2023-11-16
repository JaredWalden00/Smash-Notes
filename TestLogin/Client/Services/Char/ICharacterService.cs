using TestLogin.Shared.Dto.Blog;

namespace TestLogin.Client.Services.Char
{
    interface ICharacterService
    {
        Task AddBlogPostCharacter(int blogId, List<int> characterIds);
        Task<List<Character>> GetAllCharacters();
        Task<ServiceResponse<List<Character>>> GetBlogPostCharactersById(int id);
        Task<ServiceResponse<List<GetBlogPostDto>>> GetBlogPostsByCharacterId(int characterId);
    }
}
