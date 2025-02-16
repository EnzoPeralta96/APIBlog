using APIBlog.Models;
using APIBlog.Services;
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

    /*[HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostViewModels post_vm)
    {
        if (!ModelState.IsValid) return NotFound("Faltan datos");

        var postResult = await _postService.CreateAsync(post_vm);

        if (postResult.BlogState == State.BlogNotExist) return NotFound("El Blog indicado no existe");
        
        var post = new PostViewModels(postResult.Post);

        return Ok(post);
    }*/



}