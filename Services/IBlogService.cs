using APIBlog.Models;
using APIBlog.Shared;
using APIBlog.ViewModels;

namespace APIBlog.Services;

public interface IBlogService
{
    public Task<Result<BlogViewModel>> BlogAsync(int id);
    public Task<Result<BlogViewModel>> CreateAsync(BlogRequestViewModel blog_vm);
    public Task<Result> UpdateAsync(int id, BlogRequestViewModel blog_vm);
    public Task<Result> DeleteAsync(int id);
    public Task<List<BlogViewModel>> BlogsAsync();

    public Task<Result<List<PostViewModel>>> PostsByBlogAsync(int idBlog);

    //public Task<BlogResultViewModels> BlogAsync(string name);
    //public Task<PostResultViewModels> PostsByBlogAsync(string name);

}