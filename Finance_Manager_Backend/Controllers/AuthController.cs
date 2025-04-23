using Finance_Manager_Backend.BusinessLogic.Models.DTOs;
using Finance_Manager_Backend.BusinessLogic.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;

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
    /// <returns>AuthUserTokenDTO - userDTO with jwt token.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Invalid credentials.</response>    
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(AuthUserTokenDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]    
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("register")]
    public async Task<ActionResult<AuthUserTokenDTO>> Register([FromBody] AuthDataDTO authDataDTO)
    {
        var (userDTO, token) = await _authService.RegisterUserAsync(authDataDTO.email, authDataDTO.password);

        return Ok(new AuthUserTokenDTO { UserDTO = userDTO, Token = token });
    }

    /// <summary>
    /// Authenticate user.
    /// </summary>
    /// <param name="authDataDTO">AuthDataDTO with email and password</param>
    /// <returns>AuthUserTokenDTO - userDTO with jwt token.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Invalid credentials.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(AuthUserTokenDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("authenticate")]
    public async Task<ActionResult<AuthUserTokenDTO>> Authenticate([FromBody] AuthDataDTO authDataDTO)
    {
        var (userDTO, token) = await _authService.AuthenticateUserAsync(authDataDTO.email, authDataDTO.password);

        return Ok(new AuthUserTokenDTO { UserDTO = userDTO, Token = token });
    }
}
