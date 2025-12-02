using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liga_Modelos
{
    public class Jugador
    {
        [Key] public int Id { get; set; }
        public string Nombre { get; set; }
        public int Numero { get; set; }

        // FK
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; }
    }

}
