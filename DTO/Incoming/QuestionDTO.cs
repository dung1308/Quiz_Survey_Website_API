using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.DTO.Incoming;

public class QuestionDTO
{
    public int Id { get; set; }
    public string QuestionName { get; set; } = string.Empty;
    public IEnumerable<string> Choices{ get; set; }
    public string? Type { get; set; }
    public IEnumerable<string> Answers{ get; set; } 
    public double Score { get; set; }
    public int? QuestionBankId { get; set; }

}