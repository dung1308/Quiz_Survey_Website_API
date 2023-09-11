using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace survey_quiz_app.DTO.Outgoing;

public class RoleDTOResponse
{
    public string RoleName { get; set; } = string.Empty; // Role of the user
    public string Permission { get; set; } = string.Empty; // A permission which permits how far the user can access
}