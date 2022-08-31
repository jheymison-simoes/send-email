using SendEmail.Business.ServiceModels;

namespace SendEmail.Business.Interfaces.Services;

public interface IEmailManagerService
{
    Task<string> SendEmail(SendEmailModel request);
}