using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.DTO.Incoming;

public class ResultShowDTO
{
    public double? OnAnswer { get; set; }
}