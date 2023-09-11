using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.DTO.Outgoing;

public class QuestionDTOResponse
{
    public string QuestionName { get; set; } = string.Empty;

    public string ChoicesString { get; set; } = string.Empty;
    public string? Type { get; set; }

    public string AnswersString { get; set; } = string.Empty;
    public string OnAnswersString { get; set; } = string.Empty;

    public double Score { get; set; }
}