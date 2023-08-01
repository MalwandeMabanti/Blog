using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Data;
using ToDoList.Models;


[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<BlogUser> _userManager;
    private readonly SignInManager<BlogUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationController> _logger;
   


    public AuthenticationController(UserManager<BlogUser> userManager, 
        SignInManager<BlogUser> signInManager, 
        IConfiguration configuration, 
        ILogger<AuthenticationController> logger)
        
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
        
    }

    // POST api/authentication/register
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] Register model)
    {
        var existingUser = await this._userManager.FindByEmailAsync(model.Email);


        if (existingUser != null)
        {
            return this.BadRequest("User exists for this email");

        }

        var user = new BlogUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest("There was a problem creating the user");
        }


        return Ok(new { token = GenerateJsonWebToken(user), message = "User registered successfully!" });


    }

    // POST api/authentication/login
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] Login model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(this.ModelState);
        }



        var user = await _userManager.FindByNameAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var token = GenerateJsonWebToken(user);

            _logger.LogInformation($"Token before sending to client: {token}");


            return Ok(new { Token = token });
        }
        return Unauthorized("You are not authorized");
    }

    private string GenerateJsonWebToken(BlogUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.GivenName, user.FirstName + " " + user.LastName),
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}