using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace survey_quiz_app.Models;

[Index(nameof(SurveyCode), IsUnique = true)]
public class QuestionBank
{
    [Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //public Guid Id { get; set; }
    public int Id { get; set; }
    [StringLength(1000)]
    public string SurveyCode { get; set; } = string.Empty;
    [StringLength(1000)]
    public string? SurveyName { get; set; }
    [StringLength(1000)]
    public string? Owner { get; set; }
    [StringLength(1000)]
    public string? Timer { get; set; }
    [StringLength(1000)]
    public string? StartDate { get; set; }
    [StringLength(1000)]
    public string? EndDate { get; set; }
    [StringLength(1000)]
    public string? Status { get; set; }
    public Boolean EnableStatus { get; set; }
    public ICollection<Question>? Questions { get; set; }
    public int? CategoryListId { get; set; }
    public string? CategoryName { get; set; }
    public string? DateTimeNow { get; set; }
    public int? UserId { get; set; }

    private IEnumerable<int>? _participantIdList;
    [JsonIgnore]
    public string ParticipantIdListString { get; set; } = string.Empty;
    [NotMapped]
    public IEnumerable<int>? ParticipantIdList
    {
        get
        {
            _participantIdList = ParticipantIdListString.Split(";;", StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
            return _participantIdList;
        }
        set
        {
            _participantIdList = value;
            ParticipantIdListString = string.Join(";;", _participantIdList);
        }
    }

    private IEnumerable<int>? _userDoneIdList;
    [JsonIgnore]
    public string UserDoneIdListString { get; set; } = string.Empty;
    [NotMapped]
    public IEnumerable<int>? UserDoneIdList
    {
        get
        {
            _userDoneIdList = UserDoneIdListString.Split(";;", StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
            return _userDoneIdList;
        }
        set
        {
            _userDoneIdList = value;
            UserDoneIdListString = string.Join(";;", _userDoneIdList);
        }
    }

    [ForeignKey("CategoryListId")]
    public CategoryList? CategoryList { get; set; }

    public ICollection<QuestionBankInteract>? QuestionBankInteract { get; set; }
}