using System.ComponentModel.DataAnnotations;

namespace Capa.Shared.Entities
{
    public class Recomendacion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Puntuacion es obligatorio.")]
        public float Puntuacion { get; set; }

        [Required(ErrorMessage = "El campo Justificacion es obligatorio.")]
        public string Justificacion { get; set; } = null!;

        public ResultadoIA ResultadoIA { get; set; } = null!;
        public int ResultadoIAId { get; set; }

        public Carrera Carrera { get; set; } = null!;
        public int CarreraId { get; set; }
    }
}
