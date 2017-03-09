using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumiLabs.Common
{
    /// <summary>
    /// Very lightweight class to hold user account data
    /// </summary>
    public class LdapUserData
    {
        /// <summary>
        /// SAM = Security Accounts Manager
        /// </summary>
        public string SamAccountName { get; set; }

        /// <summary>
        /// Bit field flags that control the behavior of the AD user account.
        ///
        /// A few relevant ones:
        /// 2   = ACCOUNTDISABLE
        /// 512 = NORMAL_ACCOUNT
        ///
        /// 514 = NORMAL_ACCOUNT && ACCOUNTDISABLE
        /// </summary>
        public int UserAccountControl { get; set; }

        /// <summary>
        /// Check bit flag and include anything without a valid name as disabled.
        /// </summary>
        public bool IsAccountDisabled
        {
            get { return UserAccountControl == 514 || string.IsNullOrEmpty(SamAccountName); }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
    }

}
