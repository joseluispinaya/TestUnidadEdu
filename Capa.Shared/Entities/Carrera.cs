using System.ComponentModel.DataAnnotations;

namespace Capa.Shared.Entities
{
    public class Carrera
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "El campo Carrera no puede tener más de 50 carácteres.")]
        [Required(ErrorMessage = "El campo Carrera es obligatorio.")]
        public string Nombre { get; set; } = null!;

        public ICollection<Recomendacion> Recomendaciones { get; set; } = [];
    }
}
