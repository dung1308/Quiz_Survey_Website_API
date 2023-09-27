using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using survey_quiz_app.Core;
using survey_quiz_app.Data;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.Models;
using System.Data.SqlClient;

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

        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        result.ForEach(e=>{e.DateTimeNow = resultTime.ToString("MM/dd/yyyyTHH:mm");});
        
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
    [Route("/GetCurrentDate")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetCurrentDate() //Guid questionBankId
    {
        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        var result = resultTime.ToString("MM/dd/yyyyTHH:mm");
        return Ok(result);
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

        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        questionBankDTOs.ForEach(e=>{e.DateTimeNow = resultTime.ToString("MM/dd/yyyyTHH:mm");});

        
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

        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        questionBankDTOs.ForEach(e=>{e.DateTimeNow = resultTime.ToString("MM/dd/yyyyTHH:mm");});

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

        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        result.DateTimeNow = resultTime.ToString("MM/dd/yyyyTHH:mm");

        return Ok(result);
    }

    [HttpGet]
    [Route("/GetQuestionBankSurvey/{questionBankId}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetQuestionBankWithCategoryName(int questionBankId) //Guid questionBankId
    {
        var questionBank = await _unitOfWork.QuestionBanks.GetById(questionBankId);
        if (questionBank == null) return NotFound();
        var result = _mapper.Map<QuestionBankDTO>(questionBank);
        var questions = await _unitOfWork.Questions.GetAllByQuestionBankId(questionBankId);
        if (questions == null) return Ok(result);
        var questionDTOs = _mapper.Map<List<QuestionDTO>>(questions);
        result.QuestionDTOs = questionDTOs;

        var categoryTask = _unitOfWork.CategoryLists.All();
        
        var categories = await categoryTask;
        var filteredCategory = categories.Where(c => c.Id == result.CategoryListId).FirstOrDefault();
        if (filteredCategory == null) return NotFound("Not Found UserId in Users");
        result.CategoryName = filteredCategory.CategoryName;

        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        result.DateTimeNow = resultTime.ToString("MM/dd/yyyyTHH:mm");

        // var result2 = result1.ToString("MM-dd-yyyy HH:mm");

        // var response = new { result, CategoryName = filteredCategory.CategoryName, now =  result1};

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
        // var resultQuestion = _mapper.Map<List<Question>>(questionBank.QuestionDTOs);
        var result = _mapper.Map<QuestionBank>(questionBank);

        var categoryTask = _unitOfWork.CategoryLists.All();
        
        var categories = await categoryTask;

        var filteredCategory = categories.Where(c => c.Id == result.CategoryListId).FirstOrDefault();
        if (filteredCategory == null) return NotFound("Not Found UserId in Users");
        result.CategoryName = filteredCategory.CategoryName;

        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        result.DateTimeNow = resultTime.ToString("MM/dd/yyyyTHH:mm");

        await _unitOfWork.QuestionBanks.Add(result);
        await _unitOfWork.CompleteAsync();

        // resultQuestion.ForEach(x => x.QuestionBankId = result.Id);
        // await _unitOfWork.Questions.AddRange(resultQuestion);
        await _unitOfWork.CompleteAsync();
        var questionBankDTO = _mapper.Map<QuestionBankDTO>(result);
        // questionBankDTO.QuestionDTOs = _mapper.Map<List<QuestionDTO>>(resultQuestion);
        return Ok(questionBankDTO);
    }

    [HttpPost]
    [Route("/user/{userId}/AddQuestionBankAndInteract")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> AddQuestionBankAndInteract(int userId, [FromBody] QuestionBankDTO questionBank)
    {

        if(!ModelState.IsValid)
            return BadRequest();
        // var resultQuestion = _mapper.Map<List<Question>>(questionBank.QuestionDTOs);
        var input = _mapper.Map<QuestionBank>(questionBank);
        var usersTask = _unitOfWork.Users.All();
        var users = await usersTask;
        var filteredUser = users.Where(u => u.Id == userId).FirstOrDefault();
        if (filteredUser == null) return NotFound("Not Found UserId in Users");
        input.Owner = filteredUser.UserName;

        var categoryTask = _unitOfWork.CategoryLists.All();
        
        var categories = await categoryTask;
        var filteredCategory = categories.Where(c => c.Id == input.CategoryListId).FirstOrDefault();
        if (filteredCategory == null) return NotFound("Not Found UserId in Users");
        input.CategoryName = filteredCategory.CategoryName;

        // Create a connection to the database
        string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        // Create a SQL command that calls the getDate function
        string sql = "SELECT getDate()";
        SqlCommand command = new SqlCommand(sql, connection);

        // Execute the SQL command and get the result
        connection.Open();
        DateTime resultTime = (DateTime)command.ExecuteScalar();
        connection.Close();

        input.DateTimeNow = resultTime.ToString("MM/dd/yyyyTHH:mm");
        


        await _unitOfWork.QuestionBanks.Add(input);
        await _unitOfWork.CompleteAsync();

        var newInteract = new QuestionBankInteractDTO();
        newInteract.QuestionBankId = input.Id;
        newInteract.UserId = userId;
        newInteract.ResultScores = 0;
        var questions = await _unitOfWork.Questions.All();
        var existedQuestions = questions.Where(q => q.QuestionBankId == input.Id).ToList();
        var questionIdList = existedQuestions.Select(q => q?.Id).Distinct().ToList();
        List<ResultShowDTO> newResultShowList = new List<ResultShowDTO>();
        foreach (var i in questionIdList){
            var newResultShow = new ResultShowDTO
            {
                QuestionId = i
            };
            // newResultShow.QuestionId = i;
            newResultShowList.Add(newResultShow);
        }
        newInteract.ResultShowDTOs = newResultShowList;
        var InteractRS = _mapper.Map<QuestionBankInteract>(newInteract);
        // var resultShowAdd = _mapper.Map<List<Question>>(newResultShowList);
        // newInteract.ResultShowDTOs
        try{
            await _unitOfWork.QuestionBankInteracts.Add(InteractRS);
            await _unitOfWork.CompleteAsync();
        }
        catch(Exception){
            await _unitOfWork.QuestionBanks.Delete(input);
            await _unitOfWork.CompleteAsync();
        }


        var result = _mapper.Map<QuestionBankDTO>(input);
        // result.QuestionDTOs = _mapper.Map<List<QuestionDTO>>(resultQuestion);
        return Ok(result);
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

    // Không nên dùng delete này
    [HttpDelete]
    [Route("/DeleteQuestionBank")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteQuestionBank(int id) //Guid id
    {
        var questionBank = await _unitOfWork.QuestionBanks.GetById(id);
        if(questionBank == null) return NotFound();
        var questions = await _unitOfWork.Questions.All();
        var existedQuestions = questions.ToList().Where(x => x.QuestionBankId == id).ToList();
        if (existedQuestions != null)
        {
            questionBank.Questions = existedQuestions;
        }

        var questionBankInter = await _unitOfWork.QuestionBankInteracts.GetAllByQuestionBank(id);
        var questionBankInterList = questionBankInter.ToList();
        if (questionBankInter != null)
        {
            foreach (var questionBankInterEach in questionBankInterList)
            {
                if (questionBankInterEach != null)
                {
                    var existedQuestions2 = questions.ToList().Where(x => x.QuestionBankId == questionBankInterEach.QuestionBankId).ToList();
                    if (existedQuestions2 != null)
                    {
                        var questionIdList = existedQuestions2.Select(q => q?.Id).Distinct().ToList();
                        List<ResultShow> resultShowList = new List<ResultShow>();
                        foreach (int i in questionIdList){
                            var resultShowsAll = await _unitOfWork.ResultShows.All();
                            var resultShowsList = resultShowsAll.ToList();
                            var resultShows = resultShowsList.Where(q => q.QuestionId == i).ToList();
                            if (resultShows != null)
                            {
                                resultShowList.AddRange(resultShows);
                            }
                        }
                        questionBankInterEach.ResultShows = resultShowList;
                        await _unitOfWork.QuestionBankInteracts.Delete(questionBankInterEach);
                    }
                }
            }
        }
        // var questions = await _unitOfWork.Questions.All();
        // var existedQuestions = questions.ToList().Where(x => x.QuestionBankId == id).ToList();
        // if (existedQuestions != null)
        // {
        //     questionBank.Questions = existedQuestions;
        //     var questionIdList = existedQuestions.Select(q => q?.Id).Distinct().ToList();
        //     List<ResultShow> resultShowList = new List<ResultShow>();
        //     foreach (int i in questionIdList){
        //         var resultShowsAll = await _unitOfWork.ResultShows.All();
        //         var resultShowsList = resultShowsAll.ToList();
        //         var resultShows = resultShowsList.Where(q => q.QuestionId == i).ToList();
        //         if (resultShows != null)
        //         {
        //             resultShowList.AddRange(resultShows);
        //         }
        //     await _unitOfWork.ResultShows.DeleteMulti(resultShowList);
        // }
        // var questionBankInters = await _unitOfWork.QuestionBankInteracts.All();
        // var existedquestionBankInters = questionBankInters.ToList().Where(x => x.QuestionBankId == id).ToList();
        // if (existedquestionBankInters != null)
        // {
        //     await _unitOfWork.QuestionBankInteracts.DeleteMulti(existedquestionBankInters);
        // }
        await _unitOfWork.QuestionBanks.Delete(questionBank);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("/UpdateQuestionBank")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateQuestionBank(int id,[FromBody] QuestionBankDTO questionBank)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        var result = await _unitOfWork.QuestionBanks.GetById(id);
        if (result == null) 
            return NotFound();
        if (result.Questions != null && questionBank.QuestionDTOs != null)
        {
            var categoryTask = _unitOfWork.CategoryLists.All();
        
            var categories = await categoryTask;
            var filteredCategory = categories.Where(c => c.Id == result.CategoryListId).FirstOrDefault();
            if (filteredCategory == null) return NotFound("Not Found UserId in Users");
            result.CategoryName = filteredCategory.CategoryName;

            // Create a connection to the database
            string connectionString = "server=DESKTOP-N650AC4; database=SurveyTest; Integrated Security=True; MultipleActiveResultSets=True; TrustServerCertificate=True;";
            SqlConnection connection = new SqlConnection(connectionString);

            // Create a SQL command that calls the getDate function
            string sql = "SELECT getDate()";
            SqlCommand command = new SqlCommand(sql, connection);

            // Execute the SQL command and get the result
            connection.Open();
            DateTime resultTime = (DateTime)command.ExecuteScalar();
            connection.Close();

            result.DateTimeNow = resultTime.ToString("MM/dd/yyyyTHH:mm");

            var resultIds = result.Questions.Select(q => q?.Id).Distinct().ToList();
            var editIds = questionBank.QuestionDTOs.Select(q => q?.Id).Distinct().ToList();
            var removeIds = resultIds.Except(editIds).ToList();
            var addIds = editIds.Except(resultIds).ToList();
            foreach (int i in removeIds)
            {
                if (i == null) continue;
                int questionId = i;
                var question = await _unitOfWork.Questions.GetById(questionId);
                if(question == null) continue;
                await _unitOfWork.Questions.Delete(question);
                await _unitOfWork.CompleteAsync();
            }

            // var question = await _unitOfWork.Questions.GetById(id);
            // if(question == null) return NotFound();
            // await _unitOfWork.Questions.Delete(question);
            // await _unitOfWork.CompleteAsync();

        }
        var editQuestionBank = _mapper.Map<QuestionBank>(questionBank);
        if(editQuestionBank == null) return NotFound();
        editQuestionBank.Id = result.Id;

        result.SurveyCode = editQuestionBank.SurveyCode;
        result.SurveyName = editQuestionBank.SurveyName;
        result.Owner = editQuestionBank.Owner;
        result.Timer = editQuestionBank.Timer;
        result.StartDate = editQuestionBank.StartDate;
        result.EndDate = editQuestionBank.EndDate;
        result.Status = editQuestionBank.Status;
        result.EnableStatus = editQuestionBank.EnableStatus;
        result.CategoryListId = editQuestionBank.CategoryListId;
        result.Questions = editQuestionBank.Questions;

        await _unitOfWork.QuestionBanks.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
    

    [HttpPut]
    [Route("/UpdateEnabledStatus")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateEnabledStatus(int id,[FromBody] QuestionBankDTO questionBank)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        var result = await _unitOfWork.QuestionBanks.GetById(id);
        if (result == null) 
            return NotFound();
        var editQuestionBank = _mapper.Map<QuestionBank>(questionBank);
        if(editQuestionBank == null) return NotFound();
        editQuestionBank.Id = result.Id;

        result.SurveyCode = editQuestionBank.SurveyCode;
        result.SurveyName = editQuestionBank.SurveyName;
        result.Owner = editQuestionBank.Owner;
        result.Timer = editQuestionBank.Timer;
        result.StartDate = editQuestionBank.StartDate;
        result.EndDate = editQuestionBank.EndDate;
        result.Status = editQuestionBank.Status;
        result.EnableStatus = editQuestionBank.EnableStatus;
        result.CategoryListId = editQuestionBank.CategoryListId;
        // result.Questions = editQuestionBank.Questions;

        await _unitOfWork.QuestionBanks.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
    
}