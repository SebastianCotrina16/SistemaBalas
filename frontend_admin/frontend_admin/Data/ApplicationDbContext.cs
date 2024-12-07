using Microsoft.EntityFrameworkCore;
using frontend_admin.Models;

namespace frontend_admin.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
           
                entity.SetTableName(entity.GetTableName().ToLowerInvariant());

                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLowerInvariant());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.PrincipalEntityType.SetTableName(key.PrincipalEntityType.GetTableName().ToLowerInvariant());
                }
            }

            // Exclude "users" from migrations but map it correctly for queries
            modelBuilder.Entity<ExternalUser>()
                .ToTable("users", t => t.ExcludeFromMigrations());

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(Console.WriteLine);
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ImpactosBala> ImpactosBala { get; set; }
        public DbSet<Reportes> Reportes { get; set; }
        public DbSet<ExamenConfiguracion> ExamenesConfiguracion { get; set; }
        public DbSet<SalaTiro> SalasTiro { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Practicas> Practicas { get; set; }
        public DbSet<DetallesPracticas> DetallesPracticas { get; set; }
        public DbSet<ExternalUser> ExternalUsers { get; set; }
        public DbSet<ExternalUser> Users { get; set; }

        

    }
}
