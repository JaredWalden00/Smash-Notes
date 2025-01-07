using AutoMapper;
using TestLogin.Shared.Dto;

namespace TestLogin.Server
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<BlogPost, GetBlogPostDto>();
			CreateMap<AddBlogPostDto, BlogPost>();
            CreateMap<UpdateBlogPostDto, BlogPost>();
        }
	}
}