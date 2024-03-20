using Application.DTOS;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;



[ApiController]
[Route("api/v1/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }
    
    
    [HttpPost("create"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] EventDTO request)
    {
        var result = await _eventService.AddAsync(request);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
        
    }
    [HttpPut("update"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] EventDTO request)
    {
        var result = await _eventService.UpdateAsync(request);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpGet("get/{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _eventService.GetByIdAsync(id);
        if (result != null)
        {
            return Ok(result);
        }
        return NotFound();
    }
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _eventService.GetAllAsync();
        if (result != null)
        {
            return Ok(result);
        }
        return NotFound();
    }
    [HttpDelete("delete/{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _eventService.DeleteAsync(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}