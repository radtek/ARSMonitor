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

    public class SocketEventArgs : EventArgs
    {
        public readonly string programm;
        public readonly string parameters;
        public readonly string[] lines;
        public readonly string address;
        public SocketEventArgs(string ip, string prog, string param)
        {
            programm = prog;
            parameters = param;
            address = ip;
        }
        public SocketEventArgs(string ip, string prog, string[] lns)
        {
            programm = prog;
            lines = lns;
            address = ip;
        }
    }
}
