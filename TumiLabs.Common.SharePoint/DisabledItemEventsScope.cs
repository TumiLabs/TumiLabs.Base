using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumiLabs.Common.SharePoint
{
    /// <summary>
    /// Disabled item events scope
    /// </summary>
    /// <see cref="https://adrianhenke.wordpress.com/2010/01/29/disable-item-events-firing-during-item-update/"/>
    public class DisabledItemEventsScope : SPItemEventReceiver, IDisposable
    {
        bool oldValue;

        public DisabledItemEventsScope()
        {
            this.oldValue = base.EventFiringEnabled;
            base.EventFiringEnabled = false;
        }

        #region IDisposable Members

        public void Dispose()
        {
            base.EventFiringEnabled = oldValue;
        }

        #endregion
    }
}
