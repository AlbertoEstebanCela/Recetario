using Recetario.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Recetario
{
    /// <summary>
    /// Clase estatica que contiene Funcionalidades Utiles que se repiten en el Codigo
    /// </summary>
    public static class Funciones
    {
        /// <summary>
        /// Metodo que calcula el tiempo de minutos a un formato: hh:mm
        /// </summary>
        /// <param name="tiempoMin">
        /// Parametro del tiempo en minutos
        /// </param>
        /// <returns>
        /// Devuelve un string del tiemo en formato: hh:mm
        /// </returns>
        public static string CalcularTiempo(int tiempoMin)
        {
            string tiempo = "00:00";

            string[] tiempoArray = CalcularHorasYMinutos(tiempoMin);

            tiempo = tiempoArray[0] + ":" + tiempoArray[1];

            return tiempo;
        }

        /// <summary>
        /// Metodo que pasa de minutos a horas y minutos
        /// </summary>
        /// <param name="tiempoMin">
        /// Parametro del tiempo en minutos
        /// </param>
        /// <returns>
        /// Devuelve un array de String inidcando en la primera posicion las hora y en la segunda los minutos
        /// </returns>
        public static string[] CalcularHorasYMinutos(int tiempoMin)
        {
            string[] tiempo = new string[2];

            string horasStr = "00";
            string minutosStr = "00";

            int horas = 0;

            while (tiempoMin >= 60)
            {
                horas++;
                tiempoMin -= 60;
            }

            horasStr = NumeroConCero(horas);
            minutosStr = NumeroConCero(tiempoMin);

            tiempo[0] = horasStr;
            tiempo[1] = minutosStr;

            return tiempo;
        }

        /// <summary>
        /// Metodo para pasar de horas y minutos (en int) a solo minutos
        /// </summary>
        /// <param name="horas">
        /// Parametro donde se introduce las horas en int
        /// </param>
        /// <param name="minutos">
        /// Parametro donde se introduce los minutos en int
        /// </param>
        /// <returns>
        /// Devuelve un int con los minutos totales
        /// </returns>
        public static int CalcularTiempoMinutos(int horas, int minutos)
        {
            while (horas > 0)
            {
                horas--;
                minutos += 60;
            }

            return minutos;
        }

        /// <summary>
        /// Metodo para pasar de horas y minutos (en String) a solo minutos
        /// </summary>
        /// <param name="horasStr">
        /// Parametro donde se introduce las horas en string
        /// </param>
        /// <param name="minutosStr">
        /// Parametro donde se introduce los minutos en string
        /// </param>
        /// <returns>
        /// Devuelve un int con los minutos totales
        /// </returns>
        public static int CalcularTiempoMinutos(string horasStr, string minutosStr)
        {
            int horas;
            int minutos;

            horas = int.Parse(horasStr);
            minutos = int.Parse(minutosStr);

            minutos = CalcularTiempoMinutos(horas, minutos);

            return minutos;
        }

        /// <summary>
        /// Metodo para pasar los tiempos a dos digitos
        /// </summary>
        /// <param name="numero">
        /// Parametro del numero en int que se quiere transformar
        /// </param>
        /// <returns>
        /// Devuelve un String con el numero en formato de dos digitos
        /// </returns>
        public static string NumeroConCero(int numero)
        {
            string numeroStr = "";

            if (numero < 10)
            {
                numeroStr = "0" + numero;
            }
            else
            {
                numeroStr = numero + "";
            }

            return numeroStr;
        }

        /// <summary>
        /// Metodo que tranforma una imagen a bytes
        /// </summary>
        /// <param name="imagen">
        /// Parametro de la imagen
        /// </param>
        /// <param name="tipo">
        /// Parametro del tipo de imagen (png, jpg, etc)
        /// </param>
        /// <returns>
        /// Devuelve el array de bytes de la imagen
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static byte[] ImagenABytes(BitmapSource imagen, TipoImagen tipo)
        {
            MemoryStream memStream = new MemoryStream();
            BitmapEncoder encoder;
            switch (tipo)
            {
                case TipoImagen.PNG:
                    encoder = new PngBitmapEncoder();
                    break;
                case TipoImagen.JPG:
                    encoder = new JpegBitmapEncoder();
                    break;
                default: throw new NotSupportedException("Archivo de imagen no soportado.");
            }
            encoder.Frames.Add(BitmapFrame.Create(imagen));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        /// <summary>
        /// Metodo que transforma de bytes a imagen
        /// </summary>
        /// <param name="imagen">
        /// Parametro de los bytes de la imagen
        /// </param>
        /// <param name="tipo">
        /// Parametro del tipo de imagen (png, jpg, etc)
        /// </param>
        /// <returns>
        /// Devuelve la imagen en tipo BitmapSource
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static BitmapSource BytesAImagen(byte[] imagen, TipoImagen tipo)
        {
            BitmapDecoder decoder;
            MemoryStream memStream = new MemoryStream(imagen);
            switch (tipo)
            {
                case TipoImagen.PNG:
                    decoder = new PngBitmapDecoder(memStream, BitmapCreateOptions.None, BitmapCacheOption.Default);
                    break;
                case TipoImagen.JPG:
                    decoder = new JpegBitmapDecoder(memStream, BitmapCreateOptions.None, BitmapCacheOption.Default);
                    break;
                default: throw new NotSupportedException("Archivo de imagen no soportado.");
            }
            return decoder.Frames.First();
        }

        /// <summary>
        /// Metodo para delimitar los textos a solo numeros
        /// </summary>
        /// <param name="str">
        /// Parametro donde se introduce el string
        /// </param>
        /// <returns>
        /// Devulve booleano indicando si el string introducido es un numero (True) o no (False)
        /// </returns>
        public static bool EsNumeroSimple(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        /// <summary>
        /// Metodo recursivo que se llama hasta obtener el elemento Window
        /// </summary>
        /// <param name="control"> Elemento inicial </param>
        /// <returns>Elemento Padre</returns>
        public static FrameworkElement GetWindowPadre(FrameworkElement control)
        {
            if (control is Window)
            {
                return control;
            }
            else
            {
                return GetWindowPadre((FrameworkElement)control.Parent);
            }
        }
    }

    /// <summary>
    /// Enumeroados especificando los tipos de imagen posibles
    /// </summary>
    public enum TipoImagen
    {
        JPG,
        PNG
    }
}
