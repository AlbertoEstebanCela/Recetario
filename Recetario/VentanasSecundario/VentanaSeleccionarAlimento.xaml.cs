using Recetario.Modelos;
using Recetario.Ventanas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace Recetario.VentanasSecundario
{
    /// <summary>
    /// Clase que contiene La Ventana para Selccionar Alimento.
    /// </summary>
    /// <remarks>
    /// Hereda de Window.
    /// </remarks>
    public partial class VentanaSeleccionarAlimento : Window
    {
        #region Propiedades

        /// <summary>
        /// Propiedad estatica que Almacena el ultimo Alimento Seleccionado
        /// </summary>
        public static Alimento UltAlimentoSelec { get; private set; }

        #endregion

        #region Getters y Setters de las Propiedades

        /// <summary>
        /// <b>Get</b> y <b>Set</b> (Privado) del Alimento/>.
        /// </summary>
        public Alimento Alimento
        {
            get
            {
                Alimento a = null;
                if (LstAlimentos.SelectedItem is Alimento)
                {
                    a = (Alimento)LstAlimentos.SelectedItem;
                }
                return a;
            }
            private set { LstAlimentos.SelectedItem = value; }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor Vacio.
        /// </summary>
        public VentanaSeleccionarAlimento()
        {
            InitializeComponent();
            CargarCategoriasAlimentos();
            UltAlimentoSelec = null;
        }

        #endregion

        #region Metodos

        #region Metodos Privados

        /// <summary>
        /// Metodo Carrgar las categorias de los Alimentos en el Combo Box
        /// </summary>
        private void CargarCategoriasAlimentos()
        {
            List<Categoria> categorias = new List<Categoria>();
            categorias.Add(new Categoria() { Id = -1, Nombre = "Todos" });
            categorias.AddRange(GestorBD.RecetarioContext.Categorias.OrderBy(x => x.Nombre).ToList());
            CmbCategoriaAlimentos.ItemsSource = categorias;
            CmbCategoriaAlimentos.SelectedValue = -1;
        }

        #endregion

        #endregion

        #region Eventos

        #region Eventos del Boton Confirmar

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Confirmar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnConfirmar_RatonDentro(object sender, MouseEventArgs e)
        {
            BtnConfirmar.Background = new SolidColorBrush(Color.FromRgb(153, 255, 153));
            BtnConfirmar.Foreground = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Confirmar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnConfirmar_RatonFuera(object sender, MouseEventArgs e)
        {
            BtnConfirmar.Background = new SolidColorBrush(Color.FromRgb(0, 153, 0));
            BtnConfirmar.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Confirmar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            BtnConfirmar.Background = new SolidColorBrush(Color.FromRgb(0, 153, 0));
            BtnConfirmar.Foreground = new SolidColorBrush(Colors.Black);
            UltAlimentoSelec = Alimento;
            Close();
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
            BtnCancelar.Background = new SolidColorBrush(Color.FromRgb(195, 195, 195));
            BtnCancelar.Foreground = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Cancelar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnCancelar_RatonFuera(object sender, MouseEventArgs e)
        {
            BtnCancelar.Background = new SolidColorBrush(Color.FromRgb(88, 88, 88));
            BtnCancelar.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Cancelar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            BtnCancelar.Background = new SolidColorBrush(Color.FromRgb(88, 88, 88));
            BtnCancelar.Foreground = new SolidColorBrush(Colors.Black);
            Alimento = null;
            Close();
        }

        #endregion

        #region Eventos del ComboBox de Categoria de Alimentos

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Cambiar el Valor</b> del <b>ComboBox de Categoria</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void CmbCategoriaAlimentos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LstAlimentos.ItemsSource = GestorBD.RecetarioContext.Alimentos.Where(x => (int)CmbCategoriaAlimentos.SelectedValue == -1 || x.CategoriaId == (int)CmbCategoriaAlimentos.SelectedValue).OrderBy(x => x.Nombre).ToList();
        }

        #endregion

        #endregion
    }
}
