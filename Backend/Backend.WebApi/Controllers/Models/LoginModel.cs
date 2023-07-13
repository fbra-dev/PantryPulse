using System.ComponentModel.DataAnnotations;

namespace Backend.WebApi.Controllers.Models;

public class LoginModel
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}
