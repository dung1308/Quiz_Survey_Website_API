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
public class QuestionBankInteractController : BaseController
{
    public QuestionBankInteractController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }


    [HttpGet]
    [Route("/QuestionBankInteracts")]
    public async Task<IActionResult> GetQuestionBankInteracts() //Guid questionBankId
    {
        if(await _unitOfWork.QuestionBankInteracts.All() == null) return NotFound("QuestionBankInteracts not found");
        var result = _mapper.Map<List<QuestionBankInteractDTO>>(await _unitOfWork.QuestionBankInteracts.All());
        return Ok(result);
    }

    [HttpGet]
    [Route("/User/{userId}/questionBankInteracts")]
    public async Task<IActionResult> GetQuestionBankInteractByUser(int userId) //Guid questionBankId
    {
        var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUser(userId);
        if(questionBankInteracts == null) return NotFound("questionBankInteracts not found");
        var result = _mapper.Map<List<QuestionBankInteractDTO>>(questionBankInteracts);
        return Ok(result);
    }

    [HttpGet]
    [Route("/User/{userId}/QuestionBank/{questionBankId}/questionBankInteracts")]
    public async Task<IActionResult> GetQuestionBankInteractByUserAndQuestionBank(int userId, int questionBankId) //Guid questionBankId
    {
        var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUserAndQuestionBank(userId, questionBankId);
        if (questionBankInteracts == null) return NotFound();
        var result = _mapper.Map<List<QuestionBankInteractDTO>>(questionBankInteracts);
        return Ok(result);
    }


    [HttpGet]
    [Route("/User/{userId}/QuestionBank/{questionBankId}/questionBankInteracts/{questionBankInteractsId}")]
    public async Task<IActionResult> GetQuestionBankInteractByUserAndQuestionBank(int userId, int questionBankId, int questionBankInteractsId) //Guid questionBankId
    {
        var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUserAndQuestionBank(userId, questionBankId);
        if (questionBankInteracts == null) return NotFound(); //.FirstOrDefault(x => x.Id == questionBankId);
        var questionBankInteract = questionBankInteracts.FirstOrDefault(x => x.Id == questionBankInteractsId);
        var result = _mapper.Map<QuestionBankInteractDTO>(questionBankInteract);
        var resultShows = await _unitOfWork.ResultShows.GetAllByQuestionBankInteract(questionBankInteractsId);
        if (resultShows == null) return Ok(result);
        var resultShowDTOs = _mapper.Map<List<ResultShowDTO>>(resultShows);
        result.ResultShowDTOs = resultShowDTOs;
        return Ok(result);
    }


    // Default Version (Version 1.0)

    [HttpGet]
    [Route("/questionBankInteracts/{questionBankInteractsId}")]
    // [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get(int questionBankInteractsId)
    {
        var questionBank = await _unitOfWork.QuestionBankInteracts.GetById(questionBankInteractsId);
        if (questionBank == null) return NotFound();
        var result = _mapper.Map<QuestionBankInteractDTO>(questionBank);
        var questions = await _unitOfWork.ResultShows.GetAllByQuestionBankInteract(questionBankInteractsId);
        if (questions == null) return Ok(result);
        var questionDTOs = _mapper.Map<List<ResultShowDTO>>(questions);
        result.ResultShowDTOs = questionDTOs;
        return Ok(result);
    }

    
    [HttpPost("/[action]")]
    public async Task<IActionResult> AddQuestionBankInteract([FromBody] QuestionBankInteractDTO questionBankInteract)
    {

        if(!ModelState.IsValid)
            return BadRequest();
        var resultShow = _mapper.Map<List<ResultShow>>(questionBankInteract.ResultShowDTOs);
        var result = _mapper.Map<QuestionBankInteract>(questionBankInteract);

        await _unitOfWork.QuestionBankInteracts.Add(result);
        await _unitOfWork.CompleteAsync();

        resultShow.ForEach(x => x.QuestionBankInteractId = result.Id);
        await _unitOfWork.ResultShows.AddRange(resultShow);
        await _unitOfWork.CompleteAsync();
        var questionBankInteractDTO = _mapper.Map<QuestionBankInteractDTO>(result);
        questionBankInteractDTO.ResultShowDTOs = _mapper.Map<List<ResultShowDTO>>(resultShow);
        return Ok(questionBankInteractDTO);
        //return CreatedAtAction(nameof(GetQuestion), routeValues:new {questionBankId = result.QuestionBankId}, value:userDTO);
    }
    

    [HttpDelete]
    [Route("/DeleteQuestionBankInteract")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteQuestionBankInteract(int id)
    {
        var questionBankInteract = await _unitOfWork.QuestionBankInteracts.GetById(id);
        if(questionBankInteract == null) return NotFound();
        await _unitOfWork.QuestionBankInteracts.Delete(questionBankInteract);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("/UpdateQuestionBankInteract")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateQuestionBankInteract(int id,[FromBody] QuestionBankInteractDTO questionBankInteract)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        var result = await _unitOfWork.QuestionBankInteracts.GetById(id);
        if (result == null) 
            return NotFound("QuestionBankInteract with input id not found");
        var editQuestionBankInteract = _mapper.Map<QuestionBankInteract>(questionBankInteract);
        if(editQuestionBankInteract == null) return NotFound("Request Body Error");

        result.ResultScores = editQuestionBankInteract.ResultScores;

        await _unitOfWork.QuestionBankInteracts.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

}