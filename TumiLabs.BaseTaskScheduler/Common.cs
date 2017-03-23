using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TumiLabs.BaseTaskScheduler
{
    public class Common
    {
        public XmlDocument RecuperarArchivoHistorialEjecucion(out string PathXmlDoc)
        {
            string pathExe = Assembly.GetExecutingAssembly().Location;
            FileInfo infoArchivo = new FileInfo(pathExe);
            PathXmlDoc = infoArchivo.DirectoryName + "\\HistorialEjecucion.xml";
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(PathXmlDoc))
            {
                string xmlNew = @"<?xml version='1.0' encoding='utf-8' ?><FechaEjecucion></FechaEjecucion>";
                xmlDoc.LoadXml(xmlNew);
            }
            else
            {
                xmlDoc.Load(PathXmlDoc);
            }
            return xmlDoc;
        }

        public bool ComprobarSincronizacionOkHoyDia(bool guardarEjecucionOkDeHoy)
        {
            bool SeSincronizoListaColaboradoresHoy = false;
            string pathXmlHistorialEjecucion = string.Empty;
            XmlDocument xmlDoc = RecuperarArchivoHistorialEjecucion(out pathXmlHistorialEjecucion);

            XmlNode nodoFechaEjecucion = xmlDoc.SelectSingleNode("/FechaEjecucion");
            XmlNode nodoUltimoDiaEjecucionOk = xmlDoc.SelectSingleNode("/FechaEjecucion/Fecha[last()]");
            DateTime FechaUltimaEjecucionOk;
            if (nodoUltimoDiaEjecucionOk != null && DateTime.TryParse(nodoUltimoDiaEjecucionOk.InnerText, out FechaUltimaEjecucionOk))
            {
                DateTime FechaHoy00Horas = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime FechaEjecutoOk00Horas = new DateTime(FechaUltimaEjecucionOk.Year, FechaUltimaEjecucionOk.Month, FechaUltimaEjecucionOk.Day);
                if (FechaHoy00Horas == FechaEjecutoOk00Horas)
                {
                    SeSincronizoListaColaboradoresHoy = true;
                }
            }
            else
            {
                //WriteTextLog(string.Format("No se pudo convertir el dato nodoUltimoDiaEjecucionOk {0}", nodoUltimoDiaEjecucionOk.InnerText));
            }

            if (guardarEjecucionOkDeHoy)
            {
                //Deber ir aparte
                if (!SeSincronizoListaColaboradoresHoy)
                {
                    XmlElement xmlNodoHoy = xmlDoc.CreateElement("Fecha");
                    xmlNodoHoy.InnerText = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    nodoFechaEjecucion.AppendChild(xmlNodoHoy);
                    xmlDoc.Save(pathXmlHistorialEjecucion);
                }
            }

            return SeSincronizoListaColaboradoresHoy;
        }
    }
}
