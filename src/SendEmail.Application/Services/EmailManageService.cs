using System.Net.Mail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendEmail.Application.Interfaces.Services;
using SendEmail.Application.Models;

namespace SendEmail.Application.Services;

public class EmailManageService : IEmailManageService
{
    private readonly IConfiguration _configuration;
    private readonly SendEmailModelValidator _sendEmailModelValidator;
    
    public EmailManageService(
        IConfiguration configuration,
        SendEmailModelValidator sendEmailModelValidator)
    {
        _configuration = configuration;
        _sendEmailModelValidator = sendEmailModelValidator;
    }

    public async Task SendEmail(SendEmailModel request)
    {
        await ValidationRequest(request);
        var serviceGmail = AuthAndGetService();
        await CreateAndSendEmail(serviceGmail, request.Email, request.Subject, request.Message);
    }

    #region Private Methods
    private async Task ValidationRequest(SendEmailModel request)
    {
        var requestValidator = await _sendEmailModelValidator.ValidateAsync(request);
        if(requestValidator.IsValid) return;
        var errors = requestValidator.Errors.Select(x => x.ErrorMessage).ToList();
        throw new Exception(string.Join(", ", errors));
    }
    
    private GmailService AuthAndGetService()
    {
        var clientId = _configuration.GetSection("GmailApiCredentials")["client_id"];
        var clientSecret = _configuration.GetSection("GmailApiCredentials")["client_secret"];
        var applicatioName = _configuration.GetSection("GmailApiCredentials")["application_name"];
        
        var clientSecrets = new ClientSecrets()
        {
            ClientId = clientId, ClientSecret = clientSecret,
        };

        var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            clientSecrets,
            new []{ GmailService.Scope.MailGoogleCom },
            "user",
            CancellationToken.None,
            new FileDataStore("token.json", true)
        ).Result;
        
        return new GmailService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = applicatioName
        });
    }
    
    private async Task CreateAndSendEmail(
        GmailService gmailService,
        string emailTo, 
        string subject, 
        string message)
    {
        var newMail = new MailMessage();
        newMail.To.Add(new MailAddress(emailTo));
        newMail.Subject = subject;
        newMail.Body = message;
        newMail.IsBodyHtml = true;
        var mimeMessage = MimeMessage.CreateFromMailMessage(newMail);
        var msg = new Message
        {
            Raw = Base64UrlEncode(mimeMessage.ToString())
        };
        await gmailService.Users.Messages.Send(msg, "me").ExecuteAsync();
    }
    
    private string Base64UrlEncode(string input)
    {
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(inputBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
    }
    #endregion
}