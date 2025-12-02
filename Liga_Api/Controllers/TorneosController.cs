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
    public class TorneosController : ControllerBase
    {
        private readonly Liga_ApiContext _context;

        public TorneosController(Liga_ApiContext context)
        {
            _context = context;
        }

        // GET: api/Torneos
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Torneo>>>> GetTorneos()
        {
            try
            {
                var torneos = await _context.Torneos
                    .Include(t => t.Inscripciones)
                    .ToListAsync();

                return ApiResult<List<Torneo>>.Ok(torneos);
            }
            catch (Exception ex)
            {
                return ApiResult<List<Torneo>>.Fail(ex.Message);
            }
        }

        // GET: api/Torneos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<Torneo>>> GetTorneo(int id)
        {
            try
            {
                var torneo = await _context.Torneos
                    .Include(t => t.Inscripciones)
                        .ThenInclude(i => i.Equipo) 
                    .Include(t => t.Partidos)       
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (torneo == null)
                {
                    return ApiResult<Torneo>.Fail("Torneo no encontrado");
                }

                return ApiResult<Torneo>.Ok(torneo);
            }
            catch (Exception ex)
            {
                return ApiResult<Torneo>.Fail(ex.Message);
            }
        }

        // POST: api/Torneos
        [HttpPost]
        public async Task<ActionResult<ApiResult<Torneo>>> PostTorneo(Torneo torneo)
        {
            try
            {
                if (torneo.FechaFin < torneo.FechaInicio)
                {
                    return BadRequest(ApiResult<Torneo>.Fail("La fecha de fin no puede ser anterior a la de inicio"));
                }

                _context.Torneos.Add(torneo);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTorneo", new { id = torneo.Id }, ApiResult<Torneo>.Ok(torneo));
            }
            catch (Exception ex)
            {
                return ApiResult<Torneo>.Fail(ex.Message);
            }
        }

        // PUT: api/Torneos/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<Torneo>>> PutTorneo(int id, Torneo torneo)
        {
            if (id != torneo.Id)
            {
                return ApiResult<Torneo>.Fail("El ID de la URL no coincide con el cuerpo");
            }

            _context.Entry(torneo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return ApiResult<Torneo>.Ok(null);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TorneoExists(id))
                {
                    return ApiResult<Torneo>.Fail("Torneo no encontrado");
                }
                else
                {
                    return ApiResult<Torneo>.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return ApiResult<Torneo>.Fail(ex.Message);
            }
        }

        // DELETE: api/Torneos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<object>>> DeleteTorneo(int id)
        {
            try
            {
                var torneo = await _context.Torneos.FindAsync(id);
                if (torneo == null)
                {
                    return ApiResult<object>.Fail("Torneo no encontrado");
                }

                _context.Torneos.Remove(torneo);
                await _context.SaveChangesAsync();

                return ApiResult<object>.Ok(null);
            }
            catch (Exception ex)
            {
                return ApiResult<object>.Fail($"No se puede eliminar el torneo: {ex.Message}");
            }
        }

        private bool TorneoExists(int id)
        {
            return _context.Torneos.Any(e => e.Id == id);
        }
    }
}
