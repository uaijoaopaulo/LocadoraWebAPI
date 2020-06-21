using LocadoraWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocadoraWebApi.Repository
{
    public class ClienteRepository : BaseRepository
    {
        // Retorna cliente pelo id do cliente
        public tb_ClienteCF GetCliente(int value)
        {
            try
            {
                return DataModel.Clientes.First(e => e.idCliente == value);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        // Retorna o cliente buscando pelo nome o CPF
        public tb_ClienteCF GetCliente(string value)
        {
            try
            {
                return DataModel.Clientes.First(e => e.nomeCliente == value || e.CPF == value);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        // Retorna todos os CLiente da locadora
        public List<tb_ClienteCF> GetAll()
        {
            return DataModel.Clientes.ToList();
        }

        // Retorna todos os clientes ativos ou desativos da locadora
        public List<tb_ClienteCF> GetTodosClientes(bool Value)
        {
            return DataModel.Clientes.Where(e => e.clienteAtivo == Value).ToList();
        }

        // Salva o cliente
        public async Task<string> SalvarClienteAsync(tb_ClienteCF Value)
        {
            try
            {
                DataModel.Entry(Value).State = Value.idCliente == 0 ?
                    System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                await DataModel.SaveChangesAsync();
                return null;
            }
            catch (Exception e)
            {
                return e.Source;
                throw;
            }
        }

        // Desativa o cliente
        public async Task DesativarClienteAsync(tb_ClienteCF cliente)
        {
            cliente.clienteAtivo = false;
            await SalvarClienteAsync(cliente);
        }
    }
}