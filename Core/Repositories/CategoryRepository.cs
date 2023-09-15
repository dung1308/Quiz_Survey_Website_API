using AutoMapper;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core.Repositories;


public class CategoryRepository : GenericRepository<CategoryList, int>, ICategoryListRepository
{

    public CategoryRepository(ApiDbContext context, ILogger logger) : base(context, logger)
    {
    }




    public override async Task<IEnumerable<CategoryList>> All()
    {
        try
        {
            return await _context.CategoryList.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<CategoryList?> GetByCategoryName(string CategoryName)
    {
        try
        {
            return _context.CategoryList.FirstOrDefaultAsync(x => x.CategoryName == CategoryName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<CategoryList?> GetById(int id)
    {
        try
        {
            var rs = await _context.CategoryList.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return rs;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}