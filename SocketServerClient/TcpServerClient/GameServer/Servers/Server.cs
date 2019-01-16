using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GameServer.Controller;
using Common;

namespace GameServer.Servers
{
    /// <summary>
    /// 服务器端
    /// </summary>
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket; //服务器端
        private List<Client> clientList; //管理所有连接的客户端
        private List<Room> roomList = new List<Room>();
        private ControllerManager controllerManager;

        public Server()
        {
            
        }

        public Server(string ipStr,int port)
        {
            SetIpEndPoint(ipStr, port);
            controllerManager = new ControllerManager(this); //连接数据库
        }


        public void SetIpEndPoint(string ipStr,int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port); //建立服务器连接
        }

        /// <summary>
        /// 服务器开启监听
        /// </summary>
        public void Start()
        {
            clientList = new List<Client>();
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);

            try
            {
                Console.WriteLine("服务器启动成功");
                Console.WriteLine("===============================");
            }
            catch
            {
                Console.WriteLine("服务器启动失败！");
            }

            serverSocket.BeginAccept(AcceptCallBack,null); //异步接收客户端连接
        }

        /// <summary>
        /// 异步接收客户端的连接
        /// </summary>
        /// <param name="ar"></param>
        public void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar); //接收到的客户端请求
            IPEndPoint ipEndPot = clientSocket.RemoteEndPoint as IPEndPoint;
            Console.WriteLine("====IP:{0}【{1}】连接成功====", ipEndPot.Address, ipEndPot.Port);
            Client client = new Client(clientSocket,this);  //单个处理客户端的请求
            client.Start();
            clientList.Add(client);
            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        /// <summary>
        /// 移除与该客户端的连接
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(Client client)
        {
            lock (clientList) //防止对象访问出现异常
            {
                clientList.Remove(client);
            }
        }

        /// <summary>
        /// 向客户端的请求回复
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        public void SendResponse(Client client,ActionCode actionCode, string data)
        {
            client.Send(actionCode, data);
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        public void HandlerRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }
        /// <summary>
        /// 创建房间
        /// </summary>
        /// <param name="client"></param>
        public void CreatRoom(Client client)
        {
            Room room = new Room(this);
            room.AddClient(client);
            roomList.Add(room);
        }

        /// <summary>
        /// 总的房间个数
        /// </summary>
        /// <returns></returns>
        public List<Room> GetRoomList()
        {
            return roomList;
        }

        /// <summary>
        /// 删除房间
        /// </summary>
        /// <param name="room"></param>
        public void RemoveRoom(Room room)
        {
            if (roomList != null && room != null)
            {
                roomList.Remove(room);
            }
        }

        /// <summary>
        /// 根据ID获取房间信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Room GetRoomById(int id)
        {
            foreach (var room in roomList)
            {
                if (room.GetId() == id)
                    return room;
            }

            return null;
        }

    }
}
