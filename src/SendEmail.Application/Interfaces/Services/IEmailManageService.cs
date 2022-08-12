using SendEmail.Application.Models;

namespace SendEmail.Application.Interfaces.Services;

public interface IEmailManageService
{
    Task SendEmail(SendEmailModel request);
}