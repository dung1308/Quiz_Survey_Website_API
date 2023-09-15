using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core.Repositories;

public class QuestionBankInteractRepository : GenericRepository<QuestionBankInteract, int>, IQuestionBankInteractRepository
{
    public QuestionBankInteractRepository(ApiDbContext context, ILogger logger) : base(context, logger)
    {

    }


    public override async Task<IEnumerable<QuestionBankInteract>> All()
    {
        try
        {
            return await _context.QuestionBankInteracts.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<QuestionBankInteract?> GetById(int id)
    {
        try
        {
            var rs = await _context.QuestionBankInteracts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return rs;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<QuestionBankInteract?>> GetAllByUserAndQuestionBank(int userId, int questionBankId)
    {
        var questionBankInteracts = await _context.QuestionBankInteracts.Where(q => q.UserId == userId && q.QuestionBankId == questionBankId).ToListAsync();
        return questionBankInteracts;
    }

    public async Task<IEnumerable<QuestionBankInteract?>> GetAllByUser(int userId)
    {
        var questionBankInteracts = await _context.QuestionBankInteracts.Where(q => q.UserId == userId).ToListAsync();
        return questionBankInteracts;
    }

    public async Task<IEnumerable<QuestionBankInteract?>> GetAllByQuestionBank(int questionBankId)
    {
        var questionBankInteracts = await _context.QuestionBankInteracts.Where(q => q.QuestionBankId == questionBankId).ToListAsync();
        return questionBankInteracts;
    }
}