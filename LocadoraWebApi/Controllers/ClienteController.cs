using LocadoraWebApi.Helper;
using LocadoraWebApi.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace LocadoraWebApi.Controllers
{
    [RoutePrefix("api/Cliente")]
    public class ClienteController : ApiController
    {
        ClienteHelper clienteHelper = new ClienteHelper();
        
        // Retorna o cliente pelo ID ou CPF
        [HttpGet]
        [Route("BuscarCliente/{value}")]
        public tb_ClienteCF GetClienteByCPF(string value)
        {
            value = clienteHelper.CPFshorter(value);
            if (!clienteHelper.IsCpf(value) || value.Length != 11)
                return clienteHelper.GetCliente(int.Parse(value));
            else if (!string.IsNullOrEmpty(value) || clienteHelper.IsCpf(value) || clienteHelper.CPFshorter(value).Length == 11)
                return clienteHelper.GetCliente(value);
            else
                return null;
        }

        // Cadastra Cliente
        [HttpPost]
        [Route("CadastrarCliente")]
        public async Task<string> CadastrarClienteAsync([FromBody]tb_ClienteCF value)
        {
            var result = clienteHelper.verificaCamposCliente(value);
            if (result != null)
                return result;
            value.CPF = clienteHelper.CPFshorter(value.CPF);
            if (!clienteHelper.IsCpf(value.CPF) || clienteHelper.CPFshorter(value.CPF).Length != 11)
                return "O CPF está invalido ou incorreto";
            var cliente = clienteHelper.VerificaCliente(value.CPF, value.nomeCliente, value.idCliente);
            if (cliente == null || !cliente.clienteAtivo)
            {
                value.clienteAtivo = true;
                await clienteHelper.SalvarClienteAsync(value);
                return "Cliente cadastrado com sucesso!";
            }
            else
                return "Cliente já está cadastrado!";
        }

        // Altera Cliente
        [HttpPut]
        [Route("AlterarCliente")]
        public async Task<string> AlterarClienteAsync([FromBody]tb_ClienteCF value)
        {
            var result = clienteHelper.verificaCamposCliente(value);
            if (result != null)
                return result;
            if (!string.IsNullOrEmpty(value.CPF))
            {
                value.CPF = clienteHelper.CPFshorter(value.CPF);
                if (!clienteHelper.IsCpf(value.CPF) || clienteHelper.CPFshorter(value.CPF).Length != 11)
                    return "O CPF está invalido ou incorreto";
            }
            var cliente = clienteHelper.VerificaCliente(value.CPF, value.nomeCliente, value.idCliente);
            if (cliente != null)
            {
                if (!cliente.CPF.Equals(value.CPF) || string.IsNullOrEmpty(cliente.CPF))
                    cliente.CPF = value.CPF;
                if (!cliente.nomeCliente.Equals(value.nomeCliente) || string.IsNullOrEmpty(cliente.nomeCliente))
                    cliente.nomeCliente = value.nomeCliente;
                await clienteHelper.SalvarClienteAsync(cliente);
                return "Cliente atualizado";
            }
            else
                return "Esse Cliente não foi encontrado";
        }

        // Desativa cliente
        [HttpDelete]
        [Route("DesativarCliente")]
        public async Task<string> DesativarClienteAsync([FromBody]tb_ClienteCF value)
        {
            var result = clienteHelper.verificaCamposCliente(value);
            if (result != null)
                return result;
            value.CPF = clienteHelper.CPFshorter(value.CPF);
            if (!clienteHelper.IsCpf(value.CPF) || clienteHelper.CPFshorter(value.CPF).Length != 11)
                return "O CPF está invalido ou incorreto";
            var cliente = clienteHelper.VerificaCliente(value.CPF, value.nomeCliente, value.idCliente);
            if (cliente != null || cliente.clienteAtivo)
            {
                await clienteHelper.DesativarClienteAsync(cliente);
                return "Concluido";
            }
            else
                return "Esse Cliente não foi encontrado";
        }
    }
}
