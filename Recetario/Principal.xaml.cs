using Recetario.Comandos;
using Recetario.Componentes;
using Recetario.Modelos;
using Recetario.Ventanas;
using System;
using System.Collections;
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

namespace Recetario
{
    /// <summary>
    /// Clase de la Ventana Principal.
    /// </summary>
    /// <remarks>
    /// Hereda de Window.
    /// </remarks>
    public partial class Principal : Window
    {
        #region Propiedades
        
        /// <summary>
        /// Propiedad que comprueba si la ventana principal esta inicializada
        /// </summary>
        private bool inicializado;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la clase Principal.
        /// </summary>
        public Principal()
        {
            InitializeComponent();
            inicializado = false;
            
            ComandosMenu.AgregarBingings(CommandBindings);
            GestorBD.Inicializar();
            CargarCondiciones();
            CargarCategoriasRecetas();
            CargarCategoriasAlimentos();
            inicializado = true;
            LimpiarFiltros_Click(this, new RoutedEventArgs());
            ExpanderFiltros_Cerrado(this, new RoutedEventArgs());
            Filtrar();
        }

        #endregion

        #region Metodos

        #region Filtros

        #region Metodos de los Filtros

        /// <summary>
        /// Metodo para cargar los tipos de las Recetas en el Combo Box de los filtros
        /// </summary>
        private void CargarCategoriasRecetas()
        {
            List<Tipo> categorias = new List<Tipo>();
            categorias.Add(new Tipo() { Id = -1, Nombre = "Todas" });
            categorias.AddRange(GestorBD.RecetarioContext.Tipos.OrderBy(x => x.Nombre).ToList());
            CmbCategoriaReceta.ItemsSource = categorias;
            CmbCategoriaReceta.SelectedValue = -1;
        }

        /// <summary>
        /// Metodo para carrgar las categorias de los Alimentos en el Combo Box de los filtros
        /// </summary>
        private void CargarCategoriasAlimentos()
        {
            List<Categoria> categorias = new List<Categoria>();
            categorias.Add(new Categoria() { Id = -1, Nombre = "Todos" });
            categorias.AddRange(GestorBD.RecetarioContext.Categorias.OrderBy(x => x.Nombre).ToList());
            CmbCategoriaAlimentos.ItemsSource = categorias;
            CmbCategoriaAlimentos.SelectedValue = -1;
        }

        /// <summary>
        /// Metodo para cargar las Condiciones en el Combo Box de los filtros
        /// </summary>
        private void CargarCondiciones()
        {
            List<Condicion> condiciones = new List<Condicion>();
            condiciones.Add(new Condicion() { Id = -1, Nombre = "Todas" });
            condiciones.AddRange(GestorBD.RecetarioContext.Condiciones.OrderBy(x => x.Nombre).ToList());
            CmbCondiciones.ItemsSource = condiciones;
            CmbCondiciones.SelectedValue = -1;
        }

        /// <summary>
        /// Metodo para actualizar la tabla segun los filtros seleccionados
        /// </summary>
        private void Filtrar()
        {
            ContenedorRecetas.Children.Clear();
            foreach (Receta receta in GestorBD.RecetarioContext.Recetas.OrderBy(x => x.Nombre))
            {
                List<Alimento> alimentosTiene = (from alimento in GestorBD.RecetarioContext.Alimentos
                                                 join receAli in GestorBD.RecetarioContext.RecetasAlimentos on alimento.Id equals receAli.AlimentoId
                                                 where receAli.RecetaId == receta.Id
                                                 select alimento).ToList();

                List<Alimento> alimentosCondiciones = (from alimento in GestorBD.RecetarioContext.Alimentos
                                                       join aliCondi in GestorBD.RecetarioContext.AlimentosCondiciones on alimento.Id equals aliCondi.AlimentoId
                                                       where aliCondi.CondicionId == (int)CmbCondiciones.SelectedValue
                                                       select alimento).ToList();
                List<Alimento> alimentosInventario = (from alimento in GestorBD.RecetarioContext.Alimentos
                                                      join aliInv in GestorBD.RecetarioContext.Inventarios on alimento.Id equals aliInv.AlimentoId
                                                      select alimento).ToList();

                if (inicializado && ((int)CmbCategoriaReceta.SelectedValue == -1 || (int)CmbCategoriaReceta.SelectedValue == receta.TipoId) &&
                    (LstAlimentos.SelectedItems.Count == 0 || ComprobarAlimentos(alimentosTiene, LstAlimentos.SelectedItems)) &&
                    (alimentosCondiciones.Count == 0 || ComprobarAlimentos(alimentosCondiciones, alimentosTiene)) &&
                    (!(ChkSoloInventario.IsChecked ?? false) || ComprobarAlimentos(alimentosInventario, alimentosTiene)) && 
                    (string.IsNullOrEmpty(TxtBuscar.Text) || receta.Nombre.ToLower().Contains(TxtBuscar.Text.ToLower())) &&
                    (!(ChkFavoritos.IsChecked ?? false) || receta.Favorito))
                {
                    RecetaComponente recetaComp = new RecetaComponente(receta, this);
                    recetaComp.Padding = new Thickness(5, 5, 5, 5);
                    ContenedorRecetas.Children.Add(recetaComp);
                }
            }
        }

        /// <summary>
        /// Metodo que comprueba si los alimentos los contiene
        /// </summary>
        /// <param name="alimentosTiene"> alimentos que tiene la receta</param>
        /// <param name="alimentoDebeTener"> alimentos que debe tener la receta</param>
        /// <returns></returns>
        private bool ComprobarAlimentos(List<Alimento> alimentosTiene, IList alimentoDebeTener)
        {
            bool result = false;
            foreach (Alimento alimento in alimentoDebeTener)
            {
                result = alimentosTiene.Exists(x => x.Id == alimento.Id);
                if (!result)
                {
                    break;
                }
            }
            return result;
        }

        #endregion

        #region Eventos Filtros

        #region Eventos de la barra de busqueda

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al <b>Cambiar el Contenido</b> en el <b>Buscador</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void Buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtrar();
        }

        #endregion

        #region Eventos del Boton Limpiar Filtros

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al hacer <b>Click</b> en el <b>Limpiar Filtro</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void LimpiarFiltros_Click(object sender, RoutedEventArgs e)
        {
            ChkSoloInventario.IsChecked = false;
            CmbCategoriaReceta.SelectedIndex = 0;
            CmbCondiciones.SelectedIndex = 0;
            CmbCategoriaAlimentos.SelectedIndex = 0;
            LstAlimentos.UnselectAll();
        }

        #endregion

        #region Eventos del CheckBox Solo Inventario

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al estar el <b>CheckBox activado</b> de <b>Filtrar por inventario</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void SoloInventario_Checked(object sender, RoutedEventArgs e)
        {
            CmbCategoriaAlimentos.IsEnabled = false;
            LstAlimentos.IsEnabled = false;
            CmbCategoriaAlimentos.SelectedIndex = 0;
            LstAlimentos.UnselectAll();
            Filtrar();
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al estar el <b>CheckBox desactivado</b> de <b>Filtrar por inventario</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void SoloInventario_Unchecked(object sender, RoutedEventArgs e)
        {
            CmbCategoriaAlimentos.IsEnabled = true;
            LstAlimentos.IsEnabled = true;
            Filtrar();
        }

        #endregion

        #region Eventos del CheckBox Solo Favoritos

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al estar el <b>CheckBox activado</b> de <b>Filtrar por inventario</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void Favoritos_Checked(object sender, RoutedEventArgs e)
        {
            Filtrar();
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al estar el <b>CheckBox desactivado</b> de <b>Filtrar por inventario</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void Favoritos_Unchecked(object sender, RoutedEventArgs e)
        {
            Filtrar();
        }

        #endregion

        #region Eventos del ComboBox de Tipos

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al al cambiar el valor del <b>ComboBox de Tipos</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void CmbCategoriaReceta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtrar();
        }

        #endregion

        #region Eventos del ComboBox de Categoria de Alimentos

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al al cambiar el valor del <b>ComboBox de Categoria</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void CmbCategoriaAlimentos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LstAlimentos.ItemsSource = GestorBD.RecetarioContext.Alimentos.Where(x => (int)CmbCategoriaAlimentos.SelectedValue == -1 || x.CategoriaId == (int)CmbCategoriaAlimentos.SelectedValue).OrderBy(x => x.Nombre).ToList();
            Filtrar();
        }

        #endregion

        #region Eventos del ComboBox de Condiciones

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al al cambiar el valor del <b>ComboBox de Condiciones</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void CmbCondiciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtrar();
        }

        #endregion

        #region Eventos del ListBox de Alimentos

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al al cambiar el valor del <b>ListBox de Alimentos</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void LstAlimentos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtrar();
        }

        #endregion

        #endregion

        #endregion

        #region Eventos

        #region Eventos del Expander Filtros

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al <b>cerrar</b> el <b>Expander Filtros</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void ExpanderFiltros_Cerrado(object sender, RoutedEventArgs e)
        {
            ExpanderFiltros.Margin = new Thickness(5, 41, 0, 0);
            ExpanderFiltros.VerticalAlignment = VerticalAlignment.Top;
            ExpanderFiltros.Height = 32;

            ExpanderFiltros.Background = new SolidColorBrush(Color.FromRgb(34, 90, 255));
            ExpanderFiltros_Titulo.Foreground = new SolidColorBrush(Colors.White);

            BordeContenedorRecetas.Margin = new Thickness(5, 83, 5, 5);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al <b>abrir</b> el <b>Expander Filtros</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void ExpanderFiltros_Abierto(object sender, RoutedEventArgs e)
        {
            ExpanderFiltros.Margin = new Thickness(5, 41, 0, 5);
            ExpanderFiltros.VerticalAlignment = VerticalAlignment.Stretch;
            ExpanderFiltros.Height = double.NaN;

            ExpanderFiltros.Background = new SolidColorBrush(Color.FromRgb(153, 204, 255));
            ExpanderFiltros_Titulo.Foreground = new SolidColorBrush(Colors.Black);

            BordeContenedorRecetas.Margin = new Thickness(260, 83, 5, 5);
        }

        #endregion

        #region Eventos del Boton Add Receta

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al <b>entrar el raton</b> del <b>Boton Add Receta</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void BtnAddReceta_RatonDentro(object sender, MouseEventArgs e)
        {
            BtnAddReceta_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al <b>salir el raton</b> del <b>Boton Add Receta</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void BtnAddReceta_RatonFuera(object sender, MouseEventArgs e)
        {
            BtnAddReceta_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al hacer <b>click</b> en el <b>Boton Add Receta</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void BtnAddReceta_Click(object sender, RoutedEventArgs e)
        {
            BtnAddReceta_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd(Encima)(Click).png") as ImageSource;
            RecetaVentana recetaVentana = new RecetaVentana();
            if (WindowState == WindowState.Maximized)
            {
                recetaVentana.WindowState = WindowState.Maximized;
            }
            else
            {
                recetaVentana.Height = ActualHeight;
                recetaVentana.Width = ActualWidth;
                recetaVentana.Left = Left;
                recetaVentana.Top = Top;
            }
            recetaVentana.Show();
            Close();
        }

        #endregion

        #region Eventos del Boton Lista Compra

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al <b>entrar el raton</b> del <b>Boton Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void BtnListaCompra_RatonDentro(object sender, MouseEventArgs e)
        {
            BtnListaCompra_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al <b>salir el raton</b> del <b>Boton Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void BtnListaCompra_RatonFuera(object sender, MouseEventArgs e)
        {
            BtnListaCompra_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al hacer <b>click</b> en el <b>Boton Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
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

        #region Eventos del Boton Inventario

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al <b>entar el raton</b> del <b>Boton Inventario</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void BtnInventario_RatonDentro(object sender, MouseEventArgs e)
        {
            BtnInventario.Background = new SolidColorBrush(Color.FromRgb(153, 204, 255));
            BtnInventario.Foreground = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al <b>salir el raton</b> del <b>Boton Inventario</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
        private void BtnInventario_RatonFuera(object sender, MouseEventArgs e)
        {
            BtnInventario.Background = new SolidColorBrush(Color.FromRgb(34, 90, 255));
            BtnInventario.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se ejecuta al hacer <b>click</b> en el <b>Boton Inventario</b>.
        /// </summary>
        /// <param name="sender">Objeto que envia el evento.</param>
        /// <param name="e">Informacion asociada al Evento.</param>
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

        #endregion

        #endregion
    }
}