using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace ClearMind.ClearMind.Application.Services.EmocaoService
{
    public class countEmocoesPessoa
    {
        private readonly DBContext _dbContext;

        private readonly getEmocoesPessoaService _getEmocoesPessoaService;

        public countEmocoesPessoa(DBContext dbContext, getEmocoesPessoaService getEmocoesPessoaService)
        {
            _dbContext = dbContext;
            _getEmocoesPessoaService = getEmocoesPessoaService;
        }

        public async Task<Dictionary<string, int>> CountEmocoesByPessoa(int pessoaId)
        {
            var emocoes = await _getEmocoesPessoaService.GetEmocoesByPessoa(pessoaId);

            var medo = emocoes.Count(e => e.decisao == Data.Enuns.Decisao.MEDO || e.NomeEmocao.Contains("medo"));
            var raiva = emocoes.Count(e => e.decisao == Data.Enuns.Decisao.RAIVA || e.NomeEmocao.Contains("raiva"));
            var tristeza = emocoes.Count(e => e.decisao == Data.Enuns.Decisao.TRISTEZA || e.NomeEmocao.Contains("tristeza"));
            var inveja = emocoes.Count(e => e.decisao == Data.Enuns.Decisao.INVEJA || e.NomeEmocao.Contains("inveja"));

            return new Dictionary<string, int>
            {
                { "medo", medo },
                { "raiva", raiva },
                { "tristeza", tristeza },
                { "inveja", inveja }
            };
        }


    }
}