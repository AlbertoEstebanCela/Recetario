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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Recetario.Componentes
{
    /// <summary>
    /// Clase que contiene el Componente de los Pasos usados en las Recetas.
    /// </summary>
    /// <remarks>
    /// Hereda de UserControl.
    /// </remarks>
    public partial class RecetaPasosComp : UserControl
    {
        #region Propiedades

        /// <summary>
        /// Propiedad almacena Paso a mostrar.
        /// </summary>
        private Paso paso;

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
        /// <b>Get</b> y <b>Set</b> (Privado) de la Propiedad <see cref="paso"/>.
        /// </summary>
        public Paso Paso
        {
            get { return paso; }
            private set { paso = value; }
        }

        /// <summary>
        /// <b>Get</b> Y <b>Set</b> de la Propiedad <see cref="editable"/>.
        /// </summary>
        /// <remarks>
        /// En el <b>Set</b> al cambiar de valor:
        /// <list type="bullet">
        /// <item>
        /// <term>True:</term>
        /// <description>Hace la ventana editable</description>
        /// </item>
        /// <item>
        /// <term>False:</term>
        /// <description>Hace la ventana no editable (solo lectura)</description>
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
        /// Constructor inicando la Posicion del Paso
        /// </summary>
        public RecetaPasosComp(int numero)
        {
            InitializeComponent();
            txtNumeroPaso.Content = numero;
            Editable = true;
            Paso = new Paso();
            Paso.Id = -1;
        }

        /// <summary>
        /// Constructor dando un Paso
        /// </summary>
        /// <param name="paso">
        /// Parametro con la que reyenar el componente.
        /// </param>
        public RecetaPasosComp(Paso paso)
        {
            InitializeComponent();
            Paso = paso;
            RellenarInfo();
            Editable = false;
        }

        #endregion

        #region Metodos

        #region Metodos Publicos

        /// <summary>
        /// Metodo que Actualiza el Contenido de Paso.
        /// </summary>
        public void ActualizarPaso()
        {
            paso.NPaso = (int)txtNumeroPaso.Content;
            paso.Descripcion = txtDescripcion.Text;
        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// Metodo que Rellena la ventana con la Informacion de la propiedad <see cref="paso"/>
        /// </summary>
        private void RellenarInfo()
        {
            if (paso != null)
            {
                txtNumeroPaso.Content = paso.NPaso;
                txtDescripcion.Text = paso.Descripcion;
            }
            else
            {
                paso = new Paso();
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
            Visibility visibilidad;
            Thickness tamañoDescripcion;
            Thickness borde;

            if (editable)
            {
                borde = new Thickness(2);
                tamañoDescripcion = new Thickness(0,0,50,0);
                visibilidad = Visibility.Visible;
                TxtDescripcion_PerderFocus(this, null);
            }
            else
            {
                borde = new Thickness(0);
                tamañoDescripcion = new Thickness(0,0,0,0);
                visibilidad = Visibility.Collapsed;
                txtMarcaDescripcion.Visibility = Visibility.Collapsed;
            }

            txtDescripcion.IsReadOnly = !editable;
            txtDescripcion.BorderThickness = borde;
            txtMarcaDescripcion.BorderThickness = borde;
            txtDescripcion.Margin = tamañoDescripcion;
            txtMarcaDescripcion.Margin = tamañoDescripcion;

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
            txtDescripcion.Visibility = Visibility.Visible;
            txtDescripcion.Focus();
            txtMarcaDescripcion.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Perder el Foco</b> el <b>Texto Descripcion</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void TxtDescripcion_PerderFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                txtDescripcion.Visibility = Visibility.Collapsed;
                txtMarcaDescripcion.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Eventos del Boton Quitar

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Quitar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnQuitar_RatonDentro(object sender, MouseEventArgs e)
        {
            ImagenBtnQuitar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnQuitar(Encima).png") as ImageSource;
            txtNumeroPaso.BorderBrush = new SolidColorBrush(Color.FromRgb(153, 0, 0));
            BordeDescripcion.BorderBrush = new SolidColorBrush(Color.FromRgb(153, 0, 0));
            Thickness borde = new Thickness(2);
            txtNumeroPaso.BorderThickness = borde;
            BordeDescripcion.BorderThickness = borde;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Quitar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnQuitar_RatonFuera(object sender, MouseEventArgs e)
        {
            ImagenBtnQuitar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnQuitar.png") as ImageSource;
            txtNumeroPaso.BorderBrush = new SolidColorBrush(Colors.Black);
            BordeDescripcion.BorderBrush = new SolidColorBrush(Colors.Black);
            Thickness borde = new Thickness(1);
            txtNumeroPaso.BorderThickness = borde;
            BordeDescripcion.BorderThickness = borde;
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