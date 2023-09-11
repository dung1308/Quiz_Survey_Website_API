using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core.Repositories;

public class QuestionBankRepository : GenericRepository<QuestionBank, Guid>, IQuestionBankRepository
{
    public QuestionBankRepository(ApiDbContext context, ILogger logger) : base(context, logger)
    {

    }

    public override async Task<IEnumerable<QuestionBank>> All()
    {
        try
        {
            return await _context.QuestionBanks.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<QuestionBank?> GetById(Guid id)
    {
        try
        {
            var rs = await _context.QuestionBanks.Include(x => x.Questions).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return rs;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<QuestionBank?> GetByQuestionBankName(string QuestionBankName)
    {
        try
        {
            return _context.QuestionBanks.FirstOrDefaultAsync(x => x.SurveyName == QuestionBankName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}