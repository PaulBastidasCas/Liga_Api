using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liga_Modelos
{
    public class Equipo
    {
        [Key] public int Id { get; set; }
        public string Nombre { get; set; }

        // Relaciones
        public List<Jugador> Jugadores { get; set; }
        public List<Inscripcion> Inscripciones { get; set; }

        // Estas listas son necesarias para distinguir Local de Visitante
        [InverseProperty("EquipoLocal")]
        public List<Partido> PartidosComoLocal { get; set; } 

        [InverseProperty("EquipoVisitante")]
        public List<Partido> PartidosComoVisitante { get; set; } 
    }

}

