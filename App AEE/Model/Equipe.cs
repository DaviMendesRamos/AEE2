using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App_AEE.Model
{
    public class Equipe
    {
        
        [Key]
        public int CodEquipe { get; set; }

        public string NomeEquipe { get; set; } = string.Empty;

        public string? Modalidade { get; set; }

        [JsonIgnore]

        public string? UrlImagem { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IFormFile? Imagem { get; set; }
    }
}
