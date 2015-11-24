using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ARSMonitor
{
    public partial class MainForm : Form
    {
        public class ContextCommands
        {
            public ContextCommands()
            {
            }

            private string commandName;
            private string commandProgramm;
            private string commandParams;
            private string[] commandLines;

            private bool isMulti = false;

            public bool Multi
            {
                get
                {
                    return isMulti;
                }
                set
                {
                    isMulti = value;
                }
            }

            public string commName
            {
                get
                {
                    return commandName;
                }
            }

            public string commProgramm
            {
                get
                {
                    return commandProgramm;
                }
            }

            public string commParams
            {
                get
                {
                    return commandParams;
                }
            }


            public string[] commLines
            {
                get
                {
                    return commandLines;
                }
            }

            public ContextCommands(string nm, string prog, string par)
            {
                isMulti = false;
                commandName = nm;
                commandProgramm = prog;
                commandParams = par;
            }   

            public ContextCommands(string nm, string prog, string[] lns)
            {
                isMulti = true;
                commandName = nm;
                commandProgramm = prog;
                commandLines = lns;
            }
        }
    }
}