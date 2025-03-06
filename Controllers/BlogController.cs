using APIBlog.Services;
using APIBlog.ViewModels;
using APIBlog.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace APIBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlogController : ControllerBase
{

    private readonly ILogger<BlogController> _logger;
    private readonly IBlogService _blogService;

    public BlogController(ILogger<BlogController> logger, IBlogService blogService)
    {
        _logger = logger;
        _blogService = blogService;
    }

    /*
    Un usuario se puede crear blog para si mismo, 
    el admin tambien puede crear un blog para un usuario
    */
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BlogCreateViewModel blogCreated)
    {

        var result = await _blogService.CreateAsync(blogCreated);

        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.Forbidden => StatusCode(403, result.ErrorMessage),
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.NameInUse => BadRequest(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return Ok(result.Value);
    }


    /*
    El que puede actualizar un blog es el dueño o un admin
    */
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] BlogUpdateViewModel blogUpdate)
    {

        Result result = await _blogService.UpdateAsync(blogUpdate);

        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.Forbidden => StatusCode(403, result.ErrorMessage),
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.NameInUse => BadRequest(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return NoContent();
    }

    /*
    El que puede borrar un blog es el dueño o un admin
    */
    [HttpDelete("{idOwner}/{idBlog}")]
    public async Task<IActionResult> Delete(int idOwner, int idBlog)
    {

        Result result = await _blogService.DeleteAsync(idOwner, idBlog);

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


  

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlog(int id)
    {
        Result<BlogViewModel> result = await _blogService.BlogAsync(id);

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

    [HttpGet("UserBlogs/{userId}")]
    public async Task<IActionResult> GetBlogs(int userId)
    {
        Result<List<BlogViewModel>> result = await _blogService.BlogsAsync(userId);

        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return Ok(result.Value);
    }





}
 /* [HttpGet("{idBlog}/posts")]
    public async Task<IActionResult> GetPosts(int idBlog)
    {
        Result<List<PostViewModel>> result = await _blogService.PostsByBlogAsync(idBlog);

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
    }*/
