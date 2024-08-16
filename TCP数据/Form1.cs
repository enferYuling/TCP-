using HZH_Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCP数据
{
    public partial class Form1 : Form
    {
       

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        Thread threadClient = null; // 创建用于接收服务端消息的 线程；  
        Socket sockClient = null;
        TcpClient client = null;
        TcpListener tcpListener = null;
        public NetworkStream stream;
        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPAddress iP = IPAddress.Parse(tcptext.Text);
            int port = dktext.Text.ToInt();
            tcpListener = new TcpListener(iP, port);
           tcpListener.Start();
            this.Text = "等待客户端连接";
            client = tcpListener.AcceptTcpClient();
            this.Text = "客户端已连接";
            
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            stream=client.GetStream();
            string dataToSend = txtMessage.Text;
            byte[]dataByte=Encoding.ASCII.GetBytes(dataToSend);
            stream.Write(dataByte, 0, dataByte.Length);
            this.Text = "已发送数据";
           // stream.Close();
           // client.Close();
           // tcpListener.Stop();
        }
    }
}
