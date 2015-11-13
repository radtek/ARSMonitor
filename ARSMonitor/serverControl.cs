using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public serverControl()
        {
            InitializeComponent();
        }

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
        public void switchMode(bool m)
        {
            if (m)
                textBox1.Visible = true;
            else textBox1.Visible = false;
        }
        // функция переключения индикатора
        public void switchStatusImage(bool st)
        {
                if (st) 
                {
                    /*BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("on.jpg", UriKind.Relative);
                    bi3.EndInit();*/
                    statusImage.SizeMode = PictureBoxSizeMode.Zoom;
                    statusImage.Image = System.Drawing.Image.FromFile("on.jpg");
                }
                else 
                {
                    /*BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("off.jpg", UriKind.Relative);
                    bi3.EndInit();
                    statusImage.Stretch = Stretch.Uniform;
                    statusImage.StretchDirection = StretchDirection.Both;
                    statusImage.Source = bi3;*/

                    statusImage.SizeMode = PictureBoxSizeMode.Zoom;
                    statusImage.Image = System.Drawing.Image.FromFile("off.jpg");
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
            textBox1.Visible = false;
            editMode = false;
        }

        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData==Keys.Enter)
            {
                ok_Click(null, null);
            }
        }
    }
}
