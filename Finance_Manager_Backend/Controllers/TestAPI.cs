using Microsoft.AspNetCore.Mvc;

namespace Finance_Manager_Backend.Controllers;

[ApiController]
[Route("api/test")]
public class TestAPI : ControllerBase
{
    [HttpGet]
    public string HelloWorld() => "Hello World from API";
}
