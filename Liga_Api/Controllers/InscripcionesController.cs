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
    [ApiController]
    public class InscripcionesController : ControllerBase
    {
        private readonly Liga_ApiContext _context;

        public InscripcionesController(Liga_ApiContext context)
        {
            _context = context;
        }

        // GET: api/Inscripciones
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Inscripcion>>>> GetInscripciones()
        {
            try
            {
                var inscripciones = await _context.Inscripciones
                    .Include(i => i.Torneo) 
                    .Include(i => i.Equipo) 
                    .ToListAsync();

                return ApiResult<List<Inscripcion>>.Ok(inscripciones);
            }
            catch (Exception ex)
            {
                return ApiResult<List<Inscripcion>>.Fail(ex.Message);
            }
        }

        // GET: api/Inscripciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<Inscripcion>>> GetInscripcion(int id)
        {
            try
            {
                var inscripcion = await _context.Inscripciones
                    .Include(i => i.Torneo)
                    .Include(i => i.Equipo)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (inscripcion == null)
                {
                    return ApiResult<Inscripcion>.Fail("Inscripción no encontrada");
                }

                return ApiResult<Inscripcion>.Ok(inscripcion);
            }
            catch (Exception ex)
            {
                return ApiResult<Inscripcion>.Fail(ex.Message);
            }
        }

        // POST: api/Inscripciones
        [HttpPost]
        public async Task<ActionResult<ApiResult<Inscripcion>>> PostInscripcion(Inscripcion inscripcion)
        {
            try
            {
                bool yaExiste = await _context.Inscripciones
                    .AnyAsync(i => i.TorneoId == inscripcion.TorneoId && i.EquipoId == inscripcion.EquipoId);

                if (yaExiste)
                {
                    return BadRequest(ApiResult<Inscripcion>.Fail("El equipo ya está inscrito en este torneo"));
                }

                _context.Inscripciones.Add(inscripcion);
                await _context.SaveChangesAsync();

                var inscripcionCompleta = await _context.Inscripciones
                    .Include(i => i.Torneo)
                    .Include(i => i.Equipo)
                    .FirstOrDefaultAsync(i => i.Id == inscripcion.Id);

                return CreatedAtAction("GetInscripcion", new { id = inscripcion.Id }, ApiResult<Inscripcion>.Ok(inscripcionCompleta));
            }
            catch (Exception ex)
            {
                return ApiResult<Inscripcion>.Fail(ex.Message);
            }
        }

        // PUT: api/Inscripciones/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<Inscripcion>>> PutInscripcion(int id, Inscripcion inscripcion)
        {
            if (id != inscripcion.Id)
            {
                return ApiResult<Inscripcion>.Fail("El ID de la URL no coincide con el cuerpo");
            }

            _context.Entry(inscripcion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return ApiResult<Inscripcion>.Ok(null);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!InscripcionExists(id))
                {
                    return ApiResult<Inscripcion>.Fail("Inscripción no encontrada");
                }
                else
                {
                    return ApiResult<Inscripcion>.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return ApiResult<Inscripcion>.Fail(ex.Message);
            }
        }

        // DELETE: api/Inscripciones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<object>>> DeleteInscripcion(int id)
        {
            try
            {
                var inscripcion = await _context.Inscripciones.FindAsync(id);
                if (inscripcion == null)
                {
                    return ApiResult<object>.Fail("Inscripción no encontrada");
                }

                _context.Inscripciones.Remove(inscripcion);
                await _context.SaveChangesAsync();

                return ApiResult<object>.Ok(null);
            }
            catch (Exception ex)
            {
                return ApiResult<object>.Fail(ex.Message);
            }
        }

        private bool InscripcionExists(int id)
        {
            return _context.Inscripciones.Any(e => e.Id == id);
        }
    }
}
