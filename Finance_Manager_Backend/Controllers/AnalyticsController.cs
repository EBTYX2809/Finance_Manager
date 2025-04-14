using Finance_Manager_Backend.BuisnessLogic.Models;
using Finance_Manager_Backend.BuisnessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finance_Manager_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : Controller
{
    private readonly AnalyticsService _analyticsService;
    public AnalyticsController(AnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet]
    public async Task<ActionResult<Dictionary<Category, float>>> GetAnalytics(
        [FromQuery] int userId,
        [FromQuery] DateTime minDate, 
        [FromQuery] DateTime maxDate)
    {
        var analytics = await _analyticsService.GetAlalyticFromDateAsync(userId, minDate, maxDate);

        return Ok(analytics);
    }

    [HttpGet]
    public async Task<ActionResult<Dictionary<Category, float>>> GetInnerAnalytics(
        [FromQuery] int parentCategoryId,
        [FromQuery] int userId,
        [FromQuery] DateTime minDate,
        [FromQuery] DateTime maxDate)
    {
        var analytics = await _analyticsService.GetInnerAlalyticFromDateAsync(userId, parentCategoryId, minDate, maxDate);

        return Ok(analytics);
    }
}
