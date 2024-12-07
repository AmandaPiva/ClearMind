using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ClearMind.ClearMind.Application.Services;
using ClearMind.ClearMind.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClearMind.ClearMind.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        //propriedade somente leitura que da acesso ao Service
        private readonly PessoaService _pessoaService;
        //Construtor
        public PessoaController(PessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Pessoa>> ListarPessoas()
        {
            var pessoas =  _pessoaService.ListarPessoas();
            return Ok(pessoas);
        }

        [HttpGet("ById/{id}")]
        public async Task<ActionResult<Pessoa>> BuscarPessoaPeloId(int id)
        {
            var pessoa = await _pessoaService.BuscarPessoaPeloId(id);
            return Ok(pessoa);
        }

        [HttpGet("ByEmail/{email}")]
        public async Task<ActionResult<Pessoa>> BuscarPessoaPeloEmail(string email)
        {
            var pessoa = await _pessoaService.BuscarPessoaPeloEmail(email);
            return Ok(pessoa);
        }

        [HttpPost]
        public async Task<IActionResult> CriarPessoa([FromBody] Pessoa novaPessoa)
        {
            if(novaPessoa == null)
            {
                return BadRequest("Dados Inválidos");
            }

            var pessoaCriada = await _pessoaService.CriaPessoa(novaPessoa);
            //Método que tráz a resposta 201 (Created)
            return CreatedAtAction(
                nameof(BuscarPessoaPeloId), 
                new {id = pessoaCriada.Id,}, 
                pessoaCriada
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPessoa(int id, [FromBody] Pessoa pessoaAtualizada)
        {
            if(pessoaAtualizada == null)
            {
                return BadRequest("Dados Inválidos");
            }

            var atualizada = await _pessoaService.AtualizarPessoa(id, pessoaAtualizada);
            if(!atualizada)
            {
                return NotFound(id);
            }

            //Método que retorna uma resposta 204 (Executado com sucesso)
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPessoa(int id)
        {
            var deletada = await _pessoaService.DeletarPessoa(id);
            if(!deletada)
            {
                return NotFound("Pessoa não encontrada");
            }
            //Método que retorna uma resposta 204 (Executado com sucesso)
            return NoContent();
        }
    }
}