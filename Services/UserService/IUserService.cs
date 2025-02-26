using APIBlog.Shared;
using APIBlog.ViewModels;
namespace APIBlog.Services;
public interface IUserService
{
    Task<Result<UserViewModel>> GetUserAsync(int id);
    Task<Result> UpdateAsync(UserUpdateViewModel userRequest);
    Task<Result> UpdatePasswordAsync(UserPasswordUpdateViewModel userRequest);
    Task<Result> DeleteAsync(int id);
}