using APIBlog.Data;
using APIBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBlog.Repository;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _blogDbContext;

    public BlogRepository(BlogDbContext blogDb)
    {
        _blogDbContext = blogDb;
    }

    public void Create(Blog blog)
    {
        _blogDbContext.Blogs.Add(blog);
        _blogDbContext.SaveChanges();
    }

    public void Update(int id, Blog blog)
    {

        var searched_blog = _blogDbContext.Blogs.Find(id);
        searched_blog.Name = blog.Name;
        searched_blog.Description = blog.Description;
        _blogDbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var searched_blog = _blogDbContext.Blogs.Find(id);
        _blogDbContext.Blogs.Remove(searched_blog);
        _blogDbContext.SaveChanges();
    }
    
    public Blog GetBlog(int id)
    {
        var blog = _blogDbContext.Blogs.Find(id);
        return blog;
    }

    public Blog GetBlog(string name)
    {
        return _blogDbContext.Blogs.FirstOrDefault(bl => bl.Name == name);
    }

    public List<Blog> Blogs()
    {
        return _blogDbContext.Blogs.ToList();
    }
    public List<Post> PostsByBlog(int id)
    {
        return _blogDbContext.Posts
                .Where(p => p.BlogId == id)
                .ToList();
    }

    public List<Post> PostsByBlog(string name)
    {
        return _blogDbContext.Posts
                .Where(p => p.Blog.Name == name)
                .ToList();
    }
    
}


