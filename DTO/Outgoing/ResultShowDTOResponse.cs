using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.DTO.Outgoing;

public class ResultShowDTOResponse
{
    public IEnumerable<string> OnAnswers;

    public double? ResultScore { get; set; }
}