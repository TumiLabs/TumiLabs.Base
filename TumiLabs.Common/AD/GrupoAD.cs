using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumiLabs.Common
{
    public class GrupoAD
    {
        public string Name { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public string NombreEnSharePoint { get; set; }

        public List<LdapUserData> Usuarios { get; set; }
    }
}
