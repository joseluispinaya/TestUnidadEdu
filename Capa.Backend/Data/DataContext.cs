using Capa.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capa.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Cuestionario> Cuestionarios { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }

        public DbSet<Recomendacion> Recomendaciones { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<ResultadoIA> ResultadosIA { get; set; }
        public DbSet<IntentoTest> IntentosTest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Carrera>().HasIndex(x => x.Nombre).IsUnique();
            modelBuilder.Entity<Estudiante>().HasIndex(x => x.NroCi).IsUnique();
            modelBuilder.Entity<IntentoTest>()
                .HasOne(i => i.ResultadoIA)
                .WithOne(r => r.IntentoTest)
                .HasForeignKey<ResultadoIA>(r => r.IntentoTestId);

            DisableCascadingDelete(modelBuilder);
        }

        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
