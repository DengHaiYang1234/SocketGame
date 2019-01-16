using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class StartGameRequest : BaseRequest
{
    private RoomInfoPanel roomInfoPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.StartGame;
        roomInfoPanel = GetComponent<RoomInfoPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        Debug.Log("StartGameRequest  OnResponse StartGameRequest  OnResponse StartGameRequest  OnResponse");
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        roomInfoPanel.OnStartGameResponse(returnCode);
    }

}
