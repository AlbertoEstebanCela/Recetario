using Microsoft.EntityFrameworkCore.Query;
using Recetario.Modelos;
using Recetario.Ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Recetario.Componentes
{
    /// <summary>
    /// Clase que contiene el Componente de la Receta existentes.
    /// </summary>
    /// <remarks>
    /// Hereda de UserControl.
    /// </remarks>
    public partial class RecetaComponente : UserControl
    {
        #region Propiedades

        /// <summary>
        /// Propiedad almacena la Receta a mostrar.
        /// </summary>
        private Receta receta;

        /// <summary>
        /// Propiedad que guarda la ventana Padre.
        /// </summary>
        private Window padre;

        #endregion

        #region Getters y Setters de las Propiedades

        /// <summary>
        /// Get de la Propiedad <see cref="receta"/>.
        /// </summary>
        public Receta Receta 
        { 
            get { return receta; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor dando una Receta.
        /// </summary>
        /// <param name="receta">
        /// Parametro con la que reyenar el componente.
        /// </param>
        public RecetaComponente(Receta receta, Window padre)
        {
            InitializeComponent();
            this.receta = receta;
            this.padre = padre;
            CategoriaReceta.Content = receta.Tipo.Nombre;
            NombreReceta.Text = receta.Nombre;
            TiempoReceta.Content = Funciones.CalcularTiempo(receta.Tiempo);

            if (receta.Imagen != null && receta.Imagen.Length > 0)
            {
                ImagenRecetaComponente.Source = Funciones.BytesAImagen(receta.Imagen, receta.TipoImagen);
            }
            else
            {
                ImagenRecetaComponente.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/NoImagen.png") as ImageSource;
            }

            BtnFavorito_RatonFuera(this, new RoutedEventArgs() as MouseEventArgs);
        }

        #endregion

        #region Eventos

        #region Eventos de RecetaComponente

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en <b>RecetaComponente</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void RecetaComponente_RatonDentro(object sender, MouseEventArgs e)
        {
            BordeRecetaComponente.BorderBrush = new SolidColorBrush(Color.FromRgb(34, 90, 255));
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> de <b>RecetaComponente</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void RecetaComponente_RatonFuera(object sender, MouseEventArgs e)
        {
            BordeRecetaComponente.BorderBrush = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en <b>RecetaComponente</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void RecetaComponente_Click(object sender, MouseButtonEventArgs e)
        {
            RecetaVentana recetaVentana = new RecetaVentana(receta);
            if (padre.WindowState == WindowState.Maximized)
            {
                recetaVentana.WindowState = WindowState.Maximized;
            }
            else
            {
                recetaVentana.Height = padre.ActualHeight;
                recetaVentana.Width = padre.ActualWidth;
                recetaVentana.Left = padre.Left;
                recetaVentana.Top = padre.Top;
            }
            recetaVentana.Show();
            padre.Close();
        }

        #endregion

        #region Eventos del Boton Favoritos

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en <b>Boton Favoritos</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnFavorito_RatonDentro(object sender, MouseEventArgs e)
        {
            if (receta.Favorito)
            {
                BtnFavorito_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnFavorito(Seleccionado)(Encima).png") as ImageSource;
            }
            else
            {
                BtnFavorito_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnFavorito(NoSeleccionado)(Encima).png") as ImageSource;
            }
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> de <b>Boton Favoritos</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnFavorito_RatonFuera(object sender, MouseEventArgs e)
        {
            if (receta.Favorito)
            {
                BtnFavorito_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnFavorito(Seleccionado).png") as ImageSource;
            }
            else
            {
                BtnFavorito_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnFavorito(NoSeleccionado).png") as ImageSource;
            }
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en <b>Boton Favoritos</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnFavorito_Click(object sender, RoutedEventArgs e)
        {
            receta.Favorito = !receta.Favorito;
            BtnFavorito_RatonFuera(this, new RoutedEventArgs() as MouseEventArgs);
            GestorBD.RecetarioContext.Update(receta);
            GestorBD.RecetarioContext.SaveChanges();
        }

        #endregion

        #endregion
    }
}