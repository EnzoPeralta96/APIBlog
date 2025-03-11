using APIBlog.Shared;
using APIBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private string _friendlyMessage;
    private readonly ILoginService _loginService;


    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginViewModel userLogin)
    {
        Result<string> result = await _loginService.LoginAsync(userLogin);

        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.NameInUse => BadRequest(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        return Ok(new { token = result.Value });
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("CreateAdmin")]
    public async Task<IActionResult> AdminRegister([FromBody] UserLoginViewModel userRegister)
    {
        Result result = await _loginService.CreateAsync(userRegister, true);

        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.NameInUse => BadRequest(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        _friendlyMessage = MessageProvider.Get(result.SuccesMessage);
        return Ok(new { mesagge = _friendlyMessage });

    }

    [HttpPost("CreateAccount")]
    public async Task<IActionResult> UserRegister([FromBody] UserLoginViewModel userRegister)
    {

        Result result = await _loginService.CreateAsync(userRegister);
        if (!result.IsSucces)
        {
            _friendlyMessage = MessageProvider.Get(result.ErrorMessage);
            return result.State switch
            {
                State.NameInUse => BadRequest(new { message = _friendlyMessage }),
                State.InternalServerError => StatusCode(500, _friendlyMessage),
                _ => BadRequest(new { message = _friendlyMessage })
            };
        }

        _friendlyMessage = MessageProvider.Get(result.SuccesMessage);
        return Ok(new { mesagge = _friendlyMessage });
    }
}
