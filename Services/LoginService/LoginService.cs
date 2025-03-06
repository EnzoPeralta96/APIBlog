using APIBlog.Models;
using APIBlog.Repository;
using APIBlog.Security;
using APIBlog.Shared;
using APIBlog.ViewModels;
using AutoMapper;

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurityService _security;
    private readonly IMapper _mapper;

    public LoginService(IUserRepository userRepository, ISecurityService security, IMapper mapper)
    {
        _userRepository = userRepository;
        _security = security;
        _mapper = mapper;
    }

    public async Task<Result> CreateAsync(UserLoginViewModel userCreateAccount, bool isAmdin = false)
    {
        try
        {
            bool nameInUse = await _userRepository.NameInUseAsync(userCreateAccount.Name);

            if (nameInUse) return Result.Failure($"El nombre {userCreateAccount.Name} ya esta uso", State.NameInUse);

            userCreateAccount.Password = _security.HashingSHA256(userCreateAccount.Password);

            int roleId = isAmdin ? 1 : 2;

            var user = _mapper.Map<User>(userCreateAccount, options => options.Items["RoleId"] = roleId);
            
            await _userRepository.CreateAsync(user);

            return Result.Succes($"Usuario {userCreateAccount.Name} creado con éxito");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result<string>> LoginAsync(UserLoginViewModel userLogin)
    {
        try
        {
            userLogin.Password = _security.HashingSHA256(userLogin.Password);

            var user = await _userRepository.GetUserAsync(userLogin.Name, userLogin.Password);

            if (user is null) return Result<string>.Failure("Usuario o contraseña incorrecta", State.NotExist);
            
            var token = _security.GetJwt(user);

            return Result<string>.Succes(token);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }






}