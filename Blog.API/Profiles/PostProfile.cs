using Common.Models;
using Common.ViewModels;
using AutoMapper;

namespace APIBlog.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostViewModel>();

        CreateMap<PostCreateViewModel, Post>()
            .ForMember(dest => dest.UserId, option => option.MapFrom(src => src.OwnerPostId))
            .BeforeMap((src, dest) =>
                {
                    dest.DateCreate = DateTimeOffset.UtcNow;
                    dest.Likes = 0;
                    dest.Views = 0;
                });

        CreateMap<PostUpdateViewModel, Post>()
            .ForMember(dest => dest.Id, option => option.MapFrom(src => src.PostId));
    }
}