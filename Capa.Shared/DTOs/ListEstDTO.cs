using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Shared.DTOs
{
    public class ListEstDTO
    {
        public int Id { get; set; }
        public string NroCi { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string UnidadEducativa { get; set; } = null!;
        public string? Photo { get; set; }
        public string FullName => $"{Nombres} {Apellidos}";

    }
}
