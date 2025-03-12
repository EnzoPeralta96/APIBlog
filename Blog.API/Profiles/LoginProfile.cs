using Common.Models;
using Common.ViewModels;
using AutoMapper;

public class LoginProfile : Profile
{
    public LoginProfile()
    {
        CreateMap<UserLoginViewModel,User>()  
            .BeforeMap((src, dest, context) => 
            {
                if (context.TryGetItems(out var items) && 
                    items.TryGetValue("RoleId", out var roleIdObj) &&
                    roleIdObj is int roleId)
                {
                    dest.RoleId = roleId;
                }
            });
    }
}