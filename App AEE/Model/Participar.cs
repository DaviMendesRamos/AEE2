using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_AEE.Model
{
    public class Participar
    {
        public int CodEquipe { get; set; } // Chave estrangeira para Cadastrar (Equipe)
        public int CodEvento { get; set; } // Chave estrangeira para Cadastrar (Evento)

        public Inscricao Cadastrar { get; set; } // Propriedade de navegação para Cadastrar

        
    }
}
