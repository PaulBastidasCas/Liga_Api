using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liga_Modelos
{
    public class DetallePartido
    {
        [Key] public int Id { get; set; }
        public string Tipo { get; set; }
        public int Minuto { get; set; }

        // FKs
        public int PartidoId { get; set; }
        public Partido Partido { get; set; }

        public int JugadorId { get; set; }
        public Jugador Jugador { get; set; }

    }
}
