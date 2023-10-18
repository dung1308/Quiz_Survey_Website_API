using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace survey_quiz_app.DTO.Outgoing;

public class UserGetDTOResponse
{
    // public int Id { get; set; }
    public int Id { get; set; }
    public string? UserName { get; set; }
    public int? RoleId { get; set; }
    public Boolean IsNightMode { get; set; }
    // public string? Email { get; set; }

    // public Boolean IsNightMode { get; set; }
    // public string Token { get; set; } = string.Empty;
}