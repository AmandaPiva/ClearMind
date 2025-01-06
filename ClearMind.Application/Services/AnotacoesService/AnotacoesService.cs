using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Api.Data;
using ClearMind.ClearMind.Data.Models.Anotacoes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;

namespace ClearMind.ClearMind.Application.Services.AnotacoesService
{
    public class AnotacoesService
    {
        private readonly DBContext _dBContext;

        public AnotacoesService(DBContext dBContext)
        {
            _dBContext = dBContext;
        }
        
       public async Task<Anotacoes> CreateAnotacao(Anotacoes anotacao)
        {
            try
            {
                if (anotacao == null)
                {
                    throw new ArgumentException("Dados inválidos");
                }

                var emocao = await _dBContext.Emocao.FindAsync(anotacao.IdEmocao);
                if (emocao == null)
                {
                    throw new Exception("Emoção não encontrada pelo id informado");
                }

                anotacao.emocao = emocao;

                _dBContext.Anotacoes.Add(anotacao);
                await _dBContext.SaveChangesAsync();
                return anotacao;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao criar anotação", e);
            }
        }
      
    }
}