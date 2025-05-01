using Finance_Manager_Backend.BusinessLogic.Services.AuthServices;
using Microsoft.Extensions.Configuration;
using Finance_Manager_Tests.ServicesTests;
using AutoMapper;
using FluentAssertions;
using Finance_Manager_Backend.DataBase;
using Microsoft.Extensions.Caching.Memory;

namespace Finance_Manager_Backend_Tests.ServicesTests.AuthTests;

public class AuthenticateTest
{
    private readonly string email = "test@example.com";
    private readonly string password = "qwerty";
    private readonly TokenGenerator _tokenGenerator;
    private readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;
    private readonly AuthService _authService;

    public AuthenticateTest()
    {
        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.Test.json")
        .Build();

        _appDbContext = TestInMemoryDbContext.Create();
        _tokenGenerator = new TokenGenerator(config, _appDbContext);
        _mapper = AutoMapperFotTests.GetMapper();
        _authService = new AuthService(_appDbContext, _tokenGenerator, _mapper, new MemoryCache(new MemoryCacheOptions()));
    }

    [Fact]
    public async void AuthenticateUserFromDataBase_Test()
    {
        // Arrange

        // Act
        var registerUserDTO = await _authService.RegisterUserAsync(email, password);
        var authenticateUserDTO = await _authService.AuthenticateUserAsync(email, password);

        // Assert
        registerUserDTO.UserDTO.Should().BeEquivalentTo(authenticateUserDTO.UserDTO);
        registerUserDTO.AccessJwtToken.Should().BeEquivalentTo(authenticateUserDTO.AccessJwtToken);
        registerUserDTO.RefreshToken.Should().NotBeNull();
    }

    [Fact]
    public async void AuthenticateUserWithInvalidPasswordFromDataBase_Test()
    {
        // Arrange 

        // Act
        var userDTO = await _authService.RegisterUserAsync(email, password);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _authService.AuthenticateUserAsync(email, "invalid"));
    }
}
