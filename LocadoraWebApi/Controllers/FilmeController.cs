using LocadoraWebApi.Helper;
using LocadoraWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace LocadoraWebApi.Controllers
{
    [RoutePrefix("api/Filme")]
    public class FilmeController : ApiController
    {
        FilmeHelper filmeHelper = new FilmeHelper();

        // Retorna todos os filmes disponiveis para locação
        [HttpGet]
        [Route("FilmesDisponiveis")]
        public IEnumerable<tb_FilmeCF> GetFilmeDisponiveis()
        {
            return filmeHelper.GetTodosFilmes(true);
        }

        [HttpGet]
        [Route("FilmesLocados")]
        public IEnumerable<tb_FilmeCF> GetFilmeLocados()
        {
            return filmeHelper.GetTodosFilmes(false);
        }

        // Retorna todos os filmes na locadora
        [HttpGet]
        [Route("TodosFilmes")]
        public IEnumerable<tb_FilmeCF> GetTodosFilmes()
        {
            return filmeHelper.GetTodosFilmes();
        }

        // Procura filme por nome
        [HttpGet]
        [Route("{value}")]
        public IEnumerable<tb_FilmeCF> GetFilmeByNome(string value)
        {
            return filmeHelper.GetTodosFilmes(value);
        }

        // Cadastra Filme
        [HttpPost]
        [Route("CadastrarFilme")]
        public async Task<string> CadastrarFilmeAsync([FromBody]tb_FilmeCF value)
        {
            if (string.IsNullOrEmpty(value.nomeFilme))
                return "É necessario um nome para registrar o filme";
            var filme = filmeHelper.GetFilme(value.nomeFilme);
            if (filme == null)
            {
                value.filmeAtivo = true;
                await filmeHelper.SalvarFilmeAsync(value);
                return "Concluido";
            }
            else
                return "Esse filme já está cadastrado";
        }

        // Altera Filme
        [HttpPut]
        [Route("AlterarFilme")]
        public async Task<string> AlterarFilmeAsync([FromBody]tb_FilmeCF value)
        {
            if (string.IsNullOrEmpty(value.nomeFilme))
                return "É necessário o nome do filme para alterar o filme";
            var filme = new tb_FilmeCF();
            var result = filmeHelper.VerificaFilme(value.idFilme, value.nomeFilme);
            if (result.Item2 != null)
            {
                filme = filmeHelper.GetFilme(result.Item2.idFilme);
                filme.nomeFilme = value.nomeFilme;
                await filmeHelper.SalvarFilmeAsync(filme);
                return "Concluido";
            }
            else
                return result.Item1;
        }

        // Desativa Filve
        [HttpDelete]
        [Route("DesativarFilme")]
        public async Task<string> DesativarFilmeAsync([FromBody]tb_FilmeCF value)
        {
            var filme = new tb_FilmeCF();
            var result = filmeHelper.VerificaFilme(value.idFilme, value.nomeFilme);
            if (result.Item2 != null)
            {
                filme = filmeHelper.GetFilme(result.Item2.idFilme);
                await filmeHelper.DesativarFilmeAsync(filme);
                return "Concluido";
            }
            else
                return result.Item1;

        }
    }
}
