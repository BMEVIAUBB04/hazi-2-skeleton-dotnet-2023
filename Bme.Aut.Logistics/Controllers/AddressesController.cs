using Bme.Aut.Logistics.Dal;
using Bme.Aut.Logistics.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bme.Aut.Logistics.Controllers;

[Route("addresses")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly LogisticsDbContext dbContext;

    public AddressesController(LogisticsDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
    {
        throw new NotImplementedException();
    }
}
