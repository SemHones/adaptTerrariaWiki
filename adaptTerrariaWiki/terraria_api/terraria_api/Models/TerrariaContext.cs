using Microsoft.EntityFrameworkCore;

namespace terraria_api.Models
{
    public class TerrariaContext : DbContext
    {
        public string DbPath { get; }

        public TerrariaContext(DbContextOptions<TerrariaContext> options, IConfiguration configuration) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "terraria.db");
        }

        public DbSet<ArmorSet> ArmorSets { get; set; }
        public DbSet<ArmorPiece> ArmorPieces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

}
