using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace survey_quiz_app.DTO.Outgoing;

public class QuestionBankDTOResponse
{
    public string SurveyCode { get; set; } = string.Empty;
    public string? SurveyName { get; set; }
    public string? Owner { get; set; }
    public string? Timer { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Status { get; set; }
    public Boolean EnableStatus { get; set; }
    public string? CategoryName { get; set; }
    public string? DateTimeNow { get; set; }
    public int? UserId { get; set; }
    public IEnumerable<int>? ParticipantIdList { get; set; }
    public IEnumerable<int>? UserDoneIdList { get; set; }
}