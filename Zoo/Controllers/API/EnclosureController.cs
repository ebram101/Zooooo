using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zoo.Data;
using Zoo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<EnclosureDTO>>> GetEnclosures()
        {
            var enclosures = await _context.Enclosure
                .Include(e => e.Animals)
                .Select(e => new EnclosureDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Climate = e.Climate,
                    HabitatType = e.HabitatType,
                    SecurityLevel = e.SecurityLevel,
                    Size = e.Size,
                    Animals = e.Animals.Select(a => new ForeignAnimalDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Size = a.Size
                    }).ToList()
                })
                .ToListAsync();

            return Ok(enclosures);
        }

        // GET: api/Enclosures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnclosureDTO>> GetEnclosure(int id)
        {
            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .Where(e => e.Id == id)
                .Select(e => new EnclosureDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Climate = e.Climate,
                    HabitatType = e.HabitatType,
                    SecurityLevel = e.SecurityLevel,
                    Size = e.Size,
                    Animals = e.Animals.Select(a => new ForeignAnimalDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Size = a.Size
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (enclosure == null)
            {
                return NotFound();
            }

            return Ok(enclosure);
        }

        // PUT: api/Enclosures/5
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
