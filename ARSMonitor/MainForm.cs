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
        public MainForm()
        {
            InitializeComponent();
            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel2.Visible = false;
            directotries[0] = System.IO.Directory.GetCurrentDirectory();
            speed1 = 30;
            speed2 = 500;

            initOptions();
            o = new Options(this);
            importingServers();

            refreshControlView();
        }

        private void refreshControlView()
        {
            foreach (serverControl server in servers)
            {
                server.objectStatus = true;
                server.objectStatus = false;
            }
        }



        logNet log;

        void initOptions() // базовая инициализация настроек программы при запуске.
        {
            int f = 0;
            // файл опций жёстко структурирован
            string[] lines = System.IO.File.ReadAllLines(@"C:\ARSMonitor\options.ini");
            servPath = lines[0];
            if (!Int32.TryParse(lines[1], out speed1))
            {
                f = 1;
            }
            if (!Int32.TryParse(lines[2], out speed2))
            {
                f = 2;
            }
            if (!Int32.TryParse(lines[3], out speed3))
            {
                f = 3;
            }

            if (lines[4] == "1")
            {
                isParallel = true;
            }
            else if (lines[4] == "0")
            {
                isParallel = false;
            }
            else
            {
                f = 4;
            }

            picON = lines[5];
            picOFF = lines[6];

            if (f != 0)
                toolStripStatusLabel1.Text = "Options initialization failed on string number " + f.ToString() + "!";
            else toolStripStatusLabel1.Text = "Options initialized successfully";
        }



        // Опции. Константы и переменные опций.
        Options o;
        string[] directotries = new string[3];
        public int speed1, speed2, speed3; // задержки при выполнении
        public bool isParallel = false; // переключатель типа обхода
        public string servPath = @"C:\ARSMonitor\Servers"; 
        public string picON, picOFF; // путь к картинкам

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            networkProtocol np = new networkProtocol();
            np.serverList = servers;
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync(np);
                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel2.Text = "Waiting for ping";
                toolStripStatusLabel2.Visible = true;
            }
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopPinging();
        }

        private void StopPinging() // останавливаем опрос хостов
        {
            if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();

            if (backgroundWorker2.IsBusy)
                backgroundWorker2.CancelAsync();
            threadList.ForEach(cancelWork);
            servers.ForEach(switchOff);
            cancel = true;
        }

        // метод, создающий событие отмены для работающих фоновых процессов.
        void cancelWork(BackgroundWorker bw)
        {
            if (bw.IsBusy)
                bw.CancelAsync();
        }

        // выключатель компонента.
        void switchOff(serverControl sC)
        {
            sC.objectStatus = false;
        }

        List<BackgroundWorker> threadList = new List<BackgroundWorker>(); // запуск ветки процесса на каждый хост.
        public List<serverControl> servers = new List<serverControl>(); // список хостов
        public int x = 25, y = 25;
        public bool working = true;
        public string n = "New";
        public string a = "192.168.0.4";
        public bool success = false;
        public bool cancel = false;

        private void addServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addServByMenu();
        }

        private void addServByMenu()
        {
            working = false;
            ServerConstructor f = new ServerConstructor(this);
            f.ShowDialog();
            // если форма опций
            if (success)
            {
                addingServ(servers, n, a);
                n = "New";
                a = "192.168.0.4";
                drawServers();

                working = true;
            }
            else MessageBox.Show("Adding server cancelled. ");
        }

        private void addingServ(List<serverControl> servL, string n, string a)
        {
            // добавление уникального хоста в список для проверки
            serverControl tempS = new serverControl(n, a);
            if (!servL.Exists(x => x.objectAddress == a))
            {
                tempS.ContextMenuStrip = contextMenuStrip1;
                servL.Add(tempS);
            }
            else
            {   // сообщение о неуникальности
                tempS = servL.Find(x => x.objectAddress == a);
                MessageBox.Show("Уже существует хост с таким адресом. В программе имеет имя: " + tempS.objectName);
            }
        }

        private void drawServers()
        {
            // отображение (перерисовка) контролов на форме

            x = 25;
            y = 25;

            // заполняется сначала видимое пространство слева направо, сверху вниз.
            foreach (serverControl server in servers)
            {
                panel1.Controls.Remove(server);
                server.picktOnPath(picON);
                server.picktOffPath(picOFF);
                server.SetBounds(x, y, 200, 48);
                
                if (x + 405 > this.Width - 25)
                {
                    x = 25;
                    y += 50;
                }
                else x += 205;
                
                panel1.Controls.Add(server);
                //if (y + 50 > this.Height) {  }
                 
            }

        }

        private void drawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawServers();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // остановка работы
            StopPinging();
            // записать текущий список хостов в файл
            exportingServers();
            // закрытие главной формы
            Close();
        }


        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // import servers list from file
            // импорт списка серверов из файла
            importingServers();
        }

        private void importingServers()
        {
            // import servers list from file
            // импорт списка серверов из файла
            string[] lines = System.IO.File.ReadAllLines(servPath);
            foreach (string serv in lines)
            {
                if (serv != "")
                {
                    string[] splitted = serv.Split(' ');
                    n = splitted[1];
                    a = splitted[0];
                    addingServ(servers, n, a);
                }
            }
            /*
            servers.ForEach(delegate(serverControl serv)
            {
                serv.ContextMenuStrip = contextMenuStrip1;
            });*/
            drawServers();
        }


        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // export servers list into file
            // экспорт списка серверов в файл
            exportingServers();
        }

        private void exportingServers()
        {
            // export servers list into file
            // экспорт списка серверов в файл
            List<string> exportString = new List<string>();
            int i = 0;
            foreach (serverControl server in servers)
            {
                exportString.Add(server.objectAddress + " " + server.objectName + Environment.NewLine);
                i++;
            }
            //if ()

            System.IO.File.WriteAllLines(servPath, exportString.ToArray<string>());
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int[] speeds = { speed1, speed2, speed3 };
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;
            networkProtocol np = (networkProtocol)e.Argument;
            np.workState = working;
            np.serialPingServers(worker, e, speeds);
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
            if (server.objectStatus != state.isOnline)
            {
                server.objectStatus = state.isOnline;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {   // 
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
            if (log != null)
            {
                log.Top = this.Top;
                log.Left = this.Left + this.Width;
            }
            drawServers();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            o.ShowDialog();
        }


        public bool workState = true;


        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {   // Параллельный поток для отдельного хоста (ускорение прохода по списку хостов).
            int[] speeds = { speed1, speed2, speed3 };
            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender;
            var myArgs = e.Argument as MyWorkerArgs;
            serverControl serv = myArgs.srv;
            networkProtocol np = myArgs.netP;
            workState = working;
            np.workState = working;
            np.parallelPingServers(worker, e, speeds, serv);

        }

        private void serialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serialPinging();
        }

        private void serialPinging()
        {
            cancel = false;
            networkProtocol np = new networkProtocol();
            np.serverList = servers;
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync(np);
                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel2.Text = "Waiting for serial ping";
                toolStripStatusLabel2.Visible = true;
            }
        }
        class MyWorkerArgs
        {
            public serverControl srv;
            public networkProtocol netP;
            public MyWorkerArgs(serverControl server, networkProtocol np)
            {
                srv = server;
                netP = np;
            }
        }

        private void parallelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parallelPinging();
        }

        private void parallelPinging()
        {
            cancel = false;
            networkProtocol np = new networkProtocol();
            np.serverList = servers;
            foreach (serverControl server in servers)
            {
                threadList.Add(new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true });
                threadList.Last().DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
                threadList.Last().ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
                threadList.Last().RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
                threadList.Last().RunWorkerAsync(new MyWorkerArgs(server, np));
                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel2.Text = "Waiting for parallel ping";
                toolStripStatusLabel2.Visible = true;
                System.Threading.Thread.Sleep(speed1);
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
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
            if (server.objectStatus != state.isOnline)
            {
                server.objectStatus = state.isOnline;
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        private void переименоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverControl serv = contextMenuStrip1.SourceControl as serverControl;
            serv.editMode = true;
        }

        private void перезагрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverControl serv = contextMenuStrip1.SourceControl as serverControl;
            string hN = getHostName(serv);
            Process p = Process.Start(@"cmd.exe", @"/c shutdown /r /m " + hN + " & pause");
        }

        private static string getHostName(serverControl serv)
        {
            IPAddress hostIPAddress = IPAddress.Parse(serv.objectAddress);
            IPHostEntry hostInfo = Dns.GetHostEntry(hostIPAddress);
            return hostInfo.HostName;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void connectToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (isParallel)
                parallelPinging();
            else serialPinging();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverControl serv = contextMenuStrip1.SourceControl as serverControl;
            deleteHostFromList(serv);
        }

        private void deleteHostFromList(serverControl srv)
        {
            string message = "Вы уверены, что хотите удалить этот элемент?";
            string caption = "Удалить " + srv.objectName;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                srv.Dispose();
                servers.Remove(srv);
            }
            drawServers();
        }

        private void listenConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log = new logNet();
            AsynchronousSocketListener listen = new AsynchronousSocketListener();
            //networkProtocol np = new networkProtocol();
            //np.serverList = servers;
            if (!backgroundWorker3.IsBusy)
            {
                backgroundWorker3.RunWorkerAsync(listen);
                /* toolStripProgressBar1.Visible = true;
                 toolStripStatusLabel2.Text = "Waiting for serial ping";
                 toolStripStatusLabel2.Visible = true;*/

                log.StartPosition = FormStartPosition.Manual;
                log.Top = this.Top;
                log.Left = this.Left + this.Width;
                log.Show();
            }
            else MessageBox.Show("Not listening");
        }

        private void backgroundWorker3_DoWork(object sender1, DoWorkEventArgs e1)
        {
            AsynchronousSocketListener listen = e1.Argument as AsynchronousSocketListener;
            

            System.ComponentModel.BackgroundWorker worker;
            worker = (System.ComponentModel.BackgroundWorker)sender1;

            //e.Argument


            listen.eventFromNetworkClass += delegate(object sender, NetworkEventArgs e)
            {
                log.textBox1.Invoke((Action)delegate
                {
                    log.textBox1.AppendText(e.Message);
                    log.textBox1.AppendText(Environment.NewLine);
                });
            };

            /*Task.Factory.StartNew((Action)delegate
                    {
                        //mc.Run();
                        listen.StartListening(worker, e1);
                    });*/
            listen.StartListening(worker, e1);

            /*
            Task.Factory.StartNew((Action)delegate
            {
                //mc.Run();
                aS.Start();
            });*/
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void stopListeningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker3.CancelAsync();
            log.Close();
        }


        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string str = e.UserState as string;
            toolStripStatusLabel1.Text = str;
            log.textBox1.Text += str + Environment.NewLine + Environment.NewLine;
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (log != null)
            {
                log.Top = this.Top;
                log.Left = this.Left + this.Width;
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            // log.Activate();
        }

        private void sendCommandToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}