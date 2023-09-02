using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestLogin.Server.Data;
using TestLogin.Shared.Dto;
using static System.Net.WebRequestMethods;

namespace TestLogin.Server.BlogControlService
{
	public class BlogControlService : IBlogControlService
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		public IHttpContextAccessor _httpContextAccessor { get; }
		public BlogControlService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
			_context = context;
			_mapper = mapper;

		}

		private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User
		.FindFirstValue(ClaimTypes.NameIdentifier)!); //finds userId

		public async Task<ServiceResponse<List<GetBlogPostDto>>> GetAllBlogPost()
		{
			var serviceResponse = new ServiceResponse<List<GetBlogPostDto>>();
			var dbBlogPost = await _context.BlogPosts
			.Where(c => c.User!.Id == GetUserId())
			.OrderByDescending(c => c.DateCreated).ToListAsync(); //get access to characters table in sql

			serviceResponse.Data = dbBlogPost.Select(c => _mapper.Map<GetBlogPostDto>(c)).ToList();
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetBlogPostDto>> CreateNewBlogPost(AddBlogPostDto request)
		{
			var serviceResponse = new ServiceResponse<GetBlogPostDto>();
			var blogPost = _mapper.Map<BlogPost>(request);
			blogPost.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

			_context.Add(blogPost);
			await _context.SaveChangesAsync();

			var blogPostRequest = await _context.BlogPosts
				.FirstOrDefaultAsync(p => p.Url.ToLower().Equals(request.Url.ToLower()) && p.User!.Id == GetUserId());

			serviceResponse.Data = _mapper.Map<GetBlogPostDto>(blogPostRequest);
			serviceResponse.Message = "Blog Post Created.";

			return serviceResponse;
		}
        public async Task<ServiceResponse<GetBlogPostDto>> GetBlogPostById(int id)
        {
            var serviceResponse = new ServiceResponse<GetBlogPostDto>();
			var blogPost = await _context.BlogPosts
				.FirstOrDefaultAsync(b => b.Id == id && b.User!.Id == GetUserId());

			if (blogPost != null)
			{
                serviceResponse.Data = _mapper.Map<GetBlogPostDto>(blogPost);
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

        public async Task<ServiceResponse<GetBlogPostDto>> UpdateBlogPost(UpdateBlogPostDto blogPost)
		{
            var serviceResponse = new ServiceResponse<GetBlogPostDto>();
            var updatePost = await _context.BlogPosts
                .FirstOrDefaultAsync(b => b.Id == blogPost.Id && b.User!.Id == GetUserId());
			if (updatePost != null)
			{
				updatePost.Title = blogPost.Title;
				updatePost.Content = blogPost.Content;
				await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetBlogPostDto>(updatePost);
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
