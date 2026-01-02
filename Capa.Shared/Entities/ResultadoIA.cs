namespace Capa.Shared.Entities
{
    public class ResultadoIA
    {
        public int Id { get; set; }

        public string? ObservacionGeneral { get; set; }
        public DateTime Fecha { get; set; }

        public int IntentoTestId { get; set; }
        public IntentoTest IntentoTest { get; set; } = null!;

        public ICollection<Recomendacion> Recomendaciones { get; set; } = [];
    }
}
