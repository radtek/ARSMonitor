﻿using System;
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
            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel2.Visible = false;
        }



        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            networkProtocol np = new networkProtocol();
            np.serverList = servers;
            backgroundWorker1.RunWorkerAsync(np);
            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel2.Text = "Waiting for ping";
            toolStripStatusLabel2.Visible = true;
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        public List<serverControl> servers = new List<serverControl>();
        public int x = 50, y = 50;
        public bool working = true;
        public string n = "New";
        public string a = "192.168.0.4";
        public bool success = false;
        private void addServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            working = false;
            ServerConstructor f = new ServerConstructor(this);
            f.ShowDialog();
            if (success)
            {
                servers.Add(new ARSMonitor.serverControl(n, a));
                //PicAndLabelInline.UserControl1 newHost = new PicAndLabelInline.UserControl1(n, a);
                n = "New";
                a = "192.168.0.4";
                drawServers();

                working = true;
            }
            else MessageBox.Show("Adding server cancelled. ");
        }

        private void drawServers()
        {
            x = 25;
            y = 25;

            // заполняется сначала видимое пространство слева направо, сверху вниз.
            foreach (serverControl server in servers)
            {
                server.SetBounds(x, y, 200, 48);

                if (x + 405 > this.Width - 25)
                {
                   x = 25;
                    y += 50;
                }
                else x += 205;
                panel1.Controls.Add(server);
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
            foreach (serverControl server in servers)
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
                    servers.Add(new serverControl(n, a));
                }
            }
            drawServers();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;
            networkProtocol np = (networkProtocol)e.Argument;
            np.workState = working;
            np.pingServers(worker, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            networkProtocol.CurrentState state = (networkProtocol.CurrentState)e.UserState;
            toolStripProgressBar1.Value = e.ProgressPercentage;
            string isOn;
            if (state.isOnline)
            isOn = "online";
            else isOn = "OFFLINE!!!";
            toolStripStatusLabel2.Text = state.address + " is " + isOn + "...";
            toolStripStatusLabel1.Text = "Working. ";
            serverControl server = servers.Find(x => x.objectAddress == state.address);
            server.objectStatus = state.isOnline;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                toolStripStatusLabel1.Text = "ERROR! ";
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else if (e.Cancelled)
                toolStripStatusLabel1.Text = "Work cancelled. ";
            else
                toolStripStatusLabel1.Text = "Work Finished. ";
                //MessageBox.Show("Finished counting words.");
                ;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            drawServers();
        }
    }
}
