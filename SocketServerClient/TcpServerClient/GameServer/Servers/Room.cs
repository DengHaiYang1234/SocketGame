using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Threading;

namespace GameServer.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }

    /// <summary>
    /// 房间类
    /// </summary>
    class Room
    {
        //管理房间中的科幻段
        private List<Client> clientRoom = new List<Client>();
        //房间状态
        private RoomState roomState = RoomState.WaitingJoin;
        //服务器
        private Server server;

        private const int MAX_HP = 100;

        /// <summary>
        /// 房间状态
        /// </summary>
        /// <returns></returns>
        public bool IsWaitingJoin()
        {
            return roomState == RoomState.WaitingJoin;
        }

        public Room(Server server)
        {
            this.server = server;
        }

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(Client client)
        {
            client.HP = MAX_HP;
            clientRoom.Add(client);
            client.Room = this;
            if (clientRoom.Count >= 2)
            {
                roomState = RoomState.WaitingBattle;
            }
        }

        /// <summary>
        /// 删除房间中的客户端
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(Client client)
        {
            client.Room = null;
            clientRoom.Remove(client);
            if (clientRoom.Count >= 2)
            {
                roomState = RoomState.WaitingBattle;
            }
            else
                roomState = RoomState.WaitingJoin;
        }

        /// <summary>
        /// 获取房主
        /// </summary>
        /// <returns></returns>
        public string GetHouseOwnerData()
        {
            return clientRoom[0].GetUserData();
        }

        /// <summary>
        /// 关闭房间
        /// </summary>
        /// <param name="clinet"></param>
        public void Close(Client clinet)
        {
            if (clinet == clientRoom[0])
            {
                Close();
            }
            else
            {
                clientRoom.Remove(clinet);
            }
            
        }

        /// <summary>
        /// 关闭房间
        /// </summary>
        public void Close()
        {
            foreach (Client client in clientRoom)
            {
                client.Room = null;
            }
            server.RemoveRoom(this);
        }

        /// <summary>
        /// 获取房间ID
        /// </summary>
        /// <returns></returns>
        public int GetId()
        {
            if (clientRoom.Count > 0)
            {
                return clientRoom[0].GetUserId();
            }
            return -1;
        }

        /// <summary>
        /// 获取房间信息
        /// </summary>
        /// <returns></returns>
        public string GetRoomData()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var client in clientRoom)
            {
                sb.Append(client.GetUserData() + "|");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1,1);
            }
            return sb.ToString();
        }
        
        /// <summary>
        /// 广播房间信息
        /// </summary>
        /// <param name="excludeClient"></param>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        public void BroadcastMessage(Client excludeClient,ActionCode actionCode,string data)
        {
            foreach (var client in clientRoom)
            {
                if (client != excludeClient)
                {
                    server.SendResponse(client, actionCode, data);
                }
            }
        }

        /// <summary>
        /// 是否是房主
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsHouseOwner(Client client)
        {
            return client == clientRoom[0];
        }



        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }

        public void RunTimer()
        {
            Thread.Sleep(1000);//1秒
            for (int i = 3; i > 0; i--)
            {
                BroadcastMessage(null, ActionCode.ShowTimer, i.ToString());
                Thread.Sleep(1000);//1秒
            }

            BroadcastMessage(null, ActionCode.StartPlay, "r");
        }

        /// <summary>
        /// 角色伤害
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="excludeClient"></param>
        public void TakeDamage(int damage,Client excludeClient)
        {
            bool isDie = false;
            foreach (Client client in clientRoom)
            {
                if (client != excludeClient)
                {
                    isDie = client.TakeDamage(damage);
                }
            }

            if (isDie == false) return;
            //角色死亡  游戏结束
            foreach (Client client in clientRoom)
            {
                if (client.IsDie())
                {
                    client.UpdateResult(false);
                    client.Send(ActionCode.GameOver, ((int) ReturnCode.Fail).ToString());
                }
                else
                {
                    client.UpdateResult(true);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                }
            }

            Close();

        }
    }
}
