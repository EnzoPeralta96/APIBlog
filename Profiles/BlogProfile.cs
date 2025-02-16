using APIBlog.Models;
using APIBlog.ViewModels;
using AutoMapper;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<BlogRequestViewModel, Blog>();
        CreateMap<Blog, BlogViewModel>();
        CreateMap<Post, PostViewModel>();
        CreateMap<Reaction,ReactionViewModel>();
    }
}