using APIBlog.Services;
using APIBlog.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Common.ViewModels;
namespace APIBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlogController : ControllerBase
{

    private string _friendlyMessage;
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet("UserBlogs/{userId}")]
    public async Task<IActionResult> GetBlogs(int userId)
    {
        Result<List<BlogViewModel>> result = await _blogService.BlogsAsync(userId);

        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlog(int id)
    {
        Result<BlogViewModel> result = await _blogService.BlogAsync(id);

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
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.Forbidden => StatusCode(403, _friendlyMessage),
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.NameInUse => BadRequest(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }
        
        return CreatedAtAction(
            "GetBlog",
            new { id = result.Value.Id },
            result.Value
        );
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
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.Forbidden => StatusCode(403, _friendlyMessage),
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.NameInUse => BadRequest(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        _friendlyMessage = MessageProvider.Get(result.SuccesMessage);
        return StatusCode(201, _friendlyMessage);
    }

    /*
    El que puede borrar un blog es el dueño o un admin
    */
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        Result result = await _blogService.DeleteAsync(id);

        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.Forbidden => StatusCode(403, _friendlyMessage),
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.NameInUse => BadRequest(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        _friendlyMessage = MessageProvider.Get(result.SuccesMessage);

        return Ok(new { message = _friendlyMessage });
    }

}

