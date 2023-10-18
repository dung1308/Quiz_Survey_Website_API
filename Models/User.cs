using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace survey_quiz_app.Models;

[Index(nameof(UserName), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }
    [StringLength(1000)]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [StringLength(1000)]
    public string Password { get; set; } = string.Empty;
    [Required]
    [StringLength(1000)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    public Boolean IsNightMode { get; set; }
    // [StringLength(1000)]
    // public string Token { get; set; } = string.Empty;

    public int? RoleId { get; set; }

    // [ForeignKey("Id")]
    // public QuestionBankInteract? QuestionBankInteract { get; set; }

    public ICollection<QuestionBankInteract>? QuestionBankInteracts { get; set; }

    [ForeignKey("RoleId")]
    public Role? Role { get; set; }

    public string? Permission => Role?.Permission;

}