using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using Finance_Manager_Backend.Middleware;

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
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize]
    [SwaggerOperation(OperationId = "UpdateCurrency")]
    [HttpPut]
    public async Task<ActionResult> UpdateCurrency([FromBody] UpdateUserCurrencyQueryDTO currencyQueryDTO)
    {
        await _usersService.UpdateUserCurrencyAsync(currencyQueryDTO.UserId, currencyQueryDTO.CurrencyRang, currencyQueryDTO.CurrencyCode);

        return NoContent();
    }

    /// <summary>
    /// Get user id by telegram id.
    /// </summary>
    /// <param name="telegramId">Telegram id</param>
    /// <returns>Returns user id</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Wrong telegram id.</response>
    /// <response code="500">Internal server error.</response>  
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetUserIdByTelegramId")]
    [HttpGet]
    public async Task<ActionResult<int>> GetUserIdByTelegramId(string telegramId)
    {
        int id = await _usersService.GetUserIdByTelegramIdAsync(telegramId);

        return Ok(id);
    }

    /// <summary>
    /// Add telegram id to user
    /// </summary>
    /// <param name="userIdTelegramIdDTO">DTO object with UserId and TelegramId</param>
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
    [SwaggerOperation(OperationId = "GetUserIdByTelegramId")]
    [HttpPut]
    public async Task<ActionResult> AddTelegramIdToUser([FromBody] UserIdTelegramIdDTO userIdTelegramIdDTO)
    {
        await _usersService.AddTelegramIdToUserAsync(userIdTelegramIdDTO.UserId, userIdTelegramIdDTO.TelegramId);

        return NoContent();
    }
}
