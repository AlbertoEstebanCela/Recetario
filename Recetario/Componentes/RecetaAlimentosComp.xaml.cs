using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Recetario.Comandos;
using Recetario.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Recetario.Componentes
{
    /// <summary>
    /// Clase que contiene el componente del Alimento usados en las recetas.
    /// </summary>
    /// <remarks>
    /// Hereda de UserControl.
    /// </remarks>
    public partial class RecetaAlimentosComp : UserControl
    {
        #region Propiedades

        /// <summary>
        /// Propiedad almacena la RecetaAlimento a mostrar.
        /// </summary>
        private RecetaAlimento recetaAlimento;

        /// <summary>
        /// Propiedad que almacena si el componente es Editable (<b>True</b>) o no (<b>False</b>).
        /// </summary>
        private bool editable;

        /// <summary>
        /// Propiedad Evento Personalizado al pulsar el Boton Quitar.
        /// </summary>
        public event EventHandler BtnQuitar_Pulsado;

        #endregion

        #region Getters y Setters de las Propiedades

        /// <summary>
        /// <b>Get</b> y <b>Set</b> (Privado) de la Propiedad <see cref="recetaAlimento"/>.
        /// </summary>
        public RecetaAlimento RecetaAlimento
        {
            get { return recetaAlimento; }
            private set { recetaAlimento = value; }
        }

        /// <summary>
        /// <b>Get</b> y <b>Set</b> de la Propiedad <see cref="editable"/>.
        /// </summary>
        /// <remarks>
        /// En el <b>Set</b> al cambiar de valor:
        /// <list type="bullet">
        /// <item>
        /// <term>True:</term>
        /// <description>Hace la ventana Editable.</description>
        /// </item>
        /// <item>
        /// <term>False:</term>
        /// <description>Hace la ventana No Editable (solo lectura).</description>
        /// </item>
        /// </list>
        /// </remarks>
        public bool Editable
        {
            get { return editable; }
            set
            {
                editable = value;
                InicicalizarEditable(value);
            }
        }

        #endregion

        #region Construcotres

        /// <summary>
        /// Constructor Vacio.
        /// </summary>
        public RecetaAlimentosComp()
        {
            InitializeComponent();
            InicializarContenido();
            RecetaAlimento = new RecetaAlimento();
            RecetaAlimento.RecetaId = -1;
            Editable = true;
        }

        /// <summary>
        /// Constructor dando una RecetaAlimento.
        /// </summary>
        /// <param name="recetaAlimento">
        /// Parametro con la que reyenar el componente.
        /// </param>
        public RecetaAlimentosComp(RecetaAlimento recetaAlimento)
        {
            InitializeComponent();
            InicializarContenido();
            RecetaAlimento = recetaAlimento;
            RellenarInfo(); 
            Editable = false;
        }

        #endregion

        #region Metodos

        #region Metodos Publicos

        /// <summary>
        /// Metodo que Actualiza el Contenido de RecetaAlimento.
        /// </summary>
        public void ActualizarRecetaAlimento()
        {
            recetaAlimento.Descripcion = TxtDescripcion.Text;
            recetaAlimento.AlimentoId = (int)CmbAlimento.SelectedValue;
        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// Metodo que Inicializa los Componentes de la ventana
        /// </summary>
        private void InicializarContenido()
        {
            CmbAlimento.ItemsSource = GestorBD.AlimentosList;
        }

        /// <summary>
        /// Metodo que Rellena la ventana con la Informacion de la propiedad <see cref="recetaAlimento"/>
        /// </summary>
        private void RellenarInfo()
        {
            if(recetaAlimento != null)
            {
                TxtDescripcion.Text = recetaAlimento.Descripcion;
                CmbAlimento.SelectedValue = recetaAlimento.AlimentoId;
            }
            else
            {
                recetaAlimento = new RecetaAlimento();
            }
        }

        /// <summary>
        /// Metodo que establece el Componente en modo Editable o No.
        /// </summary>
        /// <param name="editable">
        /// Valor que indica si la Ventana es Editable (<b>True</b>) o No (<b>False</b>).
        /// </param>
        public void InicicalizarEditable(bool editable)
        {
            Thickness borde;
            Thickness tamañoComboBox;
            Visibility visibilidad;

            if (editable)
            {
                borde = new Thickness(2);
                tamañoComboBox = new Thickness(0, 0, 24, 0);
                visibilidad = Visibility.Visible;
                TxtDescripcion_PerderFocus(this, null);
            }
            else
            {
                borde = new Thickness(0);
                tamañoComboBox = new Thickness(0, 0, 0, 0);
                visibilidad = Visibility.Collapsed;
                TxtMarcaDescripcion.Visibility = Visibility.Collapsed;
            }

            TxtDescripcion.IsReadOnly = !editable;
            TxtDescripcion.BorderThickness = borde;
            TxtMarcaDescripcion.BorderThickness = borde;

            CmbAlimento.IsEditable = editable;
            CmbAlimento.IsEnabled = editable;
            CmbAlimento.BorderThickness = borde;

            CmbAlimento.Margin = tamañoComboBox;
            BtnQuitar.Visibility = visibilidad;
        }

        #endregion

        #endregion

        #region Eventos

        #region Evento de Descripcion

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Tener el Foco</b> el <b>Texto Descripcion</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void TxtDescripcion_Focus(object sender, RoutedEventArgs e)
        {
            TxtDescripcion.Visibility = Visibility.Visible;
            TxtDescripcion.Focus();
            TxtMarcaDescripcion.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Perder el Foco</b> el <b>Texto Descripcion</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void TxtDescripcion_PerderFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtDescripcion.Text))
            {
                TxtDescripcion.Visibility = Visibility.Collapsed;
                TxtMarcaDescripcion.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Eventos Boton Quitar

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Quitar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnQuitar_RatonDentro(object sender, MouseEventArgs e)
        {
            ImagenBtnQuitar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnQuitar(Encima).png") as ImageSource;
            bordeTxtDescripcion.BorderBrush = new SolidColorBrush(Color.FromRgb(153, 0, 0));
            bordeCmbAlimento.BorderBrush = new SolidColorBrush(Color.FromRgb(153, 0, 0));
            Thickness borde = new Thickness(2);
            bordeTxtDescripcion.BorderThickness = borde;
            bordeCmbAlimento.BorderThickness = borde;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Quitar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnQuitar_RatonFuera(object sender, MouseEventArgs e)
        {
            ImagenBtnQuitar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnQuitar.png") as ImageSource;
            bordeTxtDescripcion.BorderBrush = new SolidColorBrush(Colors.Black);
            bordeCmbAlimento.BorderBrush = new SolidColorBrush(Colors.Black);
            Thickness borde = new Thickness(1);
            bordeTxtDescripcion.BorderThickness = borde;
            bordeCmbAlimento.BorderThickness = borde;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Quitar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnQuitar_Click(object sender, RoutedEventArgs e)
        {
            ImagenBtnQuitar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnQuitar(Encima)(Click).png") as ImageSource;
            BtnQuitar_Pulsado.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #endregion
    }
}
