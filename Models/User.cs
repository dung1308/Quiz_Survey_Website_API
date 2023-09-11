using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [StringLength(100)]
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? QuestionBankInteractId { get; set; }

    [ForeignKey("QuestionBankInteractId")]
    public QuestionBankInteract? QuestionBankInteract { get; set; }
    // public int? RoleId { get; set; }

    // [ForeignKey("RoleId")]
    public Role? Role { get; set; }

    public string? Permission => Role?.Permission;
    
}