using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SendEmail.Api.ViewModels;
using SendEmail.Business.Exceptions;
using SendEmail.Business.Interfaces.Services;
using SendEmail.Business.ServiceModels;

namespace SendEmail.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SendEmailController : BaseController<SendEmailController>
{
    private readonly IEmailManagerService _emailManageService;

    public SendEmailController(
        ILogger<SendEmailController> logger,
        IMapper mapper,
        IEmailManagerService emailManageService) : base(logger, mapper)
    {
        _emailManageService = emailManageService;
    }
    
    [HttpPost("Send")]
    public async Task<ActionResult<BaseResponse<string>>> SendEmail(SendEmailViewModel req)
    {
        try
        {
            var request = Mapper.Map<SendEmailModel>(req);
            var result = await _emailManageService.SendEmail(request);
            return BaseResponseSuccess(result);
        }
        catch (CustomException cEx)
        {
            return BaseResponseError(cEx.Message);
        }
        catch (Exception ex)
        {
            return BaseResponseInternalError(ex.Message);
        }
    }
}