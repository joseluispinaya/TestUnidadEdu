using Capa.Backend.Data;
using Capa.Backend.DTOz;
using Capa.Backend.Helpers;
using Capa.Backend.Repositories.Intefaces;
using Capa.Shared.DTOs;
using Capa.Shared.Entities;
using Capa.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Capa.Backend.Repositories.Implementations
{
    public class EstudiantesRepository : IEstudiantesRepository
    {
        private readonly DataContext _context;
        private readonly IFileStorage _fileStorage;

        public EstudiantesRepository(DataContext context, IFileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        public async Task<ActionResponse<bool>> AddAsync(EstudianteDTO estudianteDTO)
        {
            string? photoPath = null;

            try
            {
                if (estudianteDTO.PhotoFile != null)
                    photoPath = await _fileStorage.UploadFileAsync(estudianteDTO.PhotoFile, "estudiante");

                var estudiante = new Estudiante
                {
                    NroCi = estudianteDTO.NroCi,
                    Nombres = estudianteDTO.Nombres,
                    Apellidos = estudianteDTO.Apellidos,
                    Correo = estudianteDTO.Correo,
                    UnidadEducativa = estudianteDTO.UnidadEducativa,
                    Photo = photoPath
                };

                _context.Add(estudiante);
                await _context.SaveChangesAsync();

                return new ActionResponse<bool> { WasSuccess = true, Result = true, Message = "Estudiante registrado correctamente." };
            }
            catch (DbUpdateException)
            {
                if (photoPath != null)
                    await _fileStorage.RemoveFileAsync(photoPath, "estudiante");

                return new ActionResponse<bool> { WasSuccess = false, Message = "Ya existe un estudiante con el Nro CI." };
            }

        }

        public async Task<ActionResponse<IReadOnlyList<RecomenDTO>>> GetDetalleIntentoAsync(int intentoId)
        {
            var detalle = await _context.Recomendaciones
                .AsNoTracking()
                .Where(r => r.ResultadoIA.IntentoTestId == intentoId)
                .OrderByDescending(r => r.Puntuacion)
                .Select(r => new RecomenDTO
                {
                    Carrera = r.Carrera.Nombre,
                    Puntuacion = r.Puntuacion,
                    Justificacion = r.Justificacion
                })
                .ToListAsync();

            if (detalle.Count == 0)
                return new ActionResponse<IReadOnlyList<RecomenDTO>>
                {
                    WasSuccess = false,
                    Message = "No hay recomendaciones para este test."
                };

            return new ActionResponse<IReadOnlyList<RecomenDTO>>
            {
                WasSuccess = true,
                Message = "Detalle obtenido correctamente.",
                Result = detalle
            };
        }

        public async Task<ActionResponse<IReadOnlyList<HistorialTestDTO>>> GetHistorialPorEstudianteAsync(int estudianteId)
        {
            var historial = await _context.IntentosTest
                .AsNoTracking()
                .Where(i => i.EstudianteId == estudianteId && i.ResultadoIA != null)
                .Select(i => new
                {
                    i.Id,
                    i.Fecha,
                    i.ResultadoIA!.ObservacionGeneral,
                    Mejor = i.ResultadoIA!.Recomendaciones
                                .OrderByDescending(r => r.Puntuacion)
                                .First()
                })
                .Select(x => new HistorialTestDTO
                {
                    IntentoId = x.Id,
                    FechaTest = x.Fecha,
                    ObservacionGeneral = x.ObservacionGeneral,
                    CarreraRecomendada = x.Mejor.Carrera.Nombre,
                    Puntuacion = x.Mejor.Puntuacion
                })
                .ToListAsync();

            if (historial.Count == 0)
                return new ActionResponse<IReadOnlyList<HistorialTestDTO>>
                {
                    WasSuccess = false,
                    Message = "El estudiante aún no tiene resultados."
                };

            return new ActionResponse<IReadOnlyList<HistorialTestDTO>>
            {
                WasSuccess = true,
                Message = "Historial obtenido correctamente.",
                Result = historial
            };
        }

        public async Task<ActionResponse<IEnumerable<ListEstDTO>>> GetListAsync()
        {
            var estudiantes = await _context.Estudiantes
            .Select(x => new ListEstDTO
            {
                Id = x.Id,
                NroCi = x.NroCi,
                Nombres = x.Nombres,
                Apellidos = x.Apellidos,
                Correo = x.Correo,
                UnidadEducativa = x.UnidadEducativa,
                Photo = x.Photo
            })
            .ToListAsync();

            if (estudiantes.Count == 0)
            {
                return new ActionResponse<IEnumerable<ListEstDTO>>
                {
                    WasSuccess = false,
                    Message = "No hay Datos para mostrar."
                };
            }

            return new ActionResponse<IEnumerable<ListEstDTO>>
            {
                WasSuccess = true,
                Message = "Lista obtenida correctamente.",
                Result = estudiantes
            };
        }
    }
}
