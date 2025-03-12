using Common.Models;
using Common.ViewModels;
using AutoMapper;

namespace APIBlog.Profiles;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<BlogCreateViewModel, Blog>()
            .ForMember(dest => dest.UserId, option => option.MapFrom(src => src.OwnerBlogId));

        CreateMap<BlogUpdateViewModel, Blog>()
            .ForMember(dest => dest.Id, option => option.MapFrom(src => src.IdBlog));

        CreateMap<Blog, BlogViewModel>()
            .ForMember(dest => dest.OwnerBlog, option => option.MapFrom(src => src.User));
    }

}