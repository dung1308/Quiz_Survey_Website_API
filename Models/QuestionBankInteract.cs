using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.Models;

public class QuestionBankInteract
{
    [Key]
    public int Id { get; set; }
    // public ICollection<QuestionBank>? QuestionBanks { get; set; }
    public double? ResultScores { get; set; }
    public int? UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
    public int? QuestionBankId { get; set; }
    [ForeignKey("QuestionBankId")]
    public QuestionBank? QuestionBank { get; set; }
    public ICollection<ResultShow>? ResultShows { get; set; }
}