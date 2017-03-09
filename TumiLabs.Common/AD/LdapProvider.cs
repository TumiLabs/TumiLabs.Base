using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumiLabs.Common
{
    public class LdapProvider
    {
        // Called LDAP Path but can handle GC paths too
        private readonly string _ldapPath;

        public List<GrupoAD> listaDeGrupos { get; set; }

        public LdapProvider(string path)
        {
            _ldapPath = path;
        }

        /// <summary>
        /// Provide the friendly name of the group to get all members of the group.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public List<LdapUserData> GetMembersOfGroup(string groupName)
        {
            List<LdapUserData> members = new List<LdapUserData>();
            Dictionary<string, LdapUserData> dicMiembros = new Dictionary<string, LdapUserData>();
            try
            {
                using (var directoryEntry = new DirectoryEntry(_ldapPath))
                {
                    var groupDistinguishedName = GetGroupDistinguishedName(directoryEntry, groupName);
                    dicMiembros = GetMembersOfGroup(directoryEntry, groupDistinguishedName, dicMiembros);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to query LDAP users {0} on {1}. {2}", groupName, _ldapPath, ex.Message);
            }

            foreach (KeyValuePair<string, LdapUserData> item in dicMiembros)
            {
                members.Add(item.Value);
            }
            return members;
        }

        private void SetupDefaultPropertiesOnDirectorySearcher(DirectorySearcher searcher)
        {
            // allow us to use references to other active dir domains.
            searcher.ReferralChasing = ReferralChasingOption.All;
        }

        private string GetGroupDistinguishedName(DirectoryEntry directoryEntry, string groupName)
        {
            var distinguishedName = "";

            var filter = string.Format("(&(objectClass=group)(name={0}))", groupName);
            var propertiesToLoad = new string[] { "distinguishedName" };

            using (var ds = new DirectorySearcher(directoryEntry, filter, propertiesToLoad))
            {
                SetupDefaultPropertiesOnDirectorySearcher(ds);

                var result = ds.FindOne();
                if (result != null)
                {
                    distinguishedName = result.Properties["distinguishedName"][0].ToString();
                }
            }

            return distinguishedName;
        }

        private Dictionary<string, LdapUserData> GetMembersOfGroup(DirectoryEntry directoryEntry, string groupDistinguishedName, Dictionary<string, LdapUserData> members)
        {
            //Dictionary<string, LdapUserData> members = new Dictionary<string, LdapUserData>();

            if (string.IsNullOrEmpty(groupDistinguishedName))
            {
                throw new Exception("Group name not provided. Cannot look for group members.");
            }
            //(&(|(&(objectCategory=group)(objectClass=group))(&(objectcategory=person)(objectclass=user)))(memberof={0}))
            //var filter = string.Format("(&(objectClass=user)(memberof={0}))", groupDistinguishedName);
            var filter = string.Format("(&(|(&(objectCategory=group)(objectClass=group))(&(objectcategory=person)(objectclass=user)))(memberof={0}))", groupDistinguishedName);

            // Only load what we need
            var propertiesToLoad = new string[] { "objectClass", "givenname", "samaccountname", "sn", "useraccountcontrol", "mail" };

            using (var ds = new DirectorySearcher(directoryEntry, filter, propertiesToLoad))
            {
                SetupDefaultPropertiesOnDirectorySearcher(ds);


                // get all members in a group
                foreach (SearchResult result in ds.FindAll())
                {
                    int intIter = 0;
                    try
                    {
                        string strNombreCuenta = result.Properties["samaccountname"][0].ToString();
                        string strEsGrupo = string.Empty;
                        if (result.Properties["objectClass"].IndexOf("group") > -1)
                        {
                            string strGroupDistinguishedName = GetGroupDistinguishedName(directoryEntry, strNombreCuenta);
                            GetMembersOfGroup(directoryEntry, strGroupDistinguishedName, members);
                            continue;
                            //strEsGrupo = "Grupo";
                        }
                        else
                        {
                            strEsGrupo = "Persona";
                        }
                        //start custom
                        if (result.Properties["mail"].Count == 0)
                            continue;

                        if (result.Properties["givenname"].Count == 0 && result.Properties["sn"].Count == 0)
                            continue;

                        if (result.Properties["useraccountcontrol"].Count == 0)
                            continue;
                        else
                        {
                            //string strUsuarioActivos = "512;66048";
                            string strTipoUsuario = result.Properties["useraccountcontrol"][0].ToString();
                            if (!strTipoUsuario.Equals("512") //Enabled Account
                                //|| strUsuarioActivos.Equals("66048") //Enabled, Password Doesn't Expire
                                )
                                continue;
                        }
                        //intIter = 1;
                        //string s1 = result.Properties["samaccountname"][0].ToString();
                        //intIter = 2;
                        //string s2 = result.Properties["useraccountcontrol"][0].ToString();
                        //intIter = 3;
                        //string s3 = result.Properties["givenname"][0].ToString();
                        //intIter = 4;
                        //string s4 = result.Properties["sn"][0].ToString();
                        //intIter = 5;
                        //string s5 = result.Properties["mail"][0].ToString();

                        //end custom

                        LdapUserData oCuenta = null;
                        bool bCuentaYaExiste = members.TryGetValue(strNombreCuenta, out oCuenta);
                        if (bCuentaYaExiste)
                        {
                            continue;
                        }

                        members.Add(strNombreCuenta, new LdapUserData()
                        {
                            SamAccountName = strNombreCuenta,
                            UserAccountControl =
                                (result.Properties["useraccountcontrol"][0] is int)
                                    ? (int)result.Properties["useraccountcontrol"][0]
                                    : 0,
                            FirstName = (result.Properties["givenname"].Count > 0) ? result.Properties["givenname"][0].ToString() : "",
                            LastName = (result.Properties["sn"].Count > 0) ? result.Properties["sn"][0].ToString() : "",
                            Email = result.Properties["mail"][0].ToString()
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to add user." + result.Properties["samaccountname"][0] + " error: " + intIter);
                    }
                }
            }

            return members;
        }

        private UsuarioAD GetUserByDNIoNombres(string DNI, string Nombres, string Apellidos)
        {
            UsuarioAD oUsuarioAD = null;
            //Son 4 Filtros
            string str1erFiltro_DNI = string.Format("(&(objectClass=user)(objectCategory=user)(info={0}))", DNI);

            // Only load what we need
            var propertiesToLoad = new string[] { "objectClass", "givenname", "samaccountname", "sn", "useraccountcontrol", "mail" };
            using (var directoryEntry = new DirectoryEntry(_ldapPath))
            {
                using (var ds = new DirectorySearcher(directoryEntry, str1erFiltro_DNI, propertiesToLoad))
                {
                    SetupDefaultPropertiesOnDirectorySearcher(ds);

                    foreach (SearchResult result in ds.FindAll())
                    {
                        try
                        {
                            string strNombreCuenta = result.Properties["samaccountname"][0].ToString();

                            //start custom
                            if (result.Properties["mail"].Count > 0)
                                continue;

                            if (result.Properties["givenname"].Count == 0 && result.Properties["sn"].Count == 0)
                                continue;

                            if (result.Properties["useraccountcontrol"].Count == 0)
                                continue;
                            else
                            {
                                //string strUsuarioActivos = "512;66048";
                                string strTipoUsuario = result.Properties["useraccountcontrol"][0].ToString();
                                if (!strTipoUsuario.Equals("512") //Enabled Account
                                    //|| strUsuarioActivos.Equals("66048") //Enabled, Password Doesn't Expire
                                    )
                                    continue;
                            }
                            //intIter = 1;
                            //string s1 = result.Properties["samaccountname"][0].ToString();
                            //intIter = 2;
                            //string s2 = result.Properties["useraccountcontrol"][0].ToString();
                            //intIter = 3;
                            //string s3 = result.Properties["givenname"][0].ToString();
                            //intIter = 4;
                            //string s4 = result.Properties["sn"][0].ToString();
                            //intIter = 5;
                            //string s5 = result.Properties["mail"][0].ToString();

                            //end custom

                            //LdapUserData oCuenta = null;
                            //bool bCuentaYaExiste = members.TryGetValue(strNombreCuenta, out oCuenta);
                            //if (bCuentaYaExiste)
                            //{
                            //    continue;
                            //}

                            //members.Add(strNombreCuenta, new LdapUserData()
                            //{
                            //    SamAccountName = strNombreCuenta,
                            //    UserAccountControl =
                            //        (result.Properties["useraccountcontrol"][0] is int)
                            //            ? (int)result.Properties["useraccountcontrol"][0]
                            //            : 0,
                            //    FirstName = (result.Properties["givenname"].Count > 0) ? result.Properties["givenname"][0].ToString() : "",
                            //    LastName = (result.Properties["sn"].Count > 0) ? result.Properties["sn"][0].ToString() : "",
                            //    Email = result.Properties["mail"][0].ToString()
                            //});
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine("Failed to add user." + result.Properties["samaccountname"][0] + " error: " + intIter);
                        }
                    }
                }
            }

            return oUsuarioAD;
        }

        private string GetCurrentDomainPath()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");

            return "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();
        }

        public GrupoAD GetGroupByName(string strGroupName)
        {
            // define root directory entry
            DirectoryEntry domainRoot = new DirectoryEntry(_ldapPath);//new DirectoryEntry("LDAP://" + domain, userName, password, AuthenticationTypes.Secure);

            string strNombreGrupo = GetGroupDistinguishedName(domainRoot, strGroupName);
            // setup searcher for subtree and search for groups 
            DirectorySearcher ds = new DirectorySearcher(domainRoot);
            ds.SearchScope = SearchScope.Subtree;
            ds.PropertiesToLoad.Clear();
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("description");
            ds.Filter = "(&(objectCategory=Group)(name=" + strGroupName + "))";
            //ds.Filter = string.Format("(&({0})(&(objectCategory=group)(objectClass=group))))", strNombreGrupo);

            var results = ds.FindAll();
            GrupoAD oGrupo = null;
            if (results.Count > 0)
            {
                foreach (SearchResult objResult in results)
                {
                    DirectoryEntry objGroupEntry = objResult.GetDirectoryEntry();
                    oGrupo = new GrupoAD();
                    oGrupo.Name = objGroupEntry.Name;

                    string strNombre = "";
                    if (objGroupEntry.Properties["name"].Count > 0)
                        strNombre = objGroupEntry.Properties["name"][0].ToString();

                    string strDescripcion = "Grupo de Directorio Activo (Sin descripción en AD)";
                    if (objGroupEntry.Properties["description"].Count > 0)
                        strDescripcion = objGroupEntry.Properties["description"][0].ToString() + "(Grupo AD)";

                    oGrupo.Nombre = strNombre;
                    oGrupo.Descripcion = strDescripcion;
                    //Name = objGroupEntry.Name, Nombre = strNombre, Descripcion = strDescripcion
                    // group not found -> create
                }
            }

            return oGrupo;
        }

        public void GetAllGroups()
        {
            DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());
            DirectorySearcher ds = new DirectorySearcher(de);

            // Sort by name
            ds.Sort = new SortOption("name", SortDirection.Ascending);
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("memberof");
            ds.PropertiesToLoad.Add("member");

            ds.Filter = "(&(objectCategory=Group))";
            ds.SearchScope = SearchScope.Subtree;

            SearchResultCollection results = ds.FindAll();

            foreach (SearchResult sr in results)
            {
                if (sr.Properties["name"].Count > 0)
                    Debug.WriteLine(sr.Properties["name"][0].ToString());

                //if (sr.Properties["memberof"].Count > 0)
                //{
                //    Debug.WriteLine(" Member of...");
                //    foreach (string item in sr.Properties["memberof"])
                //    {
                //        Debug.WriteLine(" " + item);
                //    }
                //}
                //if (sr.Properties["member"].Count > 0)
                //{
                //    Debug.WriteLine(" Members");
                //    foreach (string item in sr.Properties["member"])
                //    {
                //        Debug.WriteLine(" " + item);
                //    }
                //}
            }
        }

        public List<GrupoAD> GetGroups()
        {
            listaDeGrupos = new List<GrupoAD>();

            DirectoryEntry objADAM = default(DirectoryEntry);
            // Binding object. 
            DirectoryEntry objGroupEntry = default(DirectoryEntry);
            // Group Results. 
            DirectorySearcher objSearchADAM = default(DirectorySearcher);
            // Search object. 
            SearchResultCollection objSearchResults = default(SearchResultCollection);
            // Results collection. 
            string strPath = null;
            // Binding path. 
            List<string> result = new List<string>();

            // Construct the binding string. 
            strPath = GetCurrentDomainPath();//"LDAP://losportales.com.pe";
            //Change to your ADserver 

            // Get the AD LDS object. 
            try
            {
                objADAM = new DirectoryEntry(strPath);
                objADAM.RefreshCache();
            }
            catch (Exception e)
            {
                throw e;
            }

            // Get search object, specify filter and scope, 
            // perform search. 
            try
            {
                objSearchADAM = new DirectorySearcher(objADAM);
                objSearchADAM.Filter = "(&(objectClass=group))";
                objSearchADAM.SearchScope = SearchScope.Subtree;
                objSearchADAM.PropertiesToLoad.Clear();
                objSearchADAM.PropertiesToLoad.Add("name");
                objSearchADAM.PropertiesToLoad.Add("description");
                objSearchResults = objSearchADAM.FindAll();
            }
            catch (Exception e)
            {
                throw e;
            }

            int intiter = 1;
            // Enumerate groups 
            try
            {
                if (objSearchResults.Count != 0)
                {
                    foreach (SearchResult objResult in objSearchResults)
                    {
                        objGroupEntry = objResult.GetDirectoryEntry();

                        string strNombre = "";
                        if (objGroupEntry.Properties["name"].Count > 0)
                            strNombre = objGroupEntry.Properties["name"][0].ToString();

                        string strDescripcion = "Grupo de Directorio Activo (Sin descripción en AD)";
                        if (objGroupEntry.Properties["description"].Count > 0)
                            strDescripcion = objGroupEntry.Properties["description"][0].ToString() + "(Grupo AD)";


                        listaDeGrupos.Add(new GrupoAD() { Name = objGroupEntry.Name, Nombre = strNombre, Descripcion = strDescripcion });
                        //objGroupEntry.
                        //result.Add(objGroupEntry.Name);
                        intiter++;
                    }
                }
                else
                {
                    throw new Exception("No groups found");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return listaDeGrupos;
        }

        public void GetUsersInGroup(GrupoAD group)
        {
            //group.Usuarios = new List<UsuarioAD>();

            string sam = "";
            string fname = "";
            string lname = "";
            string active = "";

            string strDomain = GetCurrentDomainPath();
            DirectoryEntry de = new DirectoryEntry(strDomain);

            DirectorySearcher ds = new DirectorySearcher(de, "(objectClass=person)");
            ds.Filter = "OU=" + group.Nombre;
            //ds.Filter = "(&(objectClass=user)(objectCategory=person))";

            ds.PropertiesToLoad.Add("givenname");
            ds.PropertiesToLoad.Add("samaccountname");
            ds.PropertiesToLoad.Add("sn");
            ds.PropertiesToLoad.Add("useraccountcontrol");

            foreach (SearchResult sr in ds.FindAll())
            {
                try
                {
                    sam = sr.Properties["samaccountname"][0].ToString();
                    fname = sr.Properties["givenname"][0].ToString();
                    lname = sr.Properties["sn"][0].ToString();
                    active = sr.Properties["useraccountcontrol"][0].ToString();
                }
                catch (Exception e)
                {
                }

                // don't grab disabled users
                if (active.ToString() != "514")
                {
                    //group.Usuarios.Add(new UsuarioAD() { CuentaRed = sam, Nombres = fname });
                    //groupMemebers.Add(sam.ToString(), (fname.ToString() + " " + lname.ToString()));
                }
            }

            //return groupMemebers;
        }

        public List<UsuarioAD> GetUsersInGroup(string NombreGrupoAD)
        {
            return null;

            ///* Retreiving a principal context
            //*/
            //PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, "WM2008R2ENT", "dc=dom,dc=fr", "TheUser", "ThePassword");

            ///* Discribe the group You are looking for as a principal
            // */
            //GroupPrincipal gpPrincipal = new GroupPrincipal(domainContext);
            //gpPrincipal.Name = NombreGrupoAD; //"abc-*";

            ///* Bind a searcher
            // */
            //PrincipalSearcher searcher = new PrincipalSearcher();
            //searcher.QueryFilter = gpPrincipal;

            //PrincipalSearchResult<Principal> hRes = searcher.FindAll();

            ///* Read The result
            // */
            //foreach (GroupPrincipal grp in hRes)
            //{
            //    Console.WriteLine(grp.Name);
            //    // You are looking for "grp.Members"
            //}

        }

        public LdapUserData GetUserByDNIoNombres(string DNI, out EError Error, out string MensajeError)
        {
            LdapUserData oUsuario = null;
            Error = EError.SinError;
            MensajeError = string.Empty;

            string str1erFiltro_DNI = string.Format("(&(objectClass=user)(objectCategory=user)(info={0}))", DNI);

            //Solo cargamos lo que necesitamos
            var propertiesToLoad = new string[] { "objectClass", "givenname", "samaccountname", "sn", "useraccountcontrol", "mail" };
            using (var directoryEntry = new DirectoryEntry(_ldapPath))
            {
                using (var ds1 = new DirectorySearcher(directoryEntry, str1erFiltro_DNI, propertiesToLoad))
                {
                    SetupDefaultPropertiesOnDirectorySearcher(ds1);
                    SearchResultCollection oResultadoBusqueda = ds1.FindAll();

                    oResultadoBusqueda = ds1.FindAll();
                    if (oResultadoBusqueda.Count == 1)
                    {
                        oUsuario = new LdapUserData();
                        SearchResult result = oResultadoBusqueda[0];
                        oUsuario.SamAccountName = result.Properties["samaccountname"][0].ToString();
                        if (result.Properties["mail"].Count > 0)
                            oUsuario.Email = result.Properties["mail"][0].ToString();
                    }
                    else if (oResultadoBusqueda.Count == 0)
                    {
                        Error = EError.NoSeEncontroDNI;
                    }
                    else if (oResultadoBusqueda.Count > 1)
                    {
                        Error = EError.DNIrepetido;
                        StringBuilder sb = new StringBuilder();
                        foreach (SearchResult adusuarios in oResultadoBusqueda)
                        {
                            sb.Append(string.Format("{0},", adusuarios.Properties["samaccountname"][0].ToString()));
                        }
                        MensajeError = sb.ToString(0, sb.Length - 1);
                    }
                }
            }

            return oUsuario;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ActiveDirectory>");
            if (listaDeGrupos != null)
            {
                int intIndiceGrupo = 0;
                foreach (GrupoAD grupo in listaDeGrupos)
                {
                    sb.Append(string.Format("<Grupo i='{0}' nombre='{1}' usuarios='{2}'>", intIndiceGrupo, grupo.Nombre, grupo.Usuarios.Count));

                    int intIndiceUsuario = 0;
                    foreach (LdapUserData usuario in grupo.Usuarios)
                    {
                        sb.Append(string.Format("<Usuario i='{0}' cuenta='{1}' email='{2}' />", intIndiceUsuario, usuario.SamAccountName, usuario.Email));
                        intIndiceUsuario++;
                    }

                    sb.Append("</Grupo>");
                    intIndiceGrupo++;
                }
            }
            sb.Append("</ActiveDirectory>");
            return sb.ToString();
        }
    }

}
