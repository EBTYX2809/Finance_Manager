using Finance_Manager_Backend.BuisnessLogic.Models;
using Finance_Manager_Backend.BuisnessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finance_Manager_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SavingsController : Controller
{
    private readonly SavingsService _savingsService;
    public SavingsController(SavingsService savingsService)
    {
        _savingsService = savingsService;
    }

    /// <summary>
    /// Create saving.
    /// </summary>
    /// <param name="saving">The saving to create.</param>
    /// <returns>Returns the ID of the created saving.</returns>
    /// <response code="201">Saving successfully created.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">Internal server error.</response>
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] Saving saving)
    {
        await _savingsService.CreateSavingAsync(saving);

        return CreatedAtAction(nameof(GetById), new { id = saving.Id }, saving.Id);
    }

    /// <summary>
    /// Get saving by id.
    /// </summary>
    /// <param name="id">Saving id.</param>
    /// <returns>Returns saving object.</returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found saving.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(Saving), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Saving>> GetById(int id)
    {
        var saving = await _savingsService.GetSavingByIdAsync(id);

        return Ok(saving);
    }

    /// <summary>
    /// Get list of savings.
    /// </summary>
    /// <param name="userId">User id that want to get his savings.</param>
    /// <param name="previousSavingId">Last saving id from which savings must be loaded.</param>
    /// <param name="pageSize">Amount of savings.</param>
    /// <returns>Returns list with savings objects.</returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response>
    [ProducesResponseType(typeof(List<Saving>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<List<Saving>>> GetSavings(
        [FromQuery] int userId,
        [FromQuery] int previousSavingId,
        [FromQuery] int pageSize = 5)
    {
        var savings = await _savingsService.GetSavingsAsync(userId, previousSavingId, pageSize);

        return Ok(savings);
    }

    /// <summary>
    /// Update saving with new top up.
    /// </summary>
    /// <param name="savingId">Saving id.</param>
    /// <param name="topUpAmount">Amount of top up.</param>
    /// <response code="204">Success.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut]
    public async Task<ActionResult> Update(
        [FromQuery] int savingId, 
        [FromQuery] int topUpAmount)
    {
        await _savingsService.UpdateSavingAsync(savingId, topUpAmount);

        return NoContent();
    }

    /// <summary>
    /// Delete saving by id.
    /// </summary>
    /// <param name="id">Saving id.</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success.</response>
    /// <response code="404">Not found saving.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _savingsService.DeleteSavingAsync(id);

        return NoContent();
    }
}
