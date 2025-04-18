using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using terraria_api.Models;
using terraria_api.Services;

namespace terraria_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArmorPiecesController : ControllerBase
    {
        private readonly ArmorPiecesService _armorPiecesServices;

        public ArmorPiecesController(TerrariaContext context)
        {
            _armorPiecesServices = new ArmorPiecesService(context);
        }

        // GET: api/ArmorPieces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArmorPiece>>> GetArmorPieces()
        {
            return await _armorPiecesServices.GetArmorPieces();
        }

        // GET: api/ArmorPieces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArmorPiece>> GetArmorPiece(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id is not valid");
            }

            return await _armorPiecesServices.GetArmorPiece(id);
        }

        // POST: api/ArmorPieces
        [HttpPost]
        public async Task<ActionResult<ArmorPiece>> PostArmorPiece(ArmorPiece armorPiece)
        {
            //check if armorPiece is null
            if (armorPiece == null)
            {
                return BadRequest("ArmorPiece is null");
            }

            if (armorPiece.Id <= 0)
            {
                return BadRequest("Id is not valid");
            }

            var status = await _armorPiecesServices.PostArmorPiece(armorPiece);

            if (!status)
            {
                return NotFound();
            }


            return CreatedAtAction(nameof(GetArmorPiece), new { id = armorPiece.Id }, armorPiece);
        }

        // PUT: api/ArmorPieces/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArmorPiece(int id, ArmorPiece armorPiece)
        {
            if (armorPiece.Id <= 0 && id <= 0)
            {
                return BadRequest("One of the Id's are not valid (probably both). Go fuck yourself.");
            }

            if (id != armorPiece.Id)
            {
                return BadRequest("Id does not match the ArmorPiece object.");
            }

            var status = await _armorPiecesServices.PutArmorPiece(armorPiece);

            if (!status)
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE: api/ArmorPieces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArmorPiece(int id)
        {
            if ( id <= 0)
            {
                return BadRequest("Id is not valid");
            }

            var status = await _armorPiecesServices.DeleteArmorPiece(id);

            if (!status)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
