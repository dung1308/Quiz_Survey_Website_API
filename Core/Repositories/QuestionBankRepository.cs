using System.Text;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core.Repositories;

public class QuestionBankRepository : GenericRepository<QuestionBank, int>, IQuestionBankRepository //Guid
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

    public override async Task<QuestionBank?> GetById(int id) //Guid id
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

    public async Task<List<QuestionBank>?> GetByUser(List<int?>? userIdList)
    {
        var questionBanks = await _context.QuestionBanks.Where(q => userIdList.Contains(q.Id)).ToListAsync();
        return questionBanks;
    }

    public async Task<List<QuestionBank>?> GetByUserAndCategory(List<int?>? userIdList, int categoryId)
    {
        var questionBanks = await _context.QuestionBanks.Where(q => userIdList.Contains(q.Id) && q.CategoryListId == categoryId).ToListAsync();
        return questionBanks;
    }

    public string? GenerateRandomString(string[] excludedStrings, int length)
    {
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var result = new StringBuilder();
        var random = new Random();
        for (var i = 0; i < length; i++)
        {
            char c;
            do
            {
                c = characters[random.Next(characters.Length)];
            } while (excludedStrings.Contains(result.ToString() + c));
            result.Append(c);
        }
        if (excludedStrings.Contains(result.ToString()))
            return GenerateRandomString(excludedStrings, length + 1);

        return result.ToString();
    }

    public async Task<QuestionBank?> GetByUserName(string userName)
    {
        var result = await _context.QuestionBanks.Where(r => r.Owner == userName).FirstOrDefaultAsync();
        return result;
    }
}