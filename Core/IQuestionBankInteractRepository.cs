using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IQuestionBankInteractRepository: IGenericRepository<QuestionBankInteract, int>
{
    Task<IEnumerable<QuestionBankInteract?>> GetAllByUser(int userId);
    Task<IEnumerable<QuestionBankInteract?>> GetAllByQuestionBank(int questionBankId);
    Task<IEnumerable<QuestionBankInteract?>> GetAllByUserAndQuestionBank(int userId, int questionBankId);
    Task<PaginationDTO<object>> GetQuestionBankInteractsWithPaginationAsync(string permission, int userId, int pageSize, int pageNumber);
    Task<PaginationDTO<object>> GetQuestionBankInteractsForAdminWithPaginationAsync(int userId, int pageSize, int pageNumber);
}
