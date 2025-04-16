namespace terraria_api.Models
{
    public class ArmorPiece
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Href { get; set; }
        public string ObtainedFrom { get; set; }
        public int Defense { get; set; }
        public string BodySlot { get; set; }
        public string ToolTip { get; set; }
    }
}
