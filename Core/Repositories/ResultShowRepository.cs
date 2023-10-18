using AutoMapper;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.DTO.Outgoing;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core.Repositories;


public class ResultShowRepository : GenericRepository<ResultShow, int>, IResultShowRepository
{

    public ResultShowRepository(ApiDbContext context, ILogger logger) : base(context, logger)
    {
    }




    public override async Task<IEnumerable<ResultShow>> All()
    {
        try
        {
            return await _context.ResultShows.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<ResultShow?> GetById(int id)
    {
        try
        {
            var rs = await _context.ResultShows.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return rs;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<ResultShow?>> GetAllByQuestionBankInteract(int QuestionBankInteractId)
    {
        var resultShows = await _context.ResultShows.Where(q => q.QuestionBankInteractId == QuestionBankInteractId).ToListAsync();
        return resultShows;
    }

    public async Task<IEnumerable<ResultShow?>> GetAllByQuestion(int QuestionId)
    {
        var resultShows = await _context.ResultShows.Where(q => q.QuestionId == QuestionId).ToListAsync();
        return resultShows;
    }

    public async Task<IEnumerable<ResultShow?>> GetAllByQuestionAndQuestionBankInteract(int QuestionId, int QuestionBankInteractId)
    {
        var resultShows = await _context.ResultShows.Where(q => q.QuestionId == QuestionId && q.QuestionBankInteractId == QuestionBankInteractId).ToListAsync();
        return resultShows;
    }

    public Task<AnswerReportDTOResponse<object>> GetAnswerReport(int QuestionBankInteractId)
    {
        var queryQI = _context.ResultShows.AsNoTracking().Where(x => x.QuestionBankInteractId == QuestionBankInteractId).Select(x => new
        {
            Id = x.Id,
            QuestionName = x.Question.QuestionName ?? "Unknown",
            Result = x.ResultScore,
            OnAnswers = x.OnAnswers,
            RightAnswers = x.Question.Answers,
        });

        var data = queryQI;

        var result = new AnswerReportDTOResponse<object>
        {
            Data = data != null ? data : new List<object>()
        };


        return Task.FromResult(result);
    }

    // public Task<AnswerReportDTOResponse<object>> GetAnswerReportWithScoreAsync(int QuestionBankInteractId)
    // {
    //     var scoreQI = _context.Questions.AsNoTracking();
    //     var queryQI = _context.ResultShows.AsNoTracking().Where(x => x.QuestionBankInteractId == QuestionBankInteractId).Select(x => new
    //     {
    //         Id = x.Id,
    //         QuestionName = x.Question.QuestionName ?? "Unknown",
    //         Result = x.ResultScore,
    //         OnAnswers = x.OnAnswers,
    //         RightAnswers = x.Question.Answers,
    //     });
    //     throw new NotImplementedException();
    // }

    // Task<AnswerReportDTOResponse<object>> IResultShowRepository.GetAnswerReport(int QuestionBankInteractId)
    // {
    //     var queryQI = _context.ResultShows.AsNoTracking().Where(x => x.QuestionBankInteractId == QuestionBankInteractId).Select(x => new
    //     {
    //         Id = x.Id,
    //         QuestionName = x.Question.QuestionName ?? "Unknown",
    //         Result = x.ResultScore,
    //         OnAnswers = x.OnAnswers,
    //         RightAnswers = x.Question.Answers,
    //     });

    //     var data = queryQI;

    //     var result = new AnswerReportDTOResponse<object>
    //     {
    //         Data = data != null ? data : new List<object>()
    //     };


    //     return result;
    // }
}