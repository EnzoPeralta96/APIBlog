using APIBlog.Services;
using APIBlog.ViewModels;
using APIBlog.Shared;
using Microsoft.AspNetCore.Mvc;
namespace APIBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{

    private readonly ILogger<BlogController> _logger;
    private readonly IBlogService _blogService;

    public BlogController(ILogger<BlogController> logger, IBlogService blogService)
    {
        _logger = logger;
        _blogService = blogService;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BlogRequestViewModel blogRequest)
    {
        //No hace falta ya que el atributo ApiController hace
        //al haber un error de validación devolver un Bad request (400)
        //if (!ModelState.IsValid) return BadRequest("Faltan datos");

        var result = await _blogService.CreateAsync(blogRequest);

        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.NameInUse => BadRequest(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })

            };
        }
        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BlogRequestViewModel blogRequest)
    {
        Result result = await _blogService.UpdateAsync(id, blogRequest);

        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.NameInUse => BadRequest(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Result result = await _blogService.DeleteAsync(id);

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

    [HttpGet]
    public async Task<IActionResult> GetBlogs()
    {
        Result<List<BlogViewModel>> result = await _blogService.BlogsAsync();

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

    [HttpGet("{idBlog}/posts")]
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
    }







}
