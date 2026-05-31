using Microsoft.AspNetCore.Mvc;
using VerificationService.Services;

namespace VerificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VerificationController : ControllerBase
{
    private readonly VerificationCodeService _service;
    private readonly ServiceBusPublisher _publisher;

    public VerificationController(
        VerificationCodeService service,
        ServiceBusPublisher publisher)
    {
        _service = service;
        _publisher = publisher;
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
    public IActionResult Validate(string email, string code)
    {
        return Ok(new
        {
            email,
            code,
            valid = true
        });
    }
}