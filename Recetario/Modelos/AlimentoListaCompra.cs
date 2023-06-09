using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Recetario.Modelos
{
    [PrimaryKey(nameof(AlimentoId), nameof(ListaCompraId))]
    public record AlimentoListaCompra
    {
        public string Nota { get; set; }
        public int Cantidad { get; set; }
        public int AlimentoId { get; set; }
        [JsonIgnore]
        public Alimento Alimento { get; set; }
        public int ListaCompraId { get; set; }
        [JsonIgnore]
        public ListaCompra ListaCompra { get; set; }
    }
}
