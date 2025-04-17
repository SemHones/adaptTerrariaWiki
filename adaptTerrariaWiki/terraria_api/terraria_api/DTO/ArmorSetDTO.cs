using terraria_api.Models;

namespace terraria_api.DTO
{
    public class ArmorSetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Href { get; set; }
        public string SetBonus { get; set; }
        public int HeadId { get; set; }
        public int BodyId { get; set; }
        public int LegsId { get; set; }
    }
}
