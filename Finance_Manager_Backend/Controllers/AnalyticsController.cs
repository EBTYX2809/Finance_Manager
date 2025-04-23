using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finance_Manager_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly AnalyticsService _analyticsService;
    public AnalyticsController(AnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    /// <summary>
    /// Get spending analytics in date range.
    /// </summary>
    /// <param name="queryDTO">QueryDTO with parameters:
    /// userId - User id;
    /// minDate - Start date of the filter range;
    /// maxDate - End date of the filter range.</param>
    /// <returns>
    /// Returns spending analytics grouped by category in a given date range.
    /// Each result shows the category and the percentage of total spending it represents.
    /// For example: { "Home" → 32.5, "Transport" → 15.0 }.
    /// </returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(List<CategoryPercentDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("overview")]
    public async Task<ActionResult<List<CategoryPercentDTO>>> GetAnalytics([FromQuery] AnalyticsQueryDTO queryDTO)
    {
        var analytics = await _analyticsService.GetAlalyticFromDateAsync(queryDTO.userId, queryDTO.minDate, queryDTO.maxDate);

        List<CategoryPercentDTO> categoryPercentDTOs = new();

        foreach (var a in analytics)
        {
            categoryPercentDTOs.Add(new CategoryPercentDTO { CategoryDTO = a.Key, Percent = a.Value });
        }

        return Ok(categoryPercentDTOs);
    }

    /// <summary>
    /// Get spending analytics from general category in date range.
    /// </summary>
    /// <param name="queryDTO">QueryDTO with parameters:
    /// parentCategoryId - Id parent category, that inner stats want to know;
    /// userId - User id;
    /// minDate - Start date of the filter range;
    /// maxDate - End date of the filter range.</param>
    /// <returns>
    /// Returns spending analytics grouped by category in a given date range.
    /// Each result shows the category and the percentage of total spending it represents.
    /// For example: { "Rent" → 32.5, "Furniture" → 15.0 }.
    /// </returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(List<CategoryPercentDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("inner_categories")]
    public async Task<ActionResult<List<CategoryPercentDTO>>> GetInnerAnalytics([FromQuery] InnerAnalyticsQueryDTO queryDTO)
    {
        var analytics = await _analyticsService.GetInnerAlalyticFromDateAsync(queryDTO.userId, queryDTO.parentCategoryId, queryDTO.minDate, queryDTO.maxDate);

        List<CategoryPercentDTO> categoryPercentDTOs = new();

        foreach (var a in analytics)
        {
            categoryPercentDTOs.Add(new CategoryPercentDTO { CategoryDTO = a.Key, Percent = a.Value });
        }

        return Ok(categoryPercentDTOs);
    }
}
