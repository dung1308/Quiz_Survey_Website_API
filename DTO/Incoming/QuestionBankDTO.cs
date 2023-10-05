using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Models;

namespace survey_quiz_app.DTO.Incoming;

public class QuestionBankDTO
{
    public int Id { get; set; }
    public string SurveyCode { get; set; } = string.Empty;
    public string? SurveyName { get; set; }
    public string? Owner { get; set; }
    public string? Timer { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Status { get; set; }
    public Boolean EnableStatus { get; set; }
    public int? CategoryListId { get; set; }
    public string? CategoryName { get; set; }
    public string? DateTimeNow { get; set; }
    public int? UserId { get; set; }

    public ICollection<QuestionDTO>? QuestionDTOs { get; set; }

}