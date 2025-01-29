using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Application.Services.AnotacoesService;
using ClearMind.ClearMind.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClearMind.ClearMind.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnotacoesController : ControllerBase
    {
        private readonly DeleteAnotacaoService _deleteAnotacoesService;
        private readonly AnotacoesService _anotacoesService;

        public AnotacoesController(AnotacoesService anotacoesService, DeleteAnotacaoService deleteAnotacoesService)
        {
            _anotacoesService = anotacoesService;
            _deleteAnotacoesService = deleteAnotacoesService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAnotacao([FromBody] Anotacoes anotacao)
        {
            if (anotacao == null)
            {
                return BadRequest("Dados inválidos");
            }

            var anotacaoCriada = await _anotacoesService.CreateAnotacao(anotacao);

            return CreatedAtAction(
                nameof(CreateAnotacao),
                new { id = anotacaoCriada.Id },
                anotacaoCriada
            );
        }

        [HttpDelete("DeleteAnotacao/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAnotacao(int id)
        {
            try
            {
                await _deleteAnotacoesService.DeleteAnotacao(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}