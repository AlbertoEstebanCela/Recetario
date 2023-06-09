using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recetario.Modelos
{
    public class EstructuraJson
    {
        public Tipo[] Tipos { get; set; }
        public Receta[] Recetas { get; set; }
        public Paso[] Pasos { get; set; }
        public Categoria[] Categorias { get; set; }
        public Alimento[] Alimentos { get; set; }
        public RecetaAlimento[] RecetasAlimentos { get; set; }
        public Condicion[] Condiciones { get; set; }
        public AlimentoCondicion[] AlimentosCondiciones { get; set; }
        public ListaCompra[] ListasCompras { get; set; }
        public AlimentoListaCompra[] AlimentosListasCompras { get; set; }
        public Inventario[] Inventarios { get; set; }
    }
}