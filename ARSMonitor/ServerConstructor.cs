using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ARSMonitor
{
    public partial class ServerConstructor : Form
    {
        MainForm parent;
        public ServerConstructor(MainForm p)
        {
            parent = p;
            parent.success = false;
            InitializeComponent();
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Проверить валидность ввода
            // string temp = maskedTextBox1.Text;

            // temp
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.a = textBox1.Text + "." + textBox4.Text + "." + textBox5.Text + "." + textBox6.Text;
            parent.n = textBox2.Text;
            parent.success = true;
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Int32.Parse(textBox1.Text) > 255) { textBox1.Text = "255"; }//textBox1.Text
            else if (textBox1.TextLength > 2) SendKeys.Send("{TAB}");
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (Int32.Parse(textBox4.Text) > 255) { textBox4.Text = "255"; }//textBox5.Text
            else if (textBox4.TextLength > 2) SendKeys.Send("{TAB}");
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (Int32.Parse(textBox5.Text) > 255) { textBox5.Text = "255"; }//textBox5.Text
            else if (textBox5.TextLength > 2) SendKeys.Send("{TAB}");
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (Int32.Parse(textBox6.Text) > 255) { textBox6.Text = "255"; }//textBox6.Text
            else if (textBox6.TextLength > 2) SendKeys.Send("{TAB}");
        }
    }
}