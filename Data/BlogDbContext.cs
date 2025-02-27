using System.Data.Common;
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
    public DbSet<Reaction> Reactions { get; set; }
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().ToTable("role");
        modelBuilder.Entity<User>().ToTable("user");
        modelBuilder.Entity<Blog>().ToTable("blog");
        modelBuilder.Entity<Post>().ToTable("post");
        modelBuilder.Entity<Comment>().ToTable("comment");
        modelBuilder.Entity<Reaction>().ToTable("reaction");
    }

}