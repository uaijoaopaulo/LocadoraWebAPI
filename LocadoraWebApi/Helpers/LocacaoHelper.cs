using LocadoraWebApi.Models;
using LocadoraWebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocadoraWebApi.Helper
{
    public class LocacaoHelper : LocacaoRepository
    {
        //Retorna uma frase com a situação da locação e a locação referente
        public Tuple<string, tb_LocacaoCF> VerificaLocacaoPendente(int idCliente)
        {
            var locacaoPendente = GetLocacaoAtivaByCliente(idCliente);
            if (locacaoPendente != null)
            {
                if (locacaoPendente.dataDevolucao < DateTime.UtcNow)
                    return new Tuple<string, tb_LocacaoCF>("A devolução está atrasada", locacaoPendente);
                else
                    return new Tuple<string, tb_LocacaoCF>("Não é possível locar outro filme em nome de " + locacaoPendente.tb_ClienteCF.nomeCliente + ", há uma locação pendente de " + locacaoPendente.tb_FilmeCF.nomeFilme + " feita em " + locacaoPendente.dataLocacao.ToShortDateString() + ", devolva-o para locar outro filme", locacaoPendente);
            }
            else
                return null;
        }
    }
}