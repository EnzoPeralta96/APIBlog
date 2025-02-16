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

        if (!result.IsSucces) return NotFound(new { messsage = result.ErrorMessage });

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BlogRequestViewModel blogRequest)
    {
        //No hace falta ya que el atributo ApiController hace
        //al haber un error de validación devolver un Bad request (400)
        //if (!ModelState.IsValid) return BadRequest("Faltan datos");

        var result = await _blogService.CreateAsync(blogRequest);

        if (!result.IsSucces) return BadRequest(new { message = result.ErrorMessage });

        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BlogRequestViewModel blogRequest)
    {
        //if (!ModelState.IsValid) return BadRequest("Faltan datos");

        Result result = await _blogService.UpdateAsync(id, blogRequest);

        if (!result.IsSucces)
        {
            if (result.State == State.BlogNotExist) return NotFound(new { message = result.ErrorMessage });
            if (result.State == State.BlogNameInUse) return BadRequest(new { message = result.ErrorMessage });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Result result = await _blogService.DeleteAsync(id);

        if (!result.IsSucces) return NotFound(new { message = result.ErrorMessage });

        return Ok(new { message = result.ErrorMessage });
    }

    [HttpGet]
    public async Task<IActionResult> GetBlogs()
    {
        List<BlogViewModel> blogs = await _blogService.BlogsAsync();
        return Ok(blogs);
    }

    [HttpGet("{idBlog}/posts")]
    public async Task<IActionResult> GetPosts(int idBlog)
    {
        Result<List<PostViewModel>> result = await _blogService.PostsByBlogAsync(idBlog);

        if (!result.IsSucces) return NotFound(new { messagge = result.ErrorMessage });

        return Ok(result.Value);
    }







}
