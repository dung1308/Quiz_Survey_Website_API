using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IQuestionRepository: IGenericRepository<Question, int>
{
    Task<Question?> GetByQuestionName(string QuestionName);
    Task<IEnumerable<Question?>> GetAllByQuestionBankId(int questionBankId);
}