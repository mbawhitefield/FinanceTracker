using FinanceTracker.API.DTOs.Auth;
using FinanceTracker.API.Entities;
using FinanceTracker.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        // check if email exists
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
            return BadRequest("Email is already in use");

        var user = new AppUser
        {
            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(new AuthResponseDto
        {
            Token = _tokenService.CreateToken(user),
            Email = user.Email!,
            FullName = user.FullName,
        });

    }

    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null) return Unauthorized("Invalid email or password.");

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, dto.Passsword, false);

        if (!result.Succeeded) return Unauthorized("Invalid email of password.");

        return Ok(new AuthResponseDto
        {
            Token = _tokenService.CreateToken(user),
            Email = user.Email!,
            FullName = user.FullName,
        });

    }

}
