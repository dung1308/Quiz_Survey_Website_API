using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core.Repositories;

public class QuestionRepository : GenericRepository<Question, int>, IQuestionRepository
{
    public QuestionRepository(ApiDbContext context, ILogger logger) : base(context, logger)
    {

    }


    public override async Task<IEnumerable<Question>> All()
    {
        try
        {
            return await _context.Questions.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<Question?> GetById(int id)
    {
        try
        {
            var rs = await _context.Questions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return rs;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<Question?> GetByQuestionName(string QuestionName)
    {
        try
        {
            return _context.Questions.FirstOrDefaultAsync(x => x.QuestionName == QuestionName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Question?>> GetAllByQuestionBankId(int questionBankId)
    {
        var questions = await _context.Questions.Where(q => q.QuestionBankId == questionBankId).ToListAsync();
        return questions;
    }
}