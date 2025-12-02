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
    public class DetallePartidosController : ControllerBase
    {
        private readonly Liga_ApiContext _context;

        public DetallePartidosController(Liga_ApiContext context)
        {
            _context = context;
        }

        // GET: api/DetallePartidos
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<DetallePartido>>>> GetDetallePartidos()
        {
            try
            {
                var datos = await _context.DetallePartidos
                    .Include(d => d.Jugador) 
                    .Include(d => d.Partido)
                    .ToListAsync();

                return ApiResult<List<DetallePartido>>.Ok(datos);
            }
            catch (Exception ex)
            {
                return ApiResult<List<DetallePartido>>.Fail(ex.Message);
            }
        }

        // GET: api/DetallePartidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<DetallePartido>>> GetDetallePartido(int id)
        {
            try
            {
                var detallePartido = await _context.DetallePartidos
                    .Include(d => d.Jugador)
                    .Include(d => d.Partido)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (detallePartido == null)
                {
                    return ApiResult<DetallePartido>.Fail("Detalle no encontrado");
                }

                return ApiResult<DetallePartido>.Ok(detallePartido);
            }
            catch (Exception ex)
            {
                return ApiResult<DetallePartido>.Fail(ex.Message);
            }
        }

        // POST: api/DetallePartidos
        [HttpPost]
        public async Task<ActionResult<ApiResult<DetallePartido>>> PostDetallePartido(DetallePartido detallePartido)
        {
            try
            {
                _context.DetallePartidos.Add(detallePartido);
                await _context.SaveChangesAsync();
                var detalleCompleto = await _context.DetallePartidos
                    .Include(d => d.Jugador)
                    .Include(d => d.Partido)
                    .FirstOrDefaultAsync(d => d.Id == detallePartido.Id);

                return CreatedAtAction("GetDetallePartido", new { id = detallePartido.Id }, ApiResult<DetallePartido>.Ok(detalleCompleto));
            }
            catch (Exception ex)
            {
                return ApiResult<DetallePartido>.Fail(ex.Message);
            }
        }

        // PUT: api/DetallePartidos/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<DetallePartido>>> PutDetallePartido(int id, DetallePartido detallePartido)
        {
            if (id != detallePartido.Id)
            {
                return ApiResult<DetallePartido>.Fail("El ID de la URL no coincide con el cuerpo de la petición");
            }

            _context.Entry(detallePartido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return ApiResult<DetallePartido>.Ok(null); 
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!DetallePartidoExists(id))
                {
                    return ApiResult<DetallePartido>.Fail("Detalle no encontrado");
                }
                else
                {
                    return ApiResult<DetallePartido>.Fail(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return ApiResult<DetallePartido>.Fail(ex.Message);
            }
        }

        // DELETE: api/DetallePartidos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<DetallePartido>>> DeleteDetallePartido(int id)
        {
            try
            {
                var detallePartido = await _context.DetallePartidos.FindAsync(id);
                if (detallePartido == null)
                {
                    return ApiResult<DetallePartido>.Fail("Detalle no encontrado");
                }

                _context.DetallePartidos.Remove(detallePartido);
                await _context.SaveChangesAsync();

                return ApiResult<DetallePartido>.Ok(null);
            }
            catch (Exception ex)
            {
                return ApiResult<DetallePartido>.Fail(ex.Message);
            }
        }

        private bool DetallePartidoExists(int id)
        {
            return _context.DetallePartidos.Any(e => e.Id == id);
        }
    }
}
