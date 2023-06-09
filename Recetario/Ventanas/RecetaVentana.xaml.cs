using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Win32;
using Recetario.Comandos;
using Recetario.Componentes;
using Recetario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Recetario.Ventanas
{
    /// <summary>
    /// Clase que contiene La ventana Visual, Edicion o Creacion de Recetas.
    /// </summary>
    /// <remarks>
    /// Hereda de Window.
    /// </remarks>
    public partial class RecetaVentana : Window
    {
        #region Propiedades

        /// <summary>
        /// Propiedad almacena Receta a mostrar.
        /// </summary>
        private Receta receta;

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
        /// Lista de componentes de Pasos para ser Borradas al Editar.
        /// </summary>
        private List<Paso> pasosBorrar = new List<Paso>();

        /// <summary>
        /// Lista de componentes de Alimentos para ser Borradas al Editar
        /// </summary>
        private List<RecetaAlimento> alimentosBorrar = new List<RecetaAlimento>();

        /// <summary>
        /// Propiedad que Almacena si la Ventana esta en modo Editable (True) o No (False)
        /// </summary>
        private bool editable;

        #endregion

        #region Getters y Setters de las Propiedades

        /// <summary>
        /// <b>Get</b> y <b>Set</b> (Privado) de la Propiedad <see cref="receta"/>.
        /// </summary>
        public Receta Receta
        {
            get { return receta; }
            private set { receta = value; }
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
        /// Constructor Vacio.
        /// </summary>
        public RecetaVentana()
        {
            InitializeComponent();
            ComandosMenu.AgregarBingings(CommandBindings);
            InicializarContenido();
            Editable = true;
        }

        /// <summary>
        /// Constructor Dando una Receta
        /// </summary>
        /// <param name="rectea">
        /// Parametro con la que reyenar el componente.
        /// </param>
        public RecetaVentana(Receta rectea)
        {
            InitializeComponent();

            Receta = rectea;

            ComandosMenu.AgregarBingings(CommandBindings);
            InicializarContenido();
            RellenarInfo();

            Editable = false;
        }

        #endregion

        #region Metodos

        #region Metodos Privados

        /// <summary>
        /// Metodo Privado que Inicializa los componentes de la ventana
        /// </summary>
        private void InicializarContenido()
        {
            ComboBoxTipos.ItemsSource = GestorBD.TiposList;
            imgCargada = false;
        }

        /// <summary>
        /// Metodo que Rellena la ventana con la Informacion de la propiedad <see cref="receta"/>
        /// </summary>
        private void RellenarInfo()
        {
            if (receta != null)
            {
                // Rellenar los campos con la informacion de la Receta especificada
                BtnFavorito_RatonFuera(this, new RoutedEventArgs() as MouseEventArgs);
                TxtNombreReceta.Text = receta.Nombre;
                ComboBoxTipos.SelectedValue = receta.TipoId;
                txtRacionesReceta.Text = Funciones.NumeroConCero(receta.Raciones);
                string[] tiempo = Funciones.CalcularHorasYMinutos(receta.Tiempo);
                txtHorasReceta.Text = tiempo[0];
                txtMinutosReceta.Text = tiempo[1];
                // Rellenar el contenedor de Alimentos de la Receta especificada
                IQueryable<RecetaAlimento> recetaAlimentoFiltrado = GestorBD.RecetarioContext.RecetasAlimentos.Where(recetaAlimento => recetaAlimento.RecetaId == receta.Id);
                ContAlimentos.Children.Clear();
                foreach (RecetaAlimento recetaAlimento in recetaAlimentoFiltrado)
                {
                    RecetaAlimentosComp recetaAlimentoComp = new RecetaAlimentosComp(recetaAlimento);
                    recetaAlimentoComp.Padding = new Thickness(2, 2, 2, 2);
                    recetaAlimentoComp.Width += (ContentAlimentos.ActualWidth/2) - 25;
                    recetaAlimentoComp.BtnQuitar_Pulsado += BtnQuitar_Alimento;
                    ContAlimentos.Children.Add(recetaAlimentoComp);
                }
                // Rellenar el contenedor de Pasos de la Receta especificada
                IQueryable<Paso> pasosFiltrados = GestorBD.RecetarioContext.Pasos.Where(paso => paso.RecetaId == receta.Id).OrderBy(x => x.NPaso);
                ContPasos.Children.Clear();
                foreach (Paso paso in pasosFiltrados)
                {
                    RecetaPasosComp recetaPasosComp = new RecetaPasosComp(paso);
                    recetaPasosComp.Padding = new Thickness(3, 3, 3, 3);
                    recetaPasosComp.Width += ContentPasos.ActualWidth - 50;
                    recetaPasosComp.BtnQuitar_Pulsado += BtnQuitar_Pasos;
                    ContPasos.Children.Add(recetaPasosComp);
                }
                //Cargamos la imagen de BBDD
                if(receta.Imagen != null && receta.Imagen.Length > 0)
                {
                    //La convertimos de bytes a imagen
                    Receta_Imagen.Source = Funciones.BytesAImagen(receta.Imagen, receta.TipoImagen);
                }
            }
        }

        /// <summary>
        /// Metodo para Guardar la Informacion de la Receta
        /// </summary>
        private void GuardarInfo()
        {
            receta.Nombre = TxtNombreReceta.Text;
            receta.TipoId = (int)ComboBoxTipos.SelectedValue;
            receta.Raciones = int.Parse(txtRacionesReceta.Text);
            receta.Tiempo = Funciones.CalcularTiempoMinutos(txtHorasReceta.Text, txtMinutosReceta.Text);
            if (imgCargada)
            {
                receta.Imagen = imgTemp;
                receta.TipoImagen = tipoTemp;
            }
        }

        /// <summary>
        /// Metodo que establece la ventana de modo editable o no
        /// </summary>
        /// <param name="editable">
        /// Valor de Tipo bool que indica si la ventana es editable o no
        /// </param>
        private void InicicalizarEditable(bool editable)
        {
            Thickness borde;
            Visibility visibilidad;

            if (editable)
            {
                borde = new Thickness(2);
                visibilidad = Visibility.Visible;
                BtnAdd_Add();
                Nombre_PerderFocus(this, null);
            }
            else
            {
                borde = new Thickness(0);
                visibilidad = Visibility.Collapsed;
                BtnAdd_quitar();
                TxtMarcaNombreReceta.Visibility = Visibility.Collapsed;
            }

            BtnConfirmar.Visibility = visibilidad;
            BtnCancelar.Visibility = visibilidad;
            BtnEliminar.Visibility = visibilidad;

            TxtNombreReceta.IsReadOnly = !editable;
            TxtNombreReceta.BorderThickness = borde;
            TxtMarcaNombreReceta.BorderThickness = borde;

            ComboBoxTipos.IsEditable = editable;
            ComboBoxTipos.IsEnabled = editable;
            ComboBoxTipos.BorderThickness = borde;

            txtRacionesReceta.IsReadOnly = !editable;
            txtRacionesReceta.BorderThickness = borde;

            txtHorasReceta.IsReadOnly = !editable;
            txtHorasReceta.BorderThickness = borde;

            txtMinutosReceta.IsReadOnly = !editable;
            txtMinutosReceta.BorderThickness = borde;

            foreach (UIElement elemento in ContAlimentos.Children)
            {
                try
                {
                    RecetaAlimentosComp recetaAlimentosComp = (RecetaAlimentosComp)elemento;
                    recetaAlimentosComp.Editable = editable;
                }
                catch (InvalidCastException ex)
                {
                    //No hacer nada
                }
            }
            foreach (UIElement elemento in ContPasos.Children)
            {
                try
                {
                    RecetaPasosComp recetaPasosComp = (RecetaPasosComp)elemento;
                    recetaPasosComp.Editable = editable;
                }
                catch (InvalidCastException ex)
                {
                    //No hacer nada
                }
            }
        }

        #region Controles de los Botones Add

        /// <summary>
        /// Metodo que incluye el boton Add en los contenedores
        /// </summary>
        /// <param name="contenedor">Contenedor donde se eliminara el boton Add</param>
        private void BtnAdd_Add()
        {
            BtnAddElemento btnAddAlimentos = new BtnAddElemento(250, 25);
            btnAddAlimentos.BotonPulsado += btnAddAlimentos_Click;
            btnAddAlimentos.Padding = new Thickness(2, 2, 2, 2);

            BtnAddElemento btnAddPasos = new BtnAddElemento(500, 50);
            btnAddPasos.BotonPulsado += btnAddPasos_Click;
            btnAddPasos.Padding = new Thickness(3, 3, 3, 3);

            ContAlimentos.Children.Add(btnAddAlimentos);
            ContPasos.Children.Add(btnAddPasos);
        }

        /// <summary>
        /// Metodo que que se elimina el boton Add de los contenedores
        /// </summary>
        /// <param name="contenedor">Contenedor donde se eliminara el boton Add</param>
        private void BtnAdd_quitar()
        {
            if (ContAlimentos.Children[ContAlimentos.Children.Count - 1] is BtnAddElemento)
            {
                ContAlimentos.Children.RemoveAt(ContAlimentos.Children.Count - 1);
            }
            if (ContPasos.Children[ContPasos.Children.Count - 1] is BtnAddElemento)
            {
                ContPasos.Children.RemoveAt(ContPasos.Children.Count - 1);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Eventos

        #region Eventos de la ventana

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Cambiar de Tamaño</b> la <b>Ventana</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void CambioTamañoVentana(object sender, SizeChangedEventArgs e)
        {
            Receta_Imagen.Height = ActualHeight - 350;
            Receta_Imagen_Editar.Height = ActualHeight - 375;
            
            foreach (UIElement elemento in ContAlimentos.Children)
            {
                UserControl comp = (UserControl)elemento;
                comp.Width = (ContentAlimentos.ActualWidth/2) - 25;
            }
            foreach (UIElement elemento in ContPasos.Children)
            {
                UserControl comp = (UserControl)elemento;
                comp.Width = ContentPasos.ActualWidth - 50;
            }
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Cerrar</b> la <b>Ventana</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void CerrarVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Editable = false;
        }

        #endregion

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
            if (editable)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Desea cancelar los cambios?", "Cancelar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.OK)
                {
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
            }
            else
            {
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
        }

        #endregion

        #region Eventos del Boton Editar

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Editar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnEditar_RatonDentro(object sender, MouseEventArgs e)
        {
            ImgBtnEditar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnEditar(Encima).png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Editar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnEditar_RatonFuera(object sender, MouseEventArgs e)
        {
            ImgBtnEditar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnEditar.png") as ImageSource;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Editar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            ImgBtnEditar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnEditar(Encima)(Click).png") as ImageSource;
            Editable = true;
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
            MessageBoxResult resultado = MessageBox.Show("¿Desea guardar los cambios?", "Confirmar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.OK)
            {
                Editable = false;
                if(receta == null)
                {
                    receta = new Receta();
                    GuardarInfo();
                    GestorBD.RecetarioContext.Recetas.Add(receta);
                    GestorBD.RecetarioContext.SaveChanges();
                }
                else
                {
                    GuardarInfo();
                    GestorBD.RecetarioContext.Recetas.Update(receta);
                    GestorBD.RecetarioContext.SaveChanges();
                }
                foreach(RecetaPasosComp pasoComp in ContPasos.Children)
                {
                    pasoComp.ActualizarPaso();
                    if(pasoComp.Paso.Id == -1)
                    {
                        pasoComp.Paso.Id = 0;
                        pasoComp.Paso.RecetaId = receta.Id;
                        GestorBD.RecetarioContext.Pasos.Add(pasoComp.Paso);
                    }
                    else
                    {
                        GestorBD.RecetarioContext.Pasos.Update(pasoComp.Paso);
                    }
                }
                foreach(RecetaAlimentosComp alimentoComp in ContAlimentos.Children)
                {
                    alimentoComp.ActualizarRecetaAlimento();
                    if(alimentoComp.RecetaAlimento.RecetaId == -1)
                    {
                        alimentoComp.RecetaAlimento.RecetaId = receta.Id;
                        GestorBD.RecetarioContext.RecetasAlimentos.Add(alimentoComp.RecetaAlimento);
                    }
                    else
                    {
                        GestorBD.RecetarioContext.RecetasAlimentos.Update(alimentoComp.RecetaAlimento);
                    }
                }
                foreach(RecetaAlimento reAliBrr in alimentosBorrar)
                {
                    GestorBD.RecetarioContext.RecetasAlimentos.Remove(reAliBrr);
                }
                foreach (Paso pasoBrr in pasosBorrar)
                {
                    GestorBD.RecetarioContext.Pasos.Remove(pasoBrr);
                }
                GestorBD.RecetarioContext.SaveChanges();
                //Recargamos
                GestorBD.Inicializar();
                InicializarContenido();
                RellenarInfo();
                CambioTamañoVentana(sender, null);
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
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Cancelar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            ImgBtnCancelar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnCancelar(Encima)(Click).png") as ImageSource;
            MessageBoxResult resultado = MessageBox.Show("¿Desea cancelar los cambios?", "Cancelar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.OK)
            {
                pasosBorrar.Clear();
                alimentosBorrar.Clear();
                if(receta == null)
                {
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
                else
                {
                    Editable = false;
                    GestorBD.Inicializar();
                    InicializarContenido();
                    RellenarInfo();
                    CambioTamañoVentana(sender, null);
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
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Eliminar</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            ImgBtnEliminar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnEliminar(Encima)(Click).png") as ImageSource;
            if(receta != null)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Desea eliminar la receta?", "Eliminar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.OK)
                {
                    foreach (RecetaAlimento reAli in GestorBD.RecetarioContext.RecetasAlimentos.Where(x => x.RecetaId == receta.Id))
                    {
                        GestorBD.RecetarioContext.RecetasAlimentos.Remove(reAli);
                    }
                    foreach (Paso paso in GestorBD.RecetarioContext.Pasos.Where(x => x.RecetaId == receta.Id))
                    {
                        GestorBD.RecetarioContext.Pasos.Remove(paso);
                    }
                    GestorBD.RecetarioContext.Recetas.Remove(receta);
                    GestorBD.RecetarioContext.SaveChanges();
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
            }
            else
            {
                MessageBox.Show("La receta aun no ha sido creada.","Informacion",MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Lista Compra</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnListaCompra_Click(object sender, RoutedEventArgs e)
        {
            ImgBtnListaCompra.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnListaCompra(Encima)(Click).png") as ImageSource;

            if (editable)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Desea cancelar los cambios?", "Cancelar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.OK)
                {
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
            }
            else
            {
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

            if (editable)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Desea cancelar los cambios?", "Cancelar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.OK)
                {
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
            }
            else
            {
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
        }

        #endregion

        #region Eventos del Boton Favoritos

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en el <b>Boton Favoritos</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnFavorito_RatonDentro(object sender, MouseEventArgs e)
        {
            if (receta?.Favorito ?? false)
            {
                BtnFavorito_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnFavorito(Seleccionado)(Encima).png") as ImageSource;
            }
            else
            {
                BtnFavorito_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnFavorito(NoSeleccionado)(Encima).png") as ImageSource;
            }
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> del <b>Boton Favoritos</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnFavorito_RatonFuera(object sender, MouseEventArgs e)
        {
            if (receta?.Favorito ?? false)
            {
                BtnFavorito_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnFavorito(Seleccionado).png") as ImageSource;
            }
            else
            {
                BtnFavorito_Imagen.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/BtnFavorito(NoSeleccionado).png") as ImageSource;
            }
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Favoritos</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnFavorito_Click(object sender, RoutedEventArgs e)
        {
            if(receta != null)
            {
                receta.Favorito = !receta.Favorito;
                BtnFavorito_RatonFuera(this, new RoutedEventArgs() as MouseEventArgs);
                GestorBD.RecetarioContext.Update(receta);
                GestorBD.RecetarioContext.SaveChanges();
            }
        }

        #endregion

        #region Eventos del Boton Add Alimentos

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Add Alimentos</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void btnAddAlimentos_Click(object sender, EventArgs e)
        {
            BtnAdd_quitar();

            RecetaAlimentosComp recetaAlimentoComp = new RecetaAlimentosComp();
            recetaAlimentoComp.Padding = new Thickness(2, 2, 2, 2);
            recetaAlimentoComp.Width = (ContentAlimentos.ActualWidth / 2) - 25;
            recetaAlimentoComp.BtnQuitar_Pulsado += BtnQuitar_Alimento;
            ContAlimentos.Children.Add(recetaAlimentoComp);

            BtnAdd_Add();
        }

        #endregion

        #region Evento del Boton Add Pasos

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Add Pasos</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void btnAddPasos_Click(object sender, EventArgs e)
        {
            BtnAdd_quitar();

            RecetaPasosComp recetaPasosComp = new RecetaPasosComp(ContPasos.Children.Count+1);
            recetaPasosComp.Padding = new Thickness(3, 3, 3, 3);
            recetaPasosComp.Width = ContentPasos.ActualWidth - 50;
            recetaPasosComp.BtnQuitar_Pulsado += BtnQuitar_Pasos;
            ContPasos.Children.Add(recetaPasosComp);

            BtnAdd_Add();
        }

        #endregion

        #region Evento de los Botones Quitar Componenetes

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Quitar Alimento Componenetes</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnQuitar_Alimento(object sender, EventArgs e)
        {
            RecetaAlimentosComp recetaAlimento = (RecetaAlimentosComp)sender;
            if (recetaAlimento.RecetaAlimento.RecetaId != -1)
            {
                alimentosBorrar.Add(recetaAlimento.RecetaAlimento);
            }
            ContAlimentos.Children.Remove(recetaAlimento);
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> al <b>Boton Quitar Paso Componenetes</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void BtnQuitar_Pasos(object sender, EventArgs e)
        {
            BtnAdd_quitar();
            RecetaPasosComp pasoQuitar = (RecetaPasosComp)sender;
            if(pasoQuitar.Paso.Id != -1)
            {
                pasosBorrar.Add(pasoQuitar.Paso);
            }
            ContPasos.Children.Remove(pasoQuitar);

            for (int i = 0; i < ContPasos.Children.Count; i++)
            {
                RecetaPasosComp recetaPasosComp = (RecetaPasosComp)ContPasos.Children[i];
                recetaPasosComp.Editable = editable;
                recetaPasosComp.txtNumeroPaso.Content = i + 1;
            }

            BtnAdd_Add();
        }

        #endregion

        #region Eventos del Nombre

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Tener el Foco</b> del <b>Nombre</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void Nombre_Focus(object sender, RoutedEventArgs e)
        {
            TxtNombreReceta.Visibility = Visibility.Visible;
            TxtNombreReceta.Focus();
            TxtMarcaNombreReceta.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Perder el Foco</b> del <b>Nombre</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void Nombre_PerderFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtNombreReceta.Text))
            {
                TxtNombreReceta.Visibility = Visibility.Collapsed;
                TxtMarcaNombreReceta.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Eventos de la Imagen Receta

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Entrar el Raton</b> en la <b>Imagen Receta</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void RecetaImagen_EntrarRaton(object sender, RoutedEventArgs e)
        {
            if (editable)
            {
                Receta_Imagen_Editar.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Salir el Raton</b> de la <b>Imagen Receta</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void RecetaImagen_SalirRaton(object sender, RoutedEventArgs e)
        {
            Receta_Imagen_Editar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/EditarImagen.png") as ImageSource;
            Receta_Imagen_Editar.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al hacer <b>Click</b> en la <b>Imagen Receta</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void RecetaComponente_Click(object sender, MouseButtonEventArgs e)
        {
            Receta_Imagen_Editar.Source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Recursos/EditarImagen(Encima).png") as ImageSource;
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
                Receta_Imagen.Source = bitmap;
                imgCargada = true;
            }
        }

        #endregion

        #region Eventos Tiempo Receta

        /// <summary>
        /// <b>Evento:</b> Metodo que que se Ejecuta al <b>Cambiar el texto de manera Externa</b> en los <b>Textos de tiempo</b>.
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private void HorasMinutosReceta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Funciones.EsNumeroSimple(e.Text);
        }

        #endregion

        #endregion
    }
}
