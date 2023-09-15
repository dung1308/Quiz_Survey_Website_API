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
public class RoleController : BaseController
{
    public RoleController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }

    [HttpGet("/Roles")]
    public async Task<IActionResult> GetRoles()
    {
        if(await _unitOfWork.Roles.All() == null) return NotFound("Roles not found");
        var result = _mapper.Map<List<RoleDTO>>(await _unitOfWork.Roles.All());
        return Ok(result);
    }

    [HttpGet]
    [Route("/Role/{id}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> Get(int id)
    {

        var role = await _unitOfWork.Roles.GetById(id);
        if (role == null) return NotFound();
        var result = _mapper.Map<RoleDTO>(role);
        return Ok(result);
    }

    [HttpGet]
    [Route("/GetRoleByName/{roleName}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetRoleByName(string roleName)
    {

        var role = await _unitOfWork.Roles.GetByRoleName(roleName);
        if (role == null) return NotFound();
        var result = _mapper.Map<RoleDTO>(role);
        return Ok(result);
    }

    [HttpPost]
    [Route("/AddRole")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> AddRole([FromBody] RoleDTO role)
    {
        if(!ModelState.IsValid)
            return BadRequest();
        var result = _mapper.Map<Role>(role);
        await _unitOfWork.Roles.Add(result);
        await _unitOfWork.CompleteAsync();
        var roleDTO = _mapper.Map<RoleDTO>(result);
        return Ok(roleDTO);
        // return CreatedAtAction(nameof(GetRoles), routeValues:new {roleId = result.Id}, value:roleDTO);
    }
    

    [HttpDelete]
    [Route("/DeleteRole")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _unitOfWork.Roles.GetById(id);
        if(role == null) return NotFound();
        await _unitOfWork.Roles.Delete(role);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [HttpPut]
    [Route("/UpdateRole")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> UpdateRole(int id,[FromBody] RoleDTO role)
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