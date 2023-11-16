using System.ComponentModel.DataAnnotations;
using TestLogin.Shared;

namespace TestLogin.Shared.Dto.Blog
{
    public class GetBlogPostDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(int.MaxValue)]
        public string Content { get; set; } = string.Empty;
        public List<Character>? Characters { get; set; }
    }
}
