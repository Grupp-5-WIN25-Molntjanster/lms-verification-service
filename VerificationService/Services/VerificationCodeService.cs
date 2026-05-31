using VerificationService.Data;
using VerificationService.Models;

namespace VerificationService.Services;

public class VerificationCodeService
{
    private readonly ApplicationDbContext _dbContext;

    public VerificationCodeService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GenerateAndSaveCodeAsync(string email)
    {
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();

        var verificationCode = new VerificationCode
        {
            Email = email,
            Code = code,
            CreatedAt = DateTime.UtcNow,
            IsUsed = false
        };

        _dbContext.VerificationCodes.Add(verificationCode);
        await _dbContext.SaveChangesAsync();

        return code;
    }
}