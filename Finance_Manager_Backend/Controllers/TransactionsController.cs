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

    /// <summary>
    /// Create transaction.
    /// </summary>
    /// <param name="transaction">The transaction to create.</param>
    /// <returns>Returns the ID of the created transaction.</returns>
    /// <response code="201">Transaction successfully created.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">Internal server error.</response>
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]    
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] Transaction transaction)
    {
        await _transactionsService.CreateTransactionAsync(transaction);

        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction.Id);
    }

    /// <summary>
    /// Get transaction by id.
    /// </summary>
    /// <param name="id">Transaction id.</param>
    /// <returns>Returns transaction object.</returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found transaction.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(Transaction), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetById(int id)
    {
        var transaction = await _transactionsService.GetTransactionByIdAsync(id);

        return Ok(transaction);
    }

    /// <summary>
    /// Get list of transactions.
    /// </summary>
    /// <param name="userId">User id.</param>
    /// <param name="lastDate">Last date from which transactions must be loaded(null if from first).</param>
    /// <param name="pageSize">Amount of transactions.</param>
    /// <returns>Returns list with transaction objects.</returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(List<Transaction>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetTransactions(
    [FromQuery] int userId,
    [FromQuery] DateTime? lastDate,
    [FromQuery] int pageSize = 20)
    {
        var transactions = await _transactionsService.GetTransactionsAsync(userId, lastDate, pageSize);

        return Ok(transactions);
    }

    /// <summary>
    /// Update transaction with new data.
    /// </summary>
    /// <param name="transaction">New transaction object.</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] Transaction transaction)
    {
        await _transactionsService.UpdateTransactionAsync(transaction);

        return NoContent();
    }

    /// <summary>
    /// Delete transaction by id.
    /// </summary>
    /// <param name="id">Transaction id.</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success.</response>
    /// <response code="404">Not found transaction.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _transactionsService.DeleteTransactionAsync(id);

        return NoContent();
    }
}
