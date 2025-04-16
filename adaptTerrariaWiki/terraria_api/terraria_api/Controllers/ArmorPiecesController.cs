using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using terraria_api.Models;

namespace terraria_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArmorPiecesController : ControllerBase
    {
        private readonly TerrariaContext _context;

        public ArmorPiecesController(TerrariaContext context)
        {
            _context = context;
        }

        // GET: api/ArmorPieces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArmorPiece>>> GetArmorPieces()
        {
            return await _context.ArmorPieces.ToListAsync();
        }

        // GET: api/ArmorPieces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArmorPiece>> GetArmorPiece(int id)
        {
            var armorPiece = await _context.ArmorPieces.FindAsync(id);

            if (armorPiece == null)
            {
                return NotFound();
            }

            return armorPiece;
        }

        // POST: api/ArmorPieces
        [HttpPost]
        public async Task<ActionResult<ArmorPiece>> PostArmorPiece(ArmorPiece armorPiece)
        {
            _context.ArmorPieces.Add(armorPiece);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArmorPiece), new { id = armorPiece.Id }, armorPiece);
        }

        // PUT: api/ArmorPieces/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArmorPiece(int id, ArmorPiece armorPiece)
        {
            if (id != armorPiece.Id)
            {
                return BadRequest();
            }

            _context.Entry(armorPiece).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArmorPieceExists(id))
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

        // DELETE: api/ArmorPieces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArmorPiece(int id)
        {
            var armorPiece = await _context.ArmorPieces.FindAsync(id);
            if (armorPiece == null)
            {
                return NotFound();
            }

            _context.ArmorPieces.Remove(armorPiece);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArmorPieceExists(int id)
        {
            return _context.ArmorPieces.Any(e => e.Id == id);
        }
    }
}
