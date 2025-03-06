using System.Security.Cryptography;
using System.Text;
using APIBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBlog.Data;
public class BlogDbContext : DbContext
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName().ToLower());

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.Name.ToLower());
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName().ToLower());
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName().ToLower());
            }
        }

        modelBuilder.Entity<Role>().HasData(
            new Role {Id = 1, Rol = "admin"},
            new Role {Id = 2, Rol = "estandar"}
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "admin",
                Password = HashingSHA256("1234"),
                RoleId = 1
            }

        );
    }

    private string HashingSHA256(string text)
    {
        using (SHA256 sha256Has = SHA256.Create())
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] textHashValue = sha256Has.ComputeHash(textBytes);

            StringBuilder builder = new StringBuilder();

            foreach (var textHashByte in textHashValue)
            {
                builder.Append(textHashByte.ToString("x2"));
            }

            return builder.ToString();
        }
    }

}