using AutoMapper;
using Common.Models;
using Common.ViewModels;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User,UserViewModel>();
        CreateMap<UserUpdateViewModel,User>();
        CreateMap<UserPasswordUpdateViewModel,User>();
    }
}
