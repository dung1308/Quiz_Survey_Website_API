using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.DTO.Incoming;

public class ResultShowDTO
{
    public int Id { get; set; }
    public IEnumerable<string> OnAnswers { get; set; }

    public double? ResultScore { get; set; }

    public int? QuestionId { get; set; }
    public int? QuestionBankInteractId { get; set; }

}