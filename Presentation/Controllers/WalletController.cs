using Application.DTOS;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("get/{userId}")]
    public async Task<IActionResult> GetByUserId([FromRoute] string userId)
    {
        var result = await _walletService.GetByUserIdAsync(userId);
        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }
    
    [HttpPost("fundwallet")]
    public async Task<IActionResult> FundWallet([FromBody] WalletDTO fundWallet)
    {
        var result = await _walletService.FundWalletAsync(fundWallet);
        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }
}
