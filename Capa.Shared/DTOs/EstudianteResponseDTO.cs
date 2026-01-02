namespace Capa.Shared.DTOs
{
    public class EstudianteResponseDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string? UnidadEducativa { get; set; }
    }
}
