using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.DTO.Incoming;

public class CategoryListDTO
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;

}