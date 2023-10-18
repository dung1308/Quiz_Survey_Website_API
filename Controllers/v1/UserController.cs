using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Core;
using survey_quiz_app.Data;
using survey_quiz_app.DTO.Incoming;
using survey_quiz_app.DTO.Outgoing;
using survey_quiz_app.Models;

namespace survey_quiz_app.Controllers.v1;

[ApiController]
// [Route("api/v{version:apiVersion}/[controller]")]
[Route("api/[controller]")]
[ApiVersion("1.0")]
// [ApiVersion("1.0", Deprecated = true)] // Use for warning the version not supported much longer
public class UserController : ControllerBase
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;

    public UserController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("/User")]
    public async Task<IActionResult> GetUser()
    {
        // var user = new User{
        //     Id = 0,
        //     UserName= "Hello",
        //     Password= "111",
        //     Email = "111",
        //     QuestionBankInteractId = 0,
        //     QuestionBankInteract = {
        //         Id = 0,
        //         ResultScores = 5.6,
        //         QuestionBanks = [{
        //              id = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        //             "surveyCode": "string",
        //             "surveyName": "string",
        //             "owner": "string",
        //             "category": "string",
        //             "timer": "string",
        //             "startDate": "string",
        //             "endDate": "string",
        //             "status": "string",
        //             "enableStatus": true,
        //             "questions": [
        //                 {
        //                 "id": 0,
        //                 "questionName": "string",
        //                 "choices": [
        //                     "string"
        //                 ],
        //                 "type": "string",
        //                 "answers": [
        //                     "string"
        //                 ],
        //                 "onAnswers": [
        //                     "string"
        //                 ],
        //                 "score": 0,
        //                 "questionBankId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        //                 "questionBank": "string"
        //                 }
        //             ],
        //             "categoryList": {
        //                 "id": 0,
        //                 "categoryName": "string",
        //                 "questionBanks": [
        //                 "string"
        //                 ]
        //             }


        //         }],
        //         ResultShows = {

        //         }
        //     }
        // };
        // var newUserDTO = new UserDTO{
        //     UserName = "Dung",
        //     Password ="123",
        //     Email = "21@1234.com"
        // };
        // var newUser = _mapper.Map<User>(newUserDTO);
        // await _unitOfWork.Users.Add(newUser);

        // var users = await _unitOfWork.Users.All();
        // if(users == null || users.Count() == 0) return NotFound("Users not found");
        // var result = _mapper.Map<UserDTO>(users);
        // // await _unitOfWork.Users.Delete(newUser);
        // return Ok(result);
        var users = await _unitOfWork.Users.All();
        if (users == null || users.Count() == 0) return NotFound("Users not found");
        var result = _mapper.Map<List<UserDTO>>(await _unitOfWork.Users.All());
        return Ok(result);
    }

    [HttpGet]
    [Route("/User/{id}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get(int id)
    {

        var user = await _unitOfWork.Users.GetById(id);
        if (user == null) return NotFound();
        var result = _mapper.Map<UserDTO>(user);
        return Ok(result);
    }

    [HttpGet]
    [Route("/GetUserNames")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetUserNames() //Guid questionBankId
    {
        // var questionBankInteracts = await _unitOfWork.QuestionBankInteracts.GetAllByUser(userId);
        // var questionBankIds = questionBankInteracts.Select(q => q?.QuestionBankId).Distinct().ToList();
        // var questionBanks = await _unitOfWork.QuestionBanks.GetByUser(questionBankIds);
        // if (questionBanks == null) return NotFound("There is no questionBanks with this UserId");
        // var surveyCodes = questionBanks.Select(q => q?.SurveyCode).Distinct().ToList();
        var users = await _unitOfWork.Users.All();
        var userNames = users.Select(q => q?.UserName).Distinct().ToList();
        return Ok(userNames);
    }

    [HttpPost]
    [Route("/LoginUser")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> LoginUser(LoginUserDTO loginUser) //Guid questionBankId
    {
        var users = await _unitOfWork.Users.All();
        var usersList = users.ToList();
        var userNameList = users.Select((e) => e.UserName);
        if (!userNameList.Contains(loginUser.UserName)) return NotFound("There is no such username");
        var user = await _unitOfWork.Users.LoginData(loginUser.UserName);
        if (user == null) return NotFound("User Not Found");
        UserGetDTOResponse responseDTO = new UserGetDTOResponse{Id = user.Id,UserName = user.UserName, RoleId = user.RoleId, IsNightMode = user.IsNightMode};
        if (user.Password == loginUser.Password) return Ok(responseDTO);
        return NotFound("Wrong Password");
    }


    
    [HttpPost]
    [Route("/AddUser")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> AddUser([FromBody] UserDTO user)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var result = _mapper.Map<User>(user);
        await _unitOfWork.Users.Add(result);
        await _unitOfWork.CompleteAsync();
        var userDTO = _mapper.Map<UserDTO>(result);
        return Ok(userDTO);
        // return CreatedAtAction(nameof(GetUser), routeValues:new {userId = result.Id}, value:userDTO);
    }

    [HttpPost]
    [Route("/Register")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Register([FromBody] UserDTO user)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var result = _mapper.Map<User>(user);
        var users = await _unitOfWork.Users.All();
        var usersList = users.ToList();
        var userNameList = users.Select((e) => e.UserName);
        if (userNameList.Contains(result.UserName)) return NotFound("There is a same username");
        await _unitOfWork.Users.Add(result);
        await _unitOfWork.CompleteAsync();
        var userDTO = _mapper.Map<UserDTO>(result);
        return Ok(userDTO);
        // return CreatedAtAction(nameof(GetUser), routeValues:new {userId = result.Id}, value:userDTO);
    }




    [HttpDelete]
    [Route("/DeleteUser")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _unitOfWork.Users.GetById(id);
        if (user == null) return NotFound();
        await _unitOfWork.Users.Delete(user);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("/UpdateUser")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO user)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var result = await _unitOfWork.Users.GetById(id);
        if (result == null)
            return NotFound();
        var editUser = _mapper.Map<User>(user);
        if (editUser == null) return NotFound();

        result.UserName = editUser.UserName;
        result.Password = editUser.Password;
        result.Email = editUser.Email;
        result.RoleId = editUser.RoleId;

        await _unitOfWork.Users.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpPut]
    [Route("/[action]")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> ChangeNightMode(int id, [FromBody] UserDTO user)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var result = await _unitOfWork.Users.GetById(id);
        if (result == null)
            return NotFound();
        var editUser = _mapper.Map<User>(user);
        if (editUser == null) return NotFound();

        result.IsNightMode = editUser.IsNightMode;

        await _unitOfWork.Users.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

}