using APIBlog.Models;
using APIBlog.Services;
using APIBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIBlog.Controllers;

[ApiController]
[Route("[controller]")]
public class BlogController : ControllerBase
{

    private readonly ILogger<BlogController> _logger;
    private readonly BlogService _blogService;

    public BlogController(ILogger<BlogController> logger, BlogService blogService)
    {
        _logger = logger;
        _blogService = blogService;
    }

    [HttpPost("api/blog")]
    public IActionResult Create([FromBody]BlogViewModels blog)
    {
        if (!ModelState.IsValid) return BadRequest("Faltan datos");

        if(!_blogService.Create(blog)) return BadRequest("Blog existente");

        return Created();
    }

  
}
