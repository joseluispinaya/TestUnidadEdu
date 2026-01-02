using Capa.Backend.Data;
using Capa.Backend.Repositories.Intefaces;
using Capa.Shared.DTOs;
using Capa.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Capa.Backend.Repositories.Implementations
{
    public class PreguntasRepository : IPreguntasRepository
    {
        private readonly DataContext _context;

        public PreguntasRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResponse<EstudianteResponseDTO>> GetEstudiante(string nroCi)
        {
            var estudiante = await _context.Estudiantes
                .AsNoTracking()
                .Where(e => e.NroCi == nroCi)
                .Select(e => new EstudianteResponseDTO
                {
                    Id = e.Id,
                    NombreCompleto = e.Nombres + " " + e.Apellidos,
                    Correo = e.Correo,
                    UnidadEducativa = e.UnidadEducativa
                })
                .FirstOrDefaultAsync();

            if (estudiante is null)
            {
                return new ActionResponse<EstudianteResponseDTO>
                {
                    WasSuccess = false,
                    Message = "Estudiante no encontrado."
                };
            }

            return new ActionResponse<EstudianteResponseDTO>
            {
                WasSuccess = true,
                Result = estudiante
            };
        }

        public async Task<ActionResponse<IReadOnlyList<PreguntaItemDTO>>> GetRandomAsync(int cantidad = 6)
        {
            var preguntas = await _context.Preguntas
                .AsNoTracking()
                .OrderBy(p => Guid.NewGuid())
                .Take(cantidad)
                .Select(p => new PreguntaItemDTO
                {
                    Id = p.Id,
                    Texto = p.Texto
                })
                .ToListAsync();

            if (preguntas.Count == 0)
            {
                return new ActionResponse<IReadOnlyList<PreguntaItemDTO>>
                {
                    WasSuccess = false,
                    Message = "No existen preguntas disponibles."
                };
            }

            return new ActionResponse<IReadOnlyList<PreguntaItemDTO>>
            {
                WasSuccess = true,
                Result = preguntas
            };
        }

        public async Task<ActionResponse<IReadOnlyList<ReporteVocacionalDTO>>> GetReporteVocacionalAsync()
        {
            var reporte = await _context.IntentosTest
                .AsNoTracking()
                .Where(i => i.ResultadoIA != null)
                .Select(i => new
                {
                    Intento = i,
                    MejorRecomendacion = i.ResultadoIA!.Recomendaciones
                        .OrderByDescending(r => r.Puntuacion)
                        .First()
                })
                .Select(x => new ReporteVocacionalDTO
                {
                    NombreCompleto = x.Intento.Estudiante.Nombres + " " + x.Intento.Estudiante.Apellidos,
                    FechaTest = x.Intento.Fecha,
                    ObservacionGeneral = x.Intento.ResultadoIA!.ObservacionGeneral,
                    CarreraRecomendada = x.MejorRecomendacion.Carrera.Nombre,
                    Puntuacion = x.MejorRecomendacion.Puntuacion
                })
                .ToListAsync();

            if (reporte.Count == 0)
            {
                return new ActionResponse<IReadOnlyList<ReporteVocacionalDTO>>
                {
                    WasSuccess = false,
                    Message = "No hay Datos para mostrar."
                };
            }

            return new ActionResponse<IReadOnlyList<ReporteVocacionalDTO>>
            {
                WasSuccess = true,
                Message = "Reporte generado correctamente.",
                Result = reporte
            };
        }
    }
}
