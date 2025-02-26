using APIBlog.Models;
using APIBlog.ViewModels;
using AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User,UserViewModel>();
        CreateMap<UserUpdateViewModel,User>();
         CreateMap<UserPasswordUpdateViewModel,User>();
    }
}
