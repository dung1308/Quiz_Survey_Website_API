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
public class CategoryController : BaseController
{
    public CategoryController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }


    [HttpGet]
    [Route("/Categories")]
    public async Task<IActionResult> GetCategories() //Guid questionBankId
    {
        if(await _unitOfWork.CategoryLists.All() == null) return NotFound("CategoryLists not found");
        var result = _mapper.Map<List<CategoryListDTO>>(await _unitOfWork.CategoryLists.All());
        return Ok(result);
    }

    [HttpGet]
    [Route("/Categories/{categoryId}")]
    // [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetById(int categoryId) //Guid questionBankId
    {
        var category = await _unitOfWork.CategoryLists.GetById(categoryId);
        if (category == null) return NotFound();
        var result = _mapper.Map<CategoryListDTO>(category);
        return Ok(result);
    }

    
    [HttpPost("/[action]")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryListDTO category)
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
        var result = _mapper.Map<CategoryList>(category);
        await _unitOfWork.CategoryLists.Add(result);
        await _unitOfWork.CompleteAsync();
        var categoryDTO = _mapper.Map<CategoryList>(result);
        return Ok(categoryDTO);
        //return CreatedAtAction(nameof(GetQuestion), routeValues:new {questionBankId = result.QuestionBankId}, value:userDTO);
    }
    

    [HttpDelete]
    [Route("/DeleteCategory")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteCategory(int id) //Guid id
    {
        var category = await _unitOfWork.CategoryLists.GetById(id);
        if(category == null) return NotFound();
        await _unitOfWork.CategoryLists.Delete(category);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("/UpdateCategory")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateCategory(int id,[FromBody] RoleDTO role)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        var result = await _unitOfWork.Roles.GetById(id);
        if (result == null) 
            return NotFound();
        var editRole = _mapper.Map<Role>(role);
        if(editRole == null) return NotFound();
        result.RoleName = editRole.RoleName;
        result.Permission = editRole.Permission;
        await _unitOfWork.Roles.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}