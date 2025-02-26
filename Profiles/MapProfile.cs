using APIBlog.Models;
using APIBlog.ViewModels;
using AutoMapper;
namespace APIBlog.Profiles;
public class MapProfile : Profile
{
    public MapProfile()
    {
       
        CreateMap<Blog, BlogViewModel>();
        CreateMap<BlogRequestViewModel, Blog>();
        CreateMap<Post, PostViewModel>();
        CreateMap<PostViewModel, Post>();
        CreateMap<Reaction, ReactionViewModel>();
        CreateMap<PostRequestViewModel, Post>();
        CreateMap<PostRequestViewModel, Post>()
            .BeforeMap((src, dest, context) =>
            {
                if (context.TryGetItems(out var items) &&
                    items.TryGetValue("BlogId", out var blogIdObj) &&
                    blogIdObj is int blogId)
                {
                    dest.BlogId = blogId;
                    dest.DateCreate = DateTime.Now;
                }           
            });
    }
}