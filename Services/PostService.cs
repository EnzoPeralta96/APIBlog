using APIBlog.Models;
using APIBlog.Repository;
using APIBlog.Shared;
using APIBlog.ViewModels;

namespace APIBlog.Services;
public class PostService
{
    private readonly IPostRepository _postRepository;
    private readonly IBlogRepository _blogRepository;

    private readonly IReactionRepostiroy _reactionRepository;

    public PostService(IPostRepository postRepository, IBlogRepository blogRepository,IReactionRepostiroy reactionRepository)
    {
        _postRepository = postRepository;
        _blogRepository = blogRepository;
        _reactionRepository = reactionRepository;
    }

   /* public async Task<PostResultViewModels> CreateAsync(CreatePostViewModels post_vm)
    {
        var blog = await _blogRepository.GetBlogAsync(post_vm.BlogId);
        if (blog is null) return new PostResultViewModels(State.BlogNotExist);

        var partialPost = new Post(post_vm);
        await _postRepository.CreateAsync(partialPost);

        var post = await _postRepository.GetLastPostAsync();
        await _reactionRepository.CreateAsync(post.Id);

        var postWhitReactions = await _postRepository.GetPostWhitReactionAsync(post.Id);
        
        return new PostResultViewModels(State.BlogExisting,null,postWhitReactions);
    }*/
  
}

/*
    public Task CreateAsync(Post post);
    public Task UpdateAsync(int id, Post post);
    public Task DeleteAsync(int id);
    public Task<Post> GetPostAsync(int id);
    public Task ReactPostAsync(int id);
    public Task ReadPostAsync(int id);
*/