using AutoMapper;
using SendEmail.Api.ViewModels;
using SendEmail.Business.ServiceModels;

namespace SendEmail.Api.Configuration;

public class SendEmailMapperConfiguration : Profile
{
    public SendEmailMapperConfiguration()
    {
        CreateMap<SendEmailViewModel, SendEmailModel>().ReverseMap();
    }
    
}