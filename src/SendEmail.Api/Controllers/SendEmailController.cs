using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SendEmail.Api.ViewModels;
using SendEmail.Application.Interfaces.Services;
using SendEmail.Application.Models;

namespace SendEmail.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SendEmailController : ControllerBase
{
    private readonly ILogger<SendEmailController> _logger;
    private readonly IMapper _mapper;
    private readonly IEmailManageService _emailManageService;

    public SendEmailController(
        ILogger<SendEmailController> logger,
        IMapper mapper,
        IEmailManageService emailManageService)
    {
        _logger = logger;
        _emailManageService = emailManageService;
        _mapper = mapper;
    }
    
    [HttpPost("SendEmail")]
    public async Task<IActionResult> SendEmail(SendEmailViewModel req)
    {
        try
        {
            var request = _mapper.Map<SendEmailModel>(req);
            await _emailManageService.SendEmail(request);
            return Ok("Email successfully sent!");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}