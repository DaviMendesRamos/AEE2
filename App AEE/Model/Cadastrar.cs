using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace App_AEE.Model
{
    public class Cadastrar
    {
        public int CodEquipe { get; set; } // Identificador da equipe
        [JsonIgnore]
        public Equipe? Equipe { get; set; }
        public int CodEvento { get; set; } // Identificador do evento

        [JsonIgnore]
        public Evento? Evento { get; set; }

        [JsonIgnore]
        public ICollection<Participar>? Participar { get; set; }

    }
}
