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
        public async Task<ActionResult<IEnumerable<DetallePartido>>> GetDetallePartido()
        {
            return await _context.DetallePartidos.ToListAsync();
        }

        // GET: api/DetallePartidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePartido>> GetDetallePartido(int id)
        {
            var detallePartido = await _context.DetallePartidos.FindAsync(id);

            if (detallePartido == null)
            {
                return NotFound();
            }

            return detallePartido;
        }

        // PUT: api/DetallePartidos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallePartido(int id, DetallePartido detallePartido)
        {
            if (id != detallePartido.Id)
            {
                return BadRequest();
            }

            _context.Entry(detallePartido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetallePartidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DetallePartidos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetallePartido>> PostDetallePartido(DetallePartido detallePartido)
        {
            _context.DetallePartidos.Add(detallePartido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetallePartido", new { id = detallePartido.Id }, detallePartido);
        }

        // DELETE: api/DetallePartidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallePartido(int id)
        {
            var detallePartido = await _context.DetallePartidos.FindAsync(id);
            if (detallePartido == null)
            {
                return NotFound();
            }

            _context.DetallePartidos.Remove(detallePartido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetallePartidoExists(int id)
        {
            return _context.DetallePartidos.Any(e => e.Id == id);
        }
    }
}
