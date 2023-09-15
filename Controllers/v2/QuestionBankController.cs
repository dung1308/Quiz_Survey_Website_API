using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Core;
using survey_quiz_app.Data;
using survey_quiz_app.Models;

namespace survey_quiz_app.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
public class QuestionBankController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public QuestionBankController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [HttpGet("GetQuestionBank")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Get()
    {
        if(await _unitOfWork.QuestionBanks.All() == null) return NotFound();
        return Ok(await _unitOfWork.QuestionBanks.All());
    }

    // Default Version (Version 1.0)

    [HttpGet]
    [Route("GetQuestionBank/{id}")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> Get(int id) //Guid id
    {

        var questionBank = await _unitOfWork.QuestionBanks.GetById(id);
        if (questionBank == null) return NotFound();
        return Ok(questionBank);
    }

    [HttpPost]
    [Route("AddQuestionBank")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> AddQuestionBank(QuestionBank questionBank)
    {
        await _unitOfWork.QuestionBanks.Add(questionBank);
        await _unitOfWork.CompleteAsync();
        return Ok("Question bank added successfully");
    }

    [HttpDelete]
    [Route("DeleteQuestionBank")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> DeleteQuestionBank(int id) //Guid id
    {
        var questionBank = await _unitOfWork.QuestionBanks.GetById(id);
        if(questionBank == null) return NotFound();
        await _unitOfWork.QuestionBanks.Delete(questionBank);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPatch]
    [Route("UpdateQuestionBank")]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> UpdateQuestionBank(QuestionBank questionBank)
    {
        var existedQuestionBank = await _unitOfWork.QuestionBanks.GetById(questionBank.Id);
        if(existedQuestionBank == null) return NotFound();
        await _unitOfWork.QuestionBanks.Update(questionBank);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}