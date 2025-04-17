using Finance_Manager_Backend.BusinessLogic.Models;
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
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <returns>AuthUserTokenDTO - user object with jwt token.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Invalid credentials.</response>    
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(AuthUserTokenDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]    
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("register")]
    public async Task<ActionResult<AuthUserTokenDTO>> Register(string email, string password)
    {
        var (user, token) = await _authService.RegisterUserAsync(email, password);

        return Ok(new AuthUserTokenDTO { User = user, Token = token });
    }

    /// <summary>
    /// Authenticate user.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <returns>AuthUserTokenDTO - user object with jwt token.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Invalid credentials.</response>
    /// <response code="500">Internal server error.</response> 
    [ProducesResponseType(typeof(AuthUserTokenDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("authenticate")]
    public async Task<ActionResult<AuthUserTokenDTO>> Authenticate(string email, string password)
    {
        var (user, token) = await _authService.AuthenticateUserAsync(email, password);

        return Ok(new AuthUserTokenDTO { User = user, Token = token });
    }
}

/// <response code="401">Not authorized. Possible invalid token.</response>
[ProducesResponseType(StatusCodes.Status401Unauthorized)]

public class AuthUserTokenDTO
{
    public User User { get; set; }
    public string Token { get; set; }
}
