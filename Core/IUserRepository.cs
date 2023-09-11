
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IUserRepository: IGenericRepository<User, int>
{
    Task<User?> LoginData(string userName);
}