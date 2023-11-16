using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestLogin.Server.Data;
using TestLogin.Shared;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using TestLogin.Shared.Dto.Blog;

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
                .Include(b => b.Characters) //include the Characters navigation property
                .FirstOrDefaultAsync(b => b.Id == request.BlogId && b.User != null && b.User.Id == GetUserId()); //find blogpost

            if (blogPost != null)
            {
                var characters = await _context.Characters
                    .Where(c => request.CharacterIds.Contains(c.Id))
                    .ToListAsync(); //finds character based on character id's from request

                if (blogPost.Characters == null)
                {
                    blogPost.Characters = new List<Character>(); //initialize if null
                }

                blogPost.Characters.AddRange(characters); //adds characters to blogPosts character navigation prop

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
        public async Task<ServiceResponse<List<Character>>> GetBlogPostCharacters(int BlogId)
        {
            var serviceResponse = new ServiceResponse<List<Character>>();
            var blogPost = await _context.BlogPosts
                .FirstOrDefaultAsync(b => b.Id == BlogId && b.User!.Id == GetUserId());

            var charactersInBlogPost = _context.Characters
                .Where(c => c.BlogPosts.Any(bp => bp.Id == blogPost.Id)) //checks if any BlogPosts associated with a character have the same Id as the blogPost retrieve
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
        public async Task<ServiceResponse<List<GetBlogPostDto>>> GetBlogPostsByCharacterId(int charid)
        {
            var serviceResponse = new ServiceResponse<List<GetBlogPostDto>>();
            var blogPost = await _context.BlogPosts
                .Where(b => b.Characters.Any(c => c.Id == charid) && b.User!.Id == GetUserId()) //checks blogposts for if any of them have characters with Id equal to the charId.
                .OrderByDescending(c => c.DateCreated).ToListAsync();

            if (blogPost != null)
            {
                serviceResponse.Data = blogPost.Select(c => _mapper.Map<GetBlogPostDto>(c)).ToList(); //mapped as list of GetBlogPostDtos
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
