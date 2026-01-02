namespace Capa.Shared.DTOs
{
    public class IAResultadoDTO
    {
        public string? ObservacionGeneral { get; set; }
        public List<RecomenDTO> Recomendaciones { get; set; } = [];
    }
}
