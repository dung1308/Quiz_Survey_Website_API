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
public class QuestionController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public QuestionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Route("User/{userId}/GetQuestionBank/{questionBankId}/Question")]
    public async Task<IActionResult> Get(int userId, Guid questionBankId)
    {
        var user = await _unitOfWork.Users.GetById(userId);
        // var questionBank = await _unitOfWork.QuestionBanks.GetById(questionBankId);
        if (user == null) return NotFound();
        if (user.QuestionBankInteract.QuestionBanks == null) return NotFound();
        var questionBank =  user.QuestionBankInteract.QuestionBanks.FirstOrDefault(x => x.Id == questionBankId);
        if (questionBank == null) return NotFound();
        var question = questionBank.Questions;
        if (question == null) return NotFound();
        return Ok(question);
    }

    // Default Version (Version 1.0)

    [HttpGet]
    [Route("User/{userId}/GetQuestionBank/{questionBankId}/Question/{questionId}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetById(int userId, int CategoryId, Guid questionBankId, int questionId)
    {
        var user = await _unitOfWork.Users.GetById(userId);
        if (user == null) return NotFound();
        if (user.QuestionBankInteract.QuestionBanks == null) return NotFound();
        // var questionBank = await _unitOfWork.QuestionBanks.GetById(questionBankId);
        if (user == null) return NotFound();
        var questionBank = user.QuestionBankInteract.QuestionBanks.FirstOrDefault(x => x.Id == questionBankId);
        if (questionBank == null) return NotFound();
        if (questionBank.Questions == null) return NotFound();
        var question = questionBank.Questions.FirstOrDefault(x => x.Id == questionId);
        if (question == null) return NotFound();
        return Ok(question);
    }

    
    [HttpPost]
    [Route("AddQuestion")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> AddQuestion(int userId, Guid questionBankId, Question question)
    {
        var user = await _unitOfWork.Users.GetById(userId);
        if (user == null || user.QuestionBankInteract.QuestionBanks == null) return NotFound();
        var questionBank = user.QuestionBankInteract.QuestionBanks.FirstOrDefault(x => x.Id == questionBankId);

        // Set the QuestionBankId property of the new question
        // question.QuestionBankId = questionBank.Id;

        if (questionBank == null || questionBank.Questions == null) return NotFound();

        // Add the new question to the question bank
        questionBank.Questions.Add(question);

        // Save the changes to the database
        await _unitOfWork.CompleteAsync();

        // Return a response indicating success
        return Ok("Question added successfully.");
    }
    

    [HttpDelete]
    [Route("DeleteQuestion")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteQuestion(int userId, Guid questionBankId,int questionId)
    {
        var user = await _unitOfWork.Users.GetById(userId);
        if (user == null || user.QuestionBankInteract.QuestionBanks == null) return NotFound();
        var questionBank = user.QuestionBankInteract.QuestionBanks.FirstOrDefault(x => x.Id == questionBankId);
        if (questionBank == null || questionBank.Questions == null) return NotFound();
        var question = questionBank.Questions.FirstOrDefault(x => x.Id == questionId);
        if (question == null) return NotFound();

        // Remove the question from the question bank
        questionBank.Questions.Remove(question);

        // Save the changes to the database
        await _unitOfWork.CompleteAsync();

        // Return a response indicating success
        return NoContent();
    }

    [HttpPut]
    [Route("UpdateQuestion")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateQuestion(int userId, Guid questionBankId, Question question)
    {
        var user = await _unitOfWork.Users.GetById(userId);
        if (user == null || user.QuestionBankInteract == null || user.QuestionBankInteract.QuestionBanks == null) return NotFound();
        var questionBank = user.QuestionBankInteract.QuestionBanks.FirstOrDefault(x => x.Id == questionBankId);
        if (questionBank == null || questionBank.Questions == null) return NotFound();
        if (question == null) return NotFound();
        var existedQuestion = await _unitOfWork.Questions.GetById(question.Id);
        if(existedQuestion == null) return NotFound();
        await _unitOfWork.Questions.Update(question);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

}