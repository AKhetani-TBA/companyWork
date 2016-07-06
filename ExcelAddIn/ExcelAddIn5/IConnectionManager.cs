using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelAddIn5
{
    public interface IConnectionManager
    {
        bool Connect(string urlName, string hubName);
        void Disconnect();
        bool IsConnected { get; }
        void SendImageRequest(string calculatorName, int sifID);
    }
}
