using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using Finance_Manager_Backend.Middleware;

namespace Finance_Manager_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SavingsController : ControllerBase
{
    private readonly SavingsService _savingsService;
    public SavingsController(SavingsService savingsService)
    {
        _savingsService = savingsService;
    }

    /// <summary>
    /// Create saving.
    /// </summary>
    /// <param name="savingDTO">The savingDTO to create.</param>
    /// <returns>Returns the ID of the created saving.</returns>
    /// <response code="201">Saving successfully created.</response>
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
    [SwaggerOperation(OperationId = "CreateSaving")]
    [HttpPost]
    public async Task<ActionResult<int>> CreateSaving([FromBody] SavingDTO savingDTO)
    {
        await _savingsService.CreateSavingAsync(savingDTO);

        return CreatedAtAction(nameof(GetSavingById), new { id = savingDTO.Id }, savingDTO.Id);
    }

    /// <summary>
    /// Get saving by id.
    /// </summary>
    /// <param name="id">Saving id.</param>
    /// <returns>Returns savingDTO object.</returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found saving.</response>     
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(SavingDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetSaving")]
    [HttpGet("{id}")]
    public async Task<ActionResult<SavingDTO>> GetSavingById(int id)
    {
        var savingDTO = await _savingsService.GetSavingDTOByIdAsync(id);

        return Ok(savingDTO);
    }

    /// <summary>
    /// Get list of savings.
    /// </summary>
    /// <param name="queryDTO">QueryDTO with parameters:
    /// userId - User id that want to get his savings;
    /// previousSavingId - Last saving id from which savings must be loaded;
    /// pageSize - Amount of savings.</param>
    /// <returns>Returns list with savingsDTO objects.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response>
    [ProducesResponseType(typeof(List<SavingDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetSavings")]
    [HttpGet]
    public async Task<ActionResult<List<SavingDTO>>> GetSavings([FromQuery] GetUserSavingsQueryDTO queryDTO)
    {
        var savingsDTO = await _savingsService.GetSavingsAsync(queryDTO.userId, queryDTO.previousSavingId, queryDTO.pageSize);

        return Ok(savingsDTO);
    }

    /// <summary>
    /// Update saving with new top up.
    /// </summary>
    /// <param name="topUpDTO">TopUpDTO with parameters:
    /// savingId - Saving id;
    /// topUpAmount - Amount of top up.</param>
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
    [SwaggerOperation(OperationId = "UpdateSaving")]
    [HttpPut]
    public async Task<ActionResult> UpdateSaving([FromBody] SavingTopUpDTO topUpDTO)
    {
        await _savingsService.UpdateSavingAsync(topUpDTO.savingId, topUpDTO.topUpAmount);

        return NoContent();
    }

    /// <summary>
    /// Delete saving by id.
    /// </summary>
    /// <param name="id">Saving id.</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success.</response>
    /// <response code="404">Not found saving.</response>
    /// <response code="401">Not authorized. Possible invalid token.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize]
    [SwaggerOperation(OperationId = "DeleteSaving")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSaving(int id)
    {
        await _savingsService.DeleteSavingAsync(id);

        return NoContent();
    }
}
