using LocadoraWebApi.Helper;
using LocadoraWebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace LocadoraWebApi.Controllers
{
    [RoutePrefix("api/Locacao")]
    public class LocacaoController : ApiController
    {
        ClienteHelper clienteHelper = new ClienteHelper();
        LocacaoHelper locacaoHelper = new LocacaoHelper();
        FilmeHelper filmeHelper = new FilmeHelper();

        // Listar filmes locados por CPF
        [HttpGet]
        [Route("ListarLocacoes/{value}")]
        public IEnumerable<tb_LocacaoCF> ListarLocacoes(string value)
        {
            var dadosLocacao = new DadosLocacao();
            value = clienteHelper.CPFshorter(value);
            if (!clienteHelper.IsCpf(value) || clienteHelper.CPFshorter(value).Length != 11)
                return null;
            else
            {
                dadosLocacao.cpfCliente = value;
                var cliente = clienteHelper.VerificaCliente(value, null, 0);
                if (cliente != null || cliente.clienteAtivo)
                {
                    return locacaoHelper.GetTodaslocacoes(cliente);
                }
                else
                    return null;
            }
        }

        // Alugar filme
        [HttpPost]
        [Route("LocarFilme")]
        public async Task<string> LocarFilme([FromBody]DadosLocacao value)
        {
            var cliente = new tb_ClienteCF();
            var filme = new tb_FilmeCF();
            var locacao = new tb_LocacaoCF();
            if (string.IsNullOrEmpty(value.cpfCliente) || !clienteHelper.IsCpf(value.cpfCliente) || clienteHelper.CPFshorter(value.cpfCliente).Length != 11)
                return "O CPF está invalido ou incorreto";
            else
            {
                cliente = clienteHelper.VerificaCliente(value.cpfCliente, value.nomeCliente, value.idCliente);
                if (cliente == null)
                    return "Cliente não foi encontrado";
            }
            var locacaoPendente = locacaoHelper.VerificaLocacaoPendente(value.idCliente);
            if (locacaoPendente != null)
                return locacaoPendente.Item1;
            var result = filmeHelper.VerificaFilme(value.idCliente, value.nomeFilme);
            if (result.Item2 == null || !result.Item2.filmeAtivo)
                return result.Item1;
            else
                filme = result.Item2;
            locacao.idCliente = cliente.idCliente;
            locacao.idFilme = filme.idFilme;
            locacao.locacaoAtiva = true;
            locacao.dataLocacao = DateTime.UtcNow;
            locacao.dataDevolucao = DateTime.UtcNow.AddDays(7);
            await locacaoHelper.SalvarLocacaoAsync(locacao);
            await filmeHelper.DesativarFilmeAsync(filme);
            return "A locação do filme " + filme.nomeFilme + " foi realizada em nome de " + cliente.nomeCliente + ", a data de devolução foi marcada para " + locacao.dataDevolucao.Value.ToShortDateString() + ", Essa data pode ser renovada se necessário, se não for devolvido até a data uma multa será cobrada!";
        }

        // Renovar locação
        [HttpPut]
        [Route("RenovarLocacao")]
        public async Task<string> RenovarFilme([FromBody]DadosLocacao value)
        {
            var cliente = new tb_ClienteCF();
            if (string.IsNullOrEmpty(value.cpfCliente) || !clienteHelper.IsCpf(value.cpfCliente) || clienteHelper.CPFshorter(value.cpfCliente).Length != 11)
                return "O CPF está invalido ou incorreto";
            else
            {
                cliente = clienteHelper.VerificaCliente(value.cpfCliente, value.nomeCliente, value.idCliente);
                if (cliente == null)
                    return "Cliente não foi encontrado";
            }
            var locacaoPendente = locacaoHelper.GetLocacaoAtivaByCliente(cliente.idCliente);
            locacaoPendente.dataDevolucao = DateTime.UtcNow.AddDays(7);
            await locacaoHelper.SalvarLocacaoAsync(locacaoPendente);
            return "Concluido, foi adicionado mais 7 dias a data de devolução";
        }

        // Devolver filme
        [HttpDelete]
        [Route("DevolverLocacao")]
        public async Task<string> DevolverLocacao([FromBody]DadosLocacao value)
        {
            var cliente = new tb_ClienteCF();
            var filme = new tb_FilmeCF();
            if (string.IsNullOrEmpty(value.cpfCliente) || !clienteHelper.IsCpf(value.cpfCliente) || clienteHelper.CPFshorter(value.cpfCliente).Length != 11)
                return "O CPF está invalido ou incorreto";
            else
            {
                cliente = clienteHelper.VerificaCliente(value.cpfCliente, value.nomeCliente, value.idCliente);
                if (cliente == null)
                    return "Cliente não foi encontrado";
            }
            var locacaoPendente = locacaoHelper.VerificaLocacaoPendente(cliente.idCliente);
            if (locacaoPendente == null)
                return "Não há locação pendente para esse cliente";
            locacaoPendente.Item2.dataDevolucao = DateTime.UtcNow;
            await locacaoHelper.DesativarLocacaoAsync(locacaoPendente.Item2);
            filme = filmeHelper.GetFilme(locacaoPendente.Item2.tb_FilmeCF.idFilme);
            filme.filmeAtivo = true;
            await filmeHelper.SalvarFilmeAsync(filme);
            return "Devolução concluida! Obrigado!";
        }
    }
}
