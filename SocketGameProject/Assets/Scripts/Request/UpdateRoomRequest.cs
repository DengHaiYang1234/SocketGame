using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class UpdateRoomRequest : BaseRequest
{

    private RoomInfoPanel roomInfoPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.UpdateRoom;
        roomInfoPanel = GetComponent<RoomInfoPanel>();
        base.Awake();
    }

    /// <summary>
    /// 房间更新通知
    /// </summary>
    /// <param name="data"></param>
    public override void OnResponse(string data)
    {
        UserData userData_1 = null;
        UserData userData_2 = null;

        string[] userDataArr = data.Split('|');
        
        if (userDataArr.Length > 0)
        {
            userData_1 = new UserData(userDataArr[0]);
        }

        if (userDataArr.Length > 1)
        {
            userData_2 = new UserData(userDataArr[1]);
        }
       
        roomInfoPanel.SetAllPlayerResSync(userData_1, userData_2);
    }

}
