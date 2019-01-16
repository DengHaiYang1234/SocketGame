using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    class RoomController : BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }

        /// <summary>
        /// 获取客户端请求.创建房间
        /// </summary>
        /// <param name="data">  </param>
        /// <param name="client"> 客户端 </param>
        /// <param name="server">  </param>
        /// <returns></returns>
        public string CreatRoom(string data, Client client, Server server)
        {
            server.CreatRoom(client);
            return ((int)ReturnCode.Success).ToString() + "," + ((int)RoleType.Blue).ToString();
        }

        /// <summary>
        /// 获取房间列表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string ListRoom(string data, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Room room in server.GetRoomList())
            {
                if (room.IsWaitingJoin())
                {
                    sb.Append(room.GetHouseOwnerData() + "|");
                }
            }

            if (sb.Length == 0)
            {
                sb.Append("0");
            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 加入房间
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string JoinRoom(string data, Client client, Server server)
        {
            int id = int.Parse(data);
            Room room =  server.GetRoomById(id);
            if (room == null)
            {
                return ((int)ReturnCode.NotFound).ToString(); //房间未找到
            } 
            else if (room.IsWaitingJoin() == false)
            {
                return ((int)ReturnCode.Fail).ToString();//房间已满员
            }
            else
            {
                room.AddClient(client);
                string roomData = room.GetRoomData(); //所有房间里的信息(id,name,totalCount,winCount|id,name,totalCount,winCount)
                room.BroadcastMessage(client,ActionCode.UpdateRoom,roomData);
                return ((int) ReturnCode.Success).ToString() + "," + ((int)RoleType.Red).ToString() + "-" + roomData;
            }
        }

        /// <summary>
        /// 退出房间
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string QuitRoom(string data, Client client, Server server)
        {
            bool isHouseOwner = client.IsHouseOwner();
            Room room = client.Room;
            if (isHouseOwner) //房主退出
            {
                room.BroadcastMessage(client, ActionCode.QuitRoom, ((int)ReturnCode.Success).ToString());
                room.Close();
                return ((int)ReturnCode.Success).ToString();
            }
            else//其他成员退出
            {
                room.RemoveClient(client);
                room.BroadcastMessage(client, ActionCode.UpdateRoom, room.GetRoomData());
                return ((int)ReturnCode.Success).ToString();
            }
        }


    }
}
