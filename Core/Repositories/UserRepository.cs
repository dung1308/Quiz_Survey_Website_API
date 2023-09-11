using AutoMapper;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core.Repositories;


public class UserRepository : GenericRepository<User, int>, IUserRepository
{

    public UserRepository(ApiDbContext context, ILogger logger) : base(context, logger)
    {
    }




    public override async Task<IEnumerable<User>> All()
    {
        try
        {
            return await _context.Users.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<User?> GetById(int id)
    {
        try
        {
            var rs = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return rs;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<User?> LoginData(string userName)
    {
        try
        {
            var rs = _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            return rs;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}