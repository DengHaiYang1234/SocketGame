using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TcpServerSocket
{
    class Program
    {

        //public static byte[] dataBuffer = new byte[1024];
        private static Message msg = new Message();
        static void Main(string[] args)
        {
            StartServerAsync();
            Console.ReadKey();
            
        }

        //同步
        void StartServerSynv()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //本机IP:192.168.0.103   127.0.0.1
            //IPAddress ipAddress = new IPAddress(new byte[] {127,0,0,1});
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 888);
            serverSocket.Bind(ipEndPoint); //绑定ip和端口号
            //开始监听端口号
            serverSocket.Listen(0);  // 若为10，表示队列只能处理10个。   若有11个，那么就不监听11个。 0:表示队列不限制,处理不过来，排着就行
            //接收一个客户端连接
            Socket clientSocket = serverSocket.Accept();

            //向客户端发送一条消息
            string message = "Hello Client!你好....";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);

            //接收客户端的一条消息
            byte[] dataBuffer = new byte[1024];
            int count = clientSocket.Receive(dataBuffer);
            string meeageReveive = System.Text.Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine(meeageReveive);

            Console.ReadKey();

            clientSocket.Close();
            serverSocket.Close();
        }

        //异步
        static void StartServerAsync()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //本机IP:192.168.0.103   127.0.0.1
            //IPAddress ipAddress = new IPAddress(new byte[] {127,0,0,1});
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 888);
            serverSocket.Bind(ipEndPoint); //绑定ip和端口号
            //开始监听端口号
            serverSocket.Listen(0);  // 若为10，表示队列只能处理10个。   若有11个，那么就不监听11个。 0:表示队列不限制,处理不过来，排着就行

            //异步接收消息
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);

        }

        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            //获取连接的客户端
            Socket clientSocket = serverSocket.EndAccept(ar);

            Console.WriteLine("======={0}已连接：",clientSocket.LocalEndPoint);

            //向客户端发送一条消息
            string message = "Hello Client!你好....";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);

            //接收客户端的消息  msg.Data:数据缓冲  msg.StartIndex:(接收数据的起始位置)存储所接收数据的位置，该位置从零开始计数。  msg.RemainSize:设置接收数据的数组长度
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);

            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }


        static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                //停止接收之前的数据总长度
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }
                msg.AddCount(count);


                msg.ReadMessage();
                //string msgStr = Encoding.UTF8.GetString(msg.Data, 0, count);
                //Console.WriteLine("接收到【{0}】的消息:{1}", clientSocket.LocalEndPoint, msgStr);
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)
                    clientSocket.Close();
            }
        }


    }
}
