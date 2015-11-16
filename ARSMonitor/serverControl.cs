using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ARSMonitor
{
    public partial class serverControl : System.Windows.Forms.UserControl
    {
        MainForm parent;
        public serverControl(MainForm mf)
        {
            parent = mf;
            InitializeComponent();
            initOptions();
        }


        void initOptions()
        {
            // файл опций жёстко структурирован
            string[] lines = System.IO.File.ReadAllLines(@"C:\ARSMonitor\options.ini");
            picON = lines[4];
            picOFF = lines[5];
        }


        public string picON, picOFF;
        private bool status;
        private string name;
        private string address;
        private bool mode;

        public bool objectStatus
        {
            // Возвращает статус объекта.
            get
            {
                return status;
            }
            // Устанавливает статус объекта
            // и обновляет графический индикатор
            set
            {
                status = value;
                switchStatusImage(status);
            }
        }

        public bool editMode
        {
            // Возвращает статус объекта.
            get
            {
                return mode;
            }
            // Устанавливает статус объекта
            // и обновляет графический индикатор
            set
            {
                mode = value;
                switchMode(mode);
            }
        }

        // переключение режима редактирования
        public void switchMode(bool m)
        {
            if (m)
            {
                textBox1.Text = objectName;
                textBox1.Visible = true;
                textBox1.Focus();
            }
            else textBox1.Visible = false;
        }

        // функция переключения индикатора
        public void switchStatusImage(bool st)
        {
                if (st) 
                {
                    statusImage.SizeMode = PictureBoxSizeMode.Zoom;
                    statusImage.Image = System.Drawing.Image.FromFile(picON);
                }
                else 
                {
                    statusImage.SizeMode = PictureBoxSizeMode.Zoom;
                    statusImage.Image = System.Drawing.Image.FromFile(picOFF);
                }

        }


        public string objectName
        {
            // Возвращает статус объекта.
            get
            {
                return name;
            }
            // Устанавливает статус объекта
            // и обновляет графический индикатор
            set
            {
                name = value;
                objLabel.Text = name;
            }
        }

        public string objectAddress
        {
            // Возвращает статус объекта.
            get
            {
                return address;
            }
            // Устанавливает статус объекта
            // и обновляет графический индикатор
            set
            {
                address = value;
            }
        }

        public serverControl(string n = "new", string a = "192.168.0.1")
        {
            InitializeComponent();
            status = false;
            name = n;
            objectName = n;
            address = a;
            objectAddress = a;
        }

        private void serverControl_MouseLeave(object sender, EventArgs e)
        {
            BorderStyle = BorderStyle.FixedSingle;
        }

        private void serverControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            BorderStyle = BorderStyle.Fixed3D;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            objectName = textBox1.Text;
            editMode = false;
        }

        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData==Keys.Enter)
            {
                ok_Click(null, null);
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            //ok_Click(null, null);

            editMode = false;
        }
    }
}
