using System.ComponentModel.DataAnnotations;

namespace MyDub.Auth.Models.Requests;

public class CreateUserRequest
{
    [Length(3, 100)]
    public required string Login { get; init; }
    
    [Length(3, 100)]
    public required string Password { get; init; }
}