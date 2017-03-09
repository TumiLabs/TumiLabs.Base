using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Globalization;
using System.IO;
using Microsoft.SharePoint.Administration.Claims;

namespace TumiLabs.Common.SharePoint
{
    public class SPUtil
    {
        /// <summary>
        /// SPWeb con privilegios elevados
        /// </summary>
        /// <returns></returns>
        public static SPWeb getSPWebPermisosElevados(string url)
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
                if (string.IsNullOrEmpty(url))
                    url = "";
                WriteTextLog(string.Format("El sitio ({0}) no existe", url, ex.ToString()));
            }

            return web;
        }

        public static SPWeb getSPWebImpersonalizado(string url)
        {

            SPSite tempSite = new SPSite(url);

            SPUserToken systoken = tempSite.SystemAccount.UserToken;

            SPSite _site = new SPSite(url, systoken);

            return _site.OpenWeb();
        }

        public static SPWeb getSPWebImpersonalizadoByLoginName(string url, string strLoginName)
        {
            //string strLoginName = HttpContext.Current.User.Identity.Name;//0#.w|cilpsa\imayon
            string[] strLogin = strLoginName.Split('|');
            if (strLogin.Length == 2)
                strLoginName = strLogin[1];

            SPClaimProviderManager cpm = SPClaimProviderManager.Local;

            SPClaim userClaim = cpm.ConvertIdentifierToClaim(strLoginName, SPIdentifierTypes.WindowsSamAccountName);

            SPWeb spWebX = SPUtil.getSPWebPermisosElevados(url);

            SPUser spUsuarioActual = spWebX.EnsureUser(userClaim.ToEncodedString());
            //if (spUsuarioActual != null)
            //strCurrentUserIDLogin = string.Format("{0};#{1}", spUsuarioActual.ID, spUsuarioActual.Name);

            spWebX.Close();
            spWebX.Dispose();

            //SPSite tempSite = new SPSite(url);
            //SPUserToken systoken = tempSite.SystemAccount.UserToken;
            SPSite _site = new SPSite(url, spUsuarioActual.UserToken);



            return _site.OpenWeb();
        }

        public static SPWeb getSPWeb(string url)
        {

            SPSite tempSite = new SPSite(url);

            return tempSite.OpenWeb();
        }

        public static List<string> getColumnasCreadasEnSPList(SPWeb spWeb, SPList spList)
        {
            //StringBuilder sb = new StringBuilder();
            List<string> listaCamposFicha = new List<string>();
            //SPListItem spItem = spList.GetItemById(1);
            foreach (SPField spField in spList.Fields)
            {
                if (!spField.FromBaseType)
                {
                    //sb.Append(spField.Title + "\r");
                    listaCamposFicha.Add(spField.Title);
                }
            }
            return listaCamposFicha;
        }

        public static SPContentType getContentTypePorNombre(SPWeb spWeb, string strNombreContentType)
        {
            SPContentType spContentType = null;
            try
            {
                spContentType = spWeb.ContentTypes[strNombreContentType];
            }
            catch (Exception ex)
            {
                WriteTextLog(ex.ToString(), "SPUtil.cs");
                //throw;
            }

            return spContentType;
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

            SPSecurity.RunWithElevatedPrivileges(delegate ()
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
            //if (SPUtility.IsLoginValid(site, username))
            //{
            try
            {
                site.RootWeb.AllowUnsafeUpdates = true;
                myUser = site.RootWeb.EnsureUser(username);
                site.RootWeb.AllowUnsafeUpdates = false;
            }
            catch (Exception ex)
            {
                WriteTextLog(string.Format("No se pudo validar usuario {1} en sitio {0}. Error: {2}", site.Url, username, ex.ToString()));
            }
            //}

            return myUser;
        }

        public static SPRoleDefinition FindNivelPermiso(SPWeb spWeb, string strNombreNivelPermiso)
        {
            SPRoleDefinition spNivelPermiso = null;
            try { spNivelPermiso = spWeb.RoleDefinitions[strNombreNivelPermiso]; }
            catch (Exception ex)
            {
                WriteTextLog(string.Format("Core.ContentType.SPutil.FindNivelPermiso() No existe el nivel de permiso {0}", strNombreNivelPermiso));
                //throw;
            }
            return spNivelPermiso;
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

        public static SPUser FinSPUserInSPGroupByName(SPWeb spWeb, string strSPGroupName, string strSPUserName)
        {
            SPGroup spGroup = SPUtil.FindSPGroupByName(strSPGroupName, spWeb);
            if (spGroup != null)
            {
                SPUserCollection spUsuariosEnGrupo = spGroup.Users;
                foreach (SPUser spUsuarioIter in spUsuariosEnGrupo)
                {
                    if (spUsuarioIter.LoginName.Equals(strSPUserName))
                    {
                        return spUsuarioIter;
                    }
                }
            }
            WriteTextLog(string.Format("No se encuenta el usuario {0} en el grupo {1} ", strSPUserName, strSPGroupName), "SPUtil - FinSPUserInSPGroupByName()");

            return null;
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

        public static List<string> getEmailFromSPUser(List<SPUser> listadoSPUser)
        {
            List<string> listaEmail = new List<string>();
            if (listadoSPUser != null && listadoSPUser.Count > 0)
            {
                int intIterMax = listadoSPUser.Count;
                for (int i = 0; i < intIterMax; i++)
                    if (!string.IsNullOrEmpty(listadoSPUser[i].Email))
                        listaEmail.Add(listadoSPUser[i].Email);
            }
            return listaEmail;
        }

        public static List<SPGroup> getSPGroupByName(string[] aSPGroupName, SPWeb spWeb)
        {
            List<SPGroup> lista = new List<SPGroup>();
            if (aSPGroupName != null && aSPGroupName.Length > 0)
            {
                for (int i = 0; i < aSPGroupName.Length; i++)
                {
                    SPGroup spGroup = null;
                    try { spGroup = spWeb.Groups[aSPGroupName[i]]; }
                    catch (Exception) { }
                    if (spGroup != null)
                        lista.Add(spGroup);
                }
            }
            return lista;
        }

        public static SPGroup FindSPGroupByName(string strSPGroupName, SPWeb spWeb)
        {
            SPGroup spGroup = null;
            try { spGroup = spWeb.Groups[strSPGroupName]; }
            catch (Exception)
            {
                WriteTextLog(string.Format("FindSPGroupByName(). No existe el grupo {0} en Groups del sitio {1}", strSPGroupName, spWeb.Url));
                try { spGroup = spWeb.SiteGroups[strSPGroupName]; }
                catch (Exception)
                {
                    WriteTextLog(string.Format("FindSPGroupByName(). No existe el grupo {0} en SiteGroups del sitio {1}", strSPGroupName, spWeb.Url));
                }
            }

            return spGroup;
        }


        public static List<SPUser> getSPUsersInSpGroup(List<SPGroup> ListaSpGroup, SPWeb spWeb)
        {
            List<SPUser> listaUsuario = new List<SPUser>();
            for (int i = 0; i < ListaSpGroup.Count; i++)
            {
                SPUserCollection spUserCol = ListaSpGroup[i].Users;
                foreach (SPUser spUser in spUserCol)
                {
                    listaUsuario.Add(spUser);
                }
            }
            return listaUsuario;
        }

        public static List<SPUser> getSPUsersInSpGroupName(SPWeb spWeb, string strSPGroupName)
        {
            List<SPUser> listaUsuario = new List<SPUser>();
            SPGroup spGroup = FindSPGroupByName(strSPGroupName, spWeb);
            if (spGroup != null)
            {
                foreach (SPUser spUser in spGroup.Users)
                {
                    listaUsuario.Add(spUser);
                }
            }
            return listaUsuario;
        }

        public static SPFieldUserValue ConvertLoginAccount(string userid, SPWeb web)
        {
            SPFieldUserValue uservalue;
            //using (SPSite thissite = new SPSite(site.ID))
            //{
            //    using (SPWeb thisweb = thissite.OpenWeb(web.ID))
            //    {
            SPUser requireduser = web.EnsureUser(userid);
            uservalue = new SPFieldUserValue(web, requireduser.ID, requireduser.LoginName);
            //    }
            //}
            return uservalue;
        }

        public static string[] GetMultipleUsers(SPWeb web, SPListItem splItem, string columnName)
        {
            return GetMultipleUsers(web, splItem, columnName, true);
        }

        public static string[] GetMultipleUsers(SPWeb web, SPListItem splItem, string columnName, bool bLoginNameTrueEmailFalse)
        {
            string item = splItem[columnName] + "";
            if (string.IsNullOrEmpty(item))
                return null;

            SPFieldUserValueCollection users = new SPFieldUserValueCollection(web, item);
            string[] usersCommaDelimited = new string[users.Count];// string.Empty;
            int intIter = 0;
            foreach (SPFieldUserValue user in users)
            {
                usersCommaDelimited[intIter] = bLoginNameTrueEmailFalse ? user.User.LoginName : user.User.Email;
                intIter++;
            }

            return usersCommaDelimited;
        }

        public static List<string> getMultipleUserFromSpFieldUser(SPWeb web, SPListItem spItem, string columnName)
        {
            List<string> Users = new List<string>();
            SPFieldUserValueCollection usersFields = new SPFieldUserValueCollection(spItem.Web.Site.RootWeb, spItem[columnName] + "");

            foreach (SPFieldUserValue usersField in usersFields)
            {
                if (usersField.User == null)
                {
                    //UserField contains a SharePoint group -> extract users from it
                    SPGroup group = spItem.Web.Groups.GetByID(usersField.LookupId);
                    Users.AddRange(from SPUser user in @group.Users select user.LoginName);
                }
                else
                {
                    if (usersField.User.IsDomainGroup)
                    {
                        //UserField is actually an AD group -> Extract users from AD
                        Users = RetrieveADGroupUsers(usersField.User);
                    }
                    else
                    {
                        //UserField contains a single SharePoint user
                        Users.Add(usersField.User.LoginName);
                    }
                }
            }
            return Users;
        }

        public static List<string> RetrieveADGroupUsers(SPUser user)
        {
            List<string> loginNames = new List<string>();
            //SPSecurity.RunWithElevatedPrivileges(() =>
            //{
            //    var domainName = Environment.UserDomainName;
            //    var adDomain = string.Format("LDAP://{0}", domainName);
            //    var group = user.Name.Split('\\')[1];

            //    using (var entry = new DirectoryEntry(adDomain))
            //    {
            //        using (var dSearch = new DirectorySearcher(entry)
            //        {
            //            Filter = string.Format("(&(objectCategory=group)(cn={0}))", group)
            //        })
            //        {
            //            var result = dSearch.FindOne();
            //            foreach (string member in result.Properties["member"])
            //            {
            //                var de = new DirectoryEntry(string.Concat("LDAP://", domainName, "/", member));
            //                if (de.Properties["objectClass"].Contains("user") && de.Properties["samAccountName"].Count > 0)
            //                {
            //                    var samAccName = de.Properties["samAccountName"][0].ToString();
            //                    loginNames.Add(samAccName);
            //                }
            //            }
            //        }
            //    }
            //});
            return loginNames;
        }

        //SPGroup group = web.Groups[0];
        //SPUser user = web.Users[0];
        //SPUser user2 = web.EnsureUser("mangaldas.mano");
        //SPUser user3 = web.EnsureUser("Domain Users"); ;
        //SPPrincipal[] principals = { group, user, user2, user3 };
        public static void SetPermissions(SPListItem item, IEnumerable<SPPrincipal> principals, SPRoleType roleType)
        {
            if (item != null)
            {

                foreach (SPPrincipal principal in principals)
                {
                    SPRoleDefinition roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
                    SetPermissions(item, principal, roleDefinition);
                }
            }
        }


        public static void SetPermissions(SPListItem item, SPUser user, SPRoleType roleType)
        {
            if (item != null)
            {
                SPRoleDefinition roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
                SetPermissions(item, (SPPrincipal)user, roleDefinition);
            }
        }


        public static void SetPermissions(SPListItem item, SPPrincipal principal, SPRoleType roleType)
        {
            if (item != null)
            {
                SPRoleDefinition roleDefinition = item.Web.RoleDefinitions.GetByType(roleType);
                if (roleDefinition != null)
                    SetPermissions(item, principal, roleDefinition);
                else
                    WriteTextLog(string.Format("El rol ({0}) no existe", roleType.ToString()));
            }
        }

        public static void SetPermissions(SPListItem item, SPUser user, SPRoleDefinition roleDefinition)
        {
            if (item != null)
            {
                SetPermissions(item, (SPPrincipal)user, roleDefinition);
            }
        }

        public static void SetPermissions(SPListItem item, SPPrincipal principal, SPRoleDefinition roleDefinition)
        {
            if (item != null)
            {
                SPRoleAssignment roleAssignment = new SPRoleAssignment(principal);

                roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                item.RoleAssignments.Add(roleAssignment);
            }
        }

        public static void AddPermissionsToSPItem(SPListItem spItem, string strSPRolNombre, string strSPNivelPermiso)
        {
            SPPrincipal spObjetoConPermiso = SPUtil.FindSPGroupByName(strSPRolNombre, spItem.ParentList.ParentWeb);
            if (spObjetoConPermiso != null)
            {
                SPRoleDefinition spNivelPermiso = FindNivelPermiso(spItem.ParentList.ParentWeb, strSPNivelPermiso);
                if (spNivelPermiso != null)
                {
                    SetPermissions(spItem, spObjetoConPermiso, spNivelPermiso);
                }
            }
        }

        public static void AddPermissionsToSPItem(SPListItem spItem, string strSPRolNombre, SPRoleType spRoleType)
        {
            SPPrincipal spObjetoConPermiso = SPUtil.FindSPGroupByName(strSPRolNombre, spItem.ParentList.ParentWeb);
            if (spObjetoConPermiso != null)
            {
                SetPermissions(spItem, spObjetoConPermiso, spRoleType);
            }
        }

        //public static List<string> getEmailSPUserFromSPGroup(string[] aSPGroupParamName, SPWeb spWeb)
        //{
        //    List<string> listaEmail = new List<string>();
        //    Dictionary<string, string> dicSPGroupName = SPEsan.getParametrosSistema(aSPGroupParamName, spWeb);
        //    if (dicSPGroupName == null || dicSPGroupName.Count == 0)
        //        return listaEmail;

        //    string[] aSPGroupName = dicSPGroupName.Values.ToArray();
        //    List<SPGroup> ListaSpGroup = SPUtil.getSPGroupByName(aSPGroupName, spWeb);
        //    if (ListaSpGroup == null || ListaSpGroup.Count == 0)
        //        return listaEmail;

        //    List<SPUser> ListaUsuario = SPUtil.getSPUsersInSpGroup(ListaSpGroup, spWeb);
        //    if (ListaUsuario == null || ListaUsuario.Count == 0)
        //        return listaEmail;

        //    listaEmail = SPUtil.getEmailFromSPUser(ListaUsuario);
        //    return listaEmail;
        //}

        public static void RemovePermissions(SPListItem item, SPPrincipal principal)
        {
            //if (item != null)
            //{
            try
            {
                item.RoleAssignments.Remove(principal);
                item.SystemUpdate();
            }
            catch (Exception ex)
            {
                WriteTextLog(string.Format("No se pudo quitar permisos a {0} del item {1} .", principal.Name, item.ID) + ex.ToString(), "SPUtil.cs");
                //throw;
            }

            //item.SystemUpdate();
            //}
        }

        public static void quitarPermisosDeSpItem(SPListItem spItem, string[] aUsuariosGrupos)
        {
            int intIndice = 0;
            try
            {
                for (intIndice = 0; intIndice < aUsuariosGrupos.Length; intIndice++)
                {
                    SPPrincipal spPrincipal = SPUtil.FindUserOrSiteGroup(spItem.ParentList.ParentWeb.Site, aUsuariosGrupos[intIndice]);
                    if (spPrincipal != null)
                        spItem.RoleAssignments.Remove(spPrincipal);
                }
                spItem.Update();
            }
            catch (Exception ex)
            {
                WriteTextLog(string.Format("No se pudo quitar permisos a {0} del item {1} .", aUsuariosGrupos[intIndice], spItem.ID) + ex.ToString(), "SPUtil.cs");
                //throw;
            }

        }

        public static void WriteTextLog(string strTexto, string strOrigen, string LogFileNameSinExtension)
        {
            string LogFileNameCurrent = string.Empty;
            string strPathFolderLog = ConfigurationManager.AppSettings["PathLog"];//E:\SharePointFiles\AppCertero\sislegal\Log\
            DirectoryInfo folderPath = new DirectoryInfo(strPathFolderLog);

            FileInfo[] archivosDeLog = folderPath.GetFiles(string.Format("{0}-????-??-??-??-??.txt", LogFileNameSinExtension)).OrderByDescending(x => x.CreationTime).ToArray();
            bool bCrearNuevoArchivo = false;
            if (archivosDeLog.Length > 0)
            {
                if (archivosDeLog[0].Length > 104857600)// (1048576 * 100) = 104857600 = 100 MB
                    bCrearNuevoArchivo = true;
                else
                    LogFileNameCurrent = archivosDeLog[0].Name;
            }
            else
                bCrearNuevoArchivo = true;

            if (bCrearNuevoArchivo)
                LogFileNameCurrent = string.Format("{0}-{1}.txt", LogFileNameSinExtension, DateTime.Now.ToString("yyyy-dd-MM-HH-mm"));

            string strPathFileLog = string.Format("{0}\\{1}", strPathFolderLog, LogFileNameCurrent);

            StreamWriter ws = new StreamWriter(strPathFileLog, true);
            ws.WriteLine("Fecha - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " - " + strOrigen + " - " + strTexto);
            ws.Close();
            ws.Dispose();
        }

        public static void WriteTextLog(string strTexto, string strOrigen)
        {
            WriteTextLog(strTexto, strOrigen, "Log");
        }

        public static void WriteTextLog(string strTexto)
        {
            WriteTextLog(strTexto, "TumiLabs.Common.SharePoint");
        }
    }
}