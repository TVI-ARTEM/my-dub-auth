namespace MyDub.Auth.Models.Responses;

public class LoginResponse
{
    public required string Login { get; init; }
    public required string Token { get; init; }
}