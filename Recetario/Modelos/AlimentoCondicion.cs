using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Recetario.Modelos
{
    [PrimaryKey(nameof(AlimentoId), nameof(CondicionId))]
    public record AlimentoCondicion
    {
        public bool NoPermitido { get; set; }
        public int AlimentoId { get; set; }
        [JsonIgnore]
        public Alimento Alimento { get; set; }
        public int CondicionId { get; set; }
        [JsonIgnore]
        public Condicion Condicion { get; set; }
    }
}
