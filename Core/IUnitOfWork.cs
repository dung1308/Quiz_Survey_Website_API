using survey_quiz_app.Data;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IUnitOfWork
{
    ApiDbContext Context {get;set;}

    IQuestionBankRepository QuestionBanks { get; }
    IQuestionRepository Questions { get; }
    IUserRepository Users { get; }
    ICategoryListRepository CategoryLists { get; }
    IRoleRepository Roles { get; }
    IQuestionBankInteractRepository QuestionBankInteracts { get; }
    IResultShowRepository ResultShows { get; }
    Task CompleteAsync();
}