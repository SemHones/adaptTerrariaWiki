using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using terraria_api.Models;

namespace terraria_api.Repository
{
    public class ArmorPiecesRepository
    {
        private readonly TerrariaContext _context;

        public ArmorPiecesRepository(TerrariaContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<ArmorPiece>>> GetArmorPieces()
        {
            return await _context.ArmorPieces.ToListAsync();
        }

        public async Task<ActionResult<ArmorPiece>> GetArmorPiece(int id)
        {
            var armorPiece = await _context.ArmorPieces.FindAsync(id);

            if (armorPiece == null)
            {
                throw new Exception("armorPiece is null");
            }

            return armorPiece;
        }


        public async Task<bool> PostArmorPiece(ArmorPiece armorPiece)
        {
            _context.ArmorPieces.Add(armorPiece);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PutArmorPiece(ArmorPiece armorPiece)
        {
            _context.Entry(armorPiece).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArmorPieceExists(armorPiece.Id))
                {
                    throw new KeyNotFoundException($"ArmorPiece with id {armorPiece.Id} not found.");
                }

                throw;
            }
        }

        public async Task<bool> DeleteArmorPiece(int id)
        {
            var armorPiece = await _context.ArmorPieces.FindAsync(id);
            if (armorPiece == null)
            {
                throw new Exception("armorPiece is null");
            }

            _context.ArmorPieces.Remove(armorPiece);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool ArmorPieceExists(int id)
        {
            return _context.ArmorPieces.Any(e => e.Id == id);
        }
    }
}
