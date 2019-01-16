using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class QuitRoomRequest : BaseRequest
{
    private RoomInfoPanel roomInfoPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.QuitRoom;
        roomInfoPanel = GetComponent<RoomInfoPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("R");
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        if (returnCode == ReturnCode.Success)
        {
            roomInfoPanel.OnQuitResponse();
        }
    }

}
