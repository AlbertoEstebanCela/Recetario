﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recetario.Modelos
{
    public record Condicion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
