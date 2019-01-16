using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class JoinRequest : BaseRequest
{
    private RoomPanel roomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public void SendRequest(int id)
    {
        base.SendRequest(id.ToString());
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('-');
        string[] resultStr = strs[0].Split(',');
        ReturnCode returnCode = (ReturnCode) int.Parse(resultStr[0]); //返回成功与否结果

        UserData userData_1 = null;
        UserData userData_2 = null;
        if (returnCode == ReturnCode.Success)
        {
            string[] userDataArr = strs[1].Split('|');
            userData_1 = new UserData(userDataArr[0]);
            userData_2 = new UserData(userDataArr[1]);

            RoleType roleType = (RoleType)int.Parse(resultStr[1]); //返回角色类型结果
            facade.SetCurrentRoleType(roleType);

        }
        roomPanel.OnJoinResponse(returnCode, userData_1, userData_2);

    }

}
