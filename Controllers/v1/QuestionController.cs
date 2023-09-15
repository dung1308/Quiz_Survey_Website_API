using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Core;
using survey_quiz_app.Data;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;

namespace survey_quiz_app.Controllers.v1;

[ApiController]
// [Route("api/v{version:apiVersion}/[controller]")]
[Route("api/[controller]")]
[ApiVersion("1.0")]
// [ApiVersion("1.0", Deprecated = true)] // Use for warning the version not supported much longer
public class QuestionController : BaseController
{
    public QuestionController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }


    [HttpGet]
    [Route("/Questions")]
    public async Task<IActionResult> GetQuestions() //Guid questionBankId
    {
        if(await _unitOfWork.Questions.All() == null) return NotFound("Questions not found");
        var result = _mapper.Map<List<QuestionDTO>>(await _unitOfWork.Questions.All());
        return Ok(result);
    }


    [HttpGet]
    [Route("/GetQuestionBank/{questionBankId}/Question")]
    public async Task<IActionResult> GetQuestionByQuestionBankId(int questionBankId) //Guid questionBankId
    {
        var questions = await _unitOfWork.Questions.GetAllByQuestionBankId(questionBankId);
        if (questions == null) return NotFound("There is no questions existed with this QuestionBank Id");
        var result = _mapper.Map<List<QuestionDTO>>(questions);
        return Ok(result);
    }

    

    // Default Version (Version 1.0)

    [HttpGet]
    [Route("/Question/{questionId}")]
    // [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetById(int questionId) //Guid questionBankId
    {
        var question = await _unitOfWork.Questions.GetById(questionId);
        if (question == null) return NotFound();
        var result = _mapper.Map<QuestionDTO>(question);
        return Ok(result);
    }

    
    [HttpPost("/[action]")]
    public async Task<IActionResult> AddQuestion([FromBody] QuestionDTO question)
    {
        // var user = await _unitOfWork.Users.GetById(userId);
        // if (user == null || user.QuestionBankInteract == null || user.QuestionBankInteract.QuestionBanks == null) return NotFound();
        // var questionBank = user.QuestionBankInteract.QuestionBanks.FirstOrDefault(x => x.Id == questionBankId);

        // // Set the QuestionBankId property of the new question
        // // question.QuestionBankId = questionBank.Id;

        // if (questionBank == null || questionBank.Questions == null) return NotFound();

        // // Add the new question to the question bank
        // questionBank.Questions.Add(question);

        if(!ModelState.IsValid)
            return BadRequest();
        var result = _mapper.Map<Question>(question);
        await _unitOfWork.Questions.Add(result);
        await _unitOfWork.CompleteAsync();
        var userDTO = _mapper.Map<Question>(result);
        return Ok(userDTO);
        //return CreatedAtAction(nameof(GetQuestion), routeValues:new {questionBankId = result.QuestionBankId}, value:userDTO);
    }
    

    [HttpDelete]
    [Route("/DeleteQuestion")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var question = await _unitOfWork.Questions.GetById(id);
        if(question == null) return NotFound();
        await _unitOfWork.Questions.Delete(question);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("/UpdateQuestion")]
    [MapToApiVersion("1.0")]
    // public async Task<IActionResult> UpdateQuestion([FromQuery] int userId, [FromQuery] int questionBankId,[FromBody] Question question)
    // //Guid questionBankId
    // {
    //     var user = await _unitOfWork.Users.GetById(userId);
    //     if (user == null || user.QuestionBankInteract == null || user.QuestionBankInteract.QuestionBanks == null) return NotFound();
    //     var questionBank = user.QuestionBankInteract.QuestionBanks.FirstOrDefault(x => x.Id == questionBankId);
    //     if (questionBank == null || questionBank.Questions == null) return NotFound();
    //     if (question == null) return NotFound();
    //     var existedQuestion = await _unitOfWork.Questions.GetById(question.Id);
    //     if(existedQuestion == null) return NotFound();
    //     await _unitOfWork.Questions.Update(question);
    //     await _unitOfWork.CompleteAsync();

    //     return NoContent();
    // }
    public async Task<IActionResult> UpdateQuestion(int id,[FromBody] QuestionDTO question)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        var result = await _unitOfWork.Questions.GetById(id);
        if (result == null) 
            return NotFound();
        var editQuestion = _mapper.Map<Question>(question);
        if(editQuestion == null) return NotFound();

        result.QuestionName = editQuestion.QuestionName;
        result.ChoicesString = editQuestion.ChoicesString;
        result.Type = editQuestion.Type;
        result.AnswersString = editQuestion.AnswersString;
        result.Score = editQuestion.Score;
        //result.QuestionBankId = editQuestion.QuestionBankId;

        await _unitOfWork.Questions.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

}