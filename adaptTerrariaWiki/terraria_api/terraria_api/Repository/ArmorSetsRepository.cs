using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using terraria_api.Models;

namespace terraria_api.Repository
{
    public class ArmorSetsRepository
    {
        private readonly TerrariaContext _context;

        public ArmorSetsRepository(TerrariaContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<ArmorSet>>> GetArmorSets()
        {
            return await _context.ArmorSets.ToListAsync();
        }

        public async Task<ActionResult<ArmorSet>> GetArmorSet(int id)
        {
            var armorSet = await _context.ArmorSets.FindAsync(id);

            if (armorSet == null)
            {
                throw new Exception("ArmorSet not found");
            }

            return armorSet;
        }

        public async Task<bool> PostArmorSet(ArmorSet armorSet)
        {
            _context.ArmorSets.Add(armorSet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PutArmorSet(ArmorSet armorSet)
        {
            _context.Entry(armorSet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArmorSetExists(armorSet.Id))
                {
                    throw new KeyNotFoundException($"ArmorSet with id {armorSet.Id} not found.");
                }

                throw;
            }
        }

        public async Task<bool> DeleteArmorSet(int id)
        {
            var armorSet = await _context.ArmorSets.FindAsync(id);
            if (armorSet == null)
            {
                throw new Exception("ArmorSet not found");
            }

            _context.ArmorSets.Remove(armorSet);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool ArmorSetExists(int id)
        {
            return _context.ArmorSets.Any(e => e.Id == id);
        }
    }
}
