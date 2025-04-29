using MyDub.Auth.Models;

namespace MyDub.Auth.Repositories;

public interface IUserRepository
{
    Task<UserInfo> CreateAsync(string login, string password, CancellationToken token);
    Task<UserInfo?> GetAsync(string login, CancellationToken token);
}