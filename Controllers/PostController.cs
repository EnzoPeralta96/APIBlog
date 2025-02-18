using APIBlog.Models;
using APIBlog.Services;
using APIBlog.Shared;
using APIBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly PostService _postService;

    public PostController(ILogger<PostController> logger, PostService postService)
    {
        _logger = logger;
        _postService = postService;
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

    [HttpPost("{blogId}")]
    public async Task<IActionResult> Create(int blogId, [FromBody] PostRequestViewModel postRequest)
    {
        Result<PostViewModel> result = await _postService.CreateAsync(blogId, postRequest);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PostRequestViewModel postRequest)
    {
        Result result = await _postService.UpdateAsync(id, postRequest);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return NoContent();
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int id)
    {
        Result result = await _postService.DeleteAsync(id);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return Ok(new { message = result.SuccesMessage });
    }

    [HttpPut("{id}/like")]
    public async Task<IActionResult> Like(int id)
    {
        Result result = await _postService.LikeAsync(id);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return NoContent();
    }
}