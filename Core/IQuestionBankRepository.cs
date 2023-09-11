using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IQuestionBankRepository: IGenericRepository<QuestionBank, Guid>
{
    Task<QuestionBank?> GetByQuestionBankName(string QuestionBankName);
}