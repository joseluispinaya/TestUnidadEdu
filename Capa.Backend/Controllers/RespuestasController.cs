using Capa.Backend.Data;
using Capa.Backend.Helpers;
using Capa.Backend.Repositories.Intefaces;
using Capa.Shared.DTOs;
using Capa.Shared.Entities;
using Capa.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capa.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RespuestasController : ControllerBase
    {
        private readonly IRespuestasRepository _respuestaRepository;
        private readonly IIARecommendationService _iARecommendationService;
        private readonly DataContext _context;
        public RespuestasController(IRespuestasRepository respuestasRepository, IIARecommendationService iARecommendationService, DataContext context)
        {
            _respuestaRepository = respuestasRepository;
            _iARecommendationService = iARecommendationService;
            _context = context;
        }

        [HttpPost("guardar")]
        public async Task<IActionResult> PostTestAsync([FromBody] RegistroRespuestasDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var registro = await _respuestaRepository.AddAsync(request);

            if (!registro.WasSuccess)
                return NotFound(registro);

            int intentoId = registro.Result;

            var respuestas = await _respuestaRepository.GetConsultAsync(intentoId);

            if (!respuestas.WasSuccess)
                return NotFound(respuestas);

            var recomendacion = await _iARecommendationService
                .RecomendacionNewAsync(respuestas.Result!.ToList());

            return Ok(recomendacion);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> PostRespuAsync([FromBody] RegistroRespuestasDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Registrar intento y respuestas
            var registro = await _respuestaRepository.AddAsync(request);
            if (!registro.WasSuccess)
                return NotFound(registro);

            int intentoId = registro.Result;

            // Obtener respuestas del intento
            var respuestas = await _respuestaRepository.GetConsultAsync(intentoId);
            if (!respuestas.WasSuccess)
                return NotFound(respuestas);

            // Obtener carreras disponibles
            var carreras = await _context.Carreras.Select(c => c.Nombre).ToListAsync();

            // Llamar IA
            var ia = await _iARecommendationService.GenerRecomendacionAsync(
                respuestas.Result!.ToList(),
                carreras);

            if (!ia.WasSuccess)
                return BadRequest(ia);

            // Validación defensiva de salida de IA
            var nombresCarreras = carreras.ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var r in ia.Result!.Recomendaciones)
            {
                if (!nombresCarreras.Contains(r.Carrera))
                {
                    return BadRequest(new
                    {
                        Message = "El servicio de recomendaciones no está disponible en este momento. Intente nuevamente."
                    });
                }
            }

            // Persistencia transaccional
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var resultado = new ResultadoIA
                {
                    IntentoTestId = intentoId,
                    Fecha = DateTime.UtcNow,
                    ObservacionGeneral = ia.Result!.ObservacionGeneral
                };

                _context.ResultadosIA.Add(resultado);
                await _context.SaveChangesAsync();

                foreach (var r in ia.Result.Recomendaciones)
                {
                    var carrera = await _context.Carreras
                        .FirstAsync(c => c.Nombre == r.Carrera);

                    _context.Recomendaciones.Add(new Recomendacion
                    {
                        ResultadoIAId = resultado.Id,
                        CarreraId = carrera.Id,
                        Puntuacion = r.Puntuacion,
                        Justificacion = r.Justificacion
                    });
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(resultado);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        [HttpPost("prueba")]
        public async Task<IActionResult> PostPruebaAsync([FromBody] RegistroRespuestasDTO request)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ActionResponse<IAResultadoDTO>
                {
                    WasSuccess = false,
                    Message = "Datos enviados no válidos."
                });
            }

            // Registrar intento y respuestas
            var registro = await _respuestaRepository.AddAsync(request);
            if (!registro.WasSuccess)
            {
                return Ok(new ActionResponse<IAResultadoDTO>
                {
                    WasSuccess = false,
                    Message = registro.Message
                });
            }

            int intentoId = registro.Result;

            // Obtener respuestas del intento
            var respuestas = await _respuestaRepository.GetConsultAsync(intentoId);
            if (!respuestas.WasSuccess)
            {
                return Ok(new ActionResponse<IAResultadoDTO>
                {
                    WasSuccess = false,
                    Message = respuestas.Message
                });
            }

            // Obtener carreras disponibles
            var carreras = await _context.Carreras.Select(c => c.Nombre).ToListAsync();

            // Llamar IA
            var ia = await _iARecommendationService.GenerRecomendacionAsync(
                respuestas.Result!.ToList(),
                carreras);

            if (!ia.WasSuccess)
            {
                return Ok(new ActionResponse<IAResultadoDTO>
                {
                    WasSuccess = false,
                    Message = ia.Message
                });
            }

            // Validación defensiva de salida de IA
            var nombresCarreras = carreras.ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var r in ia.Result!.Recomendaciones)
            {
                if (!nombresCarreras.Contains(r.Carrera))
                {
                    return Ok(new ActionResponse<IAResultadoDTO>
                    {
                        WasSuccess = false,
                        Message = "El servicio de recomendaciones no está disponible en este momento. Intente nuevamente."
                    });
                }
            }

            // Persistencia transaccional
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var resultado = new ResultadoIA
                {
                    IntentoTestId = intentoId,
                    Fecha = DateTime.UtcNow,
                    ObservacionGeneral = ia.Result!.ObservacionGeneral
                };

                _context.ResultadosIA.Add(resultado);
                await _context.SaveChangesAsync();

                foreach (var r in ia.Result.Recomendaciones)
                {
                    var carrera = await _context.Carreras
                        .FirstAsync(c => c.Nombre == r.Carrera);

                    _context.Recomendaciones.Add(new Recomendacion
                    {
                        ResultadoIAId = resultado.Id,
                        CarreraId = carrera.Id,
                        Puntuacion = r.Puntuacion,
                        Justificacion = r.Justificacion
                    });
                }

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new ActionResponse<IAResultadoDTO>
                {
                    WasSuccess = true,
                    Message = "La evaluacion se genero correctamente.",
                    Result = ia.Result
                });
            }
            catch
            {
                await tx.RollbackAsync();
                return Ok(new ActionResponse<IAResultadoDTO>
                {
                    WasSuccess = false,
                    Message = "Ocurrió un error al guardar el resultado de la recomendación."
                });
            }
        }
    }
}
