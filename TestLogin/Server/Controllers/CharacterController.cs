using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestLogin.Server.CharacterControlService;
using TestLogin.Server.Data;
using TestLogin.Shared.Dto.Blog;

namespace TestLogin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterControlService _characterControlService;
        private readonly DataContext _context;
        public CharacterController(ICharacterControlService characterControlService, DataContext context)
        {
            _characterControlService = characterControlService;
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<BlogPost>> GimmeAllTheCharacters()
        {
            return Ok(_context.Characters);
        }

        [Authorize]
        [HttpGet("blog/{id}")]
        public async Task<ActionResult<ServiceResponse<List<Character>>>> GetBlogPostCharacters(int id)
        {
            return Ok(await _characterControlService.GetBlogPostCharacters(id));
        }

        [Authorize]
        [HttpGet("{charId}")]
        public async Task<ActionResult<ServiceResponse<List<GetBlogPostDto>>>> GetBlogPostsByCharacterId(int charId)
        {
            return Ok(await _characterControlService.GetBlogPostsByCharacterId(charId));
        }

        [HttpPost]
        public async Task AddCharactersToBlogPost([FromBody] BlogPostCharacterRequest request)
        {
            await _characterControlService.AddCharactersToBlogPost(request);
        }
    }
}
