using System.Globalization;
using System.Resources;
using FluentValidation;
using SendEmail.Business.Utils;

namespace SendEmail.Business.Models;

public class LogEmail : Entity
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}

public class LogEmailValidator : AbstractValidator<LogEmail>
{
    public LogEmailValidator(ResourceManager resourceManager, CultureInfo cultureInfo)
    {
        var resourceSet = resourceManager.GetResourceSet(cultureInfo, true, true);

        RuleFor(lg => lg.Email)
            .NotEmpty()
            .WithMessage(resourceSet.GetResourceFormat("LOGEMAIL-EMAIL_EMPTY"))
            .EmailAddress()
            .WithMessage(resourceSet.GetResourceFormat("LOGEMAIL-INVALID_EMAIL"));

        RuleFor(lg => lg.Message)
            .NotEmpty()
            .WithMessage(resourceSet.GetResourceFormat("LOGEMAIL-MESSAGE_EMPTY"));
    }
}