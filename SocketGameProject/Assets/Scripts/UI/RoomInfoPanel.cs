using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using Common;

public class RoomInfoPanel : BasePanel
{
    //auto
    private Text Txt_UserName_Blue = null;
    private Text Txt_TotalCount_Blue = null;
    private Text Txt_WinCount_Blue = null;
    private Text Txt_UserName_Red = null;
    private Text Txt_TotalCount_Red = null;
    private Text Txt_WinCount_Red = null;
    private Button Btn_StartGame = null;
    private Button Btn_Close = null;

    private Transform bluePanel = null;
    private Transform redPanel = null;

    //private CreatRoomRequest creatRoomRequest = null;

    private UserData ud = null;
    private UserData ud1 = null;
    private UserData ud2 = null;

    private QuitRoomRequest quitRoomRequest = null;

    private StartGameRequest startGameRequest = null;

    private bool isPop = false;

    private void Update()
    {
        if (ud != null)
        {
            SetBluePlayerRes(ud.UserName, ud.TotalCount.ToString(), ud.WinCount.ToString());
            SetEnemyPlayerRes();
            ud = null;
        }

        if (ud1 != null && ud2 != null)
        {
            SetBluePlayerRes(ud1.UserName, ud1.TotalCount.ToString(), ud1.WinCount.ToString());
            SetRedPlayerRes(ud2.UserName, ud2.TotalCount.ToString(), ud2.WinCount.ToString());
            ud1 = null;
            ud2 = null;
        }

        if (ud1 != null && ud2 == null)
        {
            SetBluePlayerRes(ud1.UserName, ud1.TotalCount.ToString(), ud1.WinCount.ToString());
            SetEnemyPlayerRes();
            ud1 = null;
        }

        if (isPop)
        {
            ExitAnim();
            isPop = false;
        }
    }

    public override void InitStart()
    {
        Txt_UserName_Blue = gameObject.transform.Find("BluePanel/Txt_UserName_Blue").GetComponent<Text>();
        Txt_TotalCount_Blue = gameObject.transform.Find("BluePanel/Txt_TotalCount_Blue").GetComponent<Text>();
        Txt_WinCount_Blue = gameObject.transform.Find("BluePanel/Txt_WinCount_Blue").GetComponent<Text>();
        Txt_UserName_Red = gameObject.transform.Find("RedPanel/Txt_UserName_Red").GetComponent<Text>();
        Txt_TotalCount_Red = gameObject.transform.Find("RedPanel/Txt_TotalCount_Red").GetComponent<Text>();
        Txt_WinCount_Red = gameObject.transform.Find("RedPanel/Txt_WinCount_Red").GetComponent<Text>();
        Btn_StartGame = gameObject.transform.Find("Btn_StartGame").GetComponent<Button>();
        Btn_Close = gameObject.transform.Find("Btn_Close").GetComponent<Button>();
        bluePanel = gameObject.transform.Find("BluePanel");
        redPanel = gameObject.transform.Find("RedPanel");
        //creatRoomRequest = GetComponent<CreatRoomRequest>();
        quitRoomRequest = GetComponent<QuitRoomRequest>();
        startGameRequest = GetComponent<StartGameRequest>();
        AddClicks();
    }



    private void AddClicks()
    {
        Btn_StartGame.onClick.AddListener(OnStartGameClick);
        Btn_Close.onClick.AddListener(OnCloseClick);
    }

    public void SetLocalPlayerResSync()
    {
        ud = facade.GetUserData();
    }

    public void SetAllPlayerResSync(UserData ud1,UserData ud2)
    {
        this.ud1 = ud1;
        this.ud2 = ud2;
    }

    public void SetBluePlayerRes(string username,string totalCount,string winCount)
    {
        Txt_UserName_Blue.text = username;
        Txt_TotalCount_Blue.text = "总场数：" + totalCount;
        Txt_WinCount_Blue.text = "胜利：" + winCount;
    }

    public void SetRedPlayerRes(string username, string totalCount, string winCount)
    {
        Txt_UserName_Red.text = username;
        Txt_TotalCount_Red.text = "总场数：" + totalCount;
        Txt_WinCount_Red.text = "胜利：" + winCount;
    }

    /// <summary>
    /// 清空战绩
    /// </summary>
    public void SetEnemyPlayerRes()
    {
        Txt_UserName_Red.text = "等待玩家加入";
        Txt_TotalCount_Red.text = "";
        Txt_WinCount_Red.text = "";
    }


    private void EnterAnim()
    {
        gameObject.SetActive(true);
        bluePanel.localPosition = new Vector3(-1000, 0, 0);
        bluePanel.DOLocalMoveX(-310, 0.4f);
        redPanel.localPosition = new Vector3(1000, 0, 0);
        redPanel.DOLocalMoveX(313, 0.4f);
    }

    //private void ExitAnim()
    //{
    //    bluePanel.DOLocalMoveX(-1000, 0.4f);
    //    redPanel.DOLocalMoveX(1000, 0.4f);
    //}

    private void ExitAnim()
    {
        bool isbluePanelComplete = false;
        bool isredPanelComplete = false;
        bluePanel.DOLocalMoveX(-1000, 0.4f);
        
        redPanel.DOLocalMoveX(1000, 0.4f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            uiMgr.PopPanel();

        });
    }
    
    private void OnStartGameClick()
    {
        startGameRequest.SendRequest();
    }

    public void OnStartGameResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMgr.ShowMessageSync("你不是房主，无法开始游戏！");
        }
        else
        {
            uiMgr.PushPanelSync(UIPanelType.Game);
            facade.EnterPlaying();
        }
    }

    private void OnCloseClick()
    {
        quitRoomRequest.SendRequest();
    }

    public void OnQuitResponse()
    {
        isPop = true;
    }


    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        EnterAnim();
        //creatRoomRequest.SendRequest();
    }

    /// <summary>
    /// 界面暂停  表示的当前的界面的上一个界面(加载下一个界面)
    /// </summary>
    public override void OnPause()
    {
        base.OnPause();
        //ExitAnim();
    }

    /// <summary>
    /// 界面继续  表示的当前的界面的上一个界面(弹出上一个界面)
    /// </summary>
    public override void OnResume()
    {
        base.OnResume();
        EnterAnim();
    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
        //ExitAnim();
    }

    //autoEnd
}
