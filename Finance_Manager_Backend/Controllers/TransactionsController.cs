using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using Finance_Manager_Backend.Middleware;

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
    /// <response code="400">Validation failed.</response>
    /// <response code="404">User not found.</response>
    /// <response code="401">Not authorized. Possible invalid token.</response>
    /// <response code="500">Internal server error.</response>
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize]
    [SwaggerOperation(OperationId = "CreateTransaction")]
    [HttpPost]
    public async Task<ActionResult<int>> CreateTransaction([FromBody] TransactionDTO transactionDTO)
    {
        await _transactionsService.CreateTransactionAsync(transactionDTO);

        return CreatedAtAction(nameof(GetTransactionById), new { id = transactionDTO.Id }, transactionDTO.Id);
    }

    /// <summary>
    /// Get transaction by id.
    /// </summary>
    /// <param name="id">Transaction id.</param>
    /// <returns>Returns transaction object.</returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found transaction.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(TransactionDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetTransaction")]
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDTO>> GetTransactionById(int id)
    {
        var transactionDTO = await _transactionsService.GetTransactionDTOByIdAsync(id);

        return Ok(transactionDTO);
    }

    /// <summary>
    /// Get list of transactions.
    /// </summary>
    /// <param name="queryDTO">QueryDTO with:
    /// userId - User id; 
    /// lastDate - Last date from which transactions must be loaded(null if from first);
    /// pageSize - Amount of transactions.</param>
    /// <returns>Returns list with transactionDTO objects.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(List<TransactionDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetTransactions")]
    [HttpGet]
    public async Task<ActionResult<List<TransactionDTO>>> GetTransactions([FromQuery] GetUserTransactionsQueryDTO queryDTO)
    {
        var transactions = await _transactionsService.GetTransactionsAsync(queryDTO.userId, queryDTO.lastDate, queryDTO.pageSize);

        return Ok(transactions);
    }

    /// <summary>
    /// Update transaction with new data.
    /// </summary>
    /// <param name="transactionDTO">New transactionDTO object.</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="401">Not authorized. Possible invalid token.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize]
    [SwaggerOperation(OperationId = "UpdateTransaction")]
    [HttpPut]
    public async Task<ActionResult> UpdateTransaction([FromBody] TransactionDTO transactionDTO)
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
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize]
    [SwaggerOperation(OperationId = "DeleteTransaction")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTransaction(int id)
    {
        await _transactionsService.DeleteTransactionAsync(id);

        return NoContent();
    }
}
