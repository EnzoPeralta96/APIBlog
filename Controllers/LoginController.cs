using APIBlog.Shared;
using APIBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly ILoginService _loginService;

    public LoginController(ILogger<LoginController> logger, ILoginService loginService)
    {
        _logger = logger;
        _loginService = loginService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginViewModel userLogin)
    {
        Result<string> result = await _loginService.LoginAsync(userLogin);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.NotExist => BadRequest(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return Ok(new {token = result.Value});
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("CreateAdmin")]
    public async Task<IActionResult> AdminRegister([FromBody] UserLoginViewModel userRegister)
    {
        Result result = await _loginService.CreateAsync(userRegister,true);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.NameInUse => BadRequest(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return Ok(new { mesagge = result.SuccesMessage });
    }

    [HttpPost("CreateAccount")]
    public async Task<IActionResult> UserRegister([FromBody] UserLoginViewModel userRegister)
    {
        Result result = await _loginService.CreateAsync(userRegister);
        if (!result.IsSucces)
        {
            return result.State switch
            {
                State.NameInUse => BadRequest(new { message = result.ErrorMessage }),
                State.InternalServerError => StatusCode(500, result.ErrorMessage),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
        return Ok(new { mesagge = result.SuccesMessage });
    }
}
