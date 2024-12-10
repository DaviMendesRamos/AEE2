using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App_AEE.Model
{
    public class Equipe
    {
        [Key]
        [JsonIgnore]
        public int CodEquipe { get; set; }
        public string NomeEquipe { get; set; }
        public string Modalidade { get; set; }
    }
}
