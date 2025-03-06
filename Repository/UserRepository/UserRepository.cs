using APIBlog.Data;
using APIBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBlog.Repository;

public class UserRepository : IUserRepository
{
    private readonly BlogDbContext _blogDbContext;

    public UserRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task<User> GetUserAsync(int id)
    {
        return await _blogDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task<User> GetUserAsync(string name, string password)
    {
        return await _blogDbContext.Users
                .AsNoTracking()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Name == name && u.Password == password);
    }

    public async Task CreateAsync(User user)
    {
        await _blogDbContext.Users.AddAsync(user);
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        await _blogDbContext.Users
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.Name, user.Name)
                );
    }

    public async Task UpdatePasswordAsync(User user)
    {
        await _blogDbContext.Users
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.Password, user.Password)
                );
    }

    public async Task DeleteAsync(int id)
    {
        await _blogDbContext.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _blogDbContext.Users
               .AsNoTracking()
               .AnyAsync(u => u.Id == id);
    }


    public async Task<bool> ExistsAsync(string name, string password)
    {
        return await _blogDbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Name == name && u.Password == password);
    }

    public async Task<bool> NameInUseAsync(string name)
    {
        return await _blogDbContext.Users
               .AsNoTracking()
               .AnyAsync(u => u.Name == name);
    }

    public async Task<bool> NameInUseAsync(int id, string name)
    {
        return await _blogDbContext.Users
               .AsNoTracking()
               .AnyAsync(u => u.Id != id && u.Name == name);
    }



}