using Microsoft.AspNetCore.Mvc;
using VerificationService.Data;
using VerificationService.Models;
using VerificationService.Services;

namespace VerificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VerificationController : ControllerBase
{
    private readonly VerificationCodeService _service;
    private readonly ServiceBusPublisher _publisher;
    private readonly ApplicationDbContext _dbContext;

    public VerificationController(
        VerificationCodeService service,
        ServiceBusPublisher publisher,
        ApplicationDbContext dbContext)
    {
        _service = service;
        _publisher = publisher;
        _dbContext = dbContext;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send(string email)
    {
        var code = await _service.GenerateAndSaveCodeAsync(email);

        await _publisher.SendVerificationEmailAsync(email, code);

        return Ok(new
        {
            message = "Verification code queued"
        });
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate(string email, string code)
    {
        var verification = _dbContext.VerificationCodes
            .Where(v => v.Email == email)
            .OrderByDescending(v => v.CreatedAt)
            .FirstOrDefault();

        if (verification == null)
        {
            return BadRequest("No code found for this email");
        }

        if (verification.Code != code)
        {
            return BadRequest("Wrong code");
        }

        if (verification.IsUsed)
        {
            return BadRequest("Code already used");
        }

        verification.IsUsed = true;
        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            message = "Code is valid"
        });
    }
}