using Common.ViewModels;
using APIBlog.Repository;
using APIBlog.Security;
using APIBlog.Shared;
using Common.Models;
using AutoMapper;

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurityService _security;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public LoginService(IUserRepository userRepository, ISecurityService security, IMapper mapper, ILogger<LoginService> logger)
    {
        _userRepository = userRepository;
        _security = security;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result> CreateAsync(UserLoginViewModel userCreateAccount, bool isAmdin = false)
    {
        try
        {
            bool nameInUse = await _userRepository.NameInUseAsync(userCreateAccount.Name);

            if (nameInUse) return Result.Failure(Message.username_in_use, State.NameInUse);

            userCreateAccount.Password = _security.HashingSHA256(userCreateAccount.Password);

            int roleId = isAmdin ? 1 : 2;

            var user = _mapper.Map<User>(userCreateAccount, options => options.Items["RoleId"] = roleId);

            await _userRepository.CreateAsync(user);

            return Result.Succes(Message.user_created);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    public async Task<Result<string>> LoginAsync(UserLoginViewModel userLogin)
    {
        try
        {
            userLogin.Password = _security.HashingSHA256(userLogin.Password);

            var user = await _userRepository.GetUserAsync(userLogin.Name, userLogin.Password);

            if (user is null) return Result<string>.Failure(Message.incorrect_login, State.NotExist);

            var token = _security.GetJwt(user);

            return Result<string>.Succes(token);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result<string>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }






}