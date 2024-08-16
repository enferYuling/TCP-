using HZH_Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
        public string GetLocalIPAddress()  //寻找本机IP地址
        {
            // 遍历所有网络接口
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // 忽略不活动的网络接口
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    // 遍历网络接口的IP地址信息
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        // 返回IPv4地址
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
            return "";
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

        private void Form1_Load(object sender, EventArgs e)
        {
            string PAddress = GetLocalIPAddress();//获取IP地址
            tcptext.Text = PAddress;
        }

        private async void jsbtn_Click(object sender, EventArgs e)
        {
            await Serve();
        }
        public async static Task  Serve()
        {
            string serverIp = "8.137.119.17";
            int port = 8080;
            TcpClient client = new TcpClient();
            client.Connect(serverIp, port);
            Console.WriteLine("已连接");
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesread = stream.Read(buffer, 0, buffer.Length);
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesread);
            Console.WriteLine("接收的数据：" + dataReceived);
            //关闭连接
            stream.Close();
            client.Close();

        }
    }
}
