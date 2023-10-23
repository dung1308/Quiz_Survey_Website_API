using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IQuestionBankRepository : IGenericRepository<QuestionBank, int>
{
    Task<QuestionBank?> GetByQuestionBankName(string QuestionBankName);
    Task<QuestionBank?> GetByUserName(string userName);
    Task<List<QuestionBank>?> GetByUser(List<int?>? userIdList);
    Task<List<QuestionBank>?> GetByUserAndCategory(List<int?>? userIdList, int categoryId);
    Task<PaginationDTO<object>> GetQuestionBankWithPaginationAscOrDesAsync(int userId, int pageSize, int pageNumber, string filterAsc);
    Task<PaginationDTO<object>> GetQuestionBankWithPaginationWithCategoryAsync(int userId, int pageSize, int pageNumber, string filterAsc, int categoryId);
    Task<PaginationDTO<object>> GetQuestionBankWithPaginationWithOwnerAsync(int userId, int pageSize, int pageNumber, string filterAsc, string owner);
    Task<PaginationDTO<object>> GetQuestionBankWithPaginationWithSurveyNameAsync(int userId, int pageSize, int pageNumber, string filterAsc, string surveyName);
    Task<PaginationDTO<object>> GetQuestionBankWithPaginationByExpiredDateAsync(int userId, int pageSize, int pageNumber, string ascForDate);
    Task<QuestionBank?> GetAndSetParticipantListAsync(UserDTO user, int questionBankId);
    Task<QuestionBank?> RemoveParticipantIdAsync(UserDTO user, int questionBankId);
    Task<QuestionBank?> RemoveUserDoneIdAsync(UserDTO user, int questionBankId);
    string? GenerateRandomString(string[] excludedStrings, int length);
}