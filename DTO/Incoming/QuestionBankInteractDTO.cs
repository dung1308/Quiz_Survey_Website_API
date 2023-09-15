using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using survey_quiz_app.Models;


namespace survey_quiz_app.DTO.Incoming;

public class QuestionBankInteractDTO
{
    public int Id { get; set; }
    public double? ResultScores { get; set; }
    public int? UserId { get; set; }
    public int? QuestionBankId { get; set; }
    public ICollection<ResultShowDTO>? ResultShowDTOs { get; set; }
}