using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Core;
using survey_quiz_app.Data;
using survey_quiz_app.Models;

namespace survey_quiz_app.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
// [ApiVersion("1.0", Deprecated = true)] // Use for warning the version not supported much longer
public class QuestionBankController : BaseController
{

    public QuestionBankController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }
    [HttpGet("GetQuestionBank")]
    public async Task<IActionResult> Get()
    {
        if(await _unitOfWork.QuestionBanks.All() == null) return NotFound();
        return Ok(await _unitOfWork.QuestionBanks.All());
    }

    // Default Version (Version 1.0)

    [HttpGet]
    [Route("User/{userId}/GetQuestionBank/{questionBankId}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get(int userId, Guid questionBankId)
    {
        var user = await _unitOfWork.Users.GetById(userId);
        if (user == null || user.QuestionBankInteract.QuestionBanks == null) return NotFound();
        // var questionBank = await _unitOfWork.QuestionBanks.GetById(questionBankId);
        var questionBank = user.QuestionBankInteract.QuestionBanks.FirstOrDefault(x => x.Id == questionBankId);
        if (questionBank == null) return NotFound();
        return Ok(questionBank);
    }

    [HttpPost]
    [Route("AddQuestionBank")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> AddQuestionBank(QuestionBank questionBank)
    {
        await _unitOfWork.QuestionBanks.Add(questionBank);
        await _unitOfWork.CompleteAsync();
        return Ok("Question bank added successfully.");
        // Create a new QuestionBank object from the QuestionBankDTO
        // var questionBank = new QuestionBank
        // {
        //     SurveyCode = questionBankDTO.SurveyCode,
        //     SurveyName = questionBankDTO.SurveyName,
        //     Owner = questionBankDTO.Owner,
        //     Category = questionBankDTO.Category,
        //     Timer = questionBankDTO.Timer,
        //     StartDate = questionBankDTO.StartDate,
        //     EndDate = questionBankDTO.EndDate,
        //     Status = questionBankDTO.Status,
        //     EnableStatus = questionBankDTO.EnableStatus,
        //     ResultScores = questionBankDTO.ResultScores,
        //     Questions = questionBankDTO.Questions
        // };

        // // Add the new QuestionBank to the database
        // await _unitOfWork.QuestionBanks.Add(questionBank);
        // await _unitOfWork.CompleteAsync();

        // // Retrieve the generated ID from the database
        // var generatedId = questionBank.Id;

        // // Create a new QuestionBankDTO object with the generated ID and other properties
        // var newQuestionBankDTO = new CreateQuestionBankDTO
        // {
        //     Id = generatedId,
        //     SurveyCode = questionBank.SurveyCode,
        //     SurveyName = questionBank.SurveyName,
        //     Owner = questionBank.Owner,
        //     Category = questionBank.Category,
        //     Timer = questionBank.Timer,
        //     StartDate = questionBank.StartDate,
        //     EndDate = questionBank.EndDate,
        //     Status = questionBank.Status,
        //     EnableStatus = questionBank.EnableStatus,
        //     ResultScores = questionBank.ResultScores,
        //     Questions = questionBank.Questions
        // };

        // // Return the new QuestionBankDTO to the client
        // return Ok(newQuestionBankDTO);
    }

    [HttpDelete]
    [Route("DeleteQuestionBank")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteQuestionBank(Guid id)
    {
        var questionBank = await _unitOfWork.QuestionBanks.GetById(id);
        if(questionBank == null) return NotFound();
        await _unitOfWork.QuestionBanks.Delete(questionBank);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("UpdateQuestionBank")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateQuestionBank(QuestionBank questionBank)
    {
        var existedQuestionBank = await _unitOfWork.QuestionBanks.GetById(questionBank.Id);
        if(existedQuestionBank == null) return NotFound();
        await _unitOfWork.QuestionBanks.Update(questionBank);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
    
}