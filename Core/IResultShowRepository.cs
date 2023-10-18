
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.DTO.Outgoing;
using survey_quiz_app.Models;

namespace survey_quiz_app.Core;

public interface IResultShowRepository: IGenericRepository<ResultShow, int>
{
    Task<IEnumerable<ResultShow?>> GetAllByQuestionBankInteract(int QuestionBankInteractId);
    Task<IEnumerable<ResultShow?>> GetAllByQuestion(int QuestionId);

    Task<IEnumerable<ResultShow?>> GetAllByQuestionAndQuestionBankInteract(int QuestionId, int QuestionBankInteractId);
    Task<AnswerReportDTOResponse<object>> GetAnswerReport(int QuestionBankInteractId);
    // Task<AnswerReportDTOResponse<object>> GetAnswerReportWithScoreAsync(int QuestionBankInteractId);
}