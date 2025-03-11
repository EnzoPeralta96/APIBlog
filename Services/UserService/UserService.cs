using APIBlog.Models;
using APIBlog.Policies.Authorization;
using APIBlog.Repository;
using APIBlog.Security;
using APIBlog.Shared;
using APIBlog.ViewModels;
using AutoMapper;
namespace APIBlog.Services;

public class UserService : IUserService
{
    private readonly ILogger _logger;
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly ISecurityService _securityService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserAuthorizationService userAuthorizationService, ISecurityService securityService, IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
    {
        _userAuthorizationService = userAuthorizationService;
        _securityService = securityService;
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<UserViewModel>> GetUserAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetUserAsync(id);

            if (user is null) return Result<UserViewModel>.Failure(Message.user_not_exist, State.NotExist);

            var UserViewModel = _mapper.Map<UserViewModel>(user);

            return Result<UserViewModel>.Succes(UserViewModel);
        }
        catch (Exception ex)
        {
             _logger.LogError("Error inesperado: " + ex.ToString());
            return Result<UserViewModel>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(UserUpdateViewModel userUpdate)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserAsync(userUpdate.Id);

            if (!authorizationResult.IsSucces) return Result.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            bool exists = await _userRepository.ExistsAsync(userUpdate.Id);

            if (!exists) return Result.Failure(Message.user_not_exist, State.NotExist);

            bool nameInUse = await _userRepository.NameInUseAsync(userUpdate.Id, userUpdate.Name);

            if (nameInUse) return Result.Failure(Message.username_in_use, State.NameInUse);

            var user = _mapper.Map<User>(userUpdate);

            await _userRepository.UpdateAsync(user);

            return Result.Succes(Message.user_updated);
        }
        catch (Exception ex)
        {
             _logger.LogError("Error inesperado: " + ex.ToString());
            return Result.Failure(Message.unexpected_error, State.InternalServerError);
        }

    }

    public async Task<Result> UpdatePasswordAsync(UserPasswordUpdateViewModel userPasswordUpdate)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserAsync(userPasswordUpdate.Id);

            if (!authorizationResult.IsSucces) return Result.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            bool exists = await _userRepository.ExistsAsync(userPasswordUpdate.Id);

            if (!exists) return Result.Failure(Message.user_not_exist, State.NotExist);

            if (userPasswordUpdate.Password != userPasswordUpdate.PasswordRepeat) return Result.Failure(Message.passwords_not_match, State.PasswordsDifferents);

            userPasswordUpdate.Password = _securityService.HashingSHA256(userPasswordUpdate.Password);

            var user = _mapper.Map<User>(userPasswordUpdate);

            await _userRepository.UpdatePasswordAsync(user);

            return Result.Succes(Message.password_updated);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserAsync(id);

            if (!authorizationResult.IsSucces) return Result.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            bool exists = await _userRepository.ExistsAsync(id);

            if (!exists) return Result.Failure(Message.user_not_exist, State.NotExist);

            await _userRepository.DeleteAsync(id);

            return Result.Succes(Message.user_deleted);
        }
        catch (Exception ex)
        {
             _logger.LogError("Error inesperado: " + ex.ToString());
            return Result.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }
}