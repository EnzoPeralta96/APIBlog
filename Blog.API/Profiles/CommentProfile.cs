using AutoMapper;
using Common.Models;
using Common.ViewModels;

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