using FluentValidation;

namespace SendEmail.Application.Models;

public class SendEmailModel
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}

public class SendEmailModelValidator : AbstractValidator<SendEmailModel>
{
    public SendEmailModelValidator()
    {
        RuleFor(r => r.Email)
            .EmailAddress()
            .WithMessage("Must be a valid email");
        
        RuleFor(r => r.Message)
            .NotEmpty()
            .WithMessage("The email must have a message");
    }
}