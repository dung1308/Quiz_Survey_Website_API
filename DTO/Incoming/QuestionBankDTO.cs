using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Models;

namespace survey_quiz_app.DTO.Incoming;

public class QuestionBankDTO
{
    public int Id { get; set; }
    public string SurveyCode { get; set; } = string.Empty;
    [StringLength(100)]
    public string? SurveyName { get; set; }
    [StringLength(100)]
    public string? Owner { get; set; }
    [StringLength(100)]
    public string? Category { get; set; }
    [StringLength(100)]
    public string? Timer { get; set; }
    [StringLength(100)]
    public string? StartDate { get; set; }
    [StringLength(100)]
    public string? EndDate { get; set; }
    [StringLength(100)]
    public string? Status { get; set; }
    public Boolean EnableStatus { get; set; }
    public int? CategoryListId { get; set; }

    public ICollection<QuestionDTO>? QuestionDTOs { get; set; }

}