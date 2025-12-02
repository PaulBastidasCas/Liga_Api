using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liga_Modelos
{
    public class Partido
    {
        [Key] public int Id { get; set; }
        public DateTime FechaJuego { get; set; }
        public string Fase { get; set; }
        public String Estado { get; set; }

        public int? GolesLocal { get; set; }
        public int? GolesVisitante { get; set; }

        // FK Torneo
        public int TorneoId { get; set; }
        public Torneo Torneo { get; set; }

        [ForeignKey("EquipoLocal")]
        public int EquipoLocalId { get; set; }
        public Equipo EquipoLocal { get; set; } 

        [ForeignKey("EquipoVisitante")]
        public int EquipoVisitanteId { get; set; }
        public Equipo EquipoVisitante { get; set; }

        // Relación Detalle
        public List<DetallePartido> Detalles { get; set; }

    }
}
