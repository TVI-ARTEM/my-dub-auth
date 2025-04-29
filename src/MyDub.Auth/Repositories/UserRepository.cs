using Microsoft.EntityFrameworkCore;
using MyDub.Auth.Contexts;
using MyDub.Auth.Models;

namespace MyDub.Auth.Repositories;

public class UserRepository(UserContext context) : IUserRepository
{
    public async Task<UserInfo> CreateAsync(string login, string password, CancellationToken token)
    {
        var userInfo = await GetAsync(login, token);

        if (userInfo is not null)
            throw new ArgumentException("Пользователь с таким именем уже существует");

        var createdUser = (await context.UserInfos.AddAsync(new UserInfo
        {
            Login = login,
            PasswordHash = password,
            CreatedAt = DateTime.UtcNow
        }, token)).Entity;

        await context.SaveChangesAsync(token);

        return createdUser;
    }

    public Task<UserInfo?> GetAsync(string login, CancellationToken token) =>
        context.UserInfos.FirstOrDefaultAsync(x => x.Login == login, cancellationToken: token);
}