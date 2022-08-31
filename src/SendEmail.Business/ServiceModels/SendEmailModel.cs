using System.Globalization;
using System.Resources;
using FluentValidation;
using SendEmail.Business.Utils;

namespace SendEmail.Business.ServiceModels;

public class SendEmailModel
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}

public class SendEmailModelValidator : AbstractValidator<SendEmailModel>
{
    public SendEmailModelValidator(ResourceManager resourceManager, CultureInfo cultureInfo)
    {
        var resourceSet = resourceManager.GetResourceSet(cultureInfo, true, true);
        
        RuleFor(r => r.Email)
            .EmailAddress()
            .WithMessage(resourceSet!.GetResourceFormat("SEND-EMAIL-INVALID_EMAIL"));
        
        RuleFor(r => r.Message)
            .NotEmpty()
            .WithMessage(resourceSet!.GetResourceFormat("SEND-EMAIL-MESSAGE_EMPTY"));
    }
}