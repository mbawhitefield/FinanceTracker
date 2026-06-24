using FinanceTracker.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SummaryController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    public SummaryController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthlySummary([FromQuery] int month, [FromQuery] int year)
    {
        if (month < 1 || month > 12)
            return BadRequest("Month must be between 1 and 12");
        var summary = await _transactionService
            .GetMonthlySummaryAsync(GetUserId(), month, year);

        return Ok(summary);
    }

}
