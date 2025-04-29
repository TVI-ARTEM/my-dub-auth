using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyDub.Auth.Configure;
using MyDub.Auth.Helpers;
using MyDub.Auth.Models;
using MyDub.Auth.Models.Requests;
using MyDub.Auth.Models.Responses;
using MyDub.Auth.Repositories;

namespace MyDub.Auth.Controllers;

[Route("api/users")]
public class AuthController(IUserRepository repository, JWTHelper jwtHelper, IOptionsMonitor<AuthOptions> options) : Controller
{
    [HttpPost]
    public async Task<CreateUserResponse> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userInfo = await repository.CreateAsync(request.Login, request.Password.ComputeSha256Hash(), cancellationToken);

        var token = jwtHelper.GenerateJwtToken(userInfo.Login);

        return new CreateUserResponse
        {
            Login = userInfo.Login,
            Token = token
        };
    }
    
    [HttpGet("login")]
    public async Task<LoginResponse> Login([FromQuery][Required] LoginParams loginParams, CancellationToken cancellationToken)
    {
        var userInfo = await repository.GetAsync(loginParams.Login, cancellationToken);

        if (userInfo is null || !userInfo.PasswordHash.Equals(loginParams.Password.ComputeSha256Hash()))
            throw new ArgumentException("Некорректный логин или пароль!");

        var token = jwtHelper.GenerateJwtToken(userInfo.Login);

        return new LoginResponse
        {
            Login = userInfo.Login,
            Token = token
        };
    }
    
    [HttpGet("auth")]
    public async Task<LoginResponse> Auth([FromHeader(Name = "x-user-token")][Required] string token, CancellationToken cancellationToken)
    {
        var userInfo = await CheckToken(token, cancellationToken);
        
        var updateToken = jwtHelper.GenerateJwtToken(userInfo.Login);

        return new LoginResponse
        {
            Login = userInfo.Login,
            Token = updateToken
        };
    }
    
    private async Task<UserInfo> CheckToken(string? token, CancellationToken cancellationToken)
    {
        if (token == null)
            throw new ArgumentException("Отсуствует токен.");

        var login = jwtHelper.GetLoginFromToken(token);

        if (login == null)
            throw new ArgumentException("Некорректный логин или пароль!");

        var userInfo = await repository.GetAsync(login, cancellationToken);

        if (userInfo is null)
            throw new ArgumentException("Некорректный логин или пароль!");

        return userInfo;
    }
}
