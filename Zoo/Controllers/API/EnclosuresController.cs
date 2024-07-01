using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zoo.Data;
using Zoo.Models;

namespace Zoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnclosuresController : ControllerBase
    {
        private readonly ZooContext _context;

        public EnclosuresController(ZooContext context)
        {
            _context = context;
        }

        // GET: api/Enclosures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enclosure>>> GetEnclosure()
        {
            return await _context.Enclosure.ToListAsync();
        }

        // GET: api/Enclosures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enclosure>> GetEnclosure(int id)
        {
            var enclosure = await _context.Enclosure.FindAsync(id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return enclosure;
        }

        // PUT: api/Enclosures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnclosure(int id, Enclosure enclosure)
        {
            if (id != enclosure.Id)
            {
                return BadRequest();
            }

            _context.Entry(enclosure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnclosureExists(id))
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

        // POST: api/Enclosures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Enclosure>> PostEnclosure(Enclosure enclosure)
        {
            _context.Enclosure.Add(enclosure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnclosure", new { id = enclosure.Id }, enclosure);
        }

        // DELETE: api/Enclosures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnclosure(int id)
        {
            var enclosure = await _context.Enclosure.FindAsync(id);
            if (enclosure == null)
            {
                return NotFound();
            }

            _context.Enclosure.Remove(enclosure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnclosureExists(int id)
        {
            return _context.Enclosure.Any(e => e.Id == id);
        }
    }
}
