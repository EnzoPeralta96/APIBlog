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
    public IActionResult GetBlog(int id)
    {
        Blog blog = _blogService.Blog(id);

        if (blog is null) return NotFound("Blog no encontrado");

        return Ok(blog);
    }

    [HttpGet]
    public IActionResult GetBlogs()
    {
        var blogs = _blogService.Blogs();
        return Ok(blogs);
    }

    [HttpPost]
    public IActionResult Create([FromBody] BlogViewModels blog)
    {
        if (!ModelState.IsValid) return BadRequest("Faltan datos");

        if (!_blogService.Create(blog)) return BadRequest("Blog existente");

        var blogCreated = _blogService.Blog(blog.Name);

        return Ok(blogCreated);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] BlogViewModels blog)
    {
        if (!ModelState.IsValid) return BadRequest("Faltan datos");
      
        if (_blogService.Blog(id) is null) return NotFound("Blog no encontrado");

        if(!_blogService.Update(id,blog)) return BadRequest("Ya existe un blog con ese titulo");

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
       bool blogDeleted = _blogService.Delete(id);

       return blogDeleted ? Ok("Blog eliminado") : NotFound("Blog no encontrado");
    }

    [HttpGet("{id}/posts")]
    public IActionResult GetPosts(int id)
    {
        if (_blogService.Blog(id) is null) return NotFound("Blog no encontrado");
        var posts = _blogService.PostsByBlog(id);
        return Ok(posts);
    }




}
