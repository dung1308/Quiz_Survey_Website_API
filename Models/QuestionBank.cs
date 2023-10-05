using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace survey_quiz_app.Models;

[Index(nameof(SurveyCode), IsUnique = true)]
public class QuestionBank
{
    [Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //public Guid Id { get; set; }
    public int Id { get; set; }
    [StringLength(1000)]
    public string SurveyCode { get; set; } = string.Empty;
    [StringLength(1000)]
    public string? SurveyName { get; set; }
    [StringLength(1000)]
    public string? Owner { get; set; }
    [StringLength(1000)]
    public string? Timer { get; set; }
    [StringLength(1000)]
    public string? StartDate { get; set; }
    [StringLength(1000)]
    public string? EndDate { get; set; }
    [StringLength(1000)]
    public string? Status { get; set; }
    public Boolean EnableStatus { get; set; }
    public ICollection<Question>? Questions { get; set; }
    public int? CategoryListId { get; set; }
    public string? CategoryName { get; set; }
    public string? DateTimeNow { get; set; }
    public int? UserId { get; set; }

    [ForeignKey("CategoryListId")]
    public CategoryList? CategoryList { get; set; }

    public ICollection<QuestionBankInteract>? QuestionBankInteract { get; set; }
}