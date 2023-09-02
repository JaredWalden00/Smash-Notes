using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestLogin.Server.Data;
using TestLogin.Shared;
using Microsoft.EntityFrameworkCore;
using Azure.Core;

namespace TestLogin.Server.CharacterControlService
{
    public class CharacterControlService : ICharacterControlService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public IHttpContextAccessor _httpContextAccessor { get; }
        public CharacterControlService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!); //finds userId

        public async Task AddCharactersToBlogPost([FromBody] BlogPostCharacterRequest request)
        {
            var blogPost = await _context.BlogPosts
                .Include(b => b.Characters) // Include the Characters navigation property
                .FirstOrDefaultAsync(b => b.Id == request.BlogId && b.User != null && b.User.Id == GetUserId());

            if (blogPost != null)
            {
                var characters = await _context.Characters
                    .Where(c => request.CharacterIds.Contains(c.Id))
                    .ToListAsync();

                if (blogPost.Characters == null)
                {
                    blogPost.Characters = new List<Character>(); // Initialize if null
                }

                blogPost.Characters.AddRange(characters);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<Character>> GimmeAllTheCharacters()
        {
            return await _context.Characters
                .Include(c=>c.Name)
                .ToListAsync();
        }
        public async Task<ServiceResponse<List<Character>>> GetBlogPostCharacters(int id)
        {
            var serviceResponse = new ServiceResponse<List<Character>>();
            var blogPost = await _context.BlogPosts
                .FirstOrDefaultAsync(b => b.Id == id && b.User!.Id == GetUserId());

            var charactersInBlogPost = _context.Characters
                .Where(c => c.BlogPosts.Any(bp => bp.Id == blogPost.Id))
                .ToList();

            if (charactersInBlogPost != null)
			{
                serviceResponse.Data = charactersInBlogPost;
				serviceResponse.Success = true;
				serviceResponse.Message = "Returned Post!";
            }
			else
			{
				serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "Post not found.";
            }    
            return serviceResponse;
        }

    }
}
