using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using terraria_api.DTO;
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

        public async Task<ActionResult<IEnumerable<ArmorSetDTO>>> GetArmorSets()
        {
            return await _context.ArmorSetDTOs.ToListAsync();
        }

        public async Task<ActionResult<ArmorSetDTO>> GetArmorSet(int id)
        {
            var armorSetDTO = await _context.ArmorSetDTOs.FindAsync(id);

            if (armorSetDTO == null)
            {
                throw new Exception("ArmorSet not found");
            }

            return armorSetDTO;
        }

        public async Task<bool> PostArmorSet(ArmorSetDTO armorSetDTO)
        {
            _context.ArmorSetDTOs.Add(armorSetDTO);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PutArmorSet(ArmorSetDTO armorSetDTO)
        {
            _context.Entry(armorSetDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArmorSetExists(armorSetDTO.Id))
                {
                    throw new KeyNotFoundException($"ArmorSet with id {armorSetDTO.Id} not found.");
                }

                throw;
            }
        }

        public async Task<bool> DeleteArmorSet(int id)
        {
            var armorSet = await _context.ArmorSetDTOs.FindAsync(id);
            if (armorSet == null)
            {
                throw new Exception("ArmorSet not found");
            }

            _context.ArmorSetDTOs.Remove(armorSet);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool ArmorSetExists(int id)
        {
            return _context.ArmorSets.Any(e => e.Id == id);
        }
    }
}
