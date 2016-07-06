using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraderList.Events
{
    public class SignalrConnectionStatusChanged
    {
        private SignalRConnectionStatus _status;
        private string _info;
        public bool _isReconnect;
        
        public SignalrConnectionStatusChanged(SignalRConnectionStatus status, string info,bool reconnect = false)
        {
            _status = status;
            _info = info;
            _isReconnect = reconnect;
        }

        public SignalRConnectionStatus Status
        {
            get { return _status; }
        }

        public string Info
        {
            get { return _info; }
        }

    }
}
