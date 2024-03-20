using Application.DTOS;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Add([FromBody] CreateBookingDTO booking)
    {
        var result = await _bookingService.AddAsync(booking);
        return Ok(result);
    }
    
    [HttpPost("cancelbooking/{id}")]
    public async Task<IActionResult> CancelBooking([FromRoute] Guid id)
    {
        var result = await _bookingService.DeleteAsync(id);
        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _bookingService.GetByIdAsync(id);
        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }
    
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bookingService.GetAllAsync();
        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }
    
    [HttpGet("getallbyuserid/{userId}")]
    public async Task<IActionResult> GetAllByUserId([FromRoute] Guid userId)
    {
        var result = await _bookingService.GetAllBookingsByUserIdAsync(userId);
        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }
    
}