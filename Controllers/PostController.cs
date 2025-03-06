using APIBlog.Services;
using APIBlog.Shared;
using APIBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostService _postService;

    public PostController(ILogger<PostController> logger, IPostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    [HttpGet("postsByBlog/{blogId}")]
    public async Task<IActionResult> GetPosts(int blogId)
    {
        Result<List<PostViewModel>> result = await _postService.GetPostsByBlogAsync(blogId);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
        Result<PostViewModel> result = await _postService.GetPostAsync(id);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return Ok(result.Value);
    }

    //Lo puede hacer el admin, el ownerBlog u otro user
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostCreateViewModel postCreate)
    {
        Result<PostViewModel> result = await _postService.CreateAsync(postCreate);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.Forbidden => StatusCode(403, result.ErrorMessage),
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return Ok(result.Value);
    }

    //Lo puede hacer el Admin, OwnerPost
    [HttpPut]
    public async Task<IActionResult> Update(PostUpdateViewModel postRequest)
    {
        Result result = await _postService.UpdateAsync(postRequest);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.Forbidden => StatusCode(403, result.ErrorMessage),
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return NoContent();
    }

    //Lo puede hacer el Admin, OwnerBlog u OwnerPost

    [HttpDelete("{ownerId}/{postId}")]
    public async Task<IActionResult> Delete(int ownerId, int postId)
    {
        Result result = await _postService.DeleteAsync(ownerId, postId);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.Forbidden => StatusCode(403, result.ErrorMessage),
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return Ok(new { message = result.SuccesMessage });
    }


}