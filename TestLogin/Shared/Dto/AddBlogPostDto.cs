using System.ComponentModel.DataAnnotations;

namespace TestLogin.Server.Dto
{
	public class AddBlogPostDto
	{
		public string Url { get; set; }

		[Required]
		public string Title { get; set; }
		[Required]
		[MaxLength(int.MaxValue)]
		public string Content { get; set; }
	}
}
