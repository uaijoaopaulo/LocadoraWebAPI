using LocadoraWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocadoraWebApi.Repository
{
    public class FilmeRepository : BaseRepository
    {
        // Retorna filme pelo id do Filme
        public tb_FilmeCF GetFilme(int value)
        {
            try
            {
                return DataModel.Filmes.First(e => e.idFilme == value);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        // Retorna filme pelo nome do Filme
        public tb_FilmeCF GetFilme(string value)
        {
            try
            {
                return DataModel.Filmes.First(e => e.nomeFilme.Equals(value));
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        // Retorna todos os filme na locadora
        public List<tb_FilmeCF> GetTodosFilmes()
        {
            return DataModel.Filmes.ToList();
        }

        // Retorna todos os filmes ativos ou desativos na locadora
        public List<tb_FilmeCF> GetTodosFilmes(bool value)
        {
            return DataModel.Filmes.Where(e => e.filmeAtivo == value).ToList();
        }

        // Retorna todos os filmes pelo nome ou que contenham o nome
        public List<tb_FilmeCF> GetTodosFilmes(string value)
        {
            return DataModel.Filmes.Where(e => e.nomeFilme == value || e.nomeFilme.Contains(value)).ToList();
        }

        // Salva o filme no banco
        public async Task SalvarFilmeAsync(tb_FilmeCF value)
        {
            DataModel.Entry(value).State = value.idFilme == 0 ?
                System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
            await DataModel.SaveChangesAsync();
        }

        // Desativa o filme 
        public async Task DesativarFilmeAsync(tb_FilmeCF value)
        {
            value.filmeAtivo = false;
            await SalvarFilmeAsync(value);
        }
    }
}