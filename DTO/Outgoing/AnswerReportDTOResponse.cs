using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.DTO.Outgoing;

public class AnswerReportDTOResponse<T> where T : class
{
    public IEnumerable<T> Data { get; set; } = new List<T>();

}