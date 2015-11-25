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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace ARSMonitor
{
    [DataContract]
    public partial class MainForm : Form
    {
        /*[DataContract]
        public ToolStripItem commandTS;
        */

        /*[KnownType(typeof(isMulti))]
        [KnownType(typeof(commandLines))]*/
        /*
        [Serializable()]
        [DataContract]*/
        //[KnownType(commandTS)]

        //[XmlRootAttribute("ContextCommands", Namespace = "ARSMonitor", IsNullable = false)]
        //[DataContract]
        //[KnownType()]
        [DataContract]
        public class ContextCommands
        {
            public ContextCommands()
            {
            }
            [DataMember]
            [XmlAttribute("toolName")]
            private string commandToolName;

            [DataMember]
            [XmlAttribute("toolText")]
            private string commandToolText;
            [DataMember]
            [XmlElement("CommandName")]
            private string commandName;
            [DataMember]
            [XmlAttribute("toolProgrammName")]
            private string commandProgramm;
            [DataMember]
            [XmlAttribute("toolBody")]
            private string commandParams;
            [DataMember]
            [XmlArrayAttribute("Lines")]
            private string[] commandLines;
            [XmlIgnore]
            private bool isMulti = false;

            [XmlIgnore]
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

            [XmlIgnore]
            public string commName
            {
                get
                {
                    return commandName;
                }
            }


            [XmlIgnore]
            public string commProgramm
            {
                get
                {
                    return commandProgramm;
                }
            }

            [XmlIgnore]
            public string commParams
            {
                get
                {
                    return commandParams;
                }
            }


            [XmlIgnore]
            public string[] commLines
            {
                get
                {
                    return commandLines;
                }
            }

            [XmlIgnore]
            public ToolStripItem commTS
            {
                get
                {
                    return commandToolStrip;
                }
                set
                {
                    commandToolStrip = value;

                    commandToolName = commandToolStrip.Name;
                    commandToolText = commandToolStrip.Text;
                }
            }

            [XmlAttribute("toolAttName")]
            public string commTSN
            {
                get
                {
                    return commandToolName;
                }
            }

            [XmlAttribute("toolAttText")]
            public string commTST
            {
                get
                {
                    return commandToolText;
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

            public ContextCommands(ToolStripItem cTS, string prog, string[] lns)
            {
                isMulti = true;
                commandToolStrip = cTS;
                commandProgramm = prog;
                commandLines = lns;
            }

            public ContextCommands(ToolStripItem cTS, string prog, string par)
            {
                isMulti = false;
                commandToolStrip = cTS;
                commandToolName = cTS.Name;
                commandToolText = cTS.Text;
                commandProgramm = prog;
                commandParams = par;
            }

            [XmlIgnore]
            private ToolStripItem commandToolStrip;
        }

        public class commandClassCollection
        {
            [XmlArray("Collection"), XmlArrayItem("Item")]
            public List<ContextCommands> Collection { get; set; }
        }
    }
}