using APIBlog.Shared;
using APIBlog.ViewModels;

namespace APIBlog.Services;

public interface IBlogService
{
    Task<Result<BlogViewModel>> BlogAsync(int id);
    Task<Result<BlogViewModel>> CreateAsync(BlogRequestViewModel blog_vm);
    Task<Result> UpdateAsync(int id, BlogRequestViewModel blog_vm);
    Task<Result> DeleteAsync(int id);
    Task<Result<List<BlogViewModel>>> BlogsAsync();
    Task<Result<List<PostViewModel>>> PostsByBlogAsync(int idBlog);

    //Task<BlogResultViewModels> BlogAsync(string name);
    //Task<PostResultViewModels> PostsByBlogAsync(string name);

}