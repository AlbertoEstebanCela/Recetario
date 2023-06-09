using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Recetario.Modelos;

namespace Recetario
{
    /// <summary>
    /// Clase Gestor de la Base de Datos
    /// </summary>
    public static class GestorBD
    {
        #region Propiedades

        /// <summary>
        /// Contexto de la Base de Datos
        /// </summary>
        public static RecetarioContext RecetarioContext { get; } = new RecetarioContext();

        /// <summary>
        /// Lista de los Alimentos de la Base de Datos
        /// </summary>
        public static List<Alimento> AlimentosList { get; set; }

        /// <summary>
        /// Lista de los Tipos de la Base de Datos
        /// </summary>
        public static List<Tipo> TiposList { get; set; }

        #endregion

        #region Metodos

        /// <summary>
        /// Metodo para Inicializar la Base de Datos
        /// </summary>
        public static void Inicializar()
        {
            bool bdCreada = RecetarioContext.Database.EnsureCreated();

            RecetarioContext.Tipos.Load();
            RecetarioContext.Recetas.Load();
            RecetarioContext.Pasos.Load();
            RecetarioContext.Categorias.Load();
            RecetarioContext.Alimentos.Load();
            RecetarioContext.AlimentosCondiciones.Load();
            RecetarioContext.Condiciones.Load();
            RecetarioContext.RecetasAlimentos.Load();
            RecetarioContext.ListasCompras.Load();
            RecetarioContext.AlimentosListasCompras.Load();
            RecetarioContext.Inventarios.Load();

            if (bdCreada)
            {
                try
                {
                    string jsonStr = File.ReadAllText(Directory.GetCurrentDirectory() + @"/Recursos/BBDD_Datos.json");
                    EstructuraJson? json = JsonSerializer.Deserialize<EstructuraJson>(jsonStr);
                    
                    foreach (Tipo tipo in json.Tipos)
                    {
                        RecetarioContext.Tipos.Add(tipo);
                    }
                    foreach (Receta receta in json.Recetas)
                    {
                        RecetarioContext.Recetas.Add(receta);
                    }
                    foreach (Paso paso in json.Pasos)
                    {
                        RecetarioContext.Pasos.Add(paso);
                    }
                    foreach (Categoria categoria in json.Categorias)
                    {
                        RecetarioContext.Categorias.Add(categoria);
                    }
                    foreach (Alimento alimento in json.Alimentos)
                    {
                        RecetarioContext.Alimentos.Add(alimento);
                    }
                    foreach (RecetaAlimento recetaAlimento in json.RecetasAlimentos)
                    {
                        RecetarioContext.RecetasAlimentos.Add(recetaAlimento);
                    }
                    foreach (Condicion condicion in json.Condiciones)
                    {
                        RecetarioContext.Condiciones.Add(condicion);
                    }
                    foreach (AlimentoCondicion AlimentoCondicion in json.AlimentosCondiciones)
                    {
                        RecetarioContext.AlimentosCondiciones.Add(AlimentoCondicion);
                    }
                    if (json.ListasCompras != null)
                    {
                        foreach (ListaCompra inv in json.ListasCompras)
                        {
                            RecetarioContext.ListasCompras.Add(inv);
                        }
                    }
                    if (json.AlimentosListasCompras != null)
                    {
                        foreach (AlimentoListaCompra inv in json.AlimentosListasCompras)
                        {
                            RecetarioContext.AlimentosListasCompras.Add(inv);
                        }
                    }
                    if (json.Inventarios != null)
                    {
                        foreach (Inventario inv in json.Inventarios)
                        {
                            RecetarioContext.Inventarios.Add(inv);
                        }
                    }
                    RecetarioContext.SaveChanges();
                
                }
                catch (Exception e)
                {
                    //TODO: log
                }
            }
            AlimentosList = RecetarioContext.Alimentos.ToList();
            TiposList = RecetarioContext.Tipos.ToList();
        }

        /// <summary>
        /// Metodo que exporta la BBDD a JSON
        /// </summary>
        /// <param name="ruta">Ruta donde se guarda el archivo</param>
        /// <returns>Booleano indicando si se ha hecho correctamente</returns>
        public static bool ExportarAJSON(string ruta)
        {
            bool correcto = true;
            try
            {
                EstructuraJson estructuraJson = new EstructuraJson();
                estructuraJson.Tipos = GestorBD.RecetarioContext.Tipos.ToArray();
                estructuraJson.Recetas = GestorBD.RecetarioContext.Recetas.ToArray();
                estructuraJson.Pasos = GestorBD.RecetarioContext.Pasos.ToArray();
                estructuraJson.Categorias = GestorBD.RecetarioContext.Categorias.ToArray();
                estructuraJson.Alimentos = GestorBD.RecetarioContext.Alimentos.ToArray();
                estructuraJson.RecetasAlimentos = GestorBD.RecetarioContext.RecetasAlimentos.ToArray();
                estructuraJson.Condiciones = GestorBD.RecetarioContext.Condiciones.ToArray();
                estructuraJson.AlimentosCondiciones = GestorBD.RecetarioContext.AlimentosCondiciones.ToArray();
                estructuraJson.ListasCompras = GestorBD.RecetarioContext.ListasCompras.ToArray();
                estructuraJson.AlimentosListasCompras = GestorBD.RecetarioContext.AlimentosListasCompras.ToArray();
                estructuraJson.Inventarios = GestorBD.RecetarioContext.Inventarios.ToArray();

                string jsonStr = JsonSerializer.Serialize(estructuraJson);
                File.WriteAllText(ruta, jsonStr);
            }
            catch (Exception e)
            {
                correcto = false;
            }
            return correcto;
        }

        #endregion
    }
}
