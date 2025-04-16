namespace terraria_api.Models
{
    public class ArmorSet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Href { get; set; }
        public string SetBonus { get; set; }
        public ArmorPiece Head { get; set; }
        public ArmorPiece Body { get; set; }
        public ArmorPiece Legs { get; set; }
    }
}
