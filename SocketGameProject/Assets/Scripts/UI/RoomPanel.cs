using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using DG.Tweening;
using Common;

public class RoomPanel : BasePanel
{
    //auto
    private Text Txt_Name = null;
    private Text Txt_Scene = null;
    private Text Txt_WinNum = null;
    private ScrollRect Scroll_View = null;
    private Scrollbar Scrollbar_Vertical = null;
    private Button Btn_CreatRoom = null;
    private Button Btn_Close = null;
    private Button Btn_Refresh = null;
    private GameObject roomItemPrefab;
    private GameObject grid;

    private UserData userdata = null;

    private List<UserData> userDataList = null;

    private CreatRoomRequest creatRoomRequest;

    private ListRoomRequest listRoomRequest;

    private JoinRequest joinRoomRequest;

    private UserData ud1;
    private UserData ud2;

    public override void InitStart()
    {
        Txt_Name = gameObject.transform.Find("BattleRes/Txt_Name").GetComponent<Text>();
        Txt_Scene = gameObject.transform.Find("BattleRes/Txt_Scene").GetComponent<Text>();
        Txt_WinNum = gameObject.transform.Find("BattleRes/Txt_WinNum").GetComponent<Text>();
        Scroll_View = gameObject.transform.Find("RoomList/Scroll_View").GetComponent<ScrollRect>();
        Scrollbar_Vertical = gameObject.transform.Find("RoomList/Scroll_View/Scrollbar_Vertical").GetComponent<Scrollbar>();
        Btn_CreatRoom = gameObject.transform.Find("RoomList/Btn_CreatRoom").GetComponent<Button>();
        Btn_Close = gameObject.transform.Find("RoomList/Btn_Close").GetComponent<Button>();
        Btn_Refresh = gameObject.transform.Find("RoomList/Btn_Refresh").GetComponent<Button>();
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;
        grid = gameObject.transform.Find("RoomList/Scroll_View/Viewport/Content").gameObject;
        creatRoomRequest = gameObject.transform.GetComponent<CreatRoomRequest>();
        listRoomRequest = gameObject.transform.GetComponent<ListRoomRequest>();
        joinRoomRequest = gameObject.transform.GetComponent<JoinRequest>();
        AddClicks();
    }

    private void Update()
    {
        if (userDataList != null)
        {
            LoadRoomItem(userDataList);
            userDataList = null;
        }

        if (userdata != null)
        {
            
        }

        if (ud1 != null && ud2 != null)
        {
            BasePanel panel = uiMgr.PushPanel(UIPanelType.RoomInfo);
            (panel as RoomInfoPanel).SetAllPlayerResSync(ud1, ud2);
            ud1 = null;
            ud2 = null;
        }
    }


    private void AddClicks()
    {
        Btn_CreatRoom.onClick.AddListener(OnCreatRoomClick);
        Btn_Close.onClick.AddListener(OnCloseClick);
        Btn_Refresh.onClick.AddListener(OnRefreshClick);
    }

    private void SetBattleResult()
    {
        UserData data = facade.GetUserData();
        Txt_Name.text = data.UserName;
        Txt_Scene.text = string.Format("总场次：{0}", data.TotalCount.ToString());
        Txt_WinNum.text = string.Format("胜利：{0}", data.WinCount.ToString());

    }

    public void OnUpdateResultResponse(int totalcount,int wincount)
    {
        facade.UpdateResult(totalcount, wincount);
        SetBattleResult();
    }

    public void LoadRoomItemSync(List<UserData> udList)
    {
        userDataList = udList;
    }

    private void LoadRoomItem(List<UserData> udList )
    {
        RoomItem[] roomArr =  grid.GetComponentsInChildren<RoomItem>();
        foreach (var roomItem in roomArr)
        {
            roomItem.DestroySelf();
        }

        foreach (var userData in udList)
        {
            var roomItem = Instantiate(roomItemPrefab);
            roomItem.transform.SetParent(grid.transform);
            UserData data = userData;
            roomItem.GetComponent<RoomItem>().SetRoomInfo(userData.Id,userData.UserName, userData.TotalCount, userData.WinCount,this);
        }
    }

    private void OnCreatRoomClick()
    {
        BasePanel basePanel = uiMgr.PushPanel(UIPanelType.RoomInfo);
        creatRoomRequest.SetPanel(basePanel);
        creatRoomRequest.SendRequest();
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        uiMgr.PopPanel();
    }

    private void OnRefreshClick()
    {
        listRoomRequest.SendRequest();
    }

    public void OnJoinClick(int id)
    {
        joinRoomRequest.SendRequest(id);
    }

    public void OnJoinResponse(ReturnCode returnCode,UserData ud1,UserData ud2)
    {
        switch (returnCode)
        {
            case ReturnCode.NotFound:
                uiMgr.ShowMessageSync("房间不存在！加入失败");
                break;
            case ReturnCode.Success:
                this.ud1 = ud1;
                this.ud2 = ud2;
                break;
            case ReturnCode.Fail:
                uiMgr.ShowMessageSync("房间已满，无法加入");
                break;
        }
    }



    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        SetBattleResult();
    }

    /// <summary>
    /// 界面暂停  表示的当前的界面的上一个界面(加载下一个界面)
    /// </summary>
    public override void OnPause()
    {
        base.OnPause();
    }

    /// <summary>
    /// 界面继续  表示的当前的界面的上一个界面(弹出上一个界面)
    /// </summary>
    public override void OnResume()
    {
        base.OnResume();
    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
    }

    //autoEnd
}
