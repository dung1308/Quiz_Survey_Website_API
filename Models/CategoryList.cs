using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.Models;

public class CategoryList
{
    [Key]
    public int Id { get; set; }
    [StringLength(1000)]
    public string CategoryName { get; set; } = string.Empty;
    public ICollection<QuestionBank>? QuestionBanks { get; set; }
}