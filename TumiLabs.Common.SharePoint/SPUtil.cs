using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.UI;
using System.Data;
using System.Configuration;
using System.Net.Mail;
//using System.Web.Mail;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Globalization;
using System.IO;

namespace TumiLabs.Common.SharePoint
{
    public class SPUtil
    {
        /// <summary>
        /// SPWeb con privilegios elevados
        /// </summary>
        /// <returns></returns>
        public static SPWeb getSPWeb(string url)
        {
            SPWeb web = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(url))
                        web = site.OpenWeb();
                });
            }
            catch (Exception ex)
            {
                WriteTextLog(string.Format("No se puede abrir la lista {0} . Error: {1}", url, ex.ToString()));
                //throw;
            }

            return web;
        }

        /// <summary>
        /// SPWeb con priviligeos del contexto
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contextEnabled"></param>
        /// <returns></returns>
        public static SPWeb getSPWeb(string url, ref bool contextEnabled)
        {
            // url of the site must be passed as a parameter
            contextEnabled = (SPContext.Current != null && SPContext.Current.Web.Url == url);

            SPWeb web;
            SPSite site;
            if (contextEnabled)
            {
                site = SPContext.Current.Site;
                web = SPContext.Current.Web;
            }
            else
            {
                site = new SPSite(url);
                web = site.OpenWeb();
            }

            // .. do whatever you need here

            //if (!contextEnabled)
            //{
            //    web.Dispose();
            //    site.Dispose();
            //}
            return web;
        }

        public static void RemovePermission(SPList list, string groupName, string permissionName)
        {

            SPPrincipal userGroup = FindUserOrSiteGroup(list.ParentWeb.Site, groupName);

            SPRoleAssignment spRoleAssign = list.RoleAssignments.GetAssignmentByPrincipal(userGroup);

            SPRoleDefinition role = list.ParentWeb.RoleDefinitions[permissionName];

            spRoleAssign.RoleDefinitionBindings.Add(role);

            spRoleAssign.Update();

            list.Update();

        }

        public static void AddPermission(SPList list, string groupName, string permissionName)
        {

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {

                //SPPrincipal userGroup = FindUserOrSiteGroup(site, groupName);

                //SPRoleAssignment spRoleAssign = list.RoleAssignments.GetAssignmentByPrincipal(userGroup);

                //SPRoleDefinition role = web.RoleDefinitions[permissionName];

                //spRoleAssign.RoleDefinitionBindings.Add(role);

                //spRoleAssign.Update();

                //list.Update();

            });

        }

        public static SPPrincipal FindUserOrSiteGroup(SPSite site, string userOrGroup)
        {
            SPPrincipal myUser = null;
            if (SPUtility.IsLoginValid(site, userOrGroup))
            {
                myUser = site.RootWeb.EnsureUser(userOrGroup);
            }
            else
            {
                //might be a group
                foreach (SPGroup g in site.RootWeb.SiteGroups)
                {
                    if (g.Name.ToUpper(CultureInfo.InvariantCulture) == userOrGroup.ToUpper(CultureInfo.InvariantCulture))
                    {
                        myUser = g;
                        break;
                    }
                }
            }
            return myUser;
        }

        public static SPUser FindUserInSite(SPSite site, string username)
        {
            SPUser myUser = null;
            if (SPUtility.IsLoginValid(site, username))
            {
                myUser = site.RootWeb.EnsureUser(username);
            }
            return myUser;
        }

        private static void deletePermisosForItem(SPRoleAssignmentCollection roleAssignmentCollection, List<SPPrincipal> permisosAquitar)
        {
            //Eliminamos permisos de usuarios|roles
            foreach (SPPrincipal roleAssignment in permisosAquitar)
                roleAssignmentCollection.Remove(roleAssignment);
        }

        /// <summary>
        /// Devuelve la lista de grupos al que pertenece el usuario
        /// </summary>
        /// <param name="SPWeb">SPWeb tiene que estar SPSecurity.RunWithElevatedPrivileges(delegate...</param>
        /// <param name="currentUserName">DOMAIN\UserLoginName</param>
        /// <param name="strGrupos">GROUP1|GROUP2|GROUP3|GROUP_ETC</param>
        /// <returns>Retoran true si está en el array de grupos o es Administrador de sitio</returns>
        public static List<SPGroup> usuarioPertenece_a_Grupos(SPWeb spWeb, string currentUserName, string strGrupos)
        {
            List<SPGroup> spGroup = new List<SPGroup>();
            string[] aGrupos = strGrupos.Split('|');
            SPUser spUser = null;
            foreach (string item in aGrupos)
            {
                SPGroup spGroupTmp = spWeb.Groups[item];
                if (spGroupTmp != null)
                {
                    try
                    {
                        spUser = spGroupTmp.Users[currentUserName];
                    }
                    catch (Exception)
                    {
                    }

                    if (spUser != null)
                        spGroup.Add(spGroupTmp);
                }
            }

            return spGroup;
        }

        /// <summary>
        /// Permite verificar si un usuario está en algún grupo o es Administrador del sitio
        /// </summary>
        /// <param name="urlSPWeb"></param>
        /// <param name="currentUserName"></param>
        /// <param name="strGrupos"></param>
        /// <param name="bIncluirSiEsAdmin"></param>
        /// <returns></returns>
        public static bool usuarioPertenece_a_Grupos_o_EsAdmin(string urlSPWeb, string currentUserName, string strGrupos, bool bIncluirSiEsAdmin)
        {
            bool bTieneAccesoApagina = false;
            List<SPGroup> spGroup = null;
            using (SPWeb spWeb = SPUtil.getSPWeb(urlSPWeb))
                spGroup = usuarioPertenece_a_Grupos(spWeb, currentUserName, strGrupos);

            if (spGroup != null && spGroup.Count > 0)
                bTieneAccesoApagina = true;

            if (bIncluirSiEsAdmin && !bTieneAccesoApagina)
            {
                if (SPContext.Current.Web.UserIsSiteAdmin)
                    bTieneAccesoApagina = true;
            }

            return bTieneAccesoApagina;
        }

        public static List<SPGroup> getGrupos_ConPermiso(SPRoleAssignmentCollection Grupos_And_Usuarios)
        {
            List<SPGroup> lista_spGroup = new List<SPGroup>();
            foreach (SPRoleAssignment item in Grupos_And_Usuarios)
            {
                SPPrincipal spPrincipal = item.Member;
                if (spPrincipal.GetType() == typeof(SPGroup))
                {
                    SPGroup spGroup = (SPGroup)spPrincipal;
                    lista_spGroup.Add(spGroup);
                }
            }
            return lista_spGroup;
        }

        public static SPGroup getGroup_Where_UserName(SPListItem spItem, string currentUserName)
        {
            List<SPGroup> lista_spGroup = getGrupos_ConPermiso(spItem.RoleAssignments);
            SPGroup spGroup = null;
            foreach (SPGroup itemSpGroup in lista_spGroup)
            {
                SPUser spUser = null;
                try { spUser = itemSpGroup.Users[currentUserName]; }
                catch (Exception) { }

                if (spUser != null)
                {
                    spGroup = itemSpGroup;
                    break;
                }
            }
            return spGroup;
        }

        public static bool bTienePermisoInSpItem(SPListItem spItem, SPPrincipal spUserOrGroup)
        {
            bool bTienePermiso = false;
            SPRoleAssignmentCollection roleAssignmentCollection = spItem.RoleAssignments;
            foreach (SPRoleAssignment roleAssignment in roleAssignmentCollection)
            {
                Type tTipo = roleAssignment.Member.GetType();
                if (tTipo == typeof(SPGroup))
                {
                    SPGroup spGrupo = (SPGroup)roleAssignment.Member;
                    //SPGroup spGrupo = spItem.Web.Groups.GetByID(roleAssignment) [roleAssignment.Member.Name];

                    SPUser spUser = null;
                    try { spUser = spGrupo.Users[spUserOrGroup.LoginName]; }
                    catch (Exception ex) { }

                    if (spUser != null)
                    {
                        bTienePermiso = true;
                        break;
                    }
                }
                else if (tTipo == typeof(SPUser))
                {
                    if (roleAssignment.Member.LoginName.Equals(spUserOrGroup.LoginName))
                    {
                        bTienePermiso = true;
                        break;
                    }
                }
            }
            return bTienePermiso;
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
            WriteTextLog(strTexto, "LosPortales.Intranet.SyncSeguimientoPagosSAP");
        }
    }
}