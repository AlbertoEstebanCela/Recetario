using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Clase que contiene el componente del Boton para incluir elementos en las recetas.
    /// </summary>
    /// <remarks>
    /// Hereda de UserControl.
    /// </remarks>
    public partial class BtnAddElemento : UserControl
    {
        #region Propiedades

        /// <summary>
        /// Propiedad Evento Personalizado.
        /// </summary>
        public event EventHandler BotonPulsado;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor indicando el Ancho y el Alto.
        /// </summary>
        /// <param name="anchoBoton">Valor del Ancho del Boton.</param>
        /// <param name="altoBoton">Valor del Alto del Boton.</param>
        public BtnAddElemento(int anchoBoton, int altoBoton)
        {
            InitializeComponent();
            ContenedorBtnAdd.Width = anchoBoton;
            ContenedorBtnAdd.Height = altoBoton;
            BtnAdd.Width = altoBoton;
            BtnAdd.Height = altoBoton;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// <b>Evento:</b> Metodo que se ejecuta al <b>Entrar el Raton</b> en el <b>Boton Confirmar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAdd_RatonDentro(object sender, MouseEventArgs e)
        {
            ImagenBtnAdd.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que se ejecuta al <b>Salir el Raton</b> del <b>Boton Confirmar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAdd_RatonFuera(object sender, MouseEventArgs e)
        {
            ImagenBtnAdd.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que se ejecuta al hacer <b>Click</b> al <b>Boton Confirmar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ImagenBtnAdd.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnAdd(Encima)(Click).png") as ImageSource;
            BotonPulsado.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
