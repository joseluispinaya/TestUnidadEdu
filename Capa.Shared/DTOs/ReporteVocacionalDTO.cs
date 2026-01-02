namespace Capa.Shared.DTOs
{
    public class ReporteVocacionalDTO
    {
        public string NombreCompleto { get; set; } = null!;
        public DateTime FechaTest { get; set; }
        public string? ObservacionGeneral { get; set; }
        public string CarreraRecomendada { get; set; } = null!;
        public float Puntuacion { get; set; }
        public string FechaString => FechaTest.ToString("dd/MM/yyyy");
        public string PuntuacionString => $"{Puntuacion:0.##} %";
    }
}
