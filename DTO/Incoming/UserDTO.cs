using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace survey_quiz_app.DTO.Incoming;

public class UserDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Boolean IsNightMode { get; set; }
    // public string Token { get; set; } = string.Empty;
    public int? RoleId { get; set; }
}