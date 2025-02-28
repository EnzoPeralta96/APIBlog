using APIBlog.Shared;
using APIBlog.ViewModels;

namespace APIBlog.Services;

public interface IBlogService
{
    Task<Result<BlogViewModel>> BlogAsync(int id);
    Task<Result<BlogViewModel>> CreateAsync(BlogCreateViewModel blogCreate);
    Task<Result> UpdateAsync(BlogUpdateViewModel blogUpdate);
    Task<Result> DeleteAsync(int ownerId, int blogId);
    Task<Result<List<BlogViewModel>>> BlogsAsync(int userId);
    Task<Result<List<PostViewModel>>> PostsByBlogAsync(int idBlog);

    //Task<BlogResultViewModels> BlogAsync(string name);
    //Task<PostResultViewModels> PostsByBlogAsync(string name);

}