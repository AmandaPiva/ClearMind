using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Api.Data;

namespace ClearMind.ClearMind.Application.Services.AnotacoesService
{
    public class DeleteAnotacaoService
    {
         private readonly DBContext _dBContext;

        public DeleteAnotacaoService(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task DeleteAnotacao(int id)
        {
            try
            {
                var anotacao = await _dBContext.Anotacoes.FindAsync(id);
                if (anotacao == null)
                {
                    throw new Exception("Anotação não encontrada pelo id informado");
                }

                _dBContext.Anotacoes.Remove(anotacao);
                await _dBContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao deletar anotação", e);
            }
        }
    }
}