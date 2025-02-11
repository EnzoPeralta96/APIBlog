using System.Threading.Tasks;
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

    public async Task<BlogState> CreateAsync(BlogViewModels blog_vm)
    {
        var blog = await blogRepository.GetBlogAsync(blog_vm.Name);

        if (blog is not null) return BlogState.NameInUse;

        await blogRepository.CreateAsync(new Blog(blog_vm));

        return BlogState.Created;
    }

    public async Task<BlogState> UpdateAsync(int id, BlogViewModels blog_vm)
    {
        var blog = await blogRepository.GetBlogAsync(id);

        if (blog is null) return BlogState.NotExist;

        var existing_blog = await blogRepository.GetBlogAsync(blog_vm.Name);

        if (blogNameInUse(id, existing_blog)) return BlogState.NameInUse;

        await blogRepository.UpdateAsync(id, new Blog(blog_vm));

        return BlogState.Updated;
    }

    public async Task<BlogState> DeleteAsync(int id)
    {
        var blog = await blogRepository.GetBlogAsync(id);

        if (blog is null) return BlogState.NotExist;

        await blogRepository.DeleteAsync(id);

        return BlogState.Deleted;
    }
    public async Task<BlogResultViewModels> BlogAsync(int id)
    {
        var blog = await blogRepository.GetBlogAsync(id);

        if (blog is null) return new BlogResultViewModels(BlogState.NotExist);

        return new BlogResultViewModels(BlogState.Existing, blog);
    }
    public async Task<BlogResultViewModels> BlogAsync(string name)
    {
        var blog = await blogRepository.GetBlogAsync(name);

        if (blog is null) return new BlogResultViewModels(BlogState.NotExist);

        return new BlogResultViewModels(BlogState.Existing, blog);
    }
    public async Task<List<Blog>> BlogsAsync()
    {
        return await blogRepository.BlogsAsync();
    }
    public async Task<PostResultViewModels> PostsByBlogAsync(int id)
    {
        var blog = await blogRepository.GetBlogAsync(id);

        if (blog is null) return new PostResultViewModels(BlogState.NotExist);

        var posts = await blogRepository.PostsByBlogAsync(id);

        return new PostResultViewModels(BlogState.Existing, posts);
    }
    public async Task<PostResultViewModels> PostsByBlogAsync(string name)
    {
        var blog = await blogRepository.GetBlogAsync(name);

        if (blog is null) return new PostResultViewModels(BlogState.NotExist);

        var posts = await blogRepository.PostsByBlogAsync(name);

        return new PostResultViewModels(BlogState.Existing, posts);
    }
    private  bool blogNameInUse(int id, Blog? existing_blog)
    {
        return existing_blog is not null && existing_blog.Id != id;
    }

   

}