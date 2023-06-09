using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Recetario.Modelos
{
    public record Paso
    {
        public int Id { get; set; }
        public int NPaso { get; set; }
        public string Descripcion { get; set; }
        public int RecetaId { get; set; }
        [JsonIgnore]
        public Receta Receta { get; set; }
    }
}
