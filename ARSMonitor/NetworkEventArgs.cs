using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARSMonitor
{
    public class NetworkEventArgs : EventArgs
    {
        public readonly string Message;
        public NetworkEventArgs(string msg)
        {
            Message = msg;
        }
    }
}
