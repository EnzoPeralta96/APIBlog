using System.Data.Common;
using APIBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBlog.Data;
public class BlogDbContext:DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options):base(options)
    {

    }
    public DbSet<Blog> Blogs {get;set;}
    public DbSet<Post> Posts{get; set;}
    public DbSet<Reaction> Reactions{get;set;}

}