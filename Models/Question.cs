using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.Models;

public class Question
{
    [Key]
    public int Id { get; set; }
    [StringLength(1000)]
    public string QuestionName { get; set; } = string.Empty;

    private IEnumerable<String>? _choices;
    [StringLength(1000)]
    [JsonIgnore]
    public string ChoicesString { get; set; } = string.Empty;
    [NotMapped]
    public IEnumerable<string> Choices
    {
        get
        {
            _choices ??= ChoicesString.Split(";;", StringSplitOptions.RemoveEmptyEntries).ToList();
            return _choices;
        }
        set
        {
            _choices = value;
            ChoicesString = string.Join(";;", _choices);
        }
    }

    [StringLength(1000)]
    public string? Type { get; set; }

    private IEnumerable<String>? _answers;
    [StringLength(1000)]
    [JsonIgnore]
    public string AnswersString { get; set; } = string.Empty;
    [NotMapped]
    public IEnumerable<string> Answers
    {
        get
        {
            _answers ??= AnswersString.Split(";;", StringSplitOptions.RemoveEmptyEntries).ToList();
            return _answers;
        }
        set
        {
            _answers = value;
            AnswersString = string.Join(";;", _answers);
        }
    }

    public double Score { get; set; }
    public int? QuestionBankId { get; set; } // Guid? QuestionBankId
    
    [ForeignKey("QuestionBankId")]
    public QuestionBank? QuestionBank { get; set; }

    public ICollection<ResultShow>? ResultShows { get; set; }
}