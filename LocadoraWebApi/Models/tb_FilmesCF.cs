using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraWebApi.Models
{
    public class tb_FilmeCF
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idFilme { get; set; }
        public string nomeFilme { get; set; }
        public bool filmeAtivo { get; set; }

        [JsonIgnore]
        public virtual ICollection<tb_LocacaoCF> tb_LocacaoCF { get; set; }
    }
}