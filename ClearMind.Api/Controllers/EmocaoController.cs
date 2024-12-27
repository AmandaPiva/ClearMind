using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Application.Services;
using ClearMind.ClearMind.Application.Services.EmocaoService;
using ClearMind.ClearMind.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClearMind.ClearMind.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmocaoController : ControllerBase
    {
        //propriedade somente leitura que da acesso ao Service
        private readonly SetEmocaoService _emocaoService;

        private readonly countEmocoesPessoa _countEmocoesPessoa;

        private readonly getEmocoesPessoaService _emocaoPessoaService;

        public EmocaoController(SetEmocaoService emocaoService, getEmocoesPessoaService emocaoPessoaService, countEmocoesPessoa countEmocoesPessoa)
        {
            _emocaoService = emocaoService;
            _emocaoPessoaService = emocaoPessoaService;
            _countEmocoesPessoa = countEmocoesPessoa;
        }

        [HttpPost]
        public async Task<IActionResult> SetEmocao([FromBody] Emocao emocao)
        {
            if(emocao == null)
            {
                return BadRequest("Dados inválidos");
            }

            var emocaoCriada = await _emocaoService.SetEmocao(emocao);

            return CreatedAtAction(
                nameof(SetEmocao), 
                new { id = emocaoCriada.Id }, 
                emocaoCriada
            );
        }

        [HttpGet("{pessoaId}")]
        public async Task<IActionResult> GetEmocoesByPessoa(int pessoaId)
        {
            if(pessoaId <= 0)
            {
                return BadRequest("ID inválido ou pessoa não encontrada");
            }

            var emocoes = await _emocaoPessoaService.GetEmocoesByPessoa(pessoaId);

            return Ok(emocoes);
        }

        [HttpGet("count/{pessoaId}")]
        public async Task<IActionResult> CountEmocoesByPessoa(int pessoaId)
        {
            if(pessoaId <= 0)
            {
                return BadRequest("ID inválido ou pessoa não encontrada");
            }

            var emocoes = await _countEmocoesPessoa.CountEmocoesByPessoa(pessoaId);

            return Ok(emocoes);
        }
        
    }
}