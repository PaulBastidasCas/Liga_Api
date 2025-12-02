using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liga_Modelos
{
    public class Inscripcion
    {
        [Key] public int Id { get; set; }
        public DateTime FechaInscripcion { get; set; }

        // FKs
        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; }
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; }

    }
}
