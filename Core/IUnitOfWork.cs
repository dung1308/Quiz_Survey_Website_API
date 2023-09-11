using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IUnitOfWork
{
    IQuestionBankRepository QuestionBanks { get; }
    IQuestionRepository Questions { get; }
    IUserRepository Users { get; }
    ICategoryListRepository CategoryLists { get; }


    Task CompleteAsync();
}