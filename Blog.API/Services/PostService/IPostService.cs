using Common.ViewModels;
using APIBlog.Shared;

namespace APIBlog.Services;
public interface IPostService
{
    Task<Result<List<PostViewModel>>> GetPostsByBlogAsync(int blogId);
    Task<Result<PostViewModel>> GetPostAsync(int id);
    Task<Result<PostViewModel>> CreateAsync(PostCreateViewModel postCreate);
    Task<Result> UpdateAsync(PostUpdateViewModel postUpdate);
    Task<Result> DeleteAsync(int postId);
}

