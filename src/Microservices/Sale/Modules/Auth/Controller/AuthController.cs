using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Spl.Crm.SaleOrder.Cache.Redis.Service;
using Spl.Crm.SaleOrder.Cache.Redis.Service.implement;
using Spl.Crm.SaleOrder.Modules.Auth.Model;
using Spl.Crm.SaleOrder.Modules.Auth.Service;

namespace Spl.Crm.SaleOrder.Modules.Auth.Controller;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;
     
    private IMasterConfigCacheService masterConfigCache;
    private IUserCacheService userCacheService;

    public AuthController(IAuthService authService, 
                            IMasterConfigCacheService masterConfigCache,
                            IUserCacheService userCacheService)
    {
        this.authService = authService; 
        this.masterConfigCache = masterConfigCache;
        this.userCacheService = userCacheService;
    }

    [HttpGet("get/{id}", Name = "test")]
    public IActionResult test([FromRoute][Required] string id)
    {   // Test design structure
        String result = authService.Login(new LoginRequest());
        return new OkObjectResult(result);
    }


    [HttpGet("redis")]
    public IActionResult addRedisData()
    {

         var val = userCacheService.Get<string>("UserCacheService");
         //userCacheService.Set("UserCacheService", "value");
         //userCacheService.Refresh("UserCacheService");


        return new OkObjectResult("okay");
    }

}