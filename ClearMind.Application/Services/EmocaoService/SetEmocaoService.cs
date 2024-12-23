using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearMind.ClearMind.Api.Data;
using ClearMind.ClearMind.Data.Enuns;
using ClearMind.ClearMind.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ClearMind.ClearMind.Application.Services
{
    public class SetEmocaoService
    {
        //atributo somente leitura que tras a classe do banco de dados
        private readonly DBContext _dbContext;

        public SetEmocaoService(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Método salva emoção no banco
        public async Task<Emocao> SetEmocao(Emocao emocao)
        {
           if(emocao == null || emocao.PessoaId <= 0)
           {
                throw new ArgumentException("Dados inválidos para a emoção.");
           }

            //Validando o Enum
           if(!Enum.IsDefined(typeof(Decisao), emocao.decisao))
           {
             throw new ArgumentException("Valor inválido para o campo 'Decisão'");
           }

           //Buscar Pessoa pelo ID
           var pessoa = await _dbContext.Pessoa.FindAsync(emocao.PessoaId);
           if(pessoa == null)
           {
                throw new InvalidOperationException("Pessoa não encontrada pelo id informado");
           }

            //Salvar no banco
            _dbContext.Emocao.Add(emocao);
            await _dbContext.SaveChangesAsync();
            return emocao;
        }
        public async Task<Emocao> ObterUltimaEmocaoAsync(int pessoaId)
        {
            var ultimaEmocao = await _dbContext.Emocao
            .Where(e => e.PessoaId == pessoaId)
            .OrderByDescending(e => e.Id)
            .FirstOrDefaultAsync();


            return ultimaEmocao;
        }

    }
}