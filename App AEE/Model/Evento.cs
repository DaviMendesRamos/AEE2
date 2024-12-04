using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_AEE.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
    }
}

