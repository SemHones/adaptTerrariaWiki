using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using terraria_api.Models;
using terraria_api.Services;

namespace terraria_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArmorSetsController : ControllerBase
    {
        private readonly ArmorSetsService _armorSetsService;

        public ArmorSetsController(TerrariaContext context)
        {
            _armorSetsService = new ArmorSetsService(context);
        }

        // GET: api/ArmorSets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArmorSet>>> GetArmorSets()
        {
            return await _armorSetsService.GetArmorSets();
        }

        // GET: api/ArmorSets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArmorSet>> GetArmorSet(int id)
        {
            try
            {
                return await _armorSetsService.GetArmorSet(id);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // POST: api/ArmorSets
        [HttpPost]
        public async Task<ActionResult<ArmorSet>> PostArmorSet(ArmorSet armorSet)
        {
            if (armorSet == null)
            {
                return BadRequest("ArmorSet is null");
            }

            var status = await _armorSetsService.PostArmorSet(armorSet);

            if (!status)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetArmorSet), new { id = armorSet.Id }, armorSet);
        }

        // PUT: api/ArmorSets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArmorSet(int id, ArmorSet armorSet)
        {
            if (id != armorSet.Id)
            {
                return BadRequest("Id does not match the ArmorSet object");
            }

            var status = await _armorSetsService.PutArmorSet(armorSet);

            if (!status)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/ArmorSets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArmorSet(int id)
        {
            var status = await _armorSetsService.DeleteArmorSet(id);

            if (!status)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
