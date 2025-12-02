using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Liga_Modelos;

namespace Liga_Api.Data
{
    public class Liga_ApiContext : DbContext
    {
        public Liga_ApiContext (DbContextOptions<Liga_ApiContext> options)
            : base(options)
        {
        }

        public DbSet<Liga_Modelos.DetallePartido> DetallePartidos { get; set; } = default!;
        public DbSet<Liga_Modelos.Equipo> Equipos { get; set; } = default!;
        public DbSet<Liga_Modelos.Inscripcion> Inscripciones { get; set; } = default!;
        public DbSet<Liga_Modelos.Jugador> Jugadores { get; set; } = default!;
        public DbSet<Liga_Modelos.Partido> Partidos { get; set; } = default!;
        public DbSet<Liga_Modelos.Torneo> Torneos { get; set; } = default!;
    }
}
