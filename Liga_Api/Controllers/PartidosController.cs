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
    public class PartidosController : ControllerBase
    {
        private readonly Liga_ApiContext _context;

        public PartidosController(Liga_ApiContext context)
        {
            _context = context;
        }

        // GET: api/Partidos
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Partido>>>> GetPartidos()
        {
            try
            {
                var partidos = await _context.Partidos
                    .Include(p => p.Torneo)         
                    .Include(p => p.EquipoLocal)     
                    .Include(p => p.EquipoVisitante) 
                    .ToListAsync();

                return ApiResult<List<Partido>>.Ok(partidos);
            }
            catch (Exception ex)
            {
                return ApiResult<List<Partido>>.Fail(ex.Message);
            }
        }

        // GET: api/Partidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<Partido>>> GetPartido(int id)
        {
            try
            {
                var partido = await _context.Partidos
                    .Include(p => p.Torneo)
                    .Include(p => p.EquipoLocal)
                    .Include(p => p.EquipoVisitante)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (partido == null)
                {
                    return ApiResult<Partido>.Fail("Partido no encontrado");
                }

                return ApiResult<Partido>.Ok(partido);
            }
            catch (Exception ex)
            {
                return ApiResult<Partido>.Fail(ex.Message);
            }
        }

        // POST: api/Partidos
        [HttpPost]
        public async Task<ActionResult<ApiResult<Partido>>> PostPartido(Partido partido)
        {
            try
            {
                if (partido.EquipoLocalId == partido.EquipoVisitanteId)
                {
                    return BadRequest(ApiResult<Partido>.Fail("El equipo local y visitante no pueden ser el mismo"));
                }

                _context.Partidos.Add(partido);
                await _context.SaveChangesAsync();

                var partidoCompleto = await _context.Partidos
                    .Include(p => p.Torneo)
                    .Include(p => p.EquipoLocal)
                    .Include(p => p.EquipoVisitante)
                    .FirstOrDefaultAsync(p => p.Id == partido.Id);

                return CreatedAtAction("GetPartido", new { id = partido.Id }, ApiResult<Partido>.Ok(partidoCompleto));
            }
            catch (Exception ex)
            {
                return ApiResult<Partido>.Fail(ex.Message);
            }
        }

        // PUT: api/Partidos/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<Partido>>> PutPartido(int id, Partido partido)
        {
            if (id != partido.Id)
            {
                return ApiResult<Partido>.Fail("El ID de la URL no coincide con el cuerpo");
            }

            _context.Entry(partido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return ApiResult<Partido>.Ok(null);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PartidoExists(id))
                {
                    return ApiResult<Partido>.Fail("Partido no encontrado");
                }
                else
                {
                    return ApiResult<Partido>.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return ApiResult<Partido>.Fail(ex.Message);
            }
        }

        // DELETE: api/Partidos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<object>>> DeletePartido(int id)
        {
            try
            {
                var partido = await _context.Partidos.FindAsync(id);
                if (partido == null)
                {
                    return ApiResult<object>.Fail("Partido no encontrado");
                }

                _context.Partidos.Remove(partido);
                await _context.SaveChangesAsync();

                return ApiResult<object>.Ok(null);
            }
            catch (Exception ex)
            {
                return ApiResult<object>.Fail(ex.Message);
            }
        }

        private bool PartidoExists(int id)
        {
            return _context.Partidos.Any(e => e.Id == id);
        }
    }
}
