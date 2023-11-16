using TestLogin.Shared.Dto.Blog;
using static System.Net.WebRequestMethods;

namespace TestLogin.Server.BlogControlService
{
    public interface IBlogControlService
	{
		Task<ServiceResponse<List<GetBlogPostDto>>> GetAllBlogPost();
		Task<ServiceResponse<GetBlogPostDto>> CreateNewBlogPost(AddBlogPostDto request);
		Task<ServiceResponse<GetBlogPostDto>> GetBlogPostById(int id);
		Task<ServiceResponse<GetBlogPostDto>> UpdateBlogPost(UpdateBlogPostDto blogPost);
    }
}
