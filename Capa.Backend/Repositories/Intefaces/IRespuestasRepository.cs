using Capa.Shared.DTOs;
using Capa.Shared.Responses;

namespace Capa.Backend.Repositories.Intefaces
{
    public interface IRespuestasRepository
    {
        Task<ActionResponse<int>> AddAsync(RegistroRespuestasDTO dto);
        Task<ActionResponse<IReadOnlyList<RespuestaConsultaDTO>>> GetConsultAsync(int intentoTestId);
        //Task<ActionResponse<IReadOnlyList<RespuestaConsultaDTO>>> GetConsultAsync(int intentoTestId);
    }
}
