using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Liga_Api.Data;
using Liga_Modelos;

namespace Liga_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadoresController : ControllerBase
    {
        private readonly Liga_ApiContext _context;

        public JugadoresController(Liga_ApiContext context)
        {
            _context = context;
        }

        // GET: api/Jugadores
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Jugador>>>> GetJugadores()
        {
            try
            {
                var jugadores = await _context.Jugadores
                    .Include(j => j.Equipo) 
                    .ToListAsync();

                return ApiResult<List<Jugador>>.Ok(jugadores);
            }
            catch (Exception ex)
            {
                return ApiResult<List<Jugador>>.Fail(ex.Message);
            }
        }

        // GET: api/Jugadores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<Jugador>>> GetJugador(int id)
        {
            try
            {
                var jugador = await _context.Jugadores
                    .Include(j => j.Equipo) 
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (jugador == null)
                {
                    return ApiResult<Jugador>.Fail("Jugador no encontrado");
                }

                return ApiResult<Jugador>.Ok(jugador);
            }
            catch (Exception ex)
            {
                return ApiResult<Jugador>.Fail(ex.Message);
            }
        }

        // POST: api/Jugadores
        [HttpPost]
        public async Task<ActionResult<ApiResult<Jugador>>> PostJugador(Jugador jugador)
        {
            try
            {
                _context.Jugadores.Add(jugador);
                await _context.SaveChangesAsync();

                var jugadorCompleto = await _context.Jugadores
                    .Include(j => j.Equipo)
                    .FirstOrDefaultAsync(j => j.Id == jugador.Id);

                return CreatedAtAction("GetJugador", new { id = jugador.Id }, ApiResult<Jugador>.Ok(jugadorCompleto));
            }
            catch (Exception ex)
            {
                return ApiResult<Jugador>.Fail(ex.Message);
            }
        }

        // PUT: api/Jugadores/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<Jugador>>> PutJugador(int id, Jugador jugador)
        {
            if (id != jugador.Id)
            {
                return ApiResult<Jugador>.Fail("El ID de la URL no coincide con el cuerpo");
            }

            _context.Entry(jugador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return ApiResult<Jugador>.Ok(null);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!JugadorExists(id))
                {
                    return ApiResult<Jugador>.Fail("Jugador no encontrado");
                }
                else
                {
                    return ApiResult<Jugador>.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return ApiResult<Jugador>.Fail(ex.Message);
            }
        }

        // DELETE: api/Jugadores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<object>>> DeleteJugador(int id)
        {
            try
            {
                var jugador = await _context.Jugadores.FindAsync(id);
                if (jugador == null)
                {
                    return ApiResult<object>.Fail("Jugador no encontrado");
                }

                _context.Jugadores.Remove(jugador);
                await _context.SaveChangesAsync();

                return ApiResult<object>.Ok(null);
            }
            catch (Exception ex)
            {
                return ApiResult<object>.Fail(ex.Message);
            }
        }

        private bool JugadorExists(int id)
        {
            return _context.Jugadores.Any(e => e.Id == id);
        }
    }
}
