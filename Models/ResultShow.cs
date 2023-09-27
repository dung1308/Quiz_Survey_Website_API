using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.Models;

public class ResultShow
{
    [Key]
    public int Id { get; set; }


    private IEnumerable<String>? _onAnswers;
    [StringLength(1000)]
    [JsonIgnore]
    public string OnAnswersString { get; set; } = string.Empty;
    [NotMapped]
    public IEnumerable<string> OnAnswers
    {
        get
        {
            _onAnswers ??= OnAnswersString.Split(";;", StringSplitOptions.RemoveEmptyEntries).ToList();
            return _onAnswers;
        }
        set
        {
            _onAnswers = value;
            OnAnswersString = string.Join(";;", _onAnswers);
        }
    }

    public double? ResultScore { get; set; }

    public int? QuestionId { get; set; } // Guid? QuestionBankId
    
    [ForeignKey("QuestionId")]
    public Question? Question { get; set; }

    public int? QuestionBankInteractId { get; set; } // Guid? QuestionBankId
    
    [ForeignKey("QuestionBankInteractId")]
    public QuestionBankInteract? QuestionBankInteract { get; set; }


}