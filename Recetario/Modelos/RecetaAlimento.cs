using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Recetario.Modelos
{
    [PrimaryKey(nameof(RecetaId), nameof(AlimentoId))]
    public record RecetaAlimento
    {
        public string? Descripcion { get; set; }
        public int RecetaId { get; set; }
        [JsonIgnore]
        public Receta Receta { get; set; }
        public int AlimentoId { get; set; }
        [JsonIgnore]
        public Alimento Alimento { get; set; }
    }
}
