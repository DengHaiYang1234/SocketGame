using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;
using System.IO;

/// <summary>
/// 管理所有UI
/// </summary>
public class UIManager :BaseManager
{

    /// 
    /// 单例模式的核心
    /// 1，定义一个静态的对象 在外界访问 在内部构造
    /// 2，构造方法私有化

    public UIManager(GameFacade facade) : base(facade)
    {
        ParseUIPanelTypeJson();
    }

    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }
    private Dictionary<UIPanelType, string> panelPathDict;//存储所有面板Prefab的路径
    private Dictionary<UIPanelType, BasePanel> panelDict;//保存所有实例化面板的游戏物体身上的BasePanel组件
    private Dictionary<UIPanelType, Component> panelMap = new Dictionary<UIPanelType, Component>();
    private Stack<BasePanel> panelStack;
    private UIPanelType panelTypeToPush = UIPanelType.None;
    public delegate bool PopCallBack();

    public delegate void PushSyncCallBack(UserData u1, UserData u2);
    


    public override void OnInit()
    {
        base.OnInit();
        PushPanel(UIPanelType.Message);
        PushPanel(UIPanelType.Start);
    }

    public override void Update()
    {
        base.Update();
        if (panelTypeToPush != UIPanelType.None)
        {
            PushPanel(panelTypeToPush);
            panelTypeToPush = UIPanelType.None;
        }
    }

    public void MapingPanelByName<T>(UIPanelType panelType,T panel) where T : Component
    {
        panelMap.Add(panelType, panel);
    }

    public Component GetComponentByType(UIPanelType panelType)
    {
        return panelMap.TryGetV<UIPanelType, Component>(panelType);
    }
    
    /// <summary>
    /// 把某个页面入栈，  把某个页面显示在界面上
    /// </summary>
    public BasePanel PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        //判断一下栈里面是否有页面  
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
        return panel;
    }

    public void PushPanelSync(UIPanelType panelType)
    {
        panelTypeToPush = panelType;
    }
    /// <summary>
    /// 出栈 ，把页面从界面上移除
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

        //关闭栈顶页面的显示
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        if (panelStack.Count <= 0) return;
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();
    }

    public void PopPanel(PopCallBack callback)
    {
        if (callback())
        {
            PopPanel();
        }
    }

    /// <summary>
    /// 根据面板类型 得到实例化的面板
    /// </summary>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        BasePanel panel = panelDict.TryGetV(panelType);

        if (panel == null)
        {
            //如果找不到，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
            //string path;
            //panelPathDict.TryGetValue(panelType, out path);
            string path = panelPathDict.TryGetV(panelType);
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
            instPanel.transform.SetParent(CanvasTransform,false);
            instPanel.GetComponent<BasePanel>().UIMgr = this; //让所有派生类持有UIManager
            instPanel.GetComponent<BasePanel>().Facade = gameFacade;//让所有派生类持有GameFacade
            instPanel.GetComponent<BasePanel>().InitStart();
            return instPanel.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }
    }
    
    public void ShowMessage(string msg)
    {
        MessagePanel msgPanel = GetComponentByType(UIPanelType.Message) as MessagePanel;
        if (msgPanel == null)
        {
            Debug.LogError("ShowMessage is Called But msgPanel == null");
            return;
        }
        msgPanel.ShowMessage(msg);
    }

    public void ShowMessageSync(string msg)
    {
        MessagePanel msgPanel = GetComponentByType(UIPanelType.Message) as MessagePanel;
        if (msgPanel == null)
        {
            Debug.LogError("ShowMessage is Called But msgPanel == null");
            return;
        }
        msgPanel.ShowMessageSync(msg);

    }
    
    [Serializable]
    class UIPanelJson
    {
        public List<PanelInfo> infoList;
    }

    [Serializable]
    class PanelInfo
    {
        public string panelTypeString;
        public string path;
    }

    private void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<UIPanelType, string>();
       
        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");


        UIPanelJson jsonObject = JsonMapper.ToObject<UIPanelJson>(ta.text);

        foreach (PanelInfo info in jsonObject.infoList)
        {
            UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), info.panelTypeString);
            panelPathDict.Add(type, info.path);
        }
    }
}
