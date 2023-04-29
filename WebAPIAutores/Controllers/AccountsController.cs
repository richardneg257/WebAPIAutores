using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIAutores.Dtos;

namespace WebAPIAutores.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponseDto>> Register([FromBody] UserCredentialsDto userCredentials)
    {
        var user = new IdentityUser()
        {
            UserName = userCredentials.Email,
            Email = userCredentials.Email
        };

        var result = await _userManager.CreateAsync(user, userCredentials.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return BuildToken(userCredentials);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponseDto>> Login(UserCredentialsDto userCredentials)
    {
        var result = await _signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);
        if (!result.Succeeded) return BadRequest("Login Incorrecto");

        return BuildToken(userCredentials);
    }

    private AuthenticationResponseDto BuildToken(UserCredentialsDto userCredentials)
    {
        var claims = new List<Claim>()
        {
            new Claim("email", userCredentials.Email),
            new Claim("lo que yo quiera", "cualquier otro valor")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtKey")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddDays(1);

        var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, 
            expires: expiration, signingCredentials: credentials);

        return new AuthenticationResponseDto()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expiration = expiration
        };
    }
}
