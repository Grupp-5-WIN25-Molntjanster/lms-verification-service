using System;

namespace VerificationService.Models;

public class VerificationCode
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public bool IsUsed { get; set; } = false;
}