using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LocadoraWebApi.Models
{
    public class tb_LocacaoCF
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idLocacao { get; set; }
        public DateTime dataLocacao { get; set; }
        public DateTime? dataDevolucao { get; set; }
        public bool locacaoAtiva { get; set; }
        public int idCliente { get; set; }
        public int idFilme { get; set; }

        [JsonIgnore]
        public virtual tb_ClienteCF tb_ClienteCF { get; set; }
        [JsonIgnore]
        public virtual tb_FilmeCF tb_FilmeCF { get; set; }
    }
}