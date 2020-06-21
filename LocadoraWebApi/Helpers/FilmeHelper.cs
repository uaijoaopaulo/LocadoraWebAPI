using LocadoraWebApi.Models;
using LocadoraWebApi.Repository;
using System;

namespace LocadoraWebApi.Helper
{
    public class FilmeHelper : FilmeRepository
    {
        // Verifica Filme a existencia ou disponibilidade do Filme buscando pelos Dados da Locação
        public Tuple<string, tb_FilmeCF> VerificaFilme(int idFilme, string nomeFilme)
        {
            var filme = GetFilme(idFilme);
            if (filme == null || !filme.filmeAtivo)
            {
                filme = GetFilme(nomeFilme);
                if (filme == null || !filme.filmeAtivo)
                    return new Tuple<string, tb_FilmeCF>("Filme não encontrado ou está indisponivel para locação no momento", filme);
                else
                    return new Tuple<string, tb_FilmeCF>("OK", filme);
            }
            else
                return new Tuple<string, tb_FilmeCF>("OK", filme);
            return new Tuple<string, tb_FilmeCF>("Filme não encontrado ou está indisponivel para locação no momento", filme);
        }

        // Verifica Filme a existencia ou disponibilidade do Filme buscando por FILME
        /*public Tuple<string, tb_FilmeCF> VerificaFilme(tb_FilmeCF value)
        {
            var filme = GetFilme(value.idFilme);
            if (filme == null || !filme.filmeAtivo)
            {
                filme = GetFilme(value.nomeFilme);
                if (filme == null || !filme.filmeAtivo)
                    return new Tuple<string, tb_FilmeCF>("Filme não encontrado ou está indisponivel para locação no momento", filme);
            }
            return new Tuple<string, tb_FilmeCF>("Filme não encontrado ou está indisponivel para locação no momento", filme);
        }*/
    }
}