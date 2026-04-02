using BCrypt.Net;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Senior2.Api.Data;
using Senior2.Api.DTOs.Auth;
using Senior2.Api.Models;
using Senior2.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
namespace Senior2.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly EmailService _emailService;

        public AuthController(
            AppDbContext context,
            IConfiguration config,
            EmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        // =====================
        // REGISTER
        // =====================
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existingUser != null)
                return BadRequest("Email already registered");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new Users
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = "User" // default role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Account created successfully"
            });
        }

        // =====================
        // LOGIN
        // =====================
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == dto.EmailOrUsername);

            if (user == null)
                return Unauthorized("Invalid credentials");

            if (user.IsBlocked)
                return Unauthorized("Your account has been blocked by the administrator.");

            bool passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!passwordValid)
                return Unauthorized("Invalid credentials");

            // ===== JWT CLAIMS =====

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, user.Role) // ⭐ important
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:ExpiresInMinutes"])
                ),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiresIn = token.ValidTo,
                user = new
                {
                    id = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    role = user.Role
                }
            });
        }

        // =====================
        // FORGOT PASSWORD
        // =====================
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return Ok("If the email exists, a reset link has been sent.");

            var token = Guid.NewGuid().ToString();

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);

            await _context.SaveChangesAsync();

            var resetLink = $"http://localhost:5173/reset-password?token={token}";

            var emailBody = $@"
                <h2>Password Reset Request</h2>
                <p>Hello {user.FirstName},</p>
                <p>Click the button below to reset your password:</p>

                <a href='{resetLink}'
                   style='padding:10px 20px;
                          background:#0dc052;
                          color:white;
                          text-decoration:none;
                          border-radius:6px;'>

                   Reset Password
                </a>

                <p>This link expires in 30 minutes.</p>
            ";

            await _emailService.SendEmailAsync(
                user.Email,
                "Reset Your Password - Senior2",
                emailBody
            );

            return Ok("Reset link sent to your email.");
        }

        // =====================
        // RESET PASSWORD
        // =====================
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.PasswordResetToken == dto.Token);

            if (user == null ||
                user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired token");
            }

            user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _context.SaveChangesAsync();

            return Ok("Password reset successful");
        }

        // =====================
        // GOOGLE LOGIN
        // =====================
        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleDto dto)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    dto.IdToken,
                    new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new List<string>
                        {
                            _config["Google:ClientId"]
                        }
                    });

                var email = payload.Email;
                var firstName = payload.GivenName ?? "Google";
                var lastName = payload.FamilyName ?? "User";

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    user = new Users
                    {
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        Role = "User"
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"])
                );

                var creds = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256
                );

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        int.Parse(_config["Jwt:ExpiresInMinutes"])
                    ),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiresIn = token.ValidTo,
                    user = new
                    {
                        id = user.Id,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        email = user.Email,
                        role = user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return Unauthorized($"Google token validation failed: {ex.Message}");
            }
        }
    }
}