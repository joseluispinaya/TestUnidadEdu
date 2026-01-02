using Capa.Backend.Data;
using Capa.Backend.Repositories.Intefaces;
using Capa.Shared.DTOs;
using Capa.Shared.Entities;
using Capa.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Capa.Backend.Repositories.Implementations
{
    public class RespuestasRepository : IRespuestasRepository
    {
        private readonly DataContext _context;
        public RespuestasRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResponse<int>> AddAsync(RegistroRespuestasDTO dto)
        {
            var estudiante = await _context.Estudiantes.FindAsync(dto.EstudianteId);
            if (estudiante is null)
            {
                return new ActionResponse<int>
                {
                    WasSuccess = false,
                    Message = "Estudiante no encontrado."
                };
            }

            var preguntaIds = dto.Respuestas.Select(x => x.PreguntaId).ToList();

            var preguntasValidas = await _context.Preguntas
                .Where(p => preguntaIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            if (preguntasValidas.Count != preguntaIds.Count)
            {
                return new ActionResponse<int>
                {
                    WasSuccess = false,
                    Message = "Una o más preguntas no existen."
                };
            }

            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var intento = new IntentoTest
                {
                    EstudianteId = dto.EstudianteId,
                    Fecha = DateTime.UtcNow
                };

                _context.IntentosTest.Add(intento);
                await _context.SaveChangesAsync();

                var respuestas = dto.Respuestas.Select(r => new Respuesta
                {
                    IntentoTestId = intento.Id,
                    PreguntaId = r.PreguntaId,
                    TextoRespuesta = r.TextoRespuesta
                });

                _context.Respuestas.AddRange(respuestas);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();

                return new ActionResponse<int>
                {
                    WasSuccess = true,
                    Result = intento.Id,
                    Message = "Test registrado correctamente."
                };
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return new ActionResponse<int>
                {
                    WasSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ActionResponse<IReadOnlyList<RespuestaConsultaDTO>>> GetConsultAsync(int intentoTestId)
        {
            var respuestas = await _context.Respuestas
                .AsNoTracking()
                .Where(r => r.IntentoTestId == intentoTestId)
                .Select(r => new RespuestaConsultaDTO
                {
                    PreguntaId = r.PreguntaId,
                    Pregunta = r.Pregunta.Texto,
                    TextoRespuesta = r.TextoRespuesta
                })
                .ToListAsync();

            if (respuestas.Count == 0)
            {
                return new ActionResponse<IReadOnlyList<RespuestaConsultaDTO>>
                {
                    WasSuccess = false,
                    Message = "No hay respuestas para este intento."
                };
            }

            return new ActionResponse<IReadOnlyList<RespuestaConsultaDTO>>
            {
                WasSuccess = true,
                Result = respuestas
            };
        }
    }
}
