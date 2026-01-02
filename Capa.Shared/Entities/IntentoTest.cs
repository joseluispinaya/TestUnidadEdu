namespace Capa.Shared.Entities
{
    public class IntentoTest
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; } = null!;

        public ICollection<Respuesta> Respuestas { get; set; } = [];
        public ResultadoIA? ResultadoIA { get; set; }
    }
}
