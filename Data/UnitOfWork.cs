using survey_quiz_app.Core;
using survey_quiz_app.Core.Repositories;

namespace survey_quiz_app.Data;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApiDbContext _context;


    public IQuestionBankRepository QuestionBanks { get; private set; }

    public IQuestionRepository Questions { get; private set; }

    public IUserRepository Users { get; private set; }

    public ICategoryListRepository CategoryLists{ get; private set; }

    public IRoleRepository Roles{ get; private set; }

    public IQuestionBankInteractRepository QuestionBankInteracts { get; private set; }

    public IResultShowRepository ResultShows { get; private set; }

    public UnitOfWork(ApiDbContext context, ILoggerFactory loggerFactory)
    {
        _context = context;
        var _logger = loggerFactory.CreateLogger(categoryName: "logs");

        QuestionBanks = new QuestionBankRepository(_context, _logger);
        // Users = new UserRepository(_context, _logger); // Error ????
        Users = new UserRepository(_context, _logger);
        Questions = new QuestionRepository(_context, _logger);
        CategoryLists = new CategoryRepository(_context, _logger);
        Roles = new RoleRepository(_context, _logger);
        QuestionBankInteracts = new QuestionBankInteractRepository(_context, _logger);
        ResultShows = new ResultShowRepository(_context, _logger);
    }


    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}