using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraWebApi.Models
{
    public class tb_ClienteCF
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idCliente { get; set; }
        public string nomeCliente { get; set; }
        public string CPF { get; set; }
        public bool clienteAtivo { get; set; }

        [JsonIgnore]
        public virtual ICollection<tb_LocacaoCF> tb_LocacaoCF { get; set; }
    }
}