using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;


namespace ARSMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            options.DontFragment = true;

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            foreach (PicAndLabelInline.UserControl1 server in servers)
            {
                //elements.Find(el => el.Child == server);
                string host = server.objectAddress;
                PingReply reply = pingSender.Send(host, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    server.objectStatus = true;
                    //server.objectName = "GO!";
                }
                else
                {
                    server.objectStatus = false;
                    server.objectName = server.objectAddress + " STOPPED!";
                }
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        public List<PicAndLabelInline.UserControl1> servers = new List<PicAndLabelInline.UserControl1>();
        public List<System.Windows.Forms.Integration.ElementHost> elements = new List<System.Windows.Forms.Integration.ElementHost>();
        public int x = 50, y = 50;

        public string n = "New";
        public string a = "192.168.0.4";

        private void addServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerConstructor f = new ServerConstructor(this);
            f.ShowDialog();
            servers.Add(new PicAndLabelInline.UserControl1(n, a));
            //PicAndLabelInline.UserControl1 newHost = new PicAndLabelInline.UserControl1(n, a);
            n = "New";
            a = "192.168.0.4";
        }

        private void drawServers()
        {
            
            foreach(PicAndLabelInline.UserControl1 server in servers)
            {
                if (elements.Find(el => el.Child == server)==null)
                {
                    elements.Add(new System.Windows.Forms.Integration.ElementHost());
                    elements.Last().Child = server;
                    //elements.Last().BackColorTransparent = true;
                }
            }

            x = 50;
            y = 50;
            foreach (System.Windows.Forms.Integration.ElementHost el in elements)
            {
                el.BackColor = SystemColors.Control;
                el.SetBounds(x, y, 200, 48);
                y += 50;
                panel1.Controls.Add(el);
            }
        }

        private void drawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawServers();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //import servers list
            List<string> importString = new List<string>();
            int i=0;
            foreach(PicAndLabelInline.UserControl1 server in servers)
            {
                importString.Add(server.objectAddress + " " + server.objectName + Environment.NewLine);
                i++;
            }
            //if ()

            System.IO.File.WriteAllLines(@"Servers.txt", importString.ToArray<string>());
            
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //export servers list

            string[] lines = System.IO.File.ReadAllLines(@"Servers.txt");
            foreach (string serv in lines)
            {
                if (serv != "")
                {
                    string[] splitted = serv.Split(' ');
                    n = splitted[1];
                    a = splitted[0];
                    servers.Add(new PicAndLabelInline.UserControl1(n, a));
                }
            }
        }
    }
}
