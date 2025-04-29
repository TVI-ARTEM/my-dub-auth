using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDub.Auth.Models;

[Table("users")]
public class UserInfo
{ 
    [Column("id")] public long Id { get; set; }
    [Column("login"), Length(3, 100)] public string Login { get; set; } = null!;
    [Column("password_hash"), Length(3, 100)] public string PasswordHash { get; set; } = null!;
    [Column("created_at")] public DateTime CreatedAt { get; set; }
}