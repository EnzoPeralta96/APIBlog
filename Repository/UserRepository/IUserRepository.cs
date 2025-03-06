using APIBlog.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace APIBlog.Repository;
public interface IUserRepository
{
    Task<User> GetUserAsync(int id);
    Task<User> GetUserAsync(string name, string password);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task UpdatePasswordAsync(User user);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsAsync(string name, string password);

    //Para actualizar
    Task<bool> NameInUseAsync(int id, string name);

    //Para crear
    Task<bool> NameInUseAsync(string name);


}