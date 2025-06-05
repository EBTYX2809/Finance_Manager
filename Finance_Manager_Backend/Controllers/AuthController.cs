using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.BusinessLogic.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using Finance_Manager_Backend.Middleware;

namespace Finance_Manager_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Register new user.
    /// </summary>
    /// <param name="authDataDTO">AuthDataDTO with email and password</param>
    /// <returns>AuthUserTokensDTO with parameters:
    /// UserDTO - user dto object with id, email, and balance;
    /// AccessJwtToken - new generated jwt token;
    /// RefreshToken - new generated refresh token.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Invalid credentials.</response>    
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(AuthUserTokensDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "Register")]
    [HttpPost("register")]
    public async Task<ActionResult<AuthUserTokensDTO>> Register([FromBody] AuthDataDTO authDataDTO)
    {
        var authUserTokensDTO = await _authService.RegisterUserAsync(authDataDTO.email, authDataDTO.password);

        return Ok(authUserTokensDTO);
    }

    /// <summary>
    /// Authenticate user.
    /// </summary>
    /// <param name="authDataDTO">AuthDataDTO with email and password</param>
    /// <returns>AuthUserTokensDTO with parameters:
    /// UserDTO - user dto object with id, email, and balance;
    /// AccessJwtToken - new generated jwt token;
    /// RefreshToken - new generated refresh token.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Invalid credentials.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(AuthUserTokensDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "Authenticate")]
    [HttpPost("authenticate")]
    public async Task<ActionResult<AuthUserTokensDTO>> Authenticate([FromBody] AuthDataDTO authDataDTO)
    {
        var authUserTokensDTO = await _authService.AuthenticateUserAsync(authDataDTO.email, authDataDTO.password);

        return Ok(authUserTokensDTO);
    }

    /// <summary>
    /// Authenticate user with refresh token.
    /// </summary>
    /// <param name="refreshToken">Refresh token.</param>
    /// <returns>AuthUserTokensDTO with parameters:
    /// UserDTO - user dto object with id, email, and balance;
    /// AccessJwtToken - new generated jwt token;
    /// RefreshToken - new generated refresh token.</returns>    
    /// <response code="200">Success.</response>
    /// <response code="400">Invalid refresh token.</response>
    /// <response code="404">User with this token not found.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(AuthUserTokensDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "RefreshToken")]
    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthUserTokensDTO>> RefreshToken([FromBody] string refreshToken)
    {
        var authUserTokensDTO = await _authService.AuthenticateUserWithRefreshTokenAsync(refreshToken);

        return Ok(authUserTokensDTO);
    }
}
