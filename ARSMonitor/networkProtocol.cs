using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace ARSMonitor
{
    class networkProtocol
    {
        public List<PicAndLabelInline.UserControl1> serverList;

        public class CurrentState
        {
            public bool isOnline;
            public string address;
        }

        public bool workState = true;

        public void pingServers(        System.ComponentModel.BackgroundWorker worker,
                                        System.ComponentModel.DoWorkEventArgs e
            )
        {
            while (!worker.CancellationPending)
            {
                System.Threading.Thread.Sleep(1000);
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; // сделать размер буфера изменяемым в настройках
                CurrentState state = new CurrentState();
                byte[] buffer = Encoding.ASCII.GetBytes(data); // сделать изменяемым в настройках
                int timeout = 120; // сделать изменяемым в настройках
                double count = serverList.Count;
                double i = 0;
                int progress;
                foreach (PicAndLabelInline.UserControl1 server in serverList)
                {
                    if (!workState)
                    {
                        break;
                    }
                    i++;
                    System.Threading.Thread.Sleep(1000);
                    progress = (int)Math.Round((i / count) * 100);
                    //elements.Find(el => el.Child == server);
                    string host = server.objectAddress; // сделать изменяемым в настройках
                    PingReply reply = pingSender.Send(host, timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        state.address = server.objectAddress;
                        state.isOnline = true;
                        worker.ReportProgress(progress, state);

                        //server.objectStatus = true;
                        //server.objectName = "GO!";
                    }
                    else
                    {
                        state.address = server.objectAddress;
                        state.isOnline = false;
                        worker.ReportProgress(progress, state);
                        //server.objectStatus = false;
                        //server.objectName = server.objectAddress + " STOPPED!";
                    }
                }
                if (!workState)
                    System.Threading.Thread.Sleep(1000);
                System.Threading.Thread.Sleep(100);
            }

            e.Cancel = true;
            
        }
    }
}
