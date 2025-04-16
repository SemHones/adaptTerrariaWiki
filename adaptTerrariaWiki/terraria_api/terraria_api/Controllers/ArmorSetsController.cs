using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using terraria_api.Models;

namespace terraria_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArmorSetsController : ControllerBase
    {
        private readonly TerrariaContext _context;

        public ArmorSetsController(TerrariaContext context)
        {
            _context = context;
        }

        // GET: api/ArmorSets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArmorSet>>> GetArmorSets()
        {
            return await _context.ArmorSets.ToListAsync();
        }

        // GET: api/ArmorSets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArmorSet>> GetArmorSet(int id)
        {
            var armorSet = await _context.ArmorSets.FindAsync(id);

            if (armorSet == null)
            {
                return NotFound();
            }

            return armorSet;
        }

        // POST: api/ArmorSets
        [HttpPost]
        public async Task<ActionResult<ArmorSet>> PostArmorSet(ArmorSet armorSet)
        {
            _context.ArmorSets.Add(armorSet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArmorSet), new { id = armorSet.Id }, armorSet);
        }

        // PUT: api/ArmorSets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArmorSet(int id, ArmorSet armorSet)
        {
            if (id != armorSet.Id)
            {
                return BadRequest();
            }

            _context.Entry(armorSet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArmorSetExists(id))
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

        // DELETE: api/ArmorSets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArmorSet(int id)
        {
            var armorSet = await _context.ArmorSets.FindAsync(id);
            if (armorSet == null)
            {
                return NotFound();
            }

            _context.ArmorSets.Remove(armorSet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArmorSetExists(int id)
        {
            return _context.ArmorSets.Any(e => e.Id == id);
        }
    }
}
