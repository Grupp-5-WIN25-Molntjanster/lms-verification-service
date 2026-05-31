namespace VerificationService.Models;

public class ValidateVerificationRequest
{
    public string Email { get; set; } = string.Empty;
    public int Code { get; set; }
}