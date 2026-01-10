using Capa.Backend.Helpers;
using Capa.Shared.Entities;

namespace Capa.Backend.Data
{
    public class SeedDb
    {

        private readonly DataContext _context;
        private readonly IFileStorage _fileStorage;
        private readonly IWebHostEnvironment _env;

        public SeedDb(DataContext context, IFileStorage fileStorage, IWebHostEnvironment env)
        {
            _context = context;
            _fileStorage = fileStorage;
            _env = env;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCarrerasAsync();
            await CheckCuestionariosAsync();
            await CheckEstudiantesAsync();
        }

        private async Task CheckCarrerasAsync()
        {
            if (_context.Carreras.Any()) return;
            _context.Carreras.AddRange(
                new Carrera { Nombre = "Ingenieria en Sistemas" },
                new Carrera { Nombre = "Ingenieria Industrial" },
                new Carrera { Nombre = "Ingenieria Petrolera" },
                new Carrera { Nombre = "Ingenieria Ambiental" },
                new Carrera { Nombre = "Ingenieria Biomedica" },
                new Carrera { Nombre = "Ingenieria Comercial" },
                new Carrera { Nombre = "Ingenieria Civil" },
                new Carrera { Nombre = "Medicina" },
                new Carrera { Nombre = "Derecho" },
                new Carrera { Nombre = "Contaduria Publica" },
                new Carrera { Nombre = "Militar" }
            );
            await _context.SaveChangesAsync();
        }

        private async Task CheckEstudiantesAsync()
        {
            if (_context.Estudiantes.Any()) return;

            await AddEstudianteAsync("65652544", "Carlos", "Colque Lazo", "carlosc@yopmail.com", "Walter Alpire", "carlos.jpg");
            await AddEstudianteAsync("11255712", "Mauro", "Diaz Pilco", "naurod@yopmail.com", "Eduardo Avaroa", "mauro.jpg");

            await _context.SaveChangesAsync();
        }

        private async Task CheckCuestionariosAsync()
        {
            if (_context.Cuestionarios.Any()) return;

            var cuestionarios = new List<Cuestionario>
            {
                new()
                {
                    Titulo = "Evaluación de Intereses Profesionales",
                    Descripcion = "Cuestionario para identificar intereses vocacionales del estudiante.",
                    FechaCreado = DateTime.Now,
                    Preguntas =
                    [
                        new() { Texto = "¿Qué actividades disfrutas más en tu tiempo libre?" },
                        new() { Texto = "¿Prefieres trabajar con personas, datos o máquinas?" },
                        new() { Texto = "¿Te gusta resolver problemas complejos?" },
                        new() { Texto = "¿Te consideras una persona creativa?" },
                        new() { Texto = "¿Te interesa liderar equipos de trabajo?" },
                        new() { Texto = "¿Disfrutas investigar y analizar información?" },
                        new() { Texto = "¿Te motiva trabajar en proyectos de largo plazo?" },
                        new() { Texto = "¿Te atrae el funcionamiento interno de la tecnología?" },
                        new() { Texto = "¿Prefieres tareas estructuradas o abiertas?" },
                        new() { Texto = "¿Te resulta satisfactorio ayudar a otras personas a resolver problemas?" },
                        new() { Texto = "¿Te sientes cómodo hablando en público?" },
                        new() { Texto = "¿Disfrutas diseñar, crear o construir cosas?" }
                    ]
                },
                new()
                {
                    Titulo = "Evaluación de Habilidades y Aptitudes",
                    Descripcion = "Cuestionario para analizar habilidades y capacidades personales.",
                    FechaCreado = DateTime.Now,
                    Preguntas =
                    [
                        new() { Texto = "¿En qué materias tienes mejor rendimiento?" },
                        new() { Texto = "¿Te resulta fácil aprender nuevas tecnologías?" },
                        new() { Texto = "¿Te consideras organizado y metódico?" },
                        new() { Texto = "¿Te sientes cómodo tomando decisiones importantes?" },
                        new() { Texto = "¿Cómo manejas situaciones de presión?" },
                        new() { Texto = "¿Te resulta fácil trabajar en equipo?" },
                        new() { Texto = "¿Tienes facilidad para comunicar tus ideas?" },
                        new() { Texto = "¿Aprendes mejor leyendo, escuchando o practicando?" },
                        new() { Texto = "¿Te adaptas con facilidad a cambios imprevistos?" },
                        new() { Texto = "¿Sueles planificar antes de actuar?" },
                        new() { Texto = "¿Te consideras una persona perseverante?" },
                        new() { Texto = "¿Qué tan cómodo te sientes asumiendo responsabilidades?" }
                    ]
                },
                new()
                {
                    Titulo = "Evaluación de Valores y Personalidad",
                    Descripcion = "Cuestionario para analizar rasgos de personalidad y valores personales.",
                    FechaCreado = DateTime.Now,
                    Preguntas =
                    [
                        new() { Texto = "¿Qué es más importante para ti: estabilidad o innovación?" },
                        new() { Texto = "¿Prefieres trabajar de manera independiente o en equipo?" },
                        new() { Texto = "¿Te motiva más el reconocimiento personal o el impacto social?" },
                        new() { Texto = "¿Te consideras una persona empática?" },
                        new() { Texto = "¿Qué tan importante es para ti ayudar a los demás?" },
                        new() { Texto = "¿Te sientes cómodo asumiendo riesgos?" },
                        new() { Texto = "¿Prefieres seguir reglas estrictas o tener libertad de acción?" },
                        new() { Texto = "¿Te resulta fácil mantener la calma en situaciones difíciles?" },
                        new() { Texto = "¿Cómo reaccionas ante la crítica?" },
                        new() { Texto = "¿Te consideras una persona disciplinada?" }
                    ]
                }
            };

            _context.Cuestionarios.AddRange(cuestionarios);
            await _context.SaveChangesAsync();
        }


        private async Task AddEstudianteAsync(string nroCi, string nombres, string apellidos, string correo, string unidadEducativa, string image)
        {
            string? photoPath = null;
            var filePath = Path.Combine(_env.ContentRootPath, "Images", "users", image);

            if (File.Exists(filePath))
            {
                var fileBytes = await File.ReadAllBytesAsync(filePath);
                photoPath = await _fileStorage.SaveFileAsync(fileBytes, ".jpg", "estudiante");
            }

            _context.Estudiantes.Add(new Estudiante
            {
                NroCi = nroCi,
                Nombres = nombres,
                Apellidos = apellidos,
                Correo = correo,
                UnidadEducativa = unidadEducativa,
                Photo = photoPath
            });
        }

    }
}
