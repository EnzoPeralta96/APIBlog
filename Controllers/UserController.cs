using APIBlog.Services;
using APIBlog.Shared;
using APIBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserAsync(int id)
    {
        Result<UserViewModel> result = await _userService.GetUserAsync(id);
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

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdateViewModel userUpdate)
    {

        Result result = await _userService.UpdateAsync(userUpdate);
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
        return Created();
    }

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordUpdateViewModel userUpdate)
    {

        Result result = await _userService.UpdatePasswordAsync(userUpdate);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.Forbidden => StatusCode(403, result.ErrorMessage),
                State.PasswordsDifferents => BadRequest(new { message = result.ErrorMessage }),
                State.NotExist => NotFound(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return Created();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        Result result = await _userService.DeleteAsync(id);
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
}