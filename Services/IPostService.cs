using APIBlog.Models;
using APIBlog.Shared;
using APIBlog.ViewModels;

namespace APIBlog.Services;
public interface IPostService
{
    Task<Result<PostViewModel>> GetPostAsync(int id);
    Task<Result<PostViewModel>> CreateAsync(int idBlog, PostRequestViewModel postRequest);
    Task<Result> UpdateAsync(int id, PostRequestViewModel postRequest);
    Task<Result> DeleteAsync(int id);
    Task<Result> LikeAsync(int postId);
   // Task<Result> ViewAsync(int PostId);
}