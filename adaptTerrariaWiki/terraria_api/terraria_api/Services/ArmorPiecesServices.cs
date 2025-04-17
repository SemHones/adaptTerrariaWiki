using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using terraria_api.Models;
using terraria_api.Repository;

namespace terraria_api.Services
{
    public class ArmorPiecesServices
    {

        private readonly ArmorPiecesRepository _armorPiecesRepository;

        public ArmorPiecesServices(TerrariaContext context)
        {
            _armorPiecesRepository = new ArmorPiecesRepository(context);
        }

        public async Task<ActionResult<IEnumerable<ArmorPiece>>> GetArmorPieces()
        {
            return await _armorPiecesRepository.GetArmorPieces();
        }

        public async Task<ActionResult<ArmorPiece>> GetArmorPiece(int id)
        {
            var armorPiece = await _armorPiecesRepository.GetArmorPiece(id);

            if (armorPiece == null)
            {
                throw new Exception("armorPiece is null");
            }

            return armorPiece;
        }


        public async Task<bool> PostArmorPiece(ArmorPiece armorPiece)
        {
            bool status = await _armorPiecesRepository.PostArmorPiece(armorPiece);

            return status;
        }

        public async Task<bool> PutArmorPiece(ArmorPiece armorPiece)
        {
            bool status = await _armorPiecesRepository.PutArmorPiece(armorPiece);

            return status;
        }

        public async Task<bool> DeleteArmorPiece(int id)
        {
            var status = await _armorPiecesRepository.DeleteArmorPiece(id);

            return status;
        }
    }
}
