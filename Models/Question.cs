using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.Models;

public class Question
{
    [Key]
    public int Id { get; set; }
    [StringLength(100)]
    public string QuestionName { get; set; } = string.Empty;

    private IEnumerable<String>? _choices;
    [StringLength(500)]
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

    [StringLength(100)]
    public string? Type { get; set; }

    [StringLength(500)]
    [JsonIgnore]
    public string AnswersString { get; set; } = string.Empty;
    [NotMapped]
    public IEnumerable<string> Answers
    {
        get
        {
            _choices ??= AnswersString.Split(";;", StringSplitOptions.RemoveEmptyEntries).ToList();
            return _choices;
        }
        set
        {
            _choices = value;
            AnswersString = string.Join(";;", _choices);
        }
    }

    [StringLength(500)]
    [JsonIgnore]
    public string OnAnswersString { get; set; } = string.Empty;
    [NotMapped]
    public IEnumerable<string> OnAnswers
    {
        get
        {
            _choices ??= OnAnswersString.Split(";;", StringSplitOptions.RemoveEmptyEntries).ToList();
            return _choices;
        }
        set
        {
            _choices = value;
            OnAnswersString = string.Join(";;", _choices);
        }
    }

    public double Score { get; set; }
    // public Guid? QuestionBankId { get; set; }
    // [ForeignKey("QuestionBankId")]
    public QuestionBank? QuestionBank { get; set; }
}