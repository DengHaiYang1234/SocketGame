using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

/// <summary>
/// 相当于是一个中介的作用。持有所有的Manager的引用
/// </summary>
public class GameFacade : MonoBehaviour
{
    private static GameFacade _instace;

    public static GameFacade Instance
    {
        get
        {
            //if (_instace == null)
            //{
            //    _instace = GameObject.Find("GameFacade").GetComponent<GameFacade>();
            //}
            return _instace;
        }
    }
    
    private UIManager uiMgr;
    private RequestManager reqMgr;
    private CamreaManager camMgr;
    private AudioMAnager audMgr;
    private PlayerManager playerMgr;
    private ClientManager clientMgr;

    private void Awake()
    {
        if (_instace != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instace = this;
    }

    private void Start()
    {
        InitManager();
    }

    private void Update()
    {
        UpdateManager();
    }

    /// <summary>
    /// 初始化所有Manager
    /// </summary>
    private void InitManager()
    {
        uiMgr = new UIManager(this);
        reqMgr = new RequestManager(this);
        camMgr = new CamreaManager(this);
        audMgr = new AudioMAnager(this);
        playerMgr = new PlayerManager(this);
        clientMgr = new ClientManager(this);

        uiMgr.OnInit();
        reqMgr.OnInit();
        camMgr.OnInit();
        audMgr.OnInit();
        playerMgr.OnInit();
        clientMgr.OnInit();
    }

    private void UpdateManager()
    {
        uiMgr.Update();
        reqMgr.Update();
        camMgr.Update();
        audMgr.Update();
        playerMgr.Update();
        clientMgr.Update();
    }

    public void AddRequest(ActionCode actionCode, BaseRequest requset)
    {
        reqMgr.AddRequest(actionCode, requset);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        reqMgr.RemoveRequset(actionCode);
    }

    public void HandleReponse(ActionCode actionCode, string data)
    {
        reqMgr.HandleReponse(actionCode, data);
    }

    public void ShowMessage(string msg)
    {
        uiMgr.ShowMessage(msg);
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        clientMgr.SendRequest(requestCode, actionCode, data);
    }

    public void PlayBgSound(string sound)
    {
        audMgr.PlayBgSound(sound);
    }

    public void PlayNoramSound(string sound,float volume,bool loop)
    {
        audMgr.PlayNoramSound(sound,volume,loop);
    }


    public void SetUserData(UserData userData)
    {
        playerMgr.UserData = userData;
    }

    public UserData GetUserData()
    {
        return playerMgr.UserData;
    }

    public void SetCurrentRoleType(RoleType rt)
    {
        playerMgr.SetCurrentRoleType(rt);
    }

    public GameObject GetCurrentRoleObj()
    {
        return playerMgr.GetCurrentRoleObj();
    }

    public void EnterPlaying()
    {
        playerMgr.SpawnRolesSync();
    }

    public void StartPlaying()
    {
        playerMgr.AddControllerScript();
        playerMgr.CreatSyncRequest();
    }

    public void SendAttack(int damage)
    {
        playerMgr.SendAttack(damage);
    }

    public void GameOver()
    {
        playerMgr.GameOver();
    }

    public void UpdateResult(int total, int wincount)
    {
        playerMgr.UpdateResult(total, wincount);
    }

    private void OnDestroyManager()
    {
        uiMgr.OnDestroy();
        reqMgr.OnDestroy();
        camMgr.OnDestroy();
        audMgr.OnDestroy();
        playerMgr.OnDestroy();
        clientMgr.OnDestroy();
    }
    
    private void OnDestroy()
    {
        OnDestroyManager();
    }

}
