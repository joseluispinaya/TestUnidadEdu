using Capa.Shared.DTOs;
using Capa.Shared.Responses;

namespace Capa.Backend.Helpers
{
    public interface IIARecommendationService
    {
        Task<ActionResponse<List<RecomendadoDTO>>> RecomendacionNewAsync(List<RespuestaConsultaDTO> respuestas);
        Task<ActionResponse<IAResultadoDTO>> GenerRecomendacionAsync(List<RespuestaConsultaDTO> respuestas, List<string> carreras);
        //Task<ActionResponse<IAResultadoDTO>> GenerRecomendacionAsync(List<RespuestaConsultaDTO> respuestas, List<string> carreras);
    }
}
