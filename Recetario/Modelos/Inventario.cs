using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Recetario.Modelos
{
    public record Inventario
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public int AlimentoId { get; set; }
        [JsonIgnore]
        public Alimento Alimento { get; set; }
    }
}
