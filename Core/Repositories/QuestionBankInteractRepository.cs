using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core.Repositories;

public class QuestionBankInteractRepository : GenericRepository<QuestionBankInteract, int>, IQuestionBankInteractRepository
{
    private readonly DbSet<QuestionBankInteract> questionBankInteracts;

    public QuestionBankInteractRepository(ApiDbContext context, ILogger logger) : base(context, logger)
    {
        questionBankInteracts = context.QuestionBankInteracts;
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

    public async Task<PaginationDTO<object>> GetQuestionBankInteractsWithPaginationAsync(string permission, int userId, int pageSize, int pageNumber)
    {
        var queryQI = questionBankInteracts.AsNoTracking().Where(x => x.UserId == userId).Select(x => new
        {
            x.Id,
            x.QuestionBankId,
            x.UserId,
            x.QuestionBank.SurveyName,
            UserName = x.User.UserName,
            x.ResultScores,
            OwnerName = x.QuestionBank.Owner
        });
        if (permission == "All")
        {
            queryQI = questionBankInteracts.AsNoTracking().Where(x => x.UserId == userId || x.QuestionBank.UserId == userId).Select(x => new
            {
                x.Id,
                x.QuestionBankId,
                x.UserId,
                x.QuestionBank.SurveyName,
                UserName = x.User.UserName,
                x.ResultScores,
                OwnerName = x.QuestionBank.Owner,
                // x.ResultShows
            });
        }
        var groupByQuery = queryQI.GroupBy(x => new
        {
            x.QuestionBankId,
            x.UserId,
            x.SurveyName,
            x.UserName,
            x.OwnerName,
        }
        , (key, values) => new
        {
            key.QuestionBankId,
            key.UserId,
            key.SurveyName,
            key.UserName,
            key.OwnerName,
            items = values.Select(x => new
            {
                x.Id,
                key.QuestionBankId,
                key.UserId,
                key.SurveyName,
                key.UserName,
                key.OwnerName,
                x.ResultScores,
            }).ToList()
        }
        );

        var records = await groupByQuery.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        groupByQuery = groupByQuery.OrderByDescending(x => x.QuestionBankId).Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        var data = await groupByQuery.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };

        return result;
    }

    public async Task<PaginationDTO<object>> GetQuestionBankInteractsForAdminWithPaginationAsync(int userId, int pageSize, int pageNumber)
    {
        var queryQI = questionBankInteracts.AsNoTracking().Where(x => x.UserId == userId || x.QuestionBank.UserId == userId).Select(x => new
        {
            x.Id,
            x.QuestionBankId,
            x.UserId,
            x.QuestionBank.SurveyName,
            UserName = x.User.UserName,
            x.ResultScores,
            OwnerName = x.QuestionBank.Owner,
            // x.ResultShows
        });

        var groupByQuery = queryQI.GroupBy(x => new
        {
            x.QuestionBankId,
            x.UserId,
            x.SurveyName,
            x.UserName,
            x.OwnerName,
        }, (key, values) => new
        {
            key.QuestionBankId,
            key.UserId,
            key.SurveyName,
            key.UserName,
            key.OwnerName,
            items = values.Select(x => new
            {
                x.Id,
                key.QuestionBankId,
                key.UserId,
                key.SurveyName,
                key.UserName,
                key.OwnerName,
                x.ResultScores,
            }).ToList()
        }
        );
        var records = await groupByQuery.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        groupByQuery = groupByQuery.OrderByDescending(x => x.QuestionBankId).Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        var data = await groupByQuery.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };

        return result;
    }
}