using AutoMapper;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core.Repositories;


public class RoleRepository : GenericRepository<Role, int>, IRoleRepository
{

    public RoleRepository(ApiDbContext context, ILogger logger) : base(context, logger)
    {
    }




    public override async Task<IEnumerable<Role>> All()
    {
        try
        {
            return await _context.Roles.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<Role?> GetById(int id)
    {
        try
        {
            var rs = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return rs;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public Task<Role?> GetByRoleName(string RoleName)
    {
        try
        {
            return _context.Roles.FirstOrDefaultAsync(x => x.RoleName == RoleName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}