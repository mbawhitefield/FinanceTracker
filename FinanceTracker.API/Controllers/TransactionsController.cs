using FinanceTracker.API.DTOs.Transactions;
using FinanceTracker.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    //Extract the logged-in user's ID from the jwt token
    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? month, [FromQuery] int? year)
    {
        var transactions = await _transactionService
            .GetTransactionAsync(GetUserId(), month, year);
        return Ok(transactions);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        var transaction = await _transactionService
            .GetTransactionByIdAsync(id, GetUserId());

        return Ok(transaction);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionDto dto)
    {
        var transaction = await _transactionService
            .CreateTransactionAsync(GetUserId(), dto);

        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTransactionDto dto)
    {
        var transaction = await _transactionService
            .UpdateTransactionAsync(id, GetUserId(), dto);
        if (transaction == null) return NotFound();

        return Ok(transaction);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _transactionService
            .DeleteTransactionAsync(id, GetUserId());

        if (!deleted) return NotFound();
        return NoContent();
    }
}
