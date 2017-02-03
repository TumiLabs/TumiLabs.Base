using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TumiLabs.Common
{
    public static class Utils
    {
        public static string strGetUserAccount(string strAccountName)
        {
            if (string.IsNullOrEmpty(strAccountName))
                return " (Anónimo)";

            string strAccountOnly = string.Empty;
            string[] asCuenta = strAccountName.Split('\\');
            if (asCuenta.Length > 1)
                strAccountOnly = asCuenta[asCuenta.Length - 1];
            else
                strAccountOnly = strAccountName;

            return string.Concat(" (", strAccountOnly, ")");
        }

        /* start Method to calculate the Julian Date */
        static public void GetJulianDate(double year, double month, double day, ref double jdn)
        {
            int y, m, temp, a, b, c, d;

            y = (int)year;
            m = (int)month;

            temp = (((int)year * 10000) + ((int)month * 100) + (int)day);

            if (m < 2)
            {
                y = y - 1;
                m = m + 12;
            }

            if (temp >= 15821015)
            {
                a = (y / 100);
                b = 2 - a + (a / 4);
            }
            else
            {
                a = 0;
                b = 0;
            }

            if (y < 0)
            {
                c = (int)((365.25 * y) - 0.75);
            }
            else
            {
                c = (int)(365.25 * y);
            }

            d = (int)(30.6001 * (m + 1));

            jdn = b + c + d + day + 1720994.5;
        }

        public static double GetJulianDate(double year, double month, double day)
        {
            int y, m, temp, a, b, c, d;

            y = (int)year;
            m = (int)month;

            temp = (((int)year * 10000) + ((int)month * 100) + (int)day);

            if (m < 2)
            {
                y = y - 1;
                m = m + 12;
            }

            if (temp >= 15821015)
            {
                a = (y / 100);
                b = 2 - a + (a / 4);
            }
            else
            {
                a = 0;
                b = 0;
            }

            if (y < 0)
            {
                c = (int)((365.25 * y) - 0.75);
            }
            else
            {
                c = (int)(365.25 * y);
            }

            d = (int)(30.6001 * (m + 1));

            //day = day + dfrac;

            return (double)((double)b + (double)c + (double)d + day + 1720994.5);
        }
        /* end Method to calculate the Julian Date */

        /// <summary>
        /// El primer caracter de las palabras a mayúscula
        /// </summary>
        /// <param name="s">hola como estas</param>
        /// <returns>Hola Como Estas</returns>
        public static string ToTitleCase(this string s)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        /// <summary>
        /// Ejemplo: Hola
        /// </summary>
        /// <param name="str">Hola</param>
        /// <param name="charSeparate">o</param>
        /// <returns>laoH</returns>
        public static string ReverseWords(string str, char charSeparate)
        {
            char temp;
            int left = 0, middle = 0;

            char[] chars = str.ToCharArray();
            Array.Reverse(chars);

            for (int i = 0; i <= chars.Length; i++)
            {
                if (i != chars.Length && chars[i] != charSeparate)
                    continue;

                if (left == i || left + 1 == i)
                {
                    left = i + 1;
                    continue;
                }

                middle = (i - left - 1) / 2 + left;

                for (int j = i - 1; j > middle; j--, left++)
                {
                    temp = chars[left];
                    chars[left] = chars[j];
                    chars[j] = temp;
                }

                left = i + 1;
            }

            return new String(chars);
        }

        /// <summary>
        /// Ejemplo: Hola como estas
        /// </summary>
        /// <param name="str">Hola como estas</param>
        /// <returns>estas como Hola</returns>
        public static string ReverseWords(string str)
        {
            return ReverseWords(str, ' ');
        }

        /// <summary>
        /// Gets the MIME type of the file name specified based on the file name's
        /// extension.  If the file's extension is unknown, returns "octet-stream"
        /// generic for streaming file bytes.
        /// </summary>
        /// <param name="strFileName">The name of the file for which the MIME type
        /// refers to.</param>
        public static string GetMimeTypeByFileName(string strFileName)
        {
            string sMime = "application/octet-stream";

            string sExtension = System.IO.Path.GetExtension(strFileName);
            if (!string.IsNullOrEmpty(sExtension))
            {
                sExtension = sExtension.Replace(".", "");
                sExtension = sExtension.ToLower();

                if (sExtension == "xls" || sExtension == "xlsx")
                {
                    sMime = "application/ms-excel";
                }
                else if (sExtension == "doc" || sExtension == "docx")
                {
                    sMime = "application/msword";
                }
                else if (sExtension == "ppt" || sExtension == "pptx")
                {
                    sMime = "application/ms-powerpoint";
                }
                else if (sExtension == "rtf")
                {
                    sMime = "application/rtf";
                }
                else if (sExtension == "zip")
                {
                    sMime = "application/zip";
                }
                else if (sExtension == "mp3")
                {
                    sMime = "audio/mpeg";
                }
                else if (sExtension == "bmp")
                {
                    sMime = "image/bmp";
                }
                else if (sExtension == "gif")
                {
                    sMime = "image/gif";
                }
                else if (sExtension == "jpg" || sExtension == "jpeg")
                {
                    sMime = "image/jpeg";
                }
                else if (sExtension == "png")
                {
                    sMime = "image/png";
                }
                else if (sExtension == "tiff" || sExtension == "tif")
                {
                    sMime = "image/tiff";
                }
                else if (sExtension == "txt")
                {
                    sMime = "text/plain";
                }
            }

            return sMime;
        }

        /// <summary>
        /// Formato de hora 24 horas: Ejemplo 23:59
        /// </summary>
        /// <param name="valor"></param>
        /// <returns>true si la hora es válida</returns>
        public static bool getIsHourValid24(string strHora)
        {//v1=>^([0]?[1-9]|[2][0-3]):([0-5][0-9]|[1-9])$ , v2=>^([0-1]?[0-9]|[2][0-3]):([0-5][0-9]|[1-9])$
            Regex regexp = new System.Text.RegularExpressions.Regex("^([0-1]?[0-9]|[2][0-3]):([0-5][0-9])$");
            return regexp.IsMatch(strHora);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intLength"></param>
        /// <returns></returns>
        public static string byteToBytes_KB_MB_GB(int intLength)
        {

            string tamanio = "0KB";
            if (intLength >= 0 && intLength <= 1024)//KB
            {
                tamanio = ((decimal)intLength / (decimal)8).ToString("N1") + " bytes";
            }
            else if (intLength >= 1025 && intLength <= 1048576)//KB
            {
                tamanio = ((decimal)intLength / (decimal)1024).ToString("N1") + " KB";
            }
            else if (intLength >= 1048577 && intLength <= 1073741824)//MB
            {
                tamanio = ((decimal)intLength / (decimal)1048576).ToString("N1") + " MB";
            }
            else if (intLength >= 1073741824)//GB
            {
                tamanio = ((decimal)intLength / (decimal)1073741824).ToString("N1") + " GB";
            }
            return tamanio;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPalabra"></param>
        /// <returns></returns>
        public static string removerCaracteresDeAutores(string strPalabra)
        {
            char[] aStr = strPalabra.ToCharArray();
            StringBuilder sb = new StringBuilder(strPalabra.Length);
            for (int i = 0; i < aStr.Length; i++)
            {
                char c = aStr[i];
                if (Char.IsLetter(c) | Char.IsWhiteSpace(c) | c == 44 | c == 59)
                {
                    char[] nuevo = sb.ToString().ToCharArray();
                    int numero = nuevo.Length;
                    if (numero > 0 && i > 0)
                    {
                        char charAnterior = nuevo[numero - 1];
                        if (c == 59 && c == charAnterior)//evita doble seguido ;;
                            continue;
                    }
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /////<summary>
        ///// Base 64 Encoding with URL and Filename Safe Alphabet using UTF-8 character set.
        /////</summary>
        /////<param name="str">The origianl string</param>
        /////<returns>The Base64 encoded string</returns>
        //public static string Base64Encode(string str)
        //{
        //    byte[] encbuff = Encoding.UTF8.GetBytes(str);
        //    return HttpServerUtility.UrlTokenEncode(encbuff);
        //}

        /////<summary>
        ///// Decode Base64 encoded string with URL and Filename Safe Alphabet using UTF-8.
        /////</summary>
        /////<param name="str">Base64 code</param>
        /////<returns>The decoded string.</returns>
        //public static string Base64Decode(string str)
        //{
        //    byte[] decbuff = HttpServerUtility.UrlTokenDecode(str);
        //    return Encoding.UTF8.GetString(decbuff);
        //}

        /*start parse Object*/
        /// <summary>
        /// this.Extension = deviceInfo["Extension"] ?? "html";
        ///this.Encoding = deviceInfo["Extension"].ParseDefault<Encoding>(Encoding.UTF8);
        ///this.MIMEType = deviceInfo["MIMEType"] ?? "text/html";
        ///this.Parse = deviceInfo["Parse"].ParseDefault<Boolean>(false);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ParseDefault<T>(this object o, T defaultValue)
        {
            if (o == null)
                return defaultValue;

            Type type = typeof(T);

            if (type.IsEnum)
                return Enum.IsDefined(typeof(T), o) ? (T)Enum.Parse(typeof(T), o.ToString()) : defaultValue;

            System.Reflection.MethodInfo mi = type.GetMethod("TryParse", new Type[] { typeof(string), type.MakeByRefType() });

            if (mi == null)
                return defaultValue;

            T outValue = default(T);
            object[] p = { o.ToString(), outValue };
            return (bool)mi.Invoke(null, p) ? (T)p[1] : defaultValue;
        }

        public static T ParseDefault<T>(this object o)
        {
            return o.ParseDefault(default(T));
        }

        /*end parse Object*/
        /*start string a cualquier tipo de dato nativo*/
        public delegate bool ParseToDelegate<T>(string strValue, out T result);

        public delegate bool ParseToCultureDelegate<T>(string strValue,
            NumberStyles style, IFormatProvider provider, out T result);

        public static T? ParseTo<T>(this string strValue, ParseToDelegate<T> method) where T : struct
        {
            T result;
            if (String.IsNullOrEmpty(strValue)) return null;
            if (method(strValue, out result)) return result;
            return null;
        }
        /*end string a cualquier tipo de dato nativo*/

        /*start xml*/
        public static XElement ToXElement<T>(this object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                }
            }
        }

        public static T FromXElement<T>(this XElement xElement)
        {
            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(xElement.ToString())))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(memoryStream);
            }
        }

        public static System.Xml.XmlDocument SerializeToXMLDocument<T>(this object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            //TextWriter textWriter = new StreamWriter(@"C:\movie.xml");
            StringBuilder sb = new StringBuilder();
            System.Xml.XmlWriter xmlwriter = System.Xml.XmlWriter.Create(sb);
            serializer.Serialize(xmlwriter, obj);
            xmlwriter.Close();
            xmlwriter.Flush();

            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
            xml.LoadXml(sb.ToString());
            return xml;
        }
        /*end xml*/

        public static string getExceptionFormated(Exception ex, DbCommand comando)
        {
            if (comando != null)
            {
                ex.Data.Add("procedimiento almacenado", comando.CommandText);
                foreach (DbParameter item in comando.Parameters)
                {
                    ex.Data.Add(item.ParameterName, item.Value ?? "");
                }
            }
            return getExceptionFormated(ex);
        }

        public static string getExceptionFormated(Exception ex)
        {
            string strInnerException = (ex.InnerException == null) ? "" : ex.InnerException.Message;
            StringBuilder sb = new StringBuilder();
            sb.Append("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n");
            sb.Append("Type: " + ex.GetType() + "\n");
            sb.Append("Message: " + ex.Message + "\n");
            sb.Append("InnerException: " + strInnerException + "\n");
            sb.Append("HelpLink: " + ex.HelpLink + "\n");
            //sb.Append("ExceptionState: " + ex. + "\n");
            sb.Append("Data: " + ex.Data + "\n");
            sb.Append("TargetSite: " + ex.TargetSite + "\n");
            sb.Append("Stack Trace: " + ex.StackTrace + "\n");

            return sb.ToString();
        }

        public static void WriteTextLog(string strTexto, string strOrigen)
        {
            string strPath = ConfigurationManager.AppSettings["PathLog"];
            StreamWriter ws = new StreamWriter(strPath, true);
            ws.WriteLine("Fecha - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - " + strOrigen + " - " + strTexto);
            ws.Close();
            ws.Dispose();
        }

        public static void WriteTextLog(string strTexto)
        {
            WriteTextLog(strTexto, "Contratos Comerciales - cocovi");
        }

        /// <summary>
        /// puede ser https://localhost:445/index.htm#search
        /// </summary>
        /// <returns>http://localhost:445</returns>
        public static string GetRootUrl(string strUrl)
        {
            Uri uriAddress = new Uri(strUrl);
            return uriAddress.GetLeftPart(UriPartial.Authority);
        }

        public static bool isValidEmail(string strEmail)
        {
            if (string.IsNullOrEmpty(strEmail))
                return false;

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(strEmail.Trim()))
                return (true);
            else
                return (false);
        }

    }
}
