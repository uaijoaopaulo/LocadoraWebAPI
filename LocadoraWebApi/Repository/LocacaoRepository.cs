using LocadoraWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocadoraWebApi.Repository
{
    public class LocacaoRepository : BaseRepository
    {
        // Retorna locação pelo id da locação
        public tb_LocacaoCF GetLocacao(int idLocacao)
        {
            try
            {
                return DataModel.Locacoes.First(e => e.idLocacao == idLocacao);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        // Retorna locação ativa pelo id do cliente
        public tb_LocacaoCF GetLocacaoAtivaByCliente(int idCliente)
        {
            try
            {
                return DataModel.Locacoes.First(e => e.idCliente == idCliente && e.locacaoAtiva == true);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        // Retorna todas as locações feitas pelos cliente
        public List<tb_LocacaoCF> GetTodaslocacoes(tb_ClienteCF value)
        {
            return DataModel.Locacoes.Where(e => e.idCliente == value.idCliente || e.tb_ClienteCF.CPF == value.CPF || e.tb_ClienteCF.nomeCliente == value.nomeCliente).ToList();
        }

        // Retorna todas as locações ativas ou desativas
        public List<tb_LocacaoCF> GetTodaslocacoes(bool value)
        {
            return DataModel.Locacoes.Where(e => e.locacaoAtiva == value).ToList();
        }

        // Salva Locação
        public async Task SalvarLocacaoAsync(tb_LocacaoCF value)
        {
            DataModel.Entry(value).State = value.idLocacao == 0 ?
                System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
            await DataModel.SaveChangesAsync();
        }

        // Desativa a locação 
        public async Task DesativarLocacaoAsync(tb_LocacaoCF value)
        {
            value.locacaoAtiva = false;
            value.dataDevolucao = DateTime.UtcNow;
            await SalvarLocacaoAsync(value);
        }
    }
}