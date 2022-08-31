using System.Globalization;
using System.Net.Mail;
using System.Resources;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendEmail.Business.Exceptions;
using SendEmail.Business.Interfaces.Repositories;
using SendEmail.Business.Interfaces.Services;
using SendEmail.Business.Models;
using SendEmail.Business.ServiceModels;
using SendEmail.Business.Utils;

namespace SendEmail.Application.Services;

public class EmailManageService : IEmailManagerService
{
    private readonly ResourceSet _resourceSet;
    private readonly IConfiguration _configuration;

    #region Repositories
    private readonly ILogEmailRepository _logEmailRepository;
    #endregion
    
    #region Validators
    private readonly SendEmailModelValidator _sendEmailModelValidator;
    private readonly LogEmailValidator _logEmailValidator;
    #endregion
    
    public EmailManageService(
        ResourceManager resourceManager,
        CultureInfo cultureInfo,
        IConfiguration configuration,
        SendEmailModelValidator sendEmailModelValidator,
        ILogEmailRepository logEmailRepository, LogEmailValidator logEmailValidator)
    {
        _resourceSet = resourceManager.GetResourceSet(cultureInfo, true, true);
        _configuration = configuration;
        _sendEmailModelValidator = sendEmailModelValidator;
        _logEmailRepository = logEmailRepository;
        _logEmailValidator = logEmailValidator;
    }

    public async Task<string> SendEmail(SendEmailModel request)
    {
        await ValidationRequest(request);
        var serviceGmail = AuthAndGetService();
        await CreateAndSendEmail(serviceGmail, request.Email, request.Subject, request.Message);
        await RegisterLogEmail(request);
        return GetMessageResource("SEND-EMAIL-SEND_SUCCESS");
    }

    private async Task RegisterLogEmail(SendEmailModel request)
    {
        var logEmail = new LogEmail()
        {
            Email = request.Email,
            Message = request.Message,
            Subject = request.Subject
        };
        await ValidationRequest(logEmail);
        _logEmailRepository.Add(logEmail);
        await _logEmailRepository.SaveChanges();
    }

    #region Private Methods
    private async Task ValidationRequest(SendEmailModel request)
    {
        var requestValidator = await _sendEmailModelValidator.ValidateAsync(request);
        if (requestValidator.IsValid) return;
        var errors = requestValidator.Errors.Select(x => x.ErrorMessage).ToList();
        throw new CustomException(string.Join(" ", errors));
    }
    
    private async Task ValidationRequest(LogEmail logEmail)
    {
        var requestValidator = await _logEmailValidator.ValidateAsync(logEmail);
        if (requestValidator.IsValid) return;
        var errors = requestValidator.Errors.Select(x => x.ErrorMessage).ToList();
        throw new CustomException(string.Join(" ", errors));
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
    
    private string GetMessageResource(string name, params object[] parameters)
    {
        return parameters.Length > default(int) 
            ? _resourceSet.GetString(name)!.ResourceFormat(parameters) 
            : _resourceSet.GetString(name);
    }
    #endregion
}