using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace FlowingFiles.Core.Services;

public class GmailService
{
    private const string GMAIL_SMTP_HOST = "smtp.gmail.com";
    private const int GMAIL_SMTP_PORT = 587;
    private readonly string _user;
    private readonly string _appPassword;

    public GmailService(IConfiguration config)
    {
        _user = config["Gmail:User"] ?? throw new InvalidOperationException("Gmail:User not configured");
        _appPassword = config["Gmail:AppPassword"] ?? throw new InvalidOperationException("Gmail:AppPassword not configured");
    }

    public async Task SendAsync(string to, string subject, string body, IEnumerable<(string fileName, Stream content)> attachments)
    {
        if (string.IsNullOrWhiteSpace(to))
            throw new ArgumentException("Recipients cannot be empty", nameof(to));
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject cannot be empty", nameof(subject));
        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Body cannot be empty", nameof(body));

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_user));

        // Parse comma-separated recipients
        var recipients = to.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(r => r.Trim())
            .Where(r => !string.IsNullOrEmpty(r))
            .ToList();

        if (recipients.Count == 0)
            throw new ArgumentException("No valid email addresses provided", nameof(to));

        foreach (var recipient in recipients)
            message.To.Add(MailboxAddress.Parse(recipient));

        message.Subject = subject;

        var builder = new BodyBuilder { TextBody = body };

        foreach (var (fileName, content) in attachments)
        {
            using var ms = new MemoryStream();
            await content.CopyToAsync(ms);
            builder.Attachments.Add(fileName, ms.ToArray());
        }

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(GMAIL_SMTP_HOST, GMAIL_SMTP_PORT, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_user, _appPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
