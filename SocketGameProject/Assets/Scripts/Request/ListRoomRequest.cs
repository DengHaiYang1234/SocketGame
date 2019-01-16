using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

/// <summary>
/// 房间列表请求
/// </summary>
public class ListRoomRequest : BaseRequest
{
    private RoomPanel roomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ListRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        List<UserData> udList = new List<UserData>();
        if (data != "0")
        {
            string[] roomArr = data.Split('|');
            foreach (string ud in roomArr)
            {
                string[] strs = ud.Split(',');
                udList.Add(new UserData(int.Parse(strs[0]), strs[1], int.Parse(strs[2]), int.Parse(strs[3])));
            }
        }
        roomPanel.LoadRoomItemSync(udList);
    }
}
