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
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
// [ApiVersion("1.0", Deprecated = true)] // Use for warning the version not supported much longer
public class UserController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("User")]
    public async Task<IActionResult> GetUser()
    {
        var users = await _unitOfWork.Users.All();
        if(users == null) return NotFound("Users not found");
        var result = _mapper.Map<UserDTO>(users);
        return Ok(result);
    }

    [HttpGet]
    [Route("User/{id}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get(int id)
    {

        var questionBank = await _unitOfWork.Users.GetById(id);
        if (questionBank == null) return NotFound();
        return Ok(questionBank);
    }

    [HttpPost]
[Route("AddUser")]
[MapToApiVersion("1.0")]
public async Task<IActionResult> AddUser([FromBody] UserDTO user)
{
    if(!ModelState.IsValid)
        return BadRequest();
    var result = _mapper.Map<User>(user);
    await _unitOfWork.Users.Add(result);
    await _unitOfWork.CompleteAsync();
    var userDTO = _mapper.Map<UserDTO>(result);
    return CreatedAtAction(nameof(GetUser), routeValues:new {userId = result.Id}, value:userDTO);
}
    

    [HttpDelete]
    [Route("DeleteUser")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _unitOfWork.Users.GetById(id);
        if(user == null) return NotFound();
        await _unitOfWork.Users.Delete(user);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("UpdateUser")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateUser([FromBody] UserDTO user)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        var result = _mapper.Map<User>(user);
        if(result == null) return NotFound();
        await _unitOfWork.Users.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

}