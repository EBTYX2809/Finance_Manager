using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Services;
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

    /// <summary>
    /// Get spending analytics in date range.
    /// </summary>
    /// <param name="userId">User id.</param>
    /// <param name="minDate">Start date of the filter range.</param>
    /// <param name="maxDate">End date of the filter range.</param>
    /// <returns>
    /// Returns spending analytics grouped by category in a given date range.
    /// Each result shows the category and the percentage of total spending it represents.
    /// For example: { "Home" → 32.5, "Transport" → 15.0 }.
    /// </returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(List<CategoryPercentDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("overview")]
    public async Task<ActionResult<List<CategoryPercentDTO>>> GetAnalytics(
        [FromQuery] int userId,
        [FromQuery] DateTime minDate, 
        [FromQuery] DateTime maxDate)
    {
        var analytics = await _analyticsService.GetAlalyticFromDateAsync(userId, minDate, maxDate);

        List<CategoryPercentDTO> categoryPercentDTOs = new();

        foreach (var a in analytics)
        {
            categoryPercentDTOs.Add(new CategoryPercentDTO { Category = a.Key, Percent = a.Value });
        }

        return Ok(categoryPercentDTOs);
    }

    /// <summary>
    /// Get spending analytics from general category in date range.
    /// </summary>
    /// <param name="parentCategoryId">Id parent category, that inner stats want to know.</param>
    /// <param name="userId">User id.</param>
    /// <param name="minDate">Start date of the filter range.</param>
    /// <param name="maxDate">End date of the filter range.</param>
    /// <returns>
    /// Returns spending analytics grouped by category in a given date range.
    /// Each result shows the category and the percentage of total spending it represents.
    /// For example: { "Rent" → 32.5, "Furniture" → 15.0 }.
    /// </returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(List<CategoryPercentDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("inner_categories")]
    public async Task<ActionResult<List<CategoryPercentDTO>>> GetInnerAnalytics(
        [FromQuery] int parentCategoryId,
        [FromQuery] int userId,
        [FromQuery] DateTime minDate,
        [FromQuery] DateTime maxDate)
    {
        var analytics = await _analyticsService.GetInnerAlalyticFromDateAsync(userId, parentCategoryId, minDate, maxDate);

        List<CategoryPercentDTO> categoryPercentDTOs = new();

        foreach (var a in analytics)
        {
            categoryPercentDTOs.Add(new CategoryPercentDTO { Category = a.Key, Percent = a.Value });
        }

        return Ok(categoryPercentDTOs);
    }
}

public class CategoryPercentDTO
{
    public Category Category { get; set; }
    public float Percent { get; set; }
}
