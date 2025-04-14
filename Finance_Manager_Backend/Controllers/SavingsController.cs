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

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] Saving saving)
    {
        await _savingsService.CreateSavingAsync(saving);

        return CreatedAtAction(nameof(GetById), new { id = saving.Id }, saving.Id);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Saving>> GetById(int id)
    {
        var saving = await _savingsService.GetSavingByIdAsync(id);

        return Ok(saving);
    }

    [HttpGet]
    public async Task<ActionResult<List<Saving>>> GetSavings(
        [FromQuery] int userId,
        [FromQuery] int previousSavingId,
        [FromQuery] int pageSize = 5)
    {
        var savings = await _savingsService.GetSavingsAsync(userId, previousSavingId, pageSize);

        return Ok(savings);
    }

    [HttpPut]
    public async Task<ActionResult> Update(
        [FromQuery] int savingId, 
        [FromQuery] int topUpAmount)
    {
        await _savingsService.UpdateSavingAsync(savingId, topUpAmount);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _savingsService.DeleteSavingAsync(id);

        return NoContent();
    }
}
