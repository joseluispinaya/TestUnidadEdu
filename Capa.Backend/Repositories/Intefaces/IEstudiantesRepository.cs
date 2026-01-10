using Capa.Backend.DTOz;
using Capa.Shared.DTOs;
using Capa.Shared.Responses;

namespace Capa.Backend.Repositories.Intefaces
{
    public interface IEstudiantesRepository
    {
        Task<ActionResponse<bool>> AddAsync(EstudianteDTO estudianteDTO);

        Task<ActionResponse<IReadOnlyList<HistorialTestDTO>>> GetHistorialPorEstudianteAsync(int estudianteId);
        Task<ActionResponse<IReadOnlyList<RecomenDTO>>> GetDetalleIntentoAsync(int intentoId);
        Task<ActionResponse<IEnumerable<ListEstDTO>>> GetListAsync();
    }
}
