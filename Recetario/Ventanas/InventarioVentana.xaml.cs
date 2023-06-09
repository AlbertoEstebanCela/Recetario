using Microsoft.Win32;
using Recetario.Comandos;
using Recetario.Componentes;
using Recetario.Modelos;
using Recetario.VentanasSecundario;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Clase que contiene la Ventana Inventario
    /// </summary>
    /// <remarks>
    /// Hereda de Window.
    /// </remarks>
    public partial class InventarioVentana : Window
    {
        #region Constructores

        /// <summary>
        /// Constructor Vacio.
        /// </summary>
        public InventarioVentana()
        {
            InitializeComponent();
            ComandosMenu.AgregarBingings(CommandBindings);
            Filtrar();
        }

        #endregion

        #region Metodos

        #region Metodos Privados

        /// <summary>
        /// Metodo para Filtrar los alimentos del Iventario
        /// </summary>
        private void Filtrar()
        {
            List<Inventario> inventario = GestorBD.RecetarioContext.Inventarios.Where(x => string.IsNullOrEmpty(TxtBuscar.Text) || x.Alimento.Nombre.ToLower().Contains(TxtBuscar.Text.ToLower())).ToList();
            inventario.ForEach(x => { 
                if (x.Alimento.Imagen != null && x.Alimento.Imagen.Length > 0) x.Alimento.ImagenCargada = Funciones.BytesAImagen(x.Alimento.Imagen, x.Alimento.TipoImagen);
                else x.Alimento.ImagenCargada = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/NoImagen.png") as BitmapSource;
            });
            TableInventario.ItemsSource = inventario;
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
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en el <b>Boton Principal</b>.
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

        #region Eventos del Boton Lista Compra

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnListaCompra_RatonDentro(object sender, MouseEventArgs e)
        {
            BtnListaCompra_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnListaCompra_RatonFuera(object sender, MouseEventArgs e)
        {
            BtnListaCompra_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en el <b>Boton Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnListaCompra_Click(object sender, RoutedEventArgs e)
        {
            BtnListaCompra_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra(Encima)(Click).png") as ImageSource;

            VentanaListaCompra ventanaListaCompra = new VentanaListaCompra();
            if (WindowState == WindowState.Maximized)
            {
                ventanaListaCompra.WindowState = WindowState.Maximized;
            }
            else
            {
                ventanaListaCompra.Height = ActualHeight;
                ventanaListaCompra.Width = ActualWidth;
                ventanaListaCompra.Left = Left;
                ventanaListaCompra.Top = Top;
            }
            ventanaListaCompra.Show();
            Close();
        }

        #endregion

        #region Eventos del Boton Add a Lista Compra

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Add a Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAddListaCompra_RatonDentro(object sender, MouseEventArgs e)
        {
            ((Image)((Button)sender).Content).Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Add a Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAddListaCompra_RatonFuera(object sender, MouseEventArgs e)
        {
            ((Image)((Button)sender).Content).Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en el <b>Boton Add a Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAddListaCompra_Click(object sender, RoutedEventArgs e)
        {
            ((Image)((Button)sender).Content).Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra(Encima)(Click).png") as ImageSource;
            if(TableInventario.SelectedItem is not null and Inventario)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Desea agregar el alimento a la lista de compra?", "Lista de compra",MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(resultado == MessageBoxResult.Yes)
                {
                    AlimentoListaCompra comp = new AlimentoListaCompra()
                    {
                        ListaCompraId = GestorBD.RecetarioContext.ListasCompras.First().Id,
                        AlimentoId = ((Inventario)TableInventario.SelectedItem).AlimentoId,
                        Cantidad = 1,
                        Nota = ""
                    };
                    GestorBD.RecetarioContext.AlimentosListasCompras.Add(comp);
                    GestorBD.RecetarioContext.SaveChanges();
                }
            }
        }

        #endregion

        #region Eventos del Boton Add Inventario

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Add</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAddInventario_RatonDentro(object sender, MouseEventArgs e)
        {
            BtnAddInventario_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Add</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAddInventario_RatonFuera(object sender, MouseEventArgs e)
        {
            BtnAddInventario_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en el <b>Boton Add</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAddInventario_Click(object sender, RoutedEventArgs e)
        {
            BtnAddInventario_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd(Encima)(Click).png") as ImageSource;
            VentanaSeleccionarAlimento ventanaSeleccionarAlimento = new VentanaSeleccionarAlimento();
            ventanaSeleccionarAlimento.ShowDialog();
            Alimento aliSelec = VentanaSeleccionarAlimento.UltAlimentoSelec;
            if(aliSelec != null) 
            {
                Inventario inv = new Inventario()
                {
                    Id = 0,
                    AlimentoId = aliSelec.Id,
                    Cantidad = 1
                };
                GestorBD.RecetarioContext.Inventarios.Add(inv);
                GestorBD.RecetarioContext.SaveChanges();
                Filtrar();
            }
        }

        #endregion

        #region Eventos del Buscador

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Cambiar el Texto</b> del <b>Buscador</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtrar();
        }

        #endregion

        #region Eventos de Cantidad

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Perder el foco</b> del <b>Texto Cantidad</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void Cantidad_LostFocus(object sender, RoutedEventArgs e)
        {
            Inventario? invActual = TableInventario.CurrentItem as Inventario;
            string nCantidad = string.IsNullOrEmpty((sender as TextBox)?.Text) ? "0" : ((TextBox)sender).Text;
            if (invActual != null && int.Parse(nCantidad) == 0)
            {
                GestorBD.RecetarioContext.Inventarios.Remove(invActual);
                GestorBD.RecetarioContext.SaveChanges();
                Filtrar();
            }
            else if (invActual != null && invActual.Cantidad.ToString() != nCantidad)
            {
                invActual.Cantidad = int.Parse(nCantidad);
                GestorBD.RecetarioContext.Inventarios.Update(invActual);
                GestorBD.RecetarioContext.SaveChanges();
                Filtrar();
                TableInventario.CurrentItem = null;
            }
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Introducir texto de manera Indipendiente</b> del <b>Texto Cantidad</b>.
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
