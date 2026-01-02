using Capa.Shared.DTOs;
using Capa.Shared.Responses;

namespace Capa.Backend.Repositories.Intefaces
{
    public interface IPreguntasRepository
    {
        Task<ActionResponse<IReadOnlyList<PreguntaItemDTO>>> GetRandomAsync(int cantidad = 6);
        Task<ActionResponse<EstudianteResponseDTO>> GetEstudiante(string nroCi);
        Task<ActionResponse<IReadOnlyList<ReporteVocacionalDTO>>> GetReporteVocacionalAsync();
    }
}
