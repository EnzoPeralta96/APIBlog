using System.Threading.Tasks;
using APIBlog.Repository;
using APIBlog.Services;
using APIBlog.Shared;
using Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SQLitePCL;
namespace APIBlog.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentController : ControllerBase
{
    private string _friendlyMessage = string.Empty;
    private readonly ICommentService _commentService;
    public CommentController(ICommentService commentService)
    {

        _commentService = commentService;
    }

    [HttpGet("CommentsByPost/{postId}")]
    public async Task<IActionResult> GetCommentsByPost(int postId)
    {
        Result<List<CommentViewModel>> result = await _commentService.GetCommentsByPost(postId);

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetComment(int id)
    {
        Result<CommentViewModel> result = await _commentService.GetComment(id);

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommentCreateViewModel commentCreate)
    {
        Result<CommentViewModel> result = await _commentService.CreateAsync(commentCreate);
        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.Forbidden => StatusCode(403, _friendlyMessage),
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        return CreatedAtAction(
            "GetComment",
            new {id = result.Value.Id},
            result.Value
        );
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CommentUpdateViewModel commentUpdate)
    {
        Result result = await _commentService.UpdateAsync(commentUpdate);

        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.Forbidden => StatusCode(403, _friendlyMessage),
                State.BadRequest => BadRequest(new { message = _friendlyMessage }),
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        _friendlyMessage = MessageProvider.Get(result.SuccesMessage);
        return StatusCode(201, _friendlyMessage);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Result result = await _commentService.DeleteAsync(id);

        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.Forbidden => StatusCode(403, _friendlyMessage),
                State.BadRequest => BadRequest(new { message = _friendlyMessage }),
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        _friendlyMessage = MessageProvider.Get(result.SuccesMessage);
        return Ok(new { message = _friendlyMessage });
    }

}
