using System.Reflection.Emit;
using APIBlog.Models;
using APIBlog.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace APIBlog.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostViewModel>();

        CreateMap<PostCreateViewModel, Post>()
            .ForMember(dest => dest.Id, option => option.MapFrom(src => src.BlogId))
            .BeforeMap((src, dest) =>
                {
                    dest.DateCreate = DateTime.Now;
                });

        CreateMap<PostUpdateViewModel, Post>()
            .ForMember(dest => dest.Id, option => option.MapFrom(src => src.Title));
    }
}