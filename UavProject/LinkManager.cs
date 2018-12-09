using ControlUtils.Comm;
using ControlUtils.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Serialization;
using UavProject.DataModel;

namespace UavProject
{
    class LinkManager : ViewModel
    {
        private static readonly Lazy<LinkManager> _data = new Lazy<LinkManager>(() => new LinkManager());
        private UdpTransport m_sock;
        private Thread m_thread;   
        private bool m_exit_thread = false;
        
        private object m_cs = new object();
        public byte[] uplink = null;       

        public static LinkManager Mgr
        {
            get { return _data.Value; }
        }
        public void Initialize()
        {
            Check();
            if (m_thread == null)
            {
                System.Diagnostics.Debug.WriteLine("Thread Start");
                m_thread = new Thread(Thread_Task);
                m_thread.Name = "linkmgr_thread";
                m_thread.IsBackground = true;
                m_thread.Priority = ThreadPriority.AboveNormal;
                m_thread.Start(this);
            }
        }
        public void Exit()
        {
            Settings.Setting.Save();
        }

        private void Thread_Task(object obj)
        {
            long last_time = 0;
            while (!m_exit_thread)
            {
                double sec = (DateTime.Now.Ticks - last_time) / (double)TimeSpan.TicksPerMillisecond;
                if (sec >= 20)
                {
                    //Hz 맞추기...
                    byte[] buffer = UavViewModel.UavModel.GeneratorDownlink();
                    if (buffer != null && m_sock != null)
                    {
                        m_sock.Send(buffer, true);
                        last_time = DateTime.Now.Ticks;
                    }
                }

                Thread.Sleep(2);
            }
        }

      
        public void Check()
        {
            try
            {
                if (m_sock == null) m_sock = new UdpTransport();
                if (m_sock.IsOpened)
                {
                    return;
                }
                if (!m_sock.IsOpened)
                {
                    if(m_sock.Open(null, Settings.Setting.RcvPort, OnReceived, Settings.Setting.SendIP, Settings.Setting.SndPort))
                    {
                        System.Diagnostics.Debug.WriteLine("udp check");
                    }
                }
            }
            catch
            {

            }
        }

        private void OnReceived(object sender, byte[] buffer) //UpLink 파싱
        {
            UavViewModel.UavModel.TryParseUplink(buffer);
        }





    }
}
