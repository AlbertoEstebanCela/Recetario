using Microsoft.Win32;
using Recetario.Componentes;
using Recetario.Ventanas;
using Recetario.VentanasSecundario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Recetario.Comandos
{
    /// <summary>
    /// Clase que contiene las Funciones del Menu Bar.
    /// </summary>
    public static class ComandosMenu
    {
        #region Propiedades

        /// <summary>
        /// Propiedad indicando la Funcionalidad de salir.
        /// </summary>
        public static readonly RoutedUICommand Salir = new RoutedUICommand(
            "Salir", "Salir", typeof(ComandosMenu),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4, ModifierKeys.Alt)
            }
        );

        /// <summary>
        /// Propiedad indicando la Funcionalidad de Exportar a JSON.
        /// </summary>
        public static readonly RoutedUICommand ExportarJSON = new RoutedUICommand(
            "Exportar a JSON", "ExportarJSON", typeof(ComandosMenu),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F6, ModifierKeys.Alt)
            }
        );

        /// <summary>
        /// Propiedad indicando la Funcionalidad de Acerca De.
        /// </summary>
        public static readonly RoutedUICommand AcercaDe = new RoutedUICommand(
            "Acerca de", "AcercaDe", typeof(ComandosMenu),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Y, (int)ModifierKeys.Alt + ModifierKeys.Control)
            }
        );

        /// <summary>
        /// Propiedad indicando la Funcionalidad de Principal.
        /// </summary>
        public static readonly RoutedUICommand Principal = new RoutedUICommand(
            "Ventana Principal", "Principal", typeof(ComandosMenu),
            new InputGestureCollection()
            {
                new KeyGesture(Key.P, (int)ModifierKeys.Alt + ModifierKeys.Control)
            }
        );

        /// <summary>
        /// Propiedad indicando la Funcionalidad de Inevntario.
        /// </summary>
        public static readonly RoutedUICommand Inventario = new RoutedUICommand(
            "Ventana Inventario", "Inventario", typeof(ComandosMenu),
            new InputGestureCollection()
            {
                new KeyGesture(Key.I, (int)ModifierKeys.Alt + ModifierKeys.Control)
            }
        );

        /// <summary>
        /// Propiedad indicando la Funcionalidad de Lista Compra.
        /// </summary>
        public static readonly RoutedUICommand ListaCompra = new RoutedUICommand(
            "Ventana Lista Compra", "ListaCompra", typeof(ComandosMenu),
            new InputGestureCollection()
            {
                new KeyGesture(Key.L, (int)ModifierKeys.Alt + ModifierKeys.Control)
            }
        );

        /// <summary>
        /// Propiedad indicando la Funcionalidad de Alimentos.
        /// </summary>
        public static readonly RoutedUICommand Alimentos = new RoutedUICommand(
            "Ventana Alimentos", "Alimentos", typeof(ComandosMenu),
            new InputGestureCollection()
            {
                new KeyGesture(Key.A, (int)ModifierKeys.Alt + ModifierKeys.Control)
            }
        );

        #endregion

        #region Metodos

        /// <summary>
        /// Agregar Comandos.
        /// </summary>
        /// <param name="CommandBindings">Comando.</param>
        public static void AgregarBingings(CommandBindingCollection CommandBindings)
        {
            CommandBindings.Add(new CommandBinding(Salir, ComandoSalir_Executed, ComandoSalir_CanExecute));
            CommandBindings.Add(new CommandBinding(ExportarJSON, ComandoExportarJSON_Executed, ComandoExportarJSON_CanExecute));
            CommandBindings.Add(new CommandBinding(AcercaDe, ComandoAcercaDe_Executed, ComandoAcercaDe_CanExecute));
            CommandBindings.Add(new CommandBinding(Principal, ComandoPrincipal_Executed, ComandoPrincipal_CanExecute));
            CommandBindings.Add(new CommandBinding(Inventario, ComandoInventario_Executed, ComandoInventario_CanExecute));
            CommandBindings.Add(new CommandBinding(ListaCompra, ComandoListaCompra_Executed, ComandoListaCompra_CanExecute));
            CommandBindings.Add(new CommandBinding(Alimentos, ComandoAlimentos_Executed, ComandoAlimentos_CanExecute));
        }

        #region Comandos Salir

        /// <summary>
        /// Comando del Menu Salir (Puede Ejecutar).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoSalir_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Comando del Menu Salir (Ejecutar).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoSalir_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Comando ExportarJSON

        /// <summary>
        /// Comando del Menu Exportar a JSON (Puede Ejecutar).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoExportarJSON_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Comando del Menu Exportar a JSON (Funcionalidad).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoExportarJSON_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Archivo json| *.json;";
            sfd.DefaultExt = "*.json";
            sfd.OverwritePrompt = true;
            bool? result = sfd.ShowDialog();
            if(result == true)
            {
                GestorBD.ExportarAJSON(sfd.FileName);
            }
        }

        #endregion

        #region Comandos AcercaDe

        /// <summary>
        /// Comando del Menu Acerca De (Puede Ejecutar).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoAcercaDe_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Comando del Menu Acerca De (Funcionalidad).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoAcercaDe_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Informacion info = new Informacion();
            info.ShowDialog();
        }

        #endregion

        #region Comandos Ventana Principal

        /// <summary>
        /// Comando del Menu Principal (Puede Ejecutar).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoPrincipal_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Comando del Menu Principal (Funcionalidad).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoPrincipal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Window padre = (Window)Funciones.GetWindowPadre((FrameworkElement)sender);
            Principal principal = new Principal();
            if (padre.WindowState == WindowState.Maximized)
            {
                principal.WindowState = WindowState.Maximized;
            }
            else
            {
                principal.Height = padre.ActualHeight;
                principal.Width = padre.ActualWidth;
                principal.Left = padre.Left;
                principal.Top = padre.Top;
            }
            principal.Show();
            padre.Close();
        }

        #endregion

        #region Comandos Ventana Inventario

        /// <summary>
        /// Comando del Menu Inventario (Puede Ejecutar).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoInventario_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Comando del Menu Inventario (Funcionalidad).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoInventario_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Window padre = (Window)Funciones.GetWindowPadre((FrameworkElement)sender);
            InventarioVentana inventarioVentana = new InventarioVentana();
            if (padre.WindowState == WindowState.Maximized)
            {
                inventarioVentana.WindowState = WindowState.Maximized;
            }
            else
            {
                inventarioVentana.Height = padre.ActualHeight;
                inventarioVentana.Width = padre.ActualWidth;
                inventarioVentana.Left = padre.Left;
                inventarioVentana.Top = padre.Top;
            }
            inventarioVentana.Show();
            padre.Close();
        }

        #endregion

        #region Comandos Ventana Lista Compra

        /// <summary>
        /// Comando del Menu Lista Compra (Puede Ejecutar).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoListaCompra_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Comando del Menu Lista Compra (Funcionalidad).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoListaCompra_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Window padre = (Window)Funciones.GetWindowPadre((FrameworkElement)sender);
            VentanaListaCompra ventanaListaCompra = new VentanaListaCompra();
            if (padre.WindowState == WindowState.Maximized)
            {
                ventanaListaCompra.WindowState = WindowState.Maximized;
            }
            else
            {
                ventanaListaCompra.Height = padre.ActualHeight;
                ventanaListaCompra.Width = padre.ActualWidth;
                ventanaListaCompra.Left = padre.Left;
                ventanaListaCompra.Top = padre.Top;
            }
            ventanaListaCompra.Show();
            padre.Close();
        }

        #endregion

        #region Comandos Ventana Alimentos

        /// <summary>
        /// Comando del Menu Lista Compra (Puede Ejecutar).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoAlimentos_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Comando del Menu Alimentos (Funcionalidad).
        /// </summary>
        /// <param name="sender">Objeto que Envia el Evento.</param>
        /// <param name="e">Informacion del Evento.</param>
        private static void ComandoAlimentos_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Window padre = (Window)Funciones.GetWindowPadre((FrameworkElement)sender);
            AlimentosVentana alimentosVentana = new AlimentosVentana();
            if (padre.WindowState == WindowState.Maximized)
            {
                alimentosVentana.WindowState = WindowState.Maximized;
            }
            else
            {
                alimentosVentana.Height = padre.ActualHeight;
                alimentosVentana.Width = padre.ActualWidth;
                alimentosVentana.Left = padre.Left;
                alimentosVentana.Top = padre.Top;
            }
            alimentosVentana.Show();
            padre.Close();
        }

        #endregion

        #endregion
    }
}
