using System.ComponentModel.DataAnnotations;

namespace Capa.Shared.Entities
{
    public class Respuesta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Respuesta es obligatorio.")]
        public string TextoRespuesta { get; set; } = null!;

        public int PreguntaId { get; set; }
        public Pregunta Pregunta { get; set; } = null!;

        public int IntentoTestId { get; set; }
        public IntentoTest IntentoTest { get; set; } = null!;
    }
}
