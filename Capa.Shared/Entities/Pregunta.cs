using System.ComponentModel.DataAnnotations;

namespace Capa.Shared.Entities
{
    public class Pregunta
    {
        public int Id { get; set; }

        [MaxLength(500, ErrorMessage = "El campo Pregunta debe tener máximo 500 caractéres.")]
        [Required(ErrorMessage = "El campo Pregunta es obligatorio.")]
        public string Texto { get; set; } = null!;

        public Cuestionario Cuestionario { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un Cuestionario.")]
        public int CuestionarioId { get; set; }
        public ICollection<Respuesta> Respuestas { get; set; } = [];
    }
}
