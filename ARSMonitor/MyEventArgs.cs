using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ARSMonitor
{
    public class MyEvArgs : DoWorkEventArgs
    {
        public MyEvArgs(object argument, serverControl srv)
            : base(argument)
        {
            Serv = srv;
        }
        public object arg;
        public readonly serverControl Serv;
    }

    public class NewClient : EventArgs
    {
        public readonly string Message;
        public NewClient(string msg)
        {
            Message = msg;
        }
    }
}
