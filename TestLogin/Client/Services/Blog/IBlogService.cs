using TestLogin.Shared;
using TestLogin.Shared.Dto.Blog;

namespace TestLogin.Client.Services.Blog
{
    public interface IBlogService
	{
        public event Func<Task> BlogPostCreated;
        Task<int> GetStoredBlogPostId();
        Task<ServiceResponse<List<GetBlogPostDto>>> GetAllBlogPosts();
		Task<ServiceResponse<GetBlogPostDto>> CreateNewBlogPost(AddBlogPostDto request);
        Task<ServiceResponse<GetBlogPostDto>> GetBlogPostById(int id);
        Task<ServiceResponse<GetBlogPostDto>> UpdateBlogPost(UpdateBlogPostDto blogPost);
    }
}
