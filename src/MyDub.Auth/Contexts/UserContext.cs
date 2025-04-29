using Microsoft.EntityFrameworkCore;
using MyDub.Auth.Models;

namespace MyDub.Auth.Contexts;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{
    public DbSet<UserInfo> UserInfos { get; set; } = null!;
}