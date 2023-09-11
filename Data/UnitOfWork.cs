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

    public UnitOfWork(ApiDbContext context, ILoggerFactory loggerFactory)
    {
        _context = context;
        var _logger = loggerFactory.CreateLogger(categoryName: "logs");

        QuestionBanks = new QuestionBankRepository(_context, _logger);
        // Users = new UserRepository(_context, _logger); // Error ????
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