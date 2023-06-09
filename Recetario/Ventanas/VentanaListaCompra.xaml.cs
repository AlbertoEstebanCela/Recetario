using Microsoft.EntityFrameworkCore.Metadata;
using Recetario.Comandos;
using Recetario.Componentes;
using Recetario.Modelos;
using Recetario.VentanasSecundario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Recetario.Ventanas
{
    /// <summary>
    /// Clase que contiene La Ventana de Lista de la Compra de Recetas.
    /// </summary>
    /// <remarks>
    /// Hereda de Window.
    /// </remarks>
    public partial class VentanaListaCompra : Window
    {
        #region Construcotres

        /// <summary>
        /// Constructor Vacio.
        /// </summary>
        public VentanaListaCompra()
        {
            InitializeComponent();
            if(GestorBD.RecetarioContext.ListasCompras.Count() == 0)
            {
                GestorBD.RecetarioContext.ListasCompras.Add(new ListaCompra() { Dinero = 0, Fecha = DateTime.Now, Id = 1, Nombre = "Default" });
                GestorBD.RecetarioContext.SaveChanges();
            }
            ComandosMenu.AgregarBingings(CommandBindings);
            Filtrar();
        }

        #endregion

        #region Metodos

        #region Metodos Privado

        /// <summary>
        /// Metodo que Filtra la Lista de Compra
        /// </summary>
        private void Filtrar()
        {
            List<AlimentoListaCompra> comp = GestorBD.RecetarioContext.AlimentosListasCompras.ToList();
            comp.ForEach(x => {
                if (x.Alimento.Imagen != null && x.Alimento.Imagen.Length > 0) x.Alimento.ImagenCargada = Funciones.BytesAImagen(x.Alimento.Imagen, x.Alimento.TipoImagen);
                else x.Alimento.ImagenCargada = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/NoImagen.png") as BitmapSource;
            });
            TableListaCompra.ItemsSource = comp;
        }

        #endregion

        #endregion

        #region Eventos

        #region Eventos del Boton Principal

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Principal</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnPrincipal_RatonDentro(object sender, MouseEventArgs e)
        {
            BtnPrincipal.Background = new SolidColorBrush(Color.FromRgb(153, 204, 255));
            BtnPrincipal.Foreground = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Principal</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnPrincipal_RatonFuera(object sender, MouseEventArgs e)
        {
            BtnPrincipal.Background = new SolidColorBrush(Color.FromRgb(34, 90, 255));
            BtnPrincipal.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Principal</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnPrincipal_Click(object sender, RoutedEventArgs e)
        {
            BtnPrincipal.Background = new SolidColorBrush(Color.FromRgb(34, 90, 255));
            BtnPrincipal.Foreground = new SolidColorBrush(Colors.Black);

            Principal p = new Principal();
            if (WindowState == WindowState.Maximized)
            {
                p.WindowState = WindowState.Maximized;
            }
            else
            {
                p.Height = ActualHeight;
                p.Width = ActualWidth;
                p.Left = Left;
                p.Top = Top;
            }
            p.Show();
            Close();
        }

        #endregion

        #region Eventos del Boton Inventario

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Inventario</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnInventario_RatonDentro(object sender, MouseEventArgs e)
        {
            BtnInventario.Background = new SolidColorBrush(Color.FromRgb(153, 204, 255));
            BtnInventario.Foreground = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Inventario</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnInventario_RatonFuera(object sender, MouseEventArgs e)
        {
            BtnInventario.Background = new SolidColorBrush(Color.FromRgb(34, 90, 255));
            BtnInventario.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Inventario</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnInventario_Click(object sender, RoutedEventArgs e)
        {
            BtnInventario.Background = new SolidColorBrush(Color.FromRgb(34, 90, 255));
            BtnInventario.Foreground = new SolidColorBrush(Colors.Black);

            InventarioVentana inventarioVentana = new InventarioVentana();
            if (WindowState == WindowState.Maximized)
            {
                inventarioVentana.WindowState = WindowState.Maximized;
            }
            else
            {
                inventarioVentana.Height = ActualHeight;
                inventarioVentana.Width = ActualWidth;
                inventarioVentana.Left = Left;
                inventarioVentana.Top = Top;
            }
            inventarioVentana.Show();
            Close();
        }

        #endregion

        #region Eventos del Boton Confirmar

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Confirmar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnConfirmar_RatonDentro(object sender, MouseEventArgs e)
        {
            ImgBtnConfirmar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnConfirmar(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Confirmar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnConfirmar_RatonFuera(object sender, MouseEventArgs e)
        {
            ImgBtnConfirmar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnConfirmar.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Confirmar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            ImgBtnConfirmar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnConfirmar(Encima)(Click).png") as ImageSource;
            foreach(AlimentoListaCompra ali in GestorBD.RecetarioContext.AlimentosListasCompras)
            {
                GestorBD.RecetarioContext.AlimentosListasCompras.Remove(ali);
            }
            GestorBD.RecetarioContext.SaveChanges();
            Filtrar();
        }

        #endregion

        #region Eventos del Boton Add Alimento Lista Compra

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Add Alimentos Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAddLista_RatonDentro(object sender, MouseEventArgs e)
        {
            ImgBtnAdd.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Add Alimentos Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAddLista_RatonFuera(object sender, MouseEventArgs e)
        {
            ImgBtnAdd.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Add Alimentos Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAddLista_Click(object sender, RoutedEventArgs e)
        {
            ImgBtnAdd.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd(Encima)(Click).png") as ImageSource;
            VentanaSeleccionarAlimento ventanaSeleccionarAlimento = new VentanaSeleccionarAlimento();
            ventanaSeleccionarAlimento.ShowDialog();
            Alimento aliSelec = VentanaSeleccionarAlimento.UltAlimentoSelec;
            if (aliSelec != null)
            {
                AlimentoListaCompra comp = new AlimentoListaCompra()
                {
                    ListaCompraId = GestorBD.RecetarioContext.ListasCompras.First().Id,
                    AlimentoId = aliSelec.Id,
                    Cantidad = 1,
                    Nota = ""
                };
                GestorBD.RecetarioContext.AlimentosListasCompras.Add(comp);
                GestorBD.RecetarioContext.SaveChanges();
                Filtrar();
            }
        }


        #endregion

        #region Eventos del Texto Nota

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Perder el Foco</b> del <b>Texto Nota</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void Nota_LostFocus(object sender, RoutedEventArgs e)
        {
            AlimentoListaCompra? aliActual = TableListaCompra.CurrentItem as AlimentoListaCompra;
            string nNota = (sender as TextBox)?.Text == null ? "" : ((TextBox)sender).Text;
            if (aliActual != null && aliActual.Nota != nNota)
            {
                aliActual.Nota = nNota;
                GestorBD.RecetarioContext.AlimentosListasCompras.Update(aliActual);
                GestorBD.RecetarioContext.SaveChanges();
                Filtrar();
                TableListaCompra.CurrentItem = null;
            }
        }

        #endregion

        #region Eventos del Texto Cantidad

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Perder el Foco</b> del <b>Texto Cantidad</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void Cantidad_LostFocus(object sender, RoutedEventArgs e)
        {
            AlimentoListaCompra? aliActual = TableListaCompra.CurrentItem as AlimentoListaCompra;
            string nCantidad = string.IsNullOrEmpty((sender as TextBox)?.Text) ? "0" : ((TextBox)sender).Text;
            if (aliActual != null && int.Parse(nCantidad) == 0)
            {
                GestorBD.RecetarioContext.AlimentosListasCompras.Remove(aliActual);
                GestorBD.RecetarioContext.SaveChanges();
                Filtrar();
            }
            else if (aliActual != null && aliActual.Cantidad.ToString() != nCantidad)
            {
                aliActual.Cantidad = int.Parse(nCantidad);
                GestorBD.RecetarioContext.AlimentosListasCompras.Update(aliActual);
                GestorBD.RecetarioContext.SaveChanges();
                Filtrar();
                
                TableListaCompra.CurrentItem = null;
            }
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Cambair el texto Externamente</b> del <b>Texto Cantidad</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void Cantidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Funciones.EsNumeroSimple(e.Text);
        }

        #endregion

        #endregion
    }
}
