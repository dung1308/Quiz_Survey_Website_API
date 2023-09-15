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
public class ResultShowController : BaseController
{
    public ResultShowController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }


    [HttpGet]
    [Route("/ResultShows")]
    public async Task<IActionResult> GetResultShows() //Guid questionBankId
    {
        if(await _unitOfWork.ResultShows.All() == null) return NotFound("ResultShows not found");
        var result = _mapper.Map<List<ResultShowDTO>>(await _unitOfWork.ResultShows.All());
        return Ok(result);
    }

    [HttpGet]
    [Route("/User/{userId}/questionBankInteract/{questionBankInteractsId}/ResultShow")]
    public async Task<IActionResult> GetResultShowByQuestionBankInteract(int userId, int questionBankInteractsId) //Guid questionBankId
    {
        var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUser(userId);
        if (questionBankInteracts == null) return NotFound();
        var questionBankInteract = questionBankInteracts.FirstOrDefault(x => x?.Id == questionBankInteractsId);
        if (questionBankInteract == null) return NotFound("QuestionBankInteract not Found");
        var resultShow = questionBankInteract.ResultShows;
        if(resultShow == null) return NotFound("resultShow not found");
        var result = _mapper.Map<ResultShowDTO>(resultShow);
        return Ok(result);
    }

    [HttpGet]
    [Route("/User/{userId}/questionBankInteract/{questionBankInteractsId}/QuestionBank/{questionId}/ResultShow/{resultShowId}")]
    public async Task<IActionResult> GetResultShowByAllId(int userId, int questionBankInteractsId, int questionId, int resultShowId) //Guid questionBankId
    {
        var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUser(userId);
        if (questionBankInteracts == null) return NotFound();
        var questionBankInteract = questionBankInteracts.FirstOrDefault(x => x?.Id == questionBankInteractsId);
        if (questionBankInteract == null) return NotFound("QuestionBankInteract not found in User");
        var resultShows = questionBankInteract.ResultShows;
        var resultShow = await _unitOfWork.ResultShows.GetAllByQuestionAndQuestionBankInteract(questionId, questionBankInteractsId);
        var rs = resultShow.FirstOrDefault(x => x?.Id == resultShowId);
        if(rs == null) return NotFound("resultShow not found");
        var result = _mapper.Map<ResultShowDTO>(rs);
        return Ok(result);
    }

    // Default Version (Version 1.0)

    [HttpGet]
    [Route("/ResultShow/{resultShowId}")]
    // [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetById(int resultShowId)
    {
        var resultShow = await _unitOfWork.ResultShows.GetById(resultShowId);
        if (resultShow == null) return NotFound();
        var result = _mapper.Map<ResultShowDTO>(resultShow);
        return Ok(result);
    }

    
    [HttpPost("/[action]")]
    public async Task<IActionResult> AddResultShow([FromBody] ResultShowDTO resultShow)
    {

        if(!ModelState.IsValid)
            return BadRequest();
        var result = _mapper.Map<ResultShow>(resultShow);
        await _unitOfWork.ResultShows.Add(result);
        await _unitOfWork.CompleteAsync();
        var resultShowDTO = _mapper.Map<ResultShow>(result);
        return Ok(resultShowDTO);
        //return CreatedAtAction(nameof(GetQuestion), routeValues:new {questionBankId = result.QuestionBankId}, value:userDTO);
    }
    

    [HttpDelete]
    [Route("/DeleteResultShow")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteResultShow(int id)
    {
        var resultShow = await _unitOfWork.ResultShows.GetById(id);
        if(resultShow == null) return NotFound();
        await _unitOfWork.ResultShows.Delete(resultShow);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("/UpdateResultShow")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateResultShow(int id,[FromBody] ResultShowDTO resultShow)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        var result = await _unitOfWork.ResultShows.GetById(id);
        if (result == null) 
            return NotFound();
        var editResultShow = _mapper.Map<ResultShow>(resultShow);
        if(editResultShow == null) return NotFound();

        result.OnAnswersString = editResultShow.OnAnswersString;
        //result.QuestionId = editResultShow.QuestionId;
        //result.QuestionBankInteractId = editResultShow.QuestionBankInteractId;

        await _unitOfWork.ResultShows.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

}