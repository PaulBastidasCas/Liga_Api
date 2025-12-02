using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liga_Modelos
{
    public class Torneo
    {
        [Key] public int Id { get; set; }
        public string Nombre { get; set; }
        public String Tipo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        // Relaciones
        public List<Inscripcion> Inscripciones { get; set; }
        public List<Partido> Partidos { get; set; }

    }

}
