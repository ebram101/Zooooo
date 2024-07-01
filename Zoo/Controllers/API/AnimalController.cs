using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zoo.Data;
using Zoo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoo.Migrations;

namespace Zoo.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly ZooContext _context;

        public AnimalsController(ZooContext context)
        {
            _context = context;
        }

        // GET: api/Animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalDTO>>> GetAnimals()
        {
            var animals = await _context.Animals
                .Include(a => a.Enclosure)
                .Include(a => a.Category)
                .Select(a => new AnimalDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Species = a.Species,
                    CategoryId = a.CategoryId,
                    CategoryName = a.Category.Name,
                    Size = a.Size,
                    DietaryClass = a.DietaryClass,
                    ActivityPattern = a.ActivityPattern,
                    Prey = a.Prey,
                    EnclosureId = a.EnclosureId,
                    EnclosureName = a.Enclosure.Name,
                    SpaceRequirement = a.SpaceRequirement,
                    SecurityRequirement = a.SecurityRequirement
                })
                .ToListAsync();

            return Ok(animals);
        }

        // GET: api/Animals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalDTO>> GetAnimal(int id)
        {
            var animal = await _context.Animals
                .Include(a => a.Enclosure)
                .Include(a => a.Category)
                .Where(a => a.Id == id)
                .Select(a => new AnimalDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Species = a.Species,
                    CategoryId = a.CategoryId,
                    CategoryName = a.Category.Name,
                    Size = a.Size,
                    DietaryClass = a.DietaryClass,
                    ActivityPattern = a.ActivityPattern,
                    Prey = a.Prey,
                    EnclosureId = a.EnclosureId,
                    EnclosureName = a.Enclosure.Name,
                    SpaceRequirement = a.SpaceRequirement,
                    SecurityRequirement = a.SecurityRequirement
                })
                .FirstOrDefaultAsync();

            if (animal == null)
            {
                return NotFound();
            }

            return Ok(animal);
        }

        // PUT: api/Animals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, Animals animal)
        {
            if (id != animal.Id)
            {
                return BadRequest();
            }

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
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

        // POST: api/Animals
        [HttpPost]
        public async Task<ActionResult<Animals>> PostAnimal(Animals animal)
        {
            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnimal", new { id = animal.Id }, animal);
        }

        // DELETE: api/Animals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.Id == id);
        }
    }
}
