using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.Models;

public class ResultShow
{
    [Key]
    public int Id { get; set; }
    public Question? Question { get; set; }
    public double? OnAnswer { get; set; }
}