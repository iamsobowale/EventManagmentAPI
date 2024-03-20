using Application.DTOS;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateUserResponseDTO request)
    {
        var result = await _userService.AddAsync(request);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
        
    }
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] CreateUserResponseDTO request)
    {
        var result = await _userService.UpdateAsync(request);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _userService.GetByIdAsync(id);
        if (result != null)
        {
            return Ok(result);
        }
        return NotFound();
    }
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();
        if (result != null)
        {
            return Ok(result);
        }
        return NotFound();
    }
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _userService.DeleteAsync(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    
    [HttpGet("getallusersbyeventid/{eventId}")]
    public async Task<IActionResult> GetAllUsersByEventId([FromRoute] Guid eventId)
    {
        var result = await _userService.GetAllUsersByEventIdAsync(eventId);
        if (result != null)
        {
            return Ok(result);
        }
        return NotFound();
    }
}