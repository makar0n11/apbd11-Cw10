using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeFirst.Data;
using CodeFirst.Models.LoginModels;
using GakkoHorizontalSlice.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CodeFirst.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : Controller
{
    
    private readonly ApplicationContext _context;
    private readonly IConfiguration _configuration;

    public AuthorizationController(ApplicationContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult RegisterStudent(Login loginModel)
    {
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(loginModel.Password);
        
        var user = new User()
        {
            Username = loginModel.Username,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok();
    }
    
    
    
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(Login login)
    {
        User user = _context.Users.Where(u => u.Username == login.Username).FirstOrDefault();

        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(login.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return Unauthorized();
        }


        Claim[] userclaim = new[]
        {
            new Claim(ClaimTypes.Name, "szef"),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Role, "admin")
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: userclaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        });
    }
    
    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public IActionResult Refresh(TokenModel refreshToken)
    {
        User user = _context.Users.Where(u => u.RefreshToken == refreshToken.RefreshToken).FirstOrDefault();
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }
        
        Claim[] userclaim = new[]
        {
            new Claim(ClaimTypes.Name, "szef"),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Role, "admin")
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: userclaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            refreshToken = user.RefreshToken
        });
    }
}