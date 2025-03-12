using APIBlog.Shared;
using Common.ViewModels;
namespace APIBlog.Services;
public interface IUserService
{
    Task<Result<UserViewModel>> GetUserAsync(int id);
    Task<Result> UpdateAsync(UserUpdateViewModel userUpdate);
    Task<Result> UpdatePasswordAsync(UserPasswordUpdateViewModel userPasswordUpdate);
    Task<Result> DeleteAsync(int id);
}