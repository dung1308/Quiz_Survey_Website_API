using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;
using survey_quiz_app.util;

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

    public async Task<PaginationDTO<object>> GetQuestionBankWithPaginationAscOrDesAsync(int userId, int pageSize, int pageNumber, string filterAsc)
    {
        var queryQI = _context.QuestionBanks.AsNoTracking().Where(x => x.UserId == userId);
        var records = await queryQI.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (filterAsc == "Asc")
            queryQI = queryQI.OrderBy(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        else if (filterAsc == "Des")
            queryQI = queryQI.OrderByDescending(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);


        var data = await queryQI.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };
        return result;
    }

    public async Task<PaginationDTO<object>> GetQuestionBankWithPaginationWithCategoryAsync(int userId, int pageSize, int pageNumber, string filterAsc, int categoryId)
    {
        var queryQI = _context.QuestionBanks.AsNoTracking().Where(x => x.UserId == userId && x.CategoryListId == categoryId);
        if (filterAsc == "Asc")
            queryQI = queryQI.OrderBy(x => x.Id);
        else if (filterAsc == "Des")
            queryQI = queryQI.OrderByDescending(x => x.Id);
        var records = await queryQI.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        var data = await queryQI.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };
        return result;
    }

    public async Task<PaginationDTO<object>> GetQuestionBankWithPaginationWithOwnerAsync(int userId, int pageSize, int pageNumber, string filterAsc, string owner)
    {
        var queryQI = _context.QuestionBanks.AsNoTracking().Where(x => x.UserId == userId);
        if (filterAsc == "Asc")
            queryQI = queryQI.OrderBy(x => x.Id);
        else if (filterAsc == "Des")
            queryQI = queryQI.OrderByDescending(x => x.Id);
        var data = await queryQI.ToListAsync();
        var newdata = data.Where(x => CompareString.findSimilarity(x.Owner, owner) > 0.6).OrderByDescending(x => CompareString.findSimilarity(x.Owner, owner));

        var records = newdata.Count();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));
        var resultData = newdata.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = resultData
        };

        return result;
    }

    public async Task<PaginationDTO<object>> GetQuestionBankWithPaginationWithSurveyNameAsync(int userId, int pageSize, int pageNumber, string filterAsc, string surveyName)
    {
        var queryQI = _context.QuestionBanks.AsNoTracking().Where(x => x.UserId == userId);
        if (filterAsc == "Asc")
            queryQI = queryQI.OrderBy(x => x.Id);
        else if (filterAsc == "Des")
            queryQI = queryQI.OrderByDescending(x => x.Id);
        var data = await queryQI.ToListAsync();
        var newdata = data.Where(x => CompareString.findSimilarity(x.SurveyName, surveyName) > 0.6).OrderByDescending(x => CompareString.findSimilarity(x.SurveyName, surveyName));

        var records = newdata.Count();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));
        var resultData = newdata.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = resultData
        };

        return result;
    }

    public async Task<PaginationDTO<object>> GetQuestionBankWithPaginationByExpiredDateAsync(int userId, int pageSize, int pageNumber, string ascForDate)
    {
        var queryQI = _context.QuestionBanks.AsNoTracking().Where(x => x.UserId == userId);
        if (ascForDate == "Asc")
            // queryQI = queryQI.OrderBy(x => DateTime.ParseExact(x.EndDate, "MM-dd-yyyyTHH:mm", CultureInfo.InvariantCulture));
            queryQI = queryQI.OrderBy(x => x.EndDate);
        else if (ascForDate == "Des")
            // queryQI = queryQI.OrderByDescending(x => DateTime.ParseExact(x.EndDate, "MM-dd-yyyyTHH:mm", CultureInfo.InvariantCulture));
            queryQI = queryQI.OrderByDescending(x => x.EndDate);
        queryQI = queryQI.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        var records = await queryQI.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        var data = await queryQI.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };
        return result;
    }

    public async Task<QuestionBank?> GetAndSetParticipantListAsync(UserDTO user, int questionBankId)
    {
        var queryQI = await _context.QuestionBanks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == questionBankId);
        if (queryQI.UserId == user.Id) return queryQI;
        var participantListId = queryQI.ParticipantIdList.ToList();
        participantListId.Add(user.Id);
        queryQI.ParticipantIdList = participantListId;
        queryQI.Status = "Busy";
        queryQI.EnableStatus = true;
        return queryQI;
    }

    public async Task<QuestionBank?> RemoveParticipantIdAsync(UserDTO user, int questionBankId)
    {
        var queryQI = await _context.QuestionBanks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == questionBankId);
        if (queryQI.UserId == user.Id) return queryQI;
        var participantListId = queryQI.ParticipantIdList.ToList();
        participantListId.Remove(user.Id);
        queryQI.ParticipantIdList = participantListId;
        queryQI.Status = "Busy";
        queryQI.EnableStatus = true;
        return queryQI;
    }
}