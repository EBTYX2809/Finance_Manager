﻿using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using Finance_Manager_Backend.Middleware;

namespace Finance_Manager_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoriesService _categoriesService;

    public CategoriesController(CategoriesService categoriesService)
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
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetCategory")]
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
    {
        return Ok(await _categoriesService.GetCategoryDTOByIdAsync(id));
    }

    /// <summary>
    /// Get list of all categories.
    /// </summary>
    /// <returns>Returns list woth categoryDTO objects.</returns>
    /// <response code="200">Success.</response>    
    /// <response code="500">Internal server error.</response>
    [ProducesResponseType(typeof(List<CategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetAllCategories")]
    [HttpGet]
    public async Task<ActionResult<List<CategoryDTO>>> GetAllCategories()
    {
        return Ok(await _categoriesService.GetAllCategoriesAsync());
    }
}
