using System.ComponentModel.DataAnnotations;

namespace Capa.Shared.DTOs
{
    public class RegistroRespuestasDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un Estudiante.")]
        public int EstudianteId { get; set; }
        public List<RespuestaItemDTO> Respuestas { get; set; } = [];
    }
}
