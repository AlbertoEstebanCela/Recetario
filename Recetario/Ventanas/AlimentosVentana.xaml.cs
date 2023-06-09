using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Win32;
using Recetario.Comandos;
using Recetario.Componentes;
using Recetario.Modelos;
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
    /// Clase que contiene La ventana de Edicion de Alimentos.
    /// </summary>
    /// <remarks>
    /// Hereda de Window.
    /// </remarks>
    public partial class AlimentosVentana : Window
    {
        #region Propiedades

        /// <summary>
        /// Propiedad de la Imagen Temporal.
        /// </summary>
        private byte[] imgTemp;

        /// <summary>
        /// Propiedad Indicando el Tipo de la Imagen (png, jpg).
        /// </summary>
        private TipoImagen tipoTemp;

        /// <summary>
        /// Propiedad de la Imagen Cargada.
        /// </summary>
        private bool imgCargada;

        /// <summary>
        /// Propiedad del Alimento en Edicion.
        /// </summary>
        private Alimento alimentoEdicion;

        #endregion Propiedades

        #region Constructores

        /// <summary>
        /// Constructor Vacio.
        /// </summary>
        public AlimentosVentana()
        {
            InitializeComponent();
            ComandosMenu.AgregarBingings(CommandBindings);
            alimentoEdicion = new Alimento() { Id = -1};
            CargarCategoriasAlimentos();
            Filtrar();
        }

        #endregion

        #region Metodos

        #region Metodos Privados

        /// <summary>
        /// Metodo para carrgar las Categorias de los Alimentos en el Combo Box
        /// </summary>
        private void CargarCategoriasAlimentos()
        {
            List<Categoria> categorias = new List<Categoria>();
            categorias.Add(new Categoria() { Id = -1, Nombre = "Todos" });
            categorias.AddRange(GestorBD.RecetarioContext.Categorias.OrderBy(x => x.Nombre).ToList());
            CmbCategoriaAlimentos.ItemsSource = categorias;
            CmbCategoriaAlimentos.SelectedValue = -1;
            CmbCategoriasEdicion.ItemsSource = GestorBD.RecetarioContext.Categorias.ToList();
        }

        /// <summary>
        /// Metodo que filtra los Alimentos segun sus Categorias
        /// </summary>
        private void Filtrar()
        {
            LstAlimentos.ItemsSource = GestorBD.RecetarioContext.Alimentos.Where(x => (int)CmbCategoriaAlimentos.SelectedValue == -1 || x.CategoriaId == (int)CmbCategoriaAlimentos.SelectedValue).OrderBy(x => x.Nombre).ToList();
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
            ImgBtnListaCompra.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnListaCompra_RatonFuera(object sender, MouseEventArgs e)
        {
            ImgBtnListaCompra.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en el <b>Boton Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnListaCompra_Click(object sender, RoutedEventArgs e)
        {
            ImgBtnListaCompra.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra(Encima)(Click).png") as ImageSource;
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
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en el <b>Boton Inventario</b>.
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

        #region Eventos del Boton Cargar

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Cargar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnCargar_RatonDentro(object sender, MouseEventArgs e)
        {
            BtnCargar.Background = new SolidColorBrush(Color.FromRgb(195, 195, 195));
            BtnCargar.Foreground = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Cargar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnCargar_RatonFuera(object sender, MouseEventArgs e)
        {
            BtnCargar.Background = new SolidColorBrush(Color.FromRgb(88, 88, 88));
            BtnCargar.Foreground = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en el <b>Boton Cargar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnCargar_Click(object sender, RoutedEventArgs e)
        {
            BtnCargar.Background = new SolidColorBrush(Color.FromRgb(88, 88, 88));
            BtnCargar.Foreground = new SolidColorBrush(Colors.Black);
            if (LstAlimentos.SelectedItem != null && LstAlimentos.SelectedItem is Alimento)
            {
                LblCrearEditar.Content = "Editar Alimento";
                alimentoEdicion = (Alimento)LstAlimentos.SelectedItem;
                TxtNombreAlimento.Text = alimentoEdicion.Nombre;
                CmbCategoriasEdicion.SelectedValue = alimentoEdicion.CategoriaId;
                //Cargamos la imagen de BBDD
                if (alimentoEdicion.Imagen != null && alimentoEdicion.Imagen.Length > 0)
                {
                    //La convertimos de bytes a imagen
                    AlimentoImagen.Source = Funciones.BytesAImagen(alimentoEdicion.Imagen, alimentoEdicion.TipoImagen);
                }
                else
                {
                    AlimentoImagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/NoImagen.png") as ImageSource;
                }
            }
        }

        #endregion

        #region Eventos del Boton Eliminar

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Eliminar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnEliminar_RatonDentro(object sender, MouseEventArgs e)
        {
            ImgBtnEliminar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnEliminar(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Eliminar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnEliminar_RatonFuera(object sender, MouseEventArgs e)
        {
            ImgBtnEliminar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnEliminar.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en el <b>Boton Eliminar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            ImgBtnEliminar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnEliminar(Encima)(Click).png") as ImageSource;
            if (LstAlimentos.SelectedItem != null)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Desea eliminar el alimento seleccionado?", "Eliminar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.OK)
                {
                    GestorBD.RecetarioContext.Alimentos.Remove((Alimento)LstAlimentos.SelectedItem);
                    GestorBD.RecetarioContext.SaveChanges();
                    Filtrar();
                }
            }
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
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en el <b>Boton Confirmar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            ImgBtnConfirmar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnConfirmar(Encima)(Click).png") as ImageSource;
            MessageBoxResult resultado = MessageBox.Show("¿Desea guardar los cambios?", "Confirmar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.OK)
            {
                LstAlimentos.ItemsSource = new List<Alimento>();
                if (string.IsNullOrEmpty(TxtNombreAlimento.Text) || (int)CmbCategoriasEdicion.SelectedValue == 0)
                {
                    MessageBox.Show("Datos no validos para guardar un alimento", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                alimentoEdicion.Nombre = TxtNombreAlimento.Text;
                alimentoEdicion.CategoriaId = (int)CmbCategoriasEdicion.SelectedValue;
                if (imgCargada)
                {
                    alimentoEdicion.Imagen = imgTemp;
                    alimentoEdicion.TipoImagen = tipoTemp;
                }
                if (alimentoEdicion.Id == -1)
                {
                    alimentoEdicion.Id = 0;
                    GestorBD.RecetarioContext.Alimentos.Add(alimentoEdicion);
                }
                else
                {
                    GestorBD.RecetarioContext.Alimentos.Update(alimentoEdicion);
                }
                GestorBD.RecetarioContext.SaveChanges();
                CargarCategoriasAlimentos();
                Filtrar();
                imgCargada = false;
            }
        }
        #endregion

        #region Eventos del Boton Cancelar

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Cancelar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnCancelar_RatonDentro(object sender, MouseEventArgs e)
        {
            ImgBtnCancelar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnCancelar(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Cancelar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnCancelar_RatonFuera(object sender, MouseEventArgs e)
        {
            ImgBtnCancelar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnCancelar.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en el <b>Boton Cancelar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            ImgBtnCancelar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnCancelar(Encima)(Click).png") as ImageSource;
            TxtNombreAlimento.Text = "";
            CmbCategoriasEdicion.SelectedValue = 0;
            imgCargada = false;
            AlimentoImagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/NoImagen.png") as ImageSource;
            LblCrearEditar.Content = "Crear Alimento";
            alimentoEdicion = new Alimento() { Id = -1 };
        }

        #endregion

        #region Eventos del Imagen Alimento

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en la <b>Imagen Alimento</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void AlimentoImagen_RatonDentro(object sender, MouseEventArgs e)
        {
            AlimentoImagenEditar.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> de la <b>Imagen Alimento</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void AlimentoImagen_RatonFuera(object sender, MouseEventArgs e)
        {
            AlimentoImagenEditar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/EditarImagen.png") as ImageSource;
            AlimentoImagenEditar.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en la <b>Imagen Alimento</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void ImagenEditar_Click(object sender, MouseButtonEventArgs e)
        {
            AlimentoImagenEditar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/EditarImagen(Encima).png") as ImageSource;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Seleccionar imagen";
            //Extensiones soportadas
            openFileDialog.Filter = "Archivos de imagen| *.jpeg; *.jpg;*.png;";
            //Se muestra el dialog y se guarda si el usuario le dio a ok
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                //Nombre del archivo selecccionado con toda la ruta
                string filename = openFileDialog.FileName;
                if (filename.Split('.').Last().ToLower() == ".png")
                {
                    tipoTemp = TipoImagen.PNG;
                }
                else
                {
                    tipoTemp = TipoImagen.JPG;
                }
                //Cargamos la imagen del sistema de archivos
                BitmapImage bitmap = new BitmapImage(new Uri(filename));
                //La convertimos a Bytes
                imgTemp = Funciones.ImagenABytes(bitmap, tipoTemp);
                //La establecemos en el control de imagen
                AlimentoImagen.Source = bitmap;
                imgCargada = true;
            }
        }

        #endregion

        #region Eventos del Nombre Alimento

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Tener el Foco</b> el <b>Texto Nombre</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void TxtMarcaNombreReceta_Focus(object sender, RoutedEventArgs e)
        {
            TxtNombreAlimento.Visibility = Visibility.Visible;
            TxtNombreAlimento.Focus();
            TxtMarcaNombreReceta.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Perder el Foco</b> el <b>Texto Nombre</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void TxtNombreReceta_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtNombreAlimento.Text))
            {
                TxtNombreAlimento.Visibility = Visibility.Collapsed;
                TxtMarcaNombreReceta.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Eventos del Combo Box de Categoria Aliemnto

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Cambiar la Seleccion</b> del <b>ComboBox Categoria</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void CmbCategoriaAlimentos_CambioSeleccion(object sender, SelectionChangedEventArgs e)
        {
            Filtrar();
        }

        #endregion

        #endregion
    }
}
