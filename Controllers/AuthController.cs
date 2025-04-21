using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projectauth.IdentityModels;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Projectauth.DTOs;


namespace Projectauth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _config;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var userExist = await _userManager.FindByEmailAsync(model.Email);
        if (userExist != null)
            return BadRequest("User already exists.");

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // Assign default role: User
        if (!await _roleManager.RoleExistsAsync("User"))
            await _roleManager.CreateAsync(new ApplicationRole { Name = "User" });

        await _userManager.AddToRoleAsync(user, "User");

        return Ok("User registered successfully.");
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin(RegisterDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // Create Admin role if not exists
        if (!await _roleManager.RoleExistsAsync("Admin"))
            await _roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });

        await _userManager.AddToRoleAsync(user, "Admin");

        return Ok("Admin registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized("Invalid login");

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

var jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing in configuration");
var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }
}
