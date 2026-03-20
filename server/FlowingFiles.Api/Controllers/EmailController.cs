using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlowingFiles.Core.Services;

namespace FlowingFiles.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly ILogger<EmailController> _logger;
    private readonly GmailService _gmailService;

    public EmailController(ILogger<EmailController> logger, GmailService gmailService)
    {
        _logger = logger;
        _gmailService = gmailService;
    }

    [HttpPost("send")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Send(
        [FromForm] string to,
        [FromForm] string subject,
        [FromForm] string body,
        [FromForm] IFormFileCollection attachments)
    {
        if (string.IsNullOrWhiteSpace(to))
            return BadRequest("Recipients are required");
        if (string.IsNullOrWhiteSpace(subject))
            return BadRequest("Subject is required");
        if (string.IsNullOrWhiteSpace(body))
            return BadRequest("Body is required");
        if (attachments?.Count == 0)
            return BadRequest("At least one attachment is required");

        try
        {
            var attachmentList = attachments
                .Select(f => (f.FileName, (Stream)f.OpenReadStream()))
                .ToList();

            await _gmailService.SendAsync(to, subject, body, attachmentList);
            return Ok(new { message = "Email sent successfully" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid email request to {To}", to);
            return BadRequest(ex.Message);
        }
        catch (SmtpCommandException ex)
        {
            _logger.LogError(ex, "SMTP error sending email to {To}", to);
            return StatusCode(500, "Failed to send email. Please verify recipient addresses.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error sending email to {To}", to);
            return StatusCode(500, "An unexpected error occurred while sending the email");
        }
    }
}
