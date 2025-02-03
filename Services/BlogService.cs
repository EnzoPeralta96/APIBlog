using APIBlog.Models;
using APIBlog.Repository;
using APIBlog.ViewModels;

namespace APIBlog.Services;
public class BlogService
{
    private readonly IBlogRepository blogRepository;
    public BlogService(IBlogRepository blogRepository)
    {
        this.blogRepository = blogRepository;
    }

    public bool Create(BlogViewModels blog_vm)
    {
        if (blogRepository.GetBlog(blog_vm.Name) is not null) return false;
        blogRepository.Create(new Blog(blog_vm));
        return true;
    }

    public bool Update(int id, BlogViewModels blog_vm)
    {
        if (blogRepository.GetBlog(id) is null) return false;

        var blog = blogRepository.GetBlog(blog_vm.Name);

        if (blog is not null && blog.Id != id) return false;
        blogRepository.Update(id, new Blog(blog_vm));
        return true;
    }

    public bool Delete(int id)
    {
        if (blogRepository.GetBlog(id) is null) return false;
        blogRepository.Delete(id);
        return true;
    }
    public Blog Blog(int id)
    {
        return blogRepository.GetBlog(id);
    }

    public Blog Blog(string name)
    {
        return blogRepository.GetBlog(name);
    }

    public List<Blog> Blogs()
    {
        return blogRepository.Blogs();
    }
    public List<Post> PostsByBlog(int id)
    {
        return blogRepository.PostsByBlog(id);
    }
    public List<Post> PostsByBlog(string name)
    {
        return blogRepository.PostsByBlog(name);
    }

}