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
    public class EquiposController : ControllerBase
    {
        private readonly Liga_ApiContext _context;

        public EquiposController(Liga_ApiContext context)
        {
            _context = context;
        }

        // GET: api/Equipos
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Equipo>>>> GetEquipos()
        {
            try
            {
                var equipos = await _context.Equipos.ToListAsync();
                return ApiResult<List<Equipo>>.Ok(equipos);
            }
            catch (Exception ex)
            {
                return ApiResult<List<Equipo>>.Fail(ex.Message);
            }
        }

        // GET: api/Equipos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<Equipo>>> GetEquipo(int id)
        {
            try
            {
                var equipo = await _context.Equipos
                    .Include(e => e.Jugadores) 
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (equipo == null)
                {
                    return ApiResult<Equipo>.Fail("Equipo no encontrado");
                }

                return ApiResult<Equipo>.Ok(equipo);
            }
            catch (Exception ex)
            {
                return ApiResult<Equipo>.Fail(ex.Message);
            }
        }

        // PUT: api/Equipos/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<Equipo>>> PutEquipo(int id, Equipo equipo)
        {
            if (id != equipo.Id)
            {
                return ApiResult<Equipo>.Fail("El ID de la URL no coincide con el cuerpo de la petición");
            }

            _context.Entry(equipo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return ApiResult<Equipo>.Ok(null);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EquipoExists(id))
                {
                    return ApiResult<Equipo>.Fail("Equipo no encontrado");
                }
                else
                {
                    return ApiResult<Equipo>.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return ApiResult<Equipo>.Fail(ex.Message);
            }
        }

        // POST: api/Equipos
        [HttpPost]
        public async Task<ActionResult<ApiResult<Equipo>>> PostEquipo(Equipo equipo)
        {
            try
            {
                _context.Equipos.Add(equipo);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEquipo", new { id = equipo.Id }, ApiResult<Equipo>.Ok(equipo));
            }
            catch (Exception ex)
            {
                return ApiResult<Equipo>.Fail(ex.Message);
            }
        }

        // DELETE: api/Equipos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<object>>> DeleteEquipo(int id)
        {
            try
            {
                var equipo = await _context.Equipos.FindAsync(id);
                if (equipo == null)
                {
                    return ApiResult<object>.Fail("Equipo no encontrado");
                }

                _context.Equipos.Remove(equipo);
                await _context.SaveChangesAsync();

                return ApiResult<object>.Ok(null);
            }
            catch (Exception ex)
            {
                return ApiResult<object>.Fail($"No se pudo eliminar: {ex.Message}");
            }
        }

        private bool EquipoExists(int id)
        {
            return _context.Equipos.Any(e => e.Id == id);
        }
    }
}
