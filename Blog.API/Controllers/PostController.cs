using APIBlog.Services;
using APIBlog.Shared;
using Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostController : ControllerBase
{
    private string _friendlyMessage;
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet("postsByBlog/{blogId}")]
    public async Task<IActionResult> GetPosts(int blogId)
    {
        Result<List<PostViewModel>> result = await _postService.GetPostsByBlogAsync(blogId);
        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
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
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
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
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.Forbidden => StatusCode(403, _friendlyMessage),
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        return CreatedAtAction(
            "GetPost",
            new {id = result.Value.Id},
            result.Value
        );
    }

    //Lo puede hacer el Admin, OwnerPost
    [HttpPut]
    public async Task<IActionResult> Update(PostUpdateViewModel postRequest)
    {
        Result result = await _postService.UpdateAsync(postRequest);

        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.Forbidden => StatusCode(403, _friendlyMessage),
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        _friendlyMessage = MessageProvider.Get(result.SuccesMessage);
        return StatusCode(201, _friendlyMessage);
    }

    //Lo puede hacer el Admin, OwnerBlog u OwnerPost

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Result result = await _postService.DeleteAsync(id);

        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.Forbidden => StatusCode(403, _friendlyMessage),
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        _friendlyMessage = MessageProvider.Get(result.SuccesMessage);
        return Ok(new { message = _friendlyMessage });
    }


}