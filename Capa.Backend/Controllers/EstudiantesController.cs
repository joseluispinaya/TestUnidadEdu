using Capa.Backend.DTOz;
using Capa.Backend.Repositories.Intefaces;
using Capa.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Capa.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly IEstudiantesRepository _estudiantesRepository;

        public EstudiantesController(IEstudiantesRepository estudiantesRepository)
        {
            _estudiantesRepository = estudiantesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _estudiantesRepository.GetListAsync();


            return Ok(result);
        }

        [HttpGet("{estudianteId}")]
        public async Task<IActionResult> GetAsync(int estudianteId)
        {
            var result = await _estudiantesRepository.GetHistorialPorEstudianteAsync(estudianteId);

            return Ok(result);
        }

        [HttpGet("intento/{intentoId}")]
        public async Task<IActionResult> GetDetalleAsync(int intentoId)
        {
            var result = await _estudiantesRepository.GetDetalleIntentoAsync(intentoId);

            return Ok(result);
        }

        [HttpPost("Registro")]
        public async Task<IActionResult> PostAsync([FromForm] EstudianteDTO estudianteDTO)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Datos enviados no válidos."
                });
            }

            var registro = await _estudiantesRepository.AddAsync(estudianteDTO);
            if (!registro.WasSuccess)
            {
                return Ok(new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = registro.Message
                });
            }
            return Ok(new ActionResponse<bool>
            {
                WasSuccess = true,
                Message = registro.Message,
                Result = true
            });
        }
    }
}
