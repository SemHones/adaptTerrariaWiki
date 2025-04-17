using Microsoft.AspNetCore.Mvc;
using terraria_api.Models;
using terraria_api.Repository;

namespace terraria_api.Services
{
    public class ArmorSetsService
    {
        private readonly ArmorSetsRepository _armorSetsRepository;

        public ArmorSetsService(TerrariaContext context)
        {
            _armorSetsRepository = new ArmorSetsRepository(context);
        }

        public async Task<ActionResult<IEnumerable<ArmorSet>>> GetArmorSets()
        {
            return await _armorSetsRepository.GetArmorSets();
        }

        public async Task<ActionResult<ArmorSet>> GetArmorSet(int id)
        {
            var armorSet = await _armorSetsRepository.GetArmorSet(id);

            if (armorSet == null)
            {
                throw new Exception("ArmorSet is not found");
            }

            return armorSet;
        }

        public async Task<bool> PostArmorSet(ArmorSet armorSet)
        {
            return await _armorSetsRepository.PostArmorSet(armorSet);
        }

        public async Task<bool> PutArmorSet(ArmorSet armorSet)
        {
            return await _armorSetsRepository.PutArmorSet(armorSet);
        }

        public async Task<bool> DeleteArmorSet(int id)
        {
            return await _armorSetsRepository.DeleteArmorSet(id);
        }
    }
}
