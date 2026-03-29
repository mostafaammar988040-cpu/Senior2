using System;
using System.ComponentModel.DataAnnotations;

public class Users
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    // Role is nvarchar in DB, so keep it as string
    public string Role { get; set; } = "User";   // default role

    public bool IsBlocked { get; set; } = false;

    // These columns are nullable in DB, so mark them nullable in C#
    public string? ProfileImageUrl { get; set; }
    public string? Bio { get; set; }

    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiry { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}