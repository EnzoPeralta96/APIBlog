using APIBlog.Models;
using APIBlog.ViewModels;
using AutoMapper;

namespace APIBlog.Profiles;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<BlogCreateViewModel, Blog>()
            .ForMember(dest => dest.UserId, option => option.MapFrom(src => src.OwnerBlogId));

        CreateMap<BlogUpdateViewModel, Blog>()
            .ForMember(dest => dest.Id, option => option.MapFrom(src => src.IdBlog))
            .ForMember(dest => dest.UserId, option => option.MapFrom(src => src.OwnerBlogId));

        CreateMap<Blog, BlogViewModel>()
            .ForMember(dest => dest.OwnerId, option => option.MapFrom(src => src.UserId));
    }

}