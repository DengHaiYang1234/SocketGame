using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Common;
using MySql.Data.MySqlClient;
using GameServer.Tool;
using System.Net;
using GameServer.Model;
using GameServer.DAO;

namespace GameServer.Servers
{
    /// <summary>
    /// 管理各个客户端对象
    /// </summary>
    class Client
    {
        private Socket clientSocket; //当前客户端
        private Server server; //服务器
        private Message msg = new Message(); //消息处理
        private MySqlConnection mySqlConnection;

        private User user;
        private Result result;

        private Room room;

        private ResultDAO resultDAO = new ResultDAO();


        public int HP { get; set; }

        public bool TakeDamage(int damage)
        {
            HP -= damage;
            HP = Math.Max(HP, 0);
            return HP <= 0 ? true : false;
        }

        public bool IsDie()
        {
            return HP <= 0;
        }

        public void SetUserData(User _user,Result _result)
        {
            user = _user;
            result = _result;
        }

        public int GetUserId()
        {
            return user.Id;
        }

        public string GetUserData()
        {
            return user.Id + "," +  user.Username + "," + result.TotalCount + "," + result.WinCount;
        }

        public User User
        {
            set { user = value; }
        }

        public Result Result
        {
            set { result = value; }
        }

        public Room Room
        {
            set { room = value; }
            get { return room; }
        }

        public Client()
        {
            
        }

        public Client(Socket clientSocket,Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            mySqlConnection = ConnHelper.Connect(); //建立数据库的连接并持有数据库的引用
        } 

        public MySqlConnection MySqlConnection
        {
            get { return mySqlConnection; }
        }

        /// <summary>
        /// 开始接收消息
        /// </summary>
        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                return;
            }
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack,null);
        }
        /// <summary>
        /// 接收消息回调
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false)
                {
                    return;
                }
                int count = clientSocket.EndReceive(ar); //消息的字节数量
                if (count == 0)
                {
                    Close();
                }
                
                msg.ReadMessage(count,OnProcessMessage);  //解析数据。并做消息回调
                    
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        /// <summary>
        /// 消息处理回调
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        private void OnProcessMessage(RequestCode requestCode,ActionCode actionCode,string data)
        {
            server.HandlerRequest(requestCode,actionCode,data,this);
        }

        private void Close()
        {
            if (room != null)
                room.Close(this);

            ConnHelper.CloseConnection(mySqlConnection);
            if (clientSocket != null)
            {
                IPEndPoint ipEndPot = clientSocket.RemoteEndPoint as IPEndPoint;
                Console.WriteLine("====IP:{0}【{1}】已断开====", ipEndPot.Address, ipEndPot.Port);
                clientSocket.Close();
            }
            server.RemoveClient(this);
        }

        /// <summary>
        /// 向客户端发送数据
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        public void Send(ActionCode actionCode, string data)
        {
            try
            {
                byte[] bytes = Message.PackData(actionCode, data);
                clientSocket.Send(bytes);
            }
            catch(Exception e)
            {
                Console.WriteLine("无法发送消息:" + e);
            }

        }

        public bool IsHouseOwner()
        {
            return room.IsHouseOwner(this);
        }

        public void UpdateResult(bool isVictor)
        {
            UpdateResultToDB(isVictor);
            UpdateResultToClient();
        }

        private void UpdateResultToDB(bool isVictor)
        {
            result.TotalCount++;
            if (isVictor)
            {
                result.WinCount++;
            }

            resultDAO.UpdateOrAddResult(mySqlConnection, result);
        }

        private void UpdateResultToClient()
        {
            Send(ActionCode.UpdateResult, string.Format("{0},{1}", result.TotalCount, result.WinCount));
        }
    }
}
