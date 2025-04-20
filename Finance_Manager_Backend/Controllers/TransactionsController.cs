using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.ModelsDTO;
using Finance_Manager_Backend.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
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
    /// <param name="transactionDTO">The transactionDTO to create.</param>
    /// <returns>Returns the ID of the created transaction.</returns>
    /// <response code="201">Transaction successfully created.</response>
    /// <response code="404">User not found.</response>
    /// <response code="401">Not authorized. Possible invalid token.</response>
    /// <response code="500">Internal server error.</response>
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] TransactionDTO transactionDTO)
    {
        await _transactionsService.CreateTransactionAsync(transactionDTO);

        return CreatedAtAction(nameof(GetById), new { id = transactionDTO.Id }, transactionDTO.Id);
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
    public async Task<ActionResult<TransactionDTO>> GetById(int id)
    {
        var transactionDTO = await _transactionsService.GetTransactionDTOByIdAsync(id);

        return Ok(transactionDTO);
    }

    /// <summary>
    /// Get list of transactions.
    /// </summary>
    /// <param name="userId">User id.</param>
    /// <param name="lastDate">Last date from which transactions must be loaded(null if from first).</param>
    /// <param name="pageSize">Amount of transactions.</param>
    /// <returns>Returns list with transactionDTO objects.</returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(List<TransactionDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<List<TransactionDTO>>> GetTransactions(
    [FromQuery] int userId,
    [FromQuery] DateTime? lastDate, //? ПОМЕНЯТЬ ПОТОМ ДОКУ
    [FromQuery] int pageSize = 20)
    {
        var transactions = await _transactionsService.GetTransactionsAsync(userId, lastDate, pageSize);

        return Ok(transactions);
    }

    /// <summary>
    /// Update transaction with new data.
    /// </summary>
    /// <param name="transactionDTO">New transactionDTO object.</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="401">Not authorized. Possible invalid token.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] TransactionDTO transactionDTO)
    {
        await _transactionsService.UpdateTransactionAsync(transactionDTO);

        return NoContent();
    }

    /// <summary>
    /// Delete transaction by id.
    /// </summary>
    /// <param name="id">Transaction id.</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success.</response>
    /// <response code="404">Not found transaction.</response>
    /// <response code="401">Not authorized. Possible invalid token.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _transactionsService.DeleteTransactionAsync(id);

        return NoContent();
    }
}
