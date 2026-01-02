using Capa.Backend.Repositories.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Capa.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreguntasController : ControllerBase
    {
        private readonly IPreguntasRepository _preguntasRepository;
        public PreguntasController(IPreguntasRepository preguntasRepository)
        {
            _preguntasRepository = preguntasRepository;
        }

        [HttpGet("preguntas")]
        public async Task<IActionResult> ObtenerPreguntas()
        {
            var result = await _preguntasRepository.GetRandomAsync();

            if (!result.WasSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("{nroCi}")]
        public async Task<IActionResult> GetEstudiante(string nroCi)
        {
            var result = await _preguntasRepository.GetEstudiante(nroCi);

            if (!result.WasSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("reporte")]
        public async Task<IActionResult> GetReporteVocacionalAsync()
        {
            var result = await _preguntasRepository.GetReporteVocacionalAsync();

            if (!result.WasSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}
