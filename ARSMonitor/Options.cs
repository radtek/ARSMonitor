using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARSMonitor
{
    public partial class Options : Form
    {
        string speed1, speed2;
        Form1 parent;
        public Options(Form1 p)
        {
            InitializeComponent(); 
            parent = p;
            trackBar1.Value = parent.speed1;
            trackBar2.Value = parent.speed2;
            textBox1.Text = trackBar1.Value.ToString();

            if ((trackBar2.Value / 2.0) >= 60)
            {
                double secs = trackBar2.Value / 2.0;
                int mins = (int)Math.Round(secs) / 60;
                textBox4.Text = mins.ToString();
                speed2 = (secs * 1000).ToString();
                textBox5.Text = (secs % 60).ToString();
                label6.Text = "минут";
                textBox5.Visible = true;
                label7.Visible = true;
            }
            else
            {
                textBox4.Text = (trackBar2.Value / 2.0).ToString();
                speed2 = (Convert.ToDouble(textBox4.Text) * 1000).ToString();
                label6.Text = "сек";
                textBox5.Visible = false;
                label7.Visible = false;
            }
        }

        private void Options_Load(object sender, EventArgs e)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            textBox3.Text = path;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            setSpeed1();
        }

        private void setSpeed1()
        {
            speed1 = trackBar1.Value.ToString();
            textBox1.Text = speed1;
            parent.speed1 = Convert.ToInt32(speed1);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            setSpeed2();
        }

        private void setSpeed2()
        {
            if ((trackBar2.Value / 2.0) >= 60)
            {
                double secs = trackBar2.Value / 2.0;
                int mins = (int)Math.Round(secs) / 60;
                textBox4.Text = mins.ToString();
                speed2 = (secs * 1000).ToString();
                textBox5.Text = (secs % 60).ToString();
                label6.Text = "минут";
                textBox5.Visible = true;
                label7.Visible = true;
            }
            else
            {
                textBox4.Text = (trackBar2.Value / 2.0).ToString();
                speed2 = (Convert.ToDouble(textBox4.Text) * 1000).ToString();
                label6.Text = "сек";
                textBox5.Visible = false;
                label7.Visible = false;
            }
            //MessageBox.Show(speed2.ToString());
            parent.speed2 = Convert.ToInt32(speed2);
        }

        void apply()
        {
            // 1
            setSpeed1();
            setSpeed2();
        }

        void cancel()
        {
            // 2
        }


        private void button1_Click(object sender, EventArgs e)
        {
            apply();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            apply();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cancel();
            Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Visible)
                trackBar2.Value = (int)Math.Round(Convert.ToDouble(textBox4.Text)) * 120 + (int)Math.Round(Convert.ToDouble(textBox5.Text)) * 2;
            if ((trackBar2.Value / 2.0) >= 60)
            {
                textBox5.Visible = true;
                label7.Visible = true;
            }
            else
            {
                textBox5.Visible = false;
                label7.Visible = false;
            }
        }
    }
}
