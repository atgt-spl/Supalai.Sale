﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Spl.Crm.SaleOrder.Services;

namespace Spl.Crm.SaleOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {

        private readonly ISaleService saleService;
        public SaleController(ISaleService saleService)
        {
            this.saleService = saleService;
        }

        [HttpGet("get/{id}", Name = "GetSaleById")]
        public IActionResult GetSaleById([FromRoute][Required] string id)
        {
            String result = saleService.GetSaleById(id);
            return new OkObjectResult(result);
        }
    }
}

