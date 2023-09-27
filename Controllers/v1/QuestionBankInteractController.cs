using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Core;
using survey_quiz_app.Data;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.util;
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
        if (await _unitOfWork.QuestionBankInteracts.All() == null) return NotFound("QuestionBankInteracts not found");
        var result = _mapper.Map<List<QuestionBankInteractDTO>>(await _unitOfWork.QuestionBankInteracts.All());
        return Ok(result);
    }

    [HttpGet]
    [Route("/User/{userId}/questionBankInteracts")]
    public async Task<IActionResult> GetQuestionBankInteractByUser(int userId) //Guid questionBankId
    {
        var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUser(userId);
        if (questionBankInteracts == null) return NotFound("questionBankInteracts not found");
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
    [Route("/User/{userId}/QuestionBank/{questionBankId}/questionBankInteractLatest")]
    public async Task<IActionResult> GetQuestionBankInteractLatestByUserAndQuestionBank(int userId, int questionBankId) //Guid questionBankId
    {
        var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUserAndQuestionBank(userId, questionBankId);
        if (questionBankInteracts == null) return NotFound();
        var latestInteractRecord = questionBankInteracts
                                .OrderByDescending(a => a?.Id)
                                .First();
        var result = _mapper.Map<QuestionBankInteractDTO>(latestInteractRecord);
        var resultShows = await _unitOfWork.ResultShows.GetAllByQuestionBankInteract(latestInteractRecord.Id);
        var resultShowDTOs = _mapper.Map<List<ResultShowDTO>>(resultShows);
        result.ResultShowDTOs = resultShowDTOs;
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

        if (!ModelState.IsValid)
            return BadRequest();
        // var resultShow = _mapper.Map<List<ResultShow>>(questionBankInteract.ResultShowDTOs);
        var result = _mapper.Map<QuestionBankInteract>(questionBankInteract);
        var resultShow = result.ResultShows.ToList();
        await _unitOfWork.QuestionBankInteracts.Add(result);
        await _unitOfWork.CompleteAsync();
        // try{
        //     resultShow.ForEach(x => x.QuestionBankInteractId = result.Id);
        //     await _unitOfWork.ResultShows.AddRange(resultShow);
        //     await _unitOfWork.CompleteAsync();
        // }
        // catch(Exception){
        //     await _unitOfWork.QuestionBankInteracts.Delete(result);
        //     _unitOfWork.CompleteAsync();
        //     Console.WriteLine("Exception");
        // }
        // resultShow.ForEach(x => x.QuestionBankInteractId = result.Id);
        // await _unitOfWork.ResultShows.AddRange(resultShow);
        // await _unitOfWork.CompleteAsync();
        var questionBankInteractDTO = _mapper.Map<QuestionBankInteractDTO>(result);
        // questionBankInteractDTO.ResultShowDTOs = _mapper.Map<List<ResultShowDTO>>(resultShow);
        return Ok(questionBankInteractDTO);
        //return CreatedAtAction(nameof(GetQuestion), routeValues:new {questionBankId = result.QuestionBankId}, value:userDTO);
    }

    [HttpPost("/[action]")]
    public async Task<IActionResult> CreateAnswer([FromBody] QuestionBankInteractDTO questionBankInteract)
    {

        if (!ModelState.IsValid)
            return BadRequest();
        // var resultShow = _mapper.Map<List<ResultShow>>(questionBankInteract.ResultShowDTOs);
        var result = _mapper.Map<QuestionBankInteract>(questionBankInteract);
        if (result == null) return BadRequest();
        if (result.ResultShows == null)
        {
            result.ResultScores = 0;
            await _unitOfWork.QuestionBankInteracts.Add(result);
            await _unitOfWork.CompleteAsync();
            return Ok(_mapper.Map<QuestionDTO>(result));
        }
        var resultShows = result.ResultShows.ToList();
        await _unitOfWork.QuestionBankInteracts.Add(result);
        await _unitOfWork.CompleteAsync();
        var questionIdList = resultShows.Select(q => q?.QuestionId).Distinct().ToList();
        var questionBank = await _unitOfWork.QuestionBanks?.GetById(result.QuestionBankId ?? 0);
        var questions = questionBank.Questions;
        var scoreList = new List<IDictionary<string, object>>();
        var scoreListNoId = new List<double?>();
        scoreListNoId = questions?.Select(q => q?.Score).ToList();
        if (questions != null) 
            scoreList = questions?.Select(q => new Dictionary<string, object> { { "id", q?.Id }, { "score", q?.Score } })
            .Distinct(new DictionaryComparer<string, object>())
            .ToList();
        List<IDictionary<string, object>> userAnswerList = resultShows
            .Select(q => new Dictionary<string, object> { { "id", q?.QuestionId }, { "answer", q?.OnAnswers } })
            .Distinct(new DictionaryComparer<string, object>())
            .ToList();

        List<IDictionary<string, object>> correctAnswerList = questions?
            .Select(q => new Dictionary<string, object> { { "id", q?.Id }, { "answer", q?.Answers } })
            .Distinct(new DictionaryComparer<string, object>())
            .ToList();

        List<bool> boolList = DictionaryListComparer.Compare(correctAnswerList, userAnswerList);
        (List<IDictionary<string, object>> trueList, List<IDictionary<string, object>> falseList) = ListFilter.Filter(scoreList, boolList);

        int totalScore = trueList?.OfType<Dictionary<string, object>>().Sum(dict => Convert.ToInt32(dict["score"])) ?? 0;

        result.ResultScores = totalScore;

        foreach (var resultShow in resultShows)
{
            // Get the index of the corresponding score in the scoreList
            int index = resultShows.IndexOf(resultShow);

            // Set the resultScore property based on the corresponding value in the booList
            resultShows[index].ResultScore = boolList[index] ? scoreListNoId[index] : 0;
        }
        result.ResultShows = resultShows;
        await _unitOfWork.QuestionBankInteracts.Update(result);
        await _unitOfWork.CompleteAsync();
        
        

        // List<bool> boolList = new List<bool>();
        // foreach (IDictionary<string, object> dict in correctAnswerList)
        // {
        //     boolList.Add(userAnswerList.Contains(dict, new DictionaryComparer<string, object>()));
        // }
        // var userAnswerList = resultShow.Select(q => new { id = q?.Id, answer = q?.OnAnswers }).Distinct().ToList();
        // var correctAnswerList = questions?.Select(q => new { id = q?.Id, answer = q?.Answers }).Distinct().ToList();

        // var comparisonResult = from correctAnswer in correctAnswerList
        //                        join userAnswer in userAnswerList on correctAnswer.id equals userAnswer.id
        //                        select correctAnswer == userAnswer;
        
        // var matchingAnswers = comparisonResult.ToList();

        // var questionBank = await _unitOfWork.Context.QuestionBanks.Include(x => x.Questions).Where(x => x.Id == questionBankInteract.QuestionBankId).FirstOrDefaultAsync();
        // if (questionBank == null)
        //     throw new Exception();
        // var rightAnswers = questionBank.Questions ?? new List<Question>();

        // var userAnswers = questionBankInteract.ResultShowDTOs;

        // var point = userAnswers.Sum(a =>
        // {
        //     var rA = rightAnswers.Where(x => x.Id == a.QuestionId).First();

        //     return (a.OnAnswers.Count() == rA.Answers.Count() && a.OnAnswers.Intersect(rA.Answers).Count() != rA.Answers.Count()) ? rA.Score : 0;
        // });

        return Ok(totalScore);
    }



    [HttpDelete]
    [Route("/DeleteQuestionBankInteract")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteQuestionBankInteract(int id)
    {
        var questionBankInteract = await _unitOfWork.QuestionBankInteracts.GetById(id);
        if (questionBankInteract == null) return NotFound();
        var questions = await _unitOfWork.Questions.All();
        var existedQuestions = questions.ToList().Where(x => x.QuestionBankId == questionBankInteract.QuestionBankId).ToList();
        if (existedQuestions != null)
        {
            var questionIdList = existedQuestions.Select(q => q?.Id).Distinct().ToList();
            List<ResultShow> resultShowList = new List<ResultShow>();
            foreach (int i in questionIdList)
            {
                var resultShowsAll = await _unitOfWork.ResultShows.All();
                var resultShowsList = resultShowsAll.ToList();
                var resultShows = resultShowsList.Where(q => q.QuestionId == i).ToList();
                if (resultShows != null)
                {
                    resultShowList.AddRange(resultShows);
                }
            }
            questionBankInteract.ResultShows = resultShowList;
        }

        await _unitOfWork.QuestionBankInteracts.Delete(questionBankInteract);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("/UpdateQuestionBankInteract")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateQuestionBankInteract(int id, [FromBody] QuestionBankInteractDTO questionBankInteract)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var result = await _unitOfWork.QuestionBankInteracts.GetById(id);
        if (result == null)
            return NotFound("QuestionBankInteract with input id not found");
        var editQuestionBankInteract = _mapper.Map<QuestionBankInteract>(questionBankInteract);
        if (editQuestionBankInteract == null) return NotFound("Request Body Error");

        result.ResultScores = editQuestionBankInteract.ResultScores;

        await _unitOfWork.QuestionBankInteracts.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

}