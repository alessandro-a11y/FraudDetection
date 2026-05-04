using FraudDetection.Application.DTOs;
using FraudDetection.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FraudDetection.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<AppUser> userManager,
        TokenService tokenService,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new AppUser
        {
            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Falha no registro do usuário {Email}: {Errors}",
                dto.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return BadRequest(result.Errors);
        }

        var role = dto.Role == "Admin" ? "Admin" : "User";
        await _userManager.AddToRoleAsync(user, role);

        _logger.LogInformation("Usuário registrado: {Email} com role {Role}", dto.Email, role);

        var (token, expiresAt) = _tokenService.GenerateToken(user, role);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            Role = role,
            ExpiresAt = expiresAt
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            _logger.LogWarning("Tentativa de login inválida para {Email}", dto.Email);
            return Unauthorized("Credenciais inválidas.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        _logger.LogInformation("Login bem-sucedido: {Email}", dto.Email);

        var (token, expiresAt) = _tokenService.GenerateToken(user, role);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            Role = role,
            ExpiresAt = expiresAt
        });
    }
}