using Microsoft.EntityFrameworkCore;
using Recetario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recetario
{
    /// <summary>
    /// Contexto de la Base de Datos de Recetario
    /// </summary>
    public class RecetarioContext : DbContext
    {
        #region Elementos BBDD

        /// <summary>
        /// Elemento de la BBDD de Tipos
        /// </summary>
        public DbSet<Tipo> Tipos { get; set; }

        /// <summary>
        /// Elemento de la BBDD de Recetas
        /// </summary>
        public DbSet<Receta> Recetas { get; set; }

        /// <summary>
        /// Elemento de la BBDD de Pasos
        /// </summary>
        public DbSet<Paso> Pasos { get; set; }

        /// <summary>
        /// Elemento de la BBDD de Categorias
        /// </summary>
        public DbSet<Categoria> Categorias { get; set; }

        /// <summary>
        /// Elemento de la BBDD de Alimentos
        /// </summary>
        public DbSet<Alimento> Alimentos { get; set; }

        /// <summary>
        /// Elemento de la BBDD de AlimentosCondiciones
        /// </summary>
        public DbSet<AlimentoCondicion> AlimentosCondiciones { get; set; }

        /// <summary>
        /// Elemento de la BBDD de Condiciones
        /// </summary>
        public DbSet<Condicion> Condiciones { get; set; }

        /// <summary>
        /// Elemento de la BBDD de RecetasAlimentos
        /// </summary>
        public DbSet<RecetaAlimento> RecetasAlimentos { get; set; }

        /// <summary>
        /// Elementos de la BBDD de ListasCompras
        /// </summary>
        public DbSet<ListaCompra> ListasCompras { get; set; }

        /// <summary>
        /// Elementos de la BBDD de AlimentosListasCompras
        /// </summary>
        public DbSet<AlimentoListaCompra> AlimentosListasCompras { get; set; }

        /// <summary>
        /// Elementos de la BBDD de Inventarios
        /// </summary>
        public DbSet<Inventario> Inventarios { get; set; }

        #endregion

        #region Metodos

        /// <summary>
        /// Configuracion de la Base de Datos 
        /// </summary>
        /// <param name="optionsBuilder">Pasar el creador Configuracion del DBContext</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Recetario.db");
        }

        #endregion
    }
}
