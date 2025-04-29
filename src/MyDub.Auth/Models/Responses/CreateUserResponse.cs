namespace MyDub.Auth.Models.Responses;

public class CreateUserResponse
{
    public required string Login { get; init; }
    public required string Token { get; init; }
}