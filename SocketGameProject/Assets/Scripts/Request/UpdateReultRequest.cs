using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class UpdateReultRequest : BaseRequest
{
    private RoomPanel roomPanel;

    private bool isUpdate = false;

    private int totalCount = 0;
    private int winCount = 0;

    public override void Awake()
    {
        actionCode = ActionCode.UpdateResult;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    private void Update()
    {
        if (isUpdate)
        {
            roomPanel.OnUpdateResultResponse(totalCount, winCount);
            isUpdate = false;
        }
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        totalCount = int.Parse(strs[0]);
        winCount = int.Parse(strs[1]);
        isUpdate = true;
    }

}
