using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Models.LoginModels;

public class User
{
    [Key]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    public string Salt { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExp { get; set; }
}