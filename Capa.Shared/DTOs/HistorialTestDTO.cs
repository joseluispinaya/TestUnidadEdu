namespace Capa.Shared.DTOs
{
    public class HistorialTestDTO
    {
        public int IntentoId { get; set; }
        public DateTime FechaTest { get; set; }
        public string? ObservacionGeneral { get; set; }
        public string CarreraRecomendada { get; set; } = null!;
        public float Puntuacion { get; set; }
        public string FechaString => FechaTest.ToString("dd/MM/yyyy");
        public string PuntuacionString => $"{Puntuacion:0.##} %";
    }
}
