using Finance_Manager_Backend.BuisnessLogic.Models;
using Finance_Manager_Backend.BuisnessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finance_Manager_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionsService _transactionsService;

    public TransactionsController(TransactionsService transactionsService)
    {
        _transactionsService = transactionsService;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] Transaction transaction)
    {
        await _transactionsService.CreateTransactionAsync(transaction);

        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction.Id);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetById(int id)
    {
        var transaction = await _transactionsService.GetTransactionByIdAsync(id);

        return Ok(transaction);
    }

    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetTransactions(
    [FromQuery] int userId,
    [FromQuery] DateTime? lastDate,
    [FromQuery] int pageSize = 20)
    {
        var transactions = await _transactionsService.GetTransactionsAsync(userId, lastDate, pageSize);

        return Ok(transactions);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Transaction transaction)
    {
        await _transactionsService.UpdateTransactionAsync(transaction);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _transactionsService.DeleteTransactionAsync(id);

        return NoContent();
    }
}
