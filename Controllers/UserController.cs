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

    private string _friendlyMessage;
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserAsync(int id)
    {
        Result<UserViewModel> result = await _userService.GetUserAsync(id);
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

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdateViewModel userUpdate)
    {

        Result result = await _userService.UpdateAsync(userUpdate);
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

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordUpdateViewModel userUpdate)
    {

        Result result = await _userService.UpdatePasswordAsync(userUpdate);

        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.Forbidden => StatusCode(403, _friendlyMessage),
                State.NotExist => NotFound(new { message = _friendlyMessage }),
                State.PasswordsDifferents => BadRequest(new { message = _friendlyMessage }),
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

        Result result = await _userService.DeleteAsync(id);

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

        _friendlyMessage = MessageProvider.Get(result.SuccesMessage);
        return Ok(new { message = _friendlyMessage });
    }
}