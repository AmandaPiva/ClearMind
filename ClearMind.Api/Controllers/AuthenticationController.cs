using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Application.Comunications;
using ClearMind.ClearMind.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClearMind.ClearMind.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromServices] GenerateTokenPessoa generateTokenPessoa ,[FromBody] CredenciaisRequestJson credenciaisRequestJson)
        {
          try
          {
            var token = await generateTokenPessoa.GenerateToken(credenciaisRequestJson);
            return Ok(new {token});
          }
          catch(Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }
    }
}