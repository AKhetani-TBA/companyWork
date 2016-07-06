using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelAddIn5.Events
{
    public class PrepareSignalrConnectionEvent
    {
        public bool _isReconnect;
        public PrepareSignalrConnectionEvent(bool reconnect = false)
        {
            _isReconnect = reconnect;
        }
    }
}
