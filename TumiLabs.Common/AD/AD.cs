
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace TumiLabs.Common
{
    public class AD
    {
        private string ADPATH = "GC://dc=losportales,dc=com,dc=pe";
        private string DOMAIN = "CILPSA";
        private string USERNAME = "shadmsystemfarm";
        private string PASSWORD = "5H9_$F4rm";

        private DirectoryEntry adObject;

        public AD()
        {
            adObject = new DirectoryEntry(ADPATH, USERNAME, PASSWORD, AuthenticationTypes.Secure);
        }

        public AD(string ADPath, string domainName, string userName, string password)
        {
            ADPATH = ADPath;
            DOMAIN = domainName;
            USERNAME = userName;
            PASSWORD = password;
            adObject = new DirectoryEntry(ADPATH, USERNAME, PASSWORD, AuthenticationTypes.Secure);
        }
        public DirectoryEntry GetDirectoryObject()
        {
            DirectoryEntry directoryObject = new DirectoryEntry(ADPATH, USERNAME, PASSWORD, AuthenticationTypes.Secure);
            return directoryObject;
        }

        public UsuarioAD GetUserNameByCompleteName(string completeName)
        {
            UsuarioAD oUsuario = null;
            DirectoryEntry adObject = GetDirectoryObject();

            //filter based on complete name
            DirectorySearcher searcher = new DirectorySearcher(adObject);
            searcher.Filter = "displayname=" + completeName;
            SearchResult result = searcher.FindOne();
            if (result != null)
            {
                DirectoryEntry userInfo = result.GetDirectoryEntry();
                oUsuario = new UsuarioAD();
                oUsuario.CuentaRed = DOMAIN + "\\" + ((string)userInfo.Properties["samaccountname"].Value ?? string.Empty);
                oUsuario.Email = (string)userInfo.Properties["mail"].Value ?? string.Empty;
                oUsuario.Nombres = (string)userInfo.Properties["displayName"].Value ?? string.Empty;
                oUsuario.Telefono = (string)userInfo.Properties["telephoneNumber"].Value ?? string.Empty;
                oUsuario.Movil = (string)userInfo.Properties["mobile"].Value ?? string.Empty;
                oUsuario.Anexo = (string)userInfo.Properties["ipphone"].Value ?? string.Empty;
                oUsuario.DNI = (string)userInfo.Properties["info"].Value ?? string.Empty;
                userInfo.Close();
                adObject.Close();

                return oUsuario;
            }
            else
            {
                Utils.WriteTextLog("No existe usuario: " + completeName);
                return null;
            }
        }

        public UsuarioAD GetUserNameByDNI(string dni)
        {
            UsuarioAD oUsuario = null;

            DirectorySearcher searcher = new DirectorySearcher(adObject);
            searcher.Filter = "info=" + dni;
            SearchResult result = searcher.FindOne();

            if (result != null)
            {
                DirectoryEntry userInfo = result.GetDirectoryEntry();
                oUsuario = new UsuarioAD();
                oUsuario.CuentaRed = DOMAIN + "\\" + ((string)userInfo.Properties["samaccountname"].Value ?? string.Empty);
                oUsuario.Email = (string)userInfo.Properties["mail"].Value ?? string.Empty;
                oUsuario.Nombres = (string)userInfo.Properties["cn"].Value ?? string.Empty;
                oUsuario.Telefono = (string)userInfo.Properties["telephoneNumber"].Value ?? string.Empty;
                oUsuario.Movil = (string)userInfo.Properties["mobile"].Value ?? string.Empty;
                oUsuario.Anexo = (string)userInfo.Properties["ipphone"].Value ?? string.Empty;
                oUsuario.DNI = (string)userInfo.Properties["info"].Value ?? string.Empty;
                userInfo.Close();
                adObject.Close();

                return oUsuario;
            }
            else
            {
                Utils.WriteTextLog("No existe usuario: " + dni);
                return null;
            }
        }

        public UsuarioAD GetUserNameByCuentaRed(string cuentaRed)
        {
            string strCuentaRedSinDominio = cuentaRed;
            if (cuentaRed != null)
            {
                string[] strCuentaRed = cuentaRed.Split('\\');
                strCuentaRedSinDominio = strCuentaRed[strCuentaRed.Length - 1];
            }

            UsuarioAD oUsuario = null;

            DirectorySearcher searcher = new DirectorySearcher(adObject);
            searcher.Filter = "samaccountname=" + strCuentaRedSinDominio;
            SearchResult result = searcher.FindOne();

            if (result != null)
            {
                DirectoryEntry userInfo = result.GetDirectoryEntry();
                oUsuario = new UsuarioAD();
                oUsuario.CuentaRed = DOMAIN + "\\" + ((string)userInfo.Properties["samaccountname"].Value ?? string.Empty);
                oUsuario.Email = (string)userInfo.Properties["mail"].Value ?? string.Empty;
                oUsuario.Nombres = (string)userInfo.Properties["displayName"].Value ?? string.Empty;//cn
                oUsuario.Telefono = (string)userInfo.Properties["telephoneNumber"].Value ?? string.Empty;
                oUsuario.Movil = (string)userInfo.Properties["mobile"].Value ?? string.Empty;
                oUsuario.Anexo = (string)userInfo.Properties["ipphone"].Value ?? string.Empty;
                oUsuario.DNI = (string)userInfo.Properties["info"].Value ?? string.Empty;
                userInfo.Close();
                adObject.Close();

                return oUsuario;
            }
            else
            {
                Utils.WriteTextLog("No existe usuario: " + cuentaRed);
                return null;
            }
        }
    }
}
