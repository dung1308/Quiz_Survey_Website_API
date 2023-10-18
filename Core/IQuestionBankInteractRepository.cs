using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IQuestionBankInteractRepository : IGenericRepository<QuestionBankInteract, int>
{
    Task<IEnumerable<QuestionBankInteract?>> GetAllByUser(int userId);
    Task<IEnumerable<QuestionBankInteract?>> GetAllByQuestionBank(int questionBankId);
    Task<IEnumerable<QuestionBankInteract?>> GetAllByUserAndQuestionBank(int userId, int questionBankId);
    Task<QuestionBankInteract?> RemoveInteractAndAllowJoining(int userId, QuestionBankInteractDTO interact);
    Task<PaginationDTO<object>> GetSurveysWithPaginationAscOrDesAsync(int userId, int pageSize, int pageNumber, string filterAsc);
    Task<PaginationDTO<object>> GetSurveyWithPaginationWithCategoryAsync(int userId, int pageSize, int pageNumber, string filterAsc, int categoryId);
    Task<PaginationDTO<object>> GetSurveyWithPaginationWithOwnerAsync(int userId, int pageSize, int pageNumber, string filterAsc, string owner);
    Task<PaginationDTO<object>> GetSurveyWithPaginationWithSurveyNameAsync(int userId, int pageSize, int pageNumber, string filterAsc, string surveyName);
    Task<PaginationDTO<object>> GetSurveyWithPaginationByExpiredDateAsync(int userId, int pageSize, int pageNumber, string ascForDate);
    Task<PaginationDTO<object>> GetQuestionBankInteractsWithPaginationAsync(string permission, int userId, int pageSize, int pageNumber);
    Task<PaginationDTO<object>> GetQuestionBankInteractsWithPaginationAscOrDesAsync(string permission, int userId, int pageSize, int pageNumber, string filterAsc);
    Task<PaginationDTO<object>> GetQuestionBankInteractsWithPaginationWithQuestionBankNameAsync(string permission, int userId, int pageSize, int pageNumber, string filterAsc, string questionBankName);
    Task<PaginationDTO<object>> GetQuestionBankInteractsWithPaginationWithUserNameAsync(string permission, int userId, int pageSize, int pageNumber, string filterAsc, string userName);
    Task<PaginationDTO<object>> GetReportsSummaryWithPaginationAsync(string permission, int userId, int pageSize, int pageNumber, string filterAsc);
    Task<PaginationDTO<object>> GetQuestionBankInteractsForAdminWithPaginationAsync(int userId, int pageSize, int pageNumber);
    Task<PaginationDTO<object>> GetReportsFilteredAsync(int userId, int pageSize, int pageNumber, bool ascOrNot, string ownerName, string surveyName, string userName);
}
