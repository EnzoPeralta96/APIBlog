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
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return NoContent();
    }

    //Lo puede hacer el Admin, OwnerBlog u OwnerPost

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

   /* [HttpPut("{id}/like")]
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
    }*/
}