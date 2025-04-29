namespace MyDub.Auth.Models.Responses;

public class AuthResponse
{
    public required string Login { get; init; }
    public required string Token { get; init; }
}