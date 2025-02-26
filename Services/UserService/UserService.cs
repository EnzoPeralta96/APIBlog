using APIBlog.AuthorizationPoliciesSample.Policies.Requeriment;
using APIBlog.Models;
using APIBlog.Repository;
using APIBlog.Shared;
using APIBlog.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
namespace APIBlog.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurityService _securityService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, ISecurityService securityService,IMapper mapper)
    {
        _userRepository = userRepository;
        _securityService = securityService;
        _mapper = mapper;
    }

    public async Task<Result<UserViewModel>> GetUserAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetUserAsync(id);
            if (user is null) return Result<UserViewModel>.Failure($"El usuario con id = {id} no existe", State.NotExist);
            var UserViewModel = _mapper.Map<UserViewModel>(user);
            return Result<UserViewModel>.Succes(UserViewModel);
        }
        catch (Exception ex)
        {
            return Result<UserViewModel>.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(UserUpdateViewModel userRequest)
    {
        try
        {
            bool exists = await _userRepository.ExistsAsync(userRequest.Id);

            if (!exists) return Result.Failure($"El usuario con id = {userRequest.Id} no existe", State.NotExist);

            bool nameInUse = await _userRepository.NameInUseAsync(userRequest.Id, userRequest.Name);

            if(nameInUse) return Result.Failure($"Nombre de usuario: {userRequest.Name} en uso", State.NameInUse);

            var user = _mapper.Map<User>(userRequest);

            await _userRepository.UpdateAsync(user);

            return Result.Succes("Usuario actualizado con éxito!");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }

    }

    public async Task<Result> UpdatePasswordAsync(UserPasswordUpdateViewModel userRequest)
    {
        try
        {
            if (userRequest.Password != userRequest.PasswordRepeat) return Result.Failure("Las contraseñas no coinciden", State.PasswordsDifferents);

            bool exists = await _userRepository.ExistsAsync(userRequest.Id);

            if (!exists) return Result.Failure($"El usuario con id = {userRequest.Id} no existe", State.NotExist);

            userRequest.Password = _securityService.HashingSHA256(userRequest.Password);

            var user = _mapper.Map<User>(userRequest);

            await _userRepository.UpdatePasswordAsync(user);

            return Result.Succes("Contraseña modificada con éxito");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            bool exists = await _userRepository.ExistsAsync(id);

            if (!exists) return Result.Failure($"El usuario con id = {id} no existe", State.NotExist);

            await _userRepository.DeleteAsync(id);

            return Result.Succes("Usuario eliminado con exito");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }



}