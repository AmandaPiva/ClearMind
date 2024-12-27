using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Api.Data;
using ClearMind.ClearMind.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ClearMind.ClearMind.Application.Services.EmocaoService
{
    public class getEmocoesPessoaService
    {
        private readonly DBContext _dbContext;
        public getEmocoesPessoaService(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Emocao>> GetEmocoesByPessoa(int pessoaId)
        {
            var emocoes = await _dbContext.Emocao
            .Where(e => e.PessoaId == pessoaId)
            .OrderByDescending(e => e.Id)
            .ToListAsync();

            return emocoes;
        }
    }
}