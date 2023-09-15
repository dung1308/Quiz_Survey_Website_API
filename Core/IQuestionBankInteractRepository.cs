using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IQuestionBankInteractRepository: IGenericRepository<QuestionBankInteract, int>
{
    Task<IEnumerable<QuestionBankInteract?>> GetAllByUser(int userId);
    Task<IEnumerable<QuestionBankInteract?>> GetAllByQuestionBank(int questionBankId);
    Task<IEnumerable<QuestionBankInteract?>> GetAllByUserAndQuestionBank(int userId, int questionBankId);
}