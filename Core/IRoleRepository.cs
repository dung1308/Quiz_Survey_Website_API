using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IRoleRepository: IGenericRepository<Role, int>
{
    Task<Role?> GetByRoleName(string RoleName);
}