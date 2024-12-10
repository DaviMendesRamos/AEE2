using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App_AEE.Model
{
    public class Evento
    {

        [Key]
        [JsonIgnore]
        public int CodEvento { get; set; } // Identificador único do evento
        public string NomeEvento { get; set; } // Nome do evento
        public string LocalEvento { get; set; } // Local onde o evento ocorre
        public DateTime DataInicio { get; set; } // Data de início do evento
        public DateTime DataFim { get; set; } // Data de término do evento
    }
}

