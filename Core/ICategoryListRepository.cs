using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface ICategoryListRepository: IGenericRepository<CategoryList, int>
{
    Task<Question?> GetByCategoryName(string CategoryName);
}