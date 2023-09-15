using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IQuestionBankRepository: IGenericRepository<QuestionBank, int>
{
    Task<QuestionBank?> GetByQuestionBankName(string QuestionBankName);
    Task<List<QuestionBank>?> GetByUser(List<int?>? userIdList);
    Task<List<QuestionBank>?> GetByUserAndCategory(List<int?>? userIdList, int categoryId);
}