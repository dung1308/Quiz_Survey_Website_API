using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IQuestionBankRepository : IGenericRepository<QuestionBank, int>
{
    Task<QuestionBank?> GetByQuestionBankName(string QuestionBankName);
    Task<QuestionBank?> GetByUserName(string userName);
    Task<List<QuestionBank>?> GetByUser(List<int?>? userIdList);
    Task<List<QuestionBank>?> GetByUserAndCategory(List<int?>? userIdList, int categoryId);
    string? GenerateRandomString(string[] excludedStrings, int length);
}