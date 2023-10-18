using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;
using survey_quiz_app.util;

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
        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        var scoreQI = _context.Questions.AsNoTracking();
        var queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId && x.ResultShows.Any())).Select(x => new
        {
            x.Id,
            x.QuestionBankId,
            x.UserId,
            x.QuestionBank.SurveyName,
            UserName = x.User.UserName,
            x.ResultScores,
            // EndDate = x.QuestionBank.EndDate,
            DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
            OwnerName = x.QuestionBank.Owner
        });
        if (permission == "All")
        {
            queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId || x.QuestionBank.UserId == userId) && x.ResultShows.Any())
            .Select(x => new
            {
                x.Id,
                x.QuestionBankId,
                x.UserId,
                x.QuestionBank.SurveyName,
                UserName = x.User.UserName,
                x.ResultScores,
                // EndDate = x.QuestionBank.EndDate,
                DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
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
                EndDate = _context.QuestionBanks.AsNoTracking().FirstOrDefault(z => z.Id == x.QuestionBankId).EndDate,
                x.DateTimeNow,
                // x.ResultScores,
                ResultScores = x.ResultScores != null && scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) != 0 ?
                Math.Round((double)(x.ResultScores / scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) * 10), 2) : (double)0,
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

    public async Task<PaginationDTO<object>> GetQuestionBankInteractsWithPaginationAscOrDesAsync(string permission, int userId, int pageSize, int pageNumber, string filterAsc)
    {
        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        var scoreQI = _context.Questions.AsNoTracking();
        var queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId && x.ResultShows.Any())).Select(x => new
        {
            x.Id,
            x.QuestionBankId,
            x.UserId,
            x.QuestionBank.SurveyName,
            UserName = x.User.UserName,
            x.ResultScores,
            // EndDate = x.QuestionBank.EndDate,
            DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
            OwnerName = x.QuestionBank.Owner
        });
        if (permission == "All")
        {
            queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId || x.QuestionBank.UserId == userId) && x.ResultShows.Any())
            .Select(x => new
            {
                x.Id,
                x.QuestionBankId,
                x.UserId,
                x.QuestionBank.SurveyName,
                UserName = x.User.UserName,
                x.ResultScores,
                // EndDate = x.QuestionBank.EndDate,
                DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
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
                EndDate = _context.QuestionBanks.AsNoTracking().FirstOrDefault(z => z.Id == x.QuestionBankId).EndDate,
                x.DateTimeNow,
                // x.ResultScores,
                ResultScores = x.ResultScores != null && scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) != 0 ?
                Math.Round((double)(x.ResultScores / scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) * 10), 2) : (double)0,
            }).ToList()
        }
        );

        var records = await groupByQuery.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (filterAsc == "Asc")
            groupByQuery = groupByQuery.OrderBy(x => x.QuestionBankId).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        else if (filterAsc == "Des")
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

    public async Task<PaginationDTO<object>> GetQuestionBankInteractsWithPaginationWithQuestionBankNameAsync(string permission, int userId, int pageSize, int pageNumber, string filterAsc, string questionBankName)
    {
        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        var scoreQI = _context.Questions.AsNoTracking();
        var queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId && x.ResultShows.Any())).Select(x => new
        {
            x.Id,
            x.QuestionBankId,
            x.UserId,
            x.QuestionBank.SurveyName,
            UserName = x.User.UserName,
            x.ResultScores,
            // EndDate = x.QuestionBank.EndDate,
            DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
            OwnerName = x.QuestionBank.Owner
        });
        if (permission == "All")
        {
            queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId || x.QuestionBank.UserId == userId) && x.ResultShows.Any())
            .Select(x => new
            {
                x.Id,
                x.QuestionBankId,
                x.UserId,
                x.QuestionBank.SurveyName,
                UserName = x.User.UserName,
                x.ResultScores,
                // EndDate = x.QuestionBank.EndDate,
                DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
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
                EndDate = _context.QuestionBanks.AsNoTracking().FirstOrDefault(z => z.Id == x.QuestionBankId).EndDate,
                x.DateTimeNow,
                // x.ResultScores,
                ResultScores = x.ResultScores != null && scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) != 0 ?
                Math.Round((double)(x.ResultScores / scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) * 10), 2) : (double)0,
            }).ToList()
        }
        );

        // var records = await groupByQuery.CountAsync();
        // if (pageSize == -1) pageSize = records;
        // var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (filterAsc == "Asc")
            groupByQuery = groupByQuery.OrderBy(x => x.QuestionBankId);
        else if (filterAsc == "Des")
            groupByQuery = groupByQuery.OrderByDescending(x => x.QuestionBankId);
        // groupByQuery = groupByQuery.Where(x => x.SurveyName == questionBankName);
        // groupByQuery = groupByQuery.Where(x => CompareString.findSimilarity(x.SurveyName, questionBankName) > 0.7).OrderByDescending(x => CompareString.findSimilarity(x.SurveyName, questionBankName));
        // groupByQuery = groupByQuery.Where(x => SqlFunctions.StringSimilarity(x.SurveyName, questionBankName) > 0.7);

        // Error in EF.Functions.FreeText x.SurveyName is not Full Text
        // var newdata = groupByQuery.Where(x => EF.Functions.FreeText(x.SurveyName, questionBankName));
        // var records = newdata.Count();
        // if (pageSize == -1) pageSize = records;
        // var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));
        // var resultData = await newdata.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

        var newdata = groupByQuery.Where(x => x.SurveyName.Contains(questionBankName));
        var records = newdata.Count();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));
        var resultData = await newdata.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

        // var data = await groupByQuery.ToListAsync();
        // var newdata = data.Where(x => CompareString.findSimilarity(x.SurveyName, questionBankName) > 0.6).OrderByDescending(x => CompareString.findSimilarity(x.SurveyName, questionBankName));

        // var records = newdata.Count();
        // if (pageSize == -1) pageSize = records;
        // var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));
        // var resultData = newdata.Skip(pageSize * (pageNumber - 1)).Take(pageSize);


        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = resultData
        };

        return result;
    }

    public async Task<PaginationDTO<object>> GetQuestionBankInteractsWithPaginationWithUserNameAsync(string permission, int userId, int pageSize, int pageNumber, string filterAsc, string userName)
    {
        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        var scoreQI = _context.Questions.AsNoTracking();
        var queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId && x.ResultShows.Any())).Select(x => new
        {
            x.Id,
            x.QuestionBankId,
            x.UserId,
            x.QuestionBank.SurveyName,
            UserName = x.User.UserName,
            x.ResultScores,
            // EndDate = x.QuestionBank.EndDate,
            DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
            OwnerName = x.QuestionBank.Owner
        });
        if (permission == "All")
        {
            queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId || x.QuestionBank.UserId == userId) && x.ResultShows.Any())
            .Select(x => new
            {
                x.Id,
                x.QuestionBankId,
                x.UserId,
                x.QuestionBank.SurveyName,
                UserName = x.User.UserName,
                x.ResultScores,
                // EndDate = x.QuestionBank.EndDate,
                DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
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
                EndDate = _context.QuestionBanks.AsNoTracking().FirstOrDefault(z => z.Id == x.QuestionBankId).EndDate,
                x.DateTimeNow,
                // x.ResultScores,
                ResultScores = x.ResultScores != null && scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) != 0 ?
                Math.Round((double)(x.ResultScores / scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) * 10), 2) : (double)0,

            }).ToList()
        }
        );

        // var records = await groupByQuery.CountAsync();
        // if (pageSize == -1) pageSize = records;
        // var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (filterAsc == "Asc")
            groupByQuery = groupByQuery.OrderBy(x => x.QuestionBankId);
        else if (filterAsc == "Des")
            groupByQuery = groupByQuery.OrderByDescending(x => x.QuestionBankId);
        // groupByQuery = groupByQuery.Where(x => x.SurveyName == questionBankName);
        // groupByQuery = groupByQuery.Where(x => CompareString.findSimilarity(x.UserName, userName) > 0.6).OrderByDescending(x => CompareString.findSimilarity(x.UserName, userName));
        // groupByQuery = groupByQuery.Where(x => SqlFunctions.StringSimilarity(x.SurveyName, questionBankName) > 0.7);

        var newdata = groupByQuery.Where(x => x.UserName.Contains(userName));
        var records = newdata.Count();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));
        var resultData = await newdata.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

        // var data = await groupByQuery.ToListAsync();
        // var newdata = data.Where(x => CompareString.findSimilarity(x.UserName, userName) > 0.6).OrderByDescending(x => CompareString.findSimilarity(x.UserName, userName));

        // var records = newdata.Count();
        // if (pageSize == -1) pageSize = records;
        // var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));
        // var resultData = newdata.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = resultData
        };

        return result;
    }

    public async Task<PaginationDTO<object>> GetQuestionBankInteractsForAdminWithPaginationAsync(int userId, int pageSize, int pageNumber)
    {
        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        var scoreQI = _context.Questions.AsNoTracking();
        var queryQI = questionBankInteracts.AsNoTracking().Where(x => x.UserId == userId || x.QuestionBank.UserId == userId).Select(x => new
        {
            x.Id,
            x.QuestionBankId,
            x.UserId,
            x.QuestionBank.SurveyName,
            UserName = x.User.UserName,
            x.ResultScores,
            // EndDate = x.QuestionBank.EndDate,
            DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
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
                EndDate = _context.QuestionBanks.AsNoTracking().FirstOrDefault(z => z.Id == x.QuestionBankId).EndDate,
                x.DateTimeNow,
                // x.ResultScores,
                ResultScores = x.ResultScores != null && scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) != 0 ?
                Math.Round((double)(x.ResultScores / scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) * 10), 2) : (double)0,

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

    public async Task<PaginationDTO<object>> GetReportsSummaryWithPaginationAsync(string permission, int userId, int pageSize, int pageNumber, string filterAsc)
    {
        var queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId && x.ResultShows.Any())).Select(x => new
        {
            x.Id,
            x.QuestionBankId,
            x.UserId,
            x.QuestionBank.SurveyName,
            UserName = x.User.UserName,
            x.ResultScores,
            NumOfParticipant = x.QuestionBank.ParticipantIdList != null ? x.QuestionBank.ParticipantIdList.Count() : 0,
            OwnerName = x.QuestionBank.Owner
        });
        if (permission == "All")
        {

            queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId || x.QuestionBank.UserId == userId) && x.ResultShows.Any())
            .Select(x => new
            {
                x.Id,
                x.QuestionBankId,
                x.UserId,
                x.QuestionBank.SurveyName,
                UserName = x.User.UserName,
                x.ResultScores,
                NumOfParticipant = x.QuestionBank.ParticipantIdList != null ? x.QuestionBank.ParticipantIdList.Count() : 0,
                OwnerName = x.QuestionBank.Owner,
                // x.ResultShows
            });
        }
        var scoreQI = _context.Questions.AsNoTracking();
        var groupByQuery = queryQI.GroupBy(x => new
        {
            x.QuestionBankId,
            // x.UserId,
            x.SurveyName,
            // x.UserName,
            x.OwnerName,
            // x.NumOfParticipant,
        }
        , (key, values) => new
        {
            key.QuestionBankId,
            // key.UserId,
            key.SurveyName,
            // key.UserName,
            key.OwnerName,
            // key.NumOfParticipant,
            NumOfParticipant = values.Where(x => x.UserName != x.OwnerName).Select(x => x.UserId).Distinct().Count(),
            // ResultScoresGreaterThanValue = values.Where(x => x.ResultScores > 0).Sum(x => x.ResultScores),
            NumOfParticipantPassed = values.Where(x => x.ResultScores >
                                                        ((scoreQI.Where(y => y.QuestionBankId == key.QuestionBankId).Sum(x => x.Score))
                                                        / (scoreQI.Where(y => y.QuestionBankId == key.QuestionBankId).Count()))
            && x.UserName != x.OwnerName).Select(x => x.UserId).Distinct().Count()
            // items = values.Select(x => new
            // {
            //     // x.Id,
            //     key.QuestionBankId,
            //     //key.UserId,
            //     key.SurveyName,
            //     // key.UserName,
            //     key.OwnerName,
            //     x.NumOfParticipant,
            //     // x.ResultScores,
            // }
            // ).ToList()
        }
        );

        var records = await groupByQuery.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (filterAsc == "Asc")
            groupByQuery = groupByQuery.OrderBy(x => x.QuestionBankId).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        else if (filterAsc == "Des")
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

    public async Task<PaginationDTO<object>> GetSurveysWithPaginationAscOrDesAsync(int userId, int pageSize, int pageNumber, string filterAsc)
    {
        var questionBanksQI = _context.QuestionBanks.AsNoTracking();
        var questionBankInteractsQI = _context.QuestionBankInteracts.AsNoTracking().Where(q => q.UserId == userId);
        var questionBankIds = questionBankInteractsQI.Select(q => q.QuestionBankId).Distinct().ToList();
        var questionBanks = _context.QuestionBanks.Where(q => questionBankIds.Contains(q.Id));

        var records = await questionBanks.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (filterAsc == "Asc")
            questionBanks = questionBanks.OrderBy(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        else if (filterAsc == "Des")
            questionBanks = questionBanks.OrderByDescending(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        var data = await questionBanks.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };
        return result;

        // throw new NotImplementedException();
    }

    public async Task<PaginationDTO<object>> GetSurveyWithPaginationWithCategoryAsync(int userId, int pageSize, int pageNumber, string filterAsc, int categoryId)
    {
        var questionBanksQI = _context.QuestionBanks.AsNoTracking();
        var questionBankInteractsQI = _context.QuestionBankInteracts.AsNoTracking().Where(q => q.UserId == userId);
        var questionBankIds = questionBankInteractsQI.Select(q => q.QuestionBankId).Distinct().ToList();
        var questionBanks = _context.QuestionBanks.Where(q => questionBankIds.Contains(q.Id) && q.UserId == userId && q.CategoryListId == categoryId);

        var records = await questionBanks.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (filterAsc == "Asc")
            questionBanks = questionBanks.OrderBy(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        else if (filterAsc == "Des")
            questionBanks = questionBanks.OrderByDescending(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        var data = await questionBanks.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };
        return result;
    }

    public async Task<PaginationDTO<object>> GetSurveyWithPaginationWithOwnerAsync(int userId, int pageSize, int pageNumber, string filterAsc, string owner)
    {
        var questionBanksQI = _context.QuestionBanks.AsNoTracking();
        var questionBankInteractsQI = _context.QuestionBankInteracts.AsNoTracking().Where(q => q.UserId == userId);
        var questionBankIds = questionBankInteractsQI.Select(q => q.QuestionBankId).Distinct().ToList();
        var questionBanks = _context.QuestionBanks.Where(q => questionBankIds.Contains(q.Id) && q.Owner.Contains(owner));

        var records = await questionBanks.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (filterAsc == "Asc")
            questionBanks = questionBanks.OrderBy(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        else if (filterAsc == "Des")
            questionBanks = questionBanks.OrderByDescending(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        var data = await questionBanks.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };
        return result;
    }

    public async Task<PaginationDTO<object>> GetSurveyWithPaginationWithSurveyNameAsync(int userId, int pageSize, int pageNumber, string filterAsc, string surveyName)
    {
        var questionBanksQI = _context.QuestionBanks.AsNoTracking();
        var questionBankInteractsQI = _context.QuestionBankInteracts.AsNoTracking().Where(q => q.UserId == userId);
        var questionBankIds = questionBankInteractsQI.Select(q => q.QuestionBankId).Distinct().ToList();
        var questionBanks = _context.QuestionBanks.Where(q => questionBankIds.Contains(q.Id) && q.SurveyName.Contains(surveyName));

        var records = await questionBanks.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (filterAsc == "Asc")
            questionBanks = questionBanks.OrderBy(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        else if (filterAsc == "Des")
            questionBanks = questionBanks.OrderByDescending(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        var data = await questionBanks.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };
        return result;
    }

    public async Task<PaginationDTO<object>> GetSurveyWithPaginationByExpiredDateAsync(int userId, int pageSize, int pageNumber, string ascForDate)
    {
        var questionBanksQI = _context.QuestionBanks.AsNoTracking();
        var questionBankInteractsQI = _context.QuestionBankInteracts.AsNoTracking().Where(q => q.UserId == userId);
        var questionBankIds = questionBankInteractsQI.Select(q => q.QuestionBankId).Distinct().ToList();
        var questionBanks = _context.QuestionBanks.Where(q => questionBankIds.Contains(q.Id));

        var records = await questionBanks.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (ascForDate == "Asc")
            questionBanks = questionBanks.OrderBy(x => x.EndDate);
        // questionBanks = questionBanks.OrderBy(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        else if (ascForDate == "Des")
            // questionBanks = questionBanks.OrderByDescending(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            questionBanks = questionBanks.OrderByDescending(x => x.EndDate);
        questionBanks = questionBanks.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        var data = await questionBanks.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };
        return result;
    }

    public async Task<PaginationDTO<object>> GetReportsFilteredAsync(int userId, int pageSize, int pageNumber, bool ascOrNot, string ownerName, string surveyName, string userName)
    {
        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        var scoreQI = _context.Questions.AsNoTracking();
        var questionBankQI = _context.QuestionBanks.AsNoTracking();
        var queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId && x.ResultShows.Any())).Select(x => new
        {
            x.Id,
            x.QuestionBankId,
            x.UserId,
            x.QuestionBank.SurveyName,
            UserName = x.User.UserName,
            x.ResultScores,
            // EndDate = x.QuestionBank.EndDate,
            DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
            OwnerName = x.QuestionBank.Owner
        });
        var foundUser = _context.Users.AsNoTracking().FirstOrDefault(x => x.Id == userId);
        var permission = _context.Roles.AsNoTracking().FirstOrDefault(x => x.Id == foundUser.RoleId).Permission;
        if (permission == "All")
        {
            queryQI = questionBankInteracts.AsNoTracking().Where(x => (x.UserId == userId || x.QuestionBank.UserId == userId) && x.ResultShows.Any())
            .Select(x => new
            {
                x.Id,
                x.QuestionBankId,
                x.UserId,
                x.QuestionBank.SurveyName,
                UserName = x.User.UserName,
                x.ResultScores,
                // EndDate = x.QuestionBank.EndDate,
                DateTimeNow = resultTime.ToString("MM-dd-yyyyTHH:mm"),
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
                OwnerId = questionBankQI.FirstOrDefault(z => z.Id == x.QuestionBankId).UserId,
                EndDate = questionBankQI.FirstOrDefault(z => z.Id == x.QuestionBankId).EndDate,
                x.DateTimeNow,
                // x.ResultScores,
                ResultScores = x.ResultScores != null && scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) != 0 ?
                Math.Round((double)(x.ResultScores / scoreQI.Where(y => y.QuestionBankId == x.QuestionBankId).Sum(x => x.Score) * 10), 2) : (double)0,
            }).ToList()
        }
        );

        if (!String.IsNullOrWhiteSpace(ownerName))
        {
            groupByQuery = groupByQuery.Where(x => x.OwnerName.Contains(ownerName));
        }
        if (!String.IsNullOrWhiteSpace(surveyName))
        {
            groupByQuery = groupByQuery.Where(x => x.SurveyName.Contains(surveyName));
        }
        if (!String.IsNullOrWhiteSpace(userName))
        {
            groupByQuery = groupByQuery.Where(x => x.UserName.Contains(userName));
        }

        var records = await groupByQuery.CountAsync();
        if (pageSize == -1) pageSize = records;
        var pages = Convert.ToInt32(Math.Ceiling(records * 1.0 / pageSize));

        if (!String.IsNullOrWhiteSpace(ownerName))
        {
            var ownerNameWords = ownerName.Split(' ');
            if (ascOrNot == true)
                groupByQuery = groupByQuery.OrderBy(x => x.OwnerName == ownerName).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
                // groupByQuery = groupByQuery.OrderBy(x => ownerNameWords.Any
                // (n => !string.IsNullOrWhiteSpace(n) && StringUtils.JaccardSimilarity(n, x.OwnerName) 
                // >= 0.7))
                // .Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            else if (ascOrNot == false)
                groupByQuery = groupByQuery.OrderByDescending(x => x.OwnerName == ownerName).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }
        if (!String.IsNullOrWhiteSpace(surveyName))
        {
            if (ascOrNot == true)
                groupByQuery = groupByQuery.OrderBy(x => x.SurveyName == surveyName).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            else if (ascOrNot == false)
                groupByQuery = groupByQuery.OrderByDescending(x => x.SurveyName == surveyName).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }
        if (!String.IsNullOrWhiteSpace(userName))
        {
            if (ascOrNot == true)
                groupByQuery = groupByQuery.OrderBy(x => x.UserName == userName).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            else if (ascOrNot == false)
                groupByQuery = groupByQuery.OrderByDescending(x => x.UserName == userName).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }
        if (String.IsNullOrWhiteSpace(ownerName) && String.IsNullOrWhiteSpace(surveyName) && String.IsNullOrWhiteSpace(userName))
        {
            if (ascOrNot == true)
                groupByQuery = groupByQuery.OrderBy(x => x.QuestionBankId).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            else if (ascOrNot == false)
                groupByQuery = groupByQuery.OrderByDescending(x => x.QuestionBankId).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }

        var data = await groupByQuery.ToListAsync();

        var result = new PaginationDTO<object>
        {
            Pages = pages,
            NumOfItems = records,
            Data = data
        };

        return result;
    }

    public async Task<QuestionBankInteract?> RemoveInteractAndAllowJoining(int userId, QuestionBankInteractDTO interact)
    {
        var queryQI = await _context.QuestionBankInteracts.Where(x => x.Id == interact.Id).FirstOrDefaultAsync();
        var questionBankQI = _context.QuestionBanks.AsNoTracking();
        var foundQuestionBank = questionBankQI.Where(x => x.Id == interact.QuestionBankId).FirstOrDefault();
        if (foundQuestionBank == null) return null;
        var foundUserId = foundQuestionBank.UserId;
        if (userId != foundUserId) return null;
        return queryQI;
    }
}