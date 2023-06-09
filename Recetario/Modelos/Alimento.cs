using Recetario.Comandos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Recetario.Modelos
{
    public record Alimento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public byte[]? Imagen { get; set; }
        public int CategoriaId { get; set; }
        [JsonIgnore]
        public Categoria Categoria { get; set; }
        public TipoImagen TipoImagen { get; set; }

        [JsonIgnore]
        [NotMapped]
        public BitmapSource ImagenCargada { get; set; }
    }
}
