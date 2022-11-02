using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using ClassifiedAds.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc;
using Spl.Crm.SaleOrder.Modules.Auth.Model;
using Spl.Crm.SaleOrder.Modules.Auth.Service;
using ClassifiedAds.Application;

namespace Spl.Crm.SaleOrder.Modules.Auth.Controller;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAppLogger _logger;
    private readonly IAuthService authService;

    public AuthController(IAuthService authService, IAppLogger _logger)
    {
        this.authService = authService;
        this._logger = _logger ;
    }

    [HttpGet("get/{id}", Name = "test")]
    public IActionResult test([FromRoute][Required] string id)
    {   // Test design structure
        String result = authService.Login(new LoginRequest());
        return new OkObjectResult(result);
    }

    [HttpGet]
    public IActionResult GetLog()
    {
        _logger.Debug("Hello from GetLog");
        _logger.Info("Hello from GetLog");
        _logger.Error("Hello from GetLog");
        _logger.Warn("Hello from GetLog");
        return new OkObjectResult("Hello world");
    }
}