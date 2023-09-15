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
public class QuestionBankController : BaseController
{

    public QuestionBankController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }
    [HttpGet("/GetQuestionBank")]
    public async Task<IActionResult> Get()
    {
        // if(await _unitOfWork.QuestionBanks.All() == null) return NotFound();
        // return Ok(await _unitOfWork.QuestionBanks.All());

        if(await _unitOfWork.QuestionBanks.All() == null) return NotFound("Users not found");
        var result = _mapper.Map<List<QuestionBankDTO>>(await _unitOfWork.QuestionBanks.All());
        
        return Ok(result);
    }

    [HttpGet]
    [Route("/Category/{categoryId}/GetQuestionBank")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetQuestionBankByCategory(int categoryId) //Guid questionBankId
    {
        var category = await _unitOfWork.CategoryLists.GetById(categoryId);
        if (category == null || category.QuestionBanks == null) return NotFound();
        var questionBank = category.QuestionBanks;
        
        return Ok(questionBank);
    }
    [HttpGet]
    [Route("/GetSurveyCode")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetSurveyCode() //Guid questionBankId
    {
        // var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUser(userId);
        // var questionBankIds = questionBankInteracts.Select(q => q?.QuestionBankId).Distinct().ToList();
        // var questionBanks = await _unitOfWork.QuestionBanks.GetByUser(questionBankIds);
        // if (questionBanks == null) return NotFound("There is no questionBanks with this UserId");
        // var surveyCodes = questionBanks.Select(q => q?.SurveyCode).Distinct().ToList();
        var questionBanks = await _unitOfWork.QuestionBanks.All();
        var surveyCodes = questionBanks.Select(q => q?.SurveyCode).Distinct().ToList();
        return Ok(surveyCodes);
    }

    [HttpGet]
    [Route("/User/{userId}/GetQuestionBank")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetQuestionBankByUser(int userId) //Guid questionBankId
    {
        var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUser(userId);
        var questionBankIds = questionBankInteracts.Select(q => q?.QuestionBankId).Distinct().ToList();
        var questionBanks = await _unitOfWork.QuestionBanks.GetByUser(questionBankIds);
        if (questionBanks == null) return NotFound("There is no questionBanks with this UserId");
        var questionBankDTOs = _mapper.Map<List<QuestionBankDTO>>(questionBanks);
        return Ok(questionBankDTOs);
    }

    [HttpGet]
    [Route("/User/{userId}/Category/{categoryId}/GetQuestionBank")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetQuestionBankByUser(int userId, int categoryId) //Guid questionBankId
    {
        var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUser(userId);
        var questionBankIds = questionBankInteracts.Select(q => q?.QuestionBankId).Distinct().ToList();
        var questionBanks = await _unitOfWork.QuestionBanks.GetByUserAndCategory(questionBankIds, categoryId);
        if (questionBanks == null) return NotFound("There is no questionBanks with this UserId and categoryId");
        var questionBankDTOs = _mapper.Map<List<QuestionBankDTO>>(questionBanks);
        return Ok(questionBankDTOs);
    }

    [HttpGet]
    [Route("/GetQuestionBank/{questionBankId}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetOneById(int questionBankId) //Guid questionBankId
    {
        var questionBank = await _unitOfWork.QuestionBanks.GetById(questionBankId);
        if (questionBank == null) return NotFound();
        var result = _mapper.Map<QuestionBankDTO>(questionBank);
        var questions = await _unitOfWork.Questions.GetAllByQuestionBankId(questionBankId);
        if (questions == null) return Ok(result);
        var questionDTOs = _mapper.Map<List<QuestionDTO>>(questions);
        result.QuestionDTOs = questionDTOs;
        return Ok(result);
    }

    

    [HttpPost]
    [Route("/AddQuestionBank")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> AddQuestionBank([FromBody] QuestionBankDTO questionBank)
    {
        // await _unitOfWork.QuestionBanks.Add(questionBank);
        // await _unitOfWork.CompleteAsync();
        // return Ok("Question bank added successfully.");

        if(!ModelState.IsValid)
            return BadRequest();
        var resultQuestion = _mapper.Map<List<Question>>(questionBank.QuestionDTOs);
        var result = _mapper.Map<QuestionBank>(questionBank);

        await _unitOfWork.QuestionBanks.Add(result);
        await _unitOfWork.CompleteAsync();

        resultQuestion.ForEach(x => x.QuestionBankId = result.Id);
        await _unitOfWork.Questions.AddRange(resultQuestion);
        await _unitOfWork.CompleteAsync();
        var questionBankDTO = _mapper.Map<QuestionBankDTO>(result);
        questionBankDTO.QuestionDTOs = _mapper.Map<List<QuestionDTO>>(resultQuestion);
        return Ok(questionBankDTO);
    }

    // [HttpPost]
    // [Route("AddQuestionBank/AddQuestions")]
    // [MapToApiVersion("1.0")]
    // public async Task<IActionResult> AddQuestionBankWithQuestions([FromBody] QuestionBankDTO questionBank)
    // {

    //     if(!ModelState.IsValid)
    //         return BadRequest();
    //     var result = _mapper.Map<QuestionBank>(questionBank);
    //     await _unitOfWork.QuestionBanks.Add(result);
    //     await _unitOfWork.CompleteAsync();
    //     // var resultQuestion = _mapper.Map<List<Question>>(listQuestion);
    //     // resultQuestion.ForEach(x => x.Id = result.Id);
    //     // await _unitOfWork.Questions.AddRange(resultQuestion);
    //     // await _unitOfWork.CompleteAsync();
    //     // var rs = _mapper.Map<List<QuestionBankDTO>>(resultQuestion);
    //     return Ok(result);
    // }

    [HttpDelete]
    [Route("/DeleteQuestionBank")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteQuestionBank(int id) //Guid id
    {
        var questionBank = await _unitOfWork.QuestionBanks.GetById(id);
        if(questionBank == null) return NotFound();
        await _unitOfWork.QuestionBanks.Delete(questionBank);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("/UpdateQuestionBank")]
    [MapToApiVersion("1.0")]
    // public async Task<IActionResult> UpdateQuestionBank(QuestionBank questionBank)
    // {
    //     var existedQuestionBank = await _unitOfWork.QuestionBanks.GetById(questionBank.Id);
    //     if(existedQuestionBank == null) return NotFound();
    //     await _unitOfWork.QuestionBanks.Update(questionBank);
    //     await _unitOfWork.CompleteAsync();

    //     return NoContent();
    // }
    public async Task<IActionResult> UpdateQuestionBank(int id,[FromBody] QuestionBankDTO questionBank)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        var result = await _unitOfWork.QuestionBanks.GetById(id);
        if (result == null) 
            return NotFound();
        var editQuestionBank = _mapper.Map<QuestionBank>(questionBank);
        if(editQuestionBank == null) return NotFound();

        result.SurveyCode = editQuestionBank.SurveyCode;
        result.SurveyName = editQuestionBank.SurveyName;
        result.Owner = editQuestionBank.Owner;
        result.Category = editQuestionBank.Category;
        result.Timer = editQuestionBank.Timer;
        result.StartDate = editQuestionBank.StartDate;
        result.EndDate = editQuestionBank.EndDate;
        result.Status = editQuestionBank.Status;
        result.EnableStatus = editQuestionBank.EnableStatus;
        result.CategoryListId = editQuestionBank.CategoryListId;

        await _unitOfWork.QuestionBanks.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
    
}