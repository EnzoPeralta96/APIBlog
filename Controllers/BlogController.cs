using System.Reflection.Metadata.Ecma335;
using APIBlog.Models;
using APIBlog.Services;
using APIBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{

    private readonly ILogger<BlogController> _logger;
    private readonly BlogService _blogService;

    public BlogController(ILogger<BlogController> logger, BlogService blogService)
    {
        _logger = logger;
        _blogService = blogService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlog(int id)
    {
        BlogResultViewModels blogResult = await _blogService.BlogAsync(id);

        if (blogNotExist(blogResult.BlogState)) return NotFound("Blog inexistente");

        return Ok(blogResult.Blog);
    }

    [HttpGet]
    public async Task<IActionResult> GetBlogs()
    {
        var blogs = await _blogService.BlogsAsync();
        return Ok(blogs);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BlogViewModels blog_vm)
    {
        if (!ModelState.IsValid) return BadRequest("Faltan datos");

        var blogState = await _blogService.CreateAsync(blog_vm);

        if (blogNameInUse(blogState)) return BadRequest("Nombre de blog en uso");

        var blogResult = await _blogService.BlogAsync(blog_vm.Name);

        return Ok(blogResult.Blog);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BlogViewModels blog)
    {
        if (!ModelState.IsValid) return BadRequest("Faltan datos");

        var blogState = await _blogService.UpdateAsync(id,blog);

        if (blogNotExist(blogState)) return NotFound("Blog inexistente");

        if (blogNameInUse(blogState)) return BadRequest("Ya existe un blog con ese titulo");

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var blogState = await _blogService.DeleteAsync(id);

        if (blogNotExist(blogState)) return NotFound("Blog inexistente");

        return Ok("Blog eliminado");
    }

    [HttpGet("{id}/posts")]
    public async Task<IActionResult> GetPosts(int id)
    {
        var postResultViewModels = await _blogService.PostsByBlogAsync(id);

        if (blogNotExist(postResultViewModels.BlogState)) return NotFound("Blog inexistente");

        return Ok(postResultViewModels.Posts);
    }

    private bool blogNotExist(BlogState blogState)
    {
        return blogState == BlogState.NotExist;
    }
    private bool blogNameInUse(BlogState blogState)
    {
        return blogState == BlogState.NameInUse;
    } 
}
