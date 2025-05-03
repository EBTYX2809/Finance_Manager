using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Finance_Manager_Backend.Controllers;

[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/[controller]")]
public class AdministratorController : ControllerBase
{
    private readonly CategoriesService _categoriesService;
    public AdministratorController(CategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    /// <summary>
    /// Get category by id.
    /// </summary>
    /// <param name="id">Category id.</param>
    /// <returns>Returns CategoryDTO object.</returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found category.</response>     
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetCategory")]
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
    {
        return Ok(await _categoriesService.GetCategoryByIdAsync(id));
    }

    /// <summary>
    /// Create all categories and seed them in data base.
    /// </summary>
    /// <returns>Ok</returns>
    /// <response code="204">Success.</response>
    /// <response code="403">Authorization failed.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "CreateAllCategories")]
    [HttpPost("seed")]
    public async Task<ActionResult> CreateAllCategories()
    {
        await _categoriesService.CreateAllCategoriesAsync();

        return Ok();
    }

    /// <summary>
    /// Create category.
    /// </summary>
    /// <param name="categoryDTO">CategoryDTO to create.</param>
    /// <returns>Returns the ID of the created category.</returns>
    /// <response code="201">Category successfully created.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="401">Not authorized. Possible invalid token.</response>
    /// <response code="403">Authorization failed.</response>
    /// <response code="500">Internal server error.</response>
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "CreateCategory")]
    [HttpPost("create")]
    public async Task<ActionResult<int>> CreateCategory([FromBody] CategoryDTO categoryDTO)
    {
        await _categoriesService.CreateNewCategoryAsync(categoryDTO);

        return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDTO.Id }, categoryDTO.Id);
    }

    /// <summary>
    /// Update category.
    /// </summary>
    /// <param name="categoryDTO">New categoryDTO object.</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="401">Not authorized. Possible invalid token.</response>
    /// <response code="403">Authorization failed.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "UpdateCategory")]
    [HttpPut]
    public async Task<ActionResult> UpdateCategory([FromBody] CategoryDTO categoryDTO)
    {
        await _categoriesService.UpdateCategoryAsync(categoryDTO);

        return NoContent();
    }

    /// <summary>
    /// Delete category by id.
    /// </summary>
    /// <param name="id">Category id.</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success.</response>
    /// <response code="401">Not authorized. Possible invalid token.</response>
    /// <response code="403">Authorization failed.</response>
    /// <response code="404">Not found category.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "DeleteCategory")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        await _categoriesService.DeleteCategoryAsync(id);

        return NoContent();
    }
}
