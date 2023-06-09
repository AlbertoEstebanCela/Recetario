using Recetario.Comandos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Recetario.Modelos
{
    public record Receta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public byte[]? Imagen { get; set; }
        public int Tiempo { get; set; }
        public int Raciones { get; set; }
        public bool Favorito { get; set; }
        public int TipoId { get; set; }
        [JsonIgnore]
        public Tipo Tipo { get; set; }
        public TipoImagen TipoImagen { get; set; }
    }
}
