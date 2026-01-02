using System.ComponentModel.DataAnnotations;

namespace Capa.Shared.Entities
{
    public class Cuestionario
    {
        public int Id { get; set; }

        [MaxLength(150, ErrorMessage = "El campo Titulo debe tener máximo 150 caractéres.")]
        [Required(ErrorMessage = "El campo Titulo es obligatorio.")]
        public string Titulo { get; set; } = null!;

        public string? Descripcion { get; set; }
        public DateTime FechaCreado { get; set; }
        public ICollection<Pregunta> Preguntas { get; set; } = [];
    }
}
