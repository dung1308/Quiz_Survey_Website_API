using survey_quiz_app.Core;
using survey_quiz_app.Core.Repositories;

namespace survey_quiz_app.Data;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public ApiDbContext Context {get;set;}


    public IQuestionBankRepository QuestionBanks { get; private set; }

    public IQuestionRepository Questions { get; private set; }

    public IUserRepository Users { get; private set; }

    public ICategoryListRepository CategoryLists{ get; private set; }

    public IRoleRepository Roles{ get; private set; }

    public IQuestionBankInteractRepository QuestionBankInteracts { get; private set; }

    public IResultShowRepository ResultShows { get; private set; }

    

    public UnitOfWork(ApiDbContext context, ILoggerFactory loggerFactory)
    {
        Context = context;
        var _logger = loggerFactory.CreateLogger(categoryName: "logs");

        QuestionBanks = new QuestionBankRepository(Context, _logger);
        // Users = new UserRepository(Context, _logger); // Error ????
        Users = new UserRepository(Context, _logger);
        Questions = new QuestionRepository(Context, _logger);
        CategoryLists = new CategoryRepository(Context, _logger);
        Roles = new RoleRepository(Context, _logger);
        QuestionBankInteracts = new QuestionBankInteractRepository(Context, _logger);
        ResultShows = new ResultShowRepository(Context, _logger);
        
    }


    public async Task CompleteAsync()
    {
        await Context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}