using APIBlog.Models;
using APIBlog.Shared;
using APIBlog.ViewModels;

namespace APIBlog.Services;
public interface IPostService
{
    Task<Result<PostViewModel>> GetPostAsync(int id);
    Task<Result<PostViewModel>> CreateAsync(PostCreateViewModel postCreate);
    Task<Result> UpdateAsync(PostUpdateViewModel postUpdate);
    Task<Result> DeleteAsync(int id);

}

   // Task<Result> LikeAsync(int postId);
   // Task<Result> ViewAsync(int PostId);