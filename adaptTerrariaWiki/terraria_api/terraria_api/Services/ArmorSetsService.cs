using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using terraria_api.DTO;
using terraria_api.Models;
using terraria_api.Repository;

namespace terraria_api.Services
{
    public class ArmorSetsService
    {
        private readonly ArmorSetsRepository _armorSetsRepository;
        private readonly ArmorPiecesService _armorPiecesService;

        public ArmorSetsService(TerrariaContext context)
        {
            _armorSetsRepository = new ArmorSetsRepository(context);
            _armorPiecesService = new ArmorPiecesService(context);
        }

        public async Task<ActionResult<IEnumerable<ArmorSet>>> GetArmorSets()
        {
            var armorSetDTOs = await _armorSetsRepository.GetArmorSets();

            if (armorSetDTOs == null || armorSetDTOs.Value == null)
            {
                throw new Exception("ArmorSetDTOs are null");
            }

            IEnumerable<ArmorSet> armorSets = new List<ArmorSet>();

            foreach (var armorSetDTO in armorSetDTOs.Value)
            {
                var armorSet = await ConvertArmorSetDTOToArmorSet(armorSetDTO);
                if (armorSet == null)
                {
                    throw new Exception("ArmorSet is null");
                }
                armorSets = armorSets.Append(armorSet);
            }

            return new ActionResult<IEnumerable<ArmorSet>>(armorSets);
        }

        public async Task<ActionResult<ArmorSet>> GetArmorSet(int id)
        {
            var actionResult = await _armorSetsRepository.GetArmorSet(id);

            if (actionResult.Result != null)
            {
                return actionResult.Result;
            }

            if (actionResult.Value == null)
            {
                return new NotFoundResult(); 
            }

            var armorSet = await ConvertArmorSetDTOToArmorSet(actionResult.Value);

            return new ActionResult<ArmorSet>(armorSet);
        }

        public async Task<bool> PostArmorSet(ArmorSetDTO armorSetDTO)
        {

            return await _armorSetsRepository.PostArmorSet(armorSetDTO);
        }

        public async Task<bool> PutArmorSet(ArmorSetDTO armorSetDTO)
        {
            

            return await _armorSetsRepository.PutArmorSet(armorSetDTO);
        }

        public async Task<bool> DeleteArmorSet(int id)
        {
            return await _armorSetsRepository.DeleteArmorSet(id);
        }

        private async Task<ArmorPiece> GetArmorPiece(int id)
        {
            var armorPiece = await _armorPiecesService.GetArmorPiece(id);
            if (armorPiece.Value == null)
            {
                throw new Exception("ArmorPiece not found");
            }
            return armorPiece.Value;
        }

        private async Task<ArmorSet> ConvertArmorSetDTOToArmorSet(ArmorSetDTO armorSetDTO)
        {
            var head = await GetArmorPiece(armorSetDTO.HeadId);
            if(head == null)
            {
                throw new Exception("Head armor piece not found.");
            }

            var body = await GetArmorPiece(armorSetDTO.BodyId);
            if (head == null)
            {
                throw new Exception("Body armor piece not found.");
            }

            var legs = await GetArmorPiece(armorSetDTO.LegsId);
            if (head == null)
            {
                throw new Exception("Legs armor piece not found.");
            }

            return new ArmorSet
            {
                Id = armorSetDTO.Id,
                Name = armorSetDTO.Name,
                Image = armorSetDTO.Image,
                Href = armorSetDTO.Href,
                SetBonus = armorSetDTO.SetBonus,
                Head = head,
                Body = body,
                Legs = legs
            };
        }

    }
}
