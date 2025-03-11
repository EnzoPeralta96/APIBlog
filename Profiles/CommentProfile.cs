using System.Runtime.CompilerServices;
using APIBlog.Models;
using APIBlog.ViewModels;
using AutoMapper;

namespace APIBlog.Profiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment,CommentViewModel>();
        CreateMap<CommentCreateViewModel,Comment>();
        CreateMap<CommentUpdateViewModel,Comment>();
    }
}