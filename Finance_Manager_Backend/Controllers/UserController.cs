using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Finance_Manager_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UsersService _usersService;
    public UserController(UsersService usersService)
    {
        _usersService = usersService;
    }

    /// <summary>
    /// Get user balance by id.
    /// </summary>
    /// <param name="id">User id.</param>
    /// <returns>Returns UserBalanceDTO with parameters: 
    /// PrimaryBalance - object with currency code and balance in primary currency; 
    /// SecondaryBalance1 - object with currency code and balance converted from primary to this; 
    /// SecondaryBalance2 - object with currency code and balance converted from primary to this.</returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Not found user.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(UserBalanceDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetBalance")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserBalanceDTO>> GetBalanceById(int id)
    {
        var balanceDTO = await _usersService.GetUserBalanceByIdAsync(id);

        return Ok(balanceDTO);
    }

    /// <summary>
    /// Update user currency.
    /// </summary>
    /// <param name="currencyQueryDTO">currencyQueryDTO with parameters:
    /// UserId - user id;
    /// CurrencyRang - rang of currency that will be update, can be "Primary", "Secondary1" or "Secondary2".
    /// CurrencyCode - code for this currency.</param>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Success.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="404">Not found some resource.</response>
    /// <response code="401">Not authorized. Possible invalid token.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    [SwaggerOperation(OperationId = "UpdateCurrency")]
    [HttpPut]
    public async Task<ActionResult> UpdateCurrency([FromBody] UpdateUserCurrencyQueryDTO currencyQueryDTO)
    {
        await _usersService.UpdateUserCurrencyAsync(currencyQueryDTO.UserId, currencyQueryDTO.CurrencyRang, currencyQueryDTO.CurrencyCode);

        return NoContent();
    }
}
