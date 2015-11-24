using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARSMonitor
{
    public partial class MonitorExplorerEditor : Form
    {
        MainForm parent;
        public MonitorExplorerEditor(MainForm main)
        {
            InitializeComponent();
            parent = main;
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            //checkBox1.Checked;
            textBox3.Multiline = checkBox1.Checked;
        }

        private void textBox3_MultilineChanged(object sender, EventArgs e)
        {
            textBox3.Height = 50;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addContextMenuItem();
        }
        
        private void addContextMenuItem()
        {
            MainForm.ContextCommands cmd = new MainForm.ContextCommands();
            string temp = textBox1.Text;
            while (temp.Contains(' ')) temp = temp.Replace(" ", "");
            parent.contextMenuStrip1.Items.Add(temp + "ToolStripMenuItem");
            //while (textBox1.Text.Contains(' ')) textBox1.Text = textBox1.Text.Replace(" ", "");
            parent.contextMenuStrip1.Items[parent.contextMenuStrip1.Items.Count - 1].Text = textBox1.Text;
            parent.contextMenuStrip1.Items[parent.contextMenuStrip1.Items.Count - 1].Click += new System.EventHandler(parent.addCommandButton1);
            if (checkBox1.Checked)           ///////////////////////////////////////// ДОДЕЛАТЬ
            {
                string[] lines = textBox3.Text.Split(' '); // символ переноса строки
                cmd = new MainForm.ContextCommands(temp + "ToolStripMenuItem", textBox2.Text, lines);
            }
            else cmd = new MainForm.ContextCommands(temp + "ToolStripMenuItem", textBox2.Text, textBox3.Text);
            parent.commands.Add(cmd);
            Close();
        }
    }
}
