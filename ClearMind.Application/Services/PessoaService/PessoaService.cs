using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Api.Data;
using ClearMind.ClearMind.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ClearMind.ClearMind.Application.Services
{
    public class PessoaService
    {
        //atributo somente leitura que tras a classe do banco de dados
        private readonly DBContext _dbContext;
        //construtor que armazenará os dados do banco de dados na propriedade acima
        public PessoaService(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<Pessoa> CriaPessoa(Pessoa pessoa)
        {
            if(pessoa == null)
            {
                throw new ArgumentNullException("Pessoa não encontrada pelo argumento passado");
            }

            //Criptografia da senha
            pessoa.Senha = BCrypt.Net.BCrypt.HashPassword(pessoa.Senha);

            await _dbContext.Pessoa.AddAsync(pessoa);
            await _dbContext.SaveChangesAsync();

            return pessoa;
        }

        public async Task<IEnumerable<Pessoa>> ListarPessoas()
        {
            return _dbContext.Pessoa.ToList();
        }
        //O ? representa que este método pode retornar uma informação nula
        public async Task<Pessoa?> BuscarPessoaPeloId(int id)
        {
            return await _dbContext.Pessoa.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pessoa?> BuscarPessoaPeloEmail(string email)
        {
            return await _dbContext.Pessoa.FirstOrDefaultAsync(p => p.Email == email);
        }

         public async Task<bool> AtualizarPessoa(int id, Pessoa pessoaAtualizada)
         {
            var pessoaExistente = await _dbContext.Pessoa.FirstOrDefaultAsync(p => p.Id == id);
            if(pessoaExistente == null)
            {
                throw new ArgumentNullException("Nenhuma pessoa encontrada por esse id");
            }

            pessoaExistente.Nome = pessoaAtualizada.Nome;
            pessoaExistente.Email = pessoaAtualizada.Email;
              // Atualizando a senha criptografada, se fornecida
            if (!string.IsNullOrWhiteSpace(pessoaAtualizada.Senha))
            {
                pessoaExistente.Senha = BCrypt.Net.BCrypt.HashPassword(pessoaAtualizada.Senha);
            }

            await _dbContext.SaveChangesAsync();

            return true;
         }

         public async Task<bool> DeletarPessoa(int id)
         {
            var pessoa = await _dbContext.Pessoa.FirstOrDefaultAsync(p => p.Id == id);
            if(pessoa == null)
            {
                throw new ArgumentNullException("Nenhuma pessoa encontrada por esse id");
            }

            _dbContext.Pessoa.Remove(pessoa);
            await _dbContext.SaveChangesAsync();

            return true;
         }
    }
}