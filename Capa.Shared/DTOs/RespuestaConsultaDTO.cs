namespace Capa.Shared.DTOs
{
    public class RespuestaConsultaDTO
    {
        public int PreguntaId { get; set; }
        public string Pregunta { get; set; } = null!;
        public string TextoRespuesta { get; set; } = null!;
    }
}
