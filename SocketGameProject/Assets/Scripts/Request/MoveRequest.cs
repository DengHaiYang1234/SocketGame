using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class MoveRequest : BaseRequest
{
    private Transform localPlayerTransform;
    private PlayerMove localPlayerMove;
    private float syncRate = 30f;
    
    private Transform remotePlayerTransform;
    private Animator remotePlayerAnim;
        
    private bool isSyncRemotePlayer = false;
    private Vector3 pos;
    private Vector3 rot;
    private float foward;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }

    private void Start()
    {
        InvokeRepeating("SyncLocalPlayer", 1f, 1f/syncRate);
    }

    private void FixedUpdate()
    {
        if (isSyncRemotePlayer)
        {
            SyncRemotePlayer();
            isSyncRemotePlayer = false; 
        }
    }

    #region 发送并同步当前玩家信息
    /// <summary>
    /// 发送玩家控制角色的位置信息 
    /// </summary>
    private void SyncLocalPlayer()
    {
        SendRequest(localPlayerTransform.position.x, localPlayerTransform.position.y, localPlayerTransform.position.z,
            localPlayerTransform.eulerAngles.x, localPlayerTransform.eulerAngles.y, localPlayerTransform.eulerAngles.z,
            localPlayerMove.forward);
    }

    /// <summary>
    /// 获取当前玩家位置信息
    /// </summary>
    /// <param name="localPlayerTransform"></param>
    /// <param name="localPlayerMove"></param>
    public MoveRequest SetLocalPlayer(Transform localPlayerTransform,PlayerMove localPlayerMove)
    {
        this.localPlayerTransform = localPlayerTransform;
        this.localPlayerMove = localPlayerMove;
        return this;
    }

    /// <summary>
    /// 开始发送同步信息
    /// </summary>
    /// <param name="x"> position:X </param>
    /// <param name="y"> position:Y </param>
    /// <param name="z"> position:Z </param>
    /// <param name="rotationX"> rotation:X </param>
    /// <param name="rotationY"> rotation:Y </param>
    /// <param name="rotationZ"> rotation:Z </param>
    /// <param name="forward"> Animator:forward </param>
    private void SendRequest(float x,float y,float z,float rotationX,float rotationY,float rotationZ,float forward)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", x, y, z, rotationX, rotationY, rotationZ, forward);
        base.SendRequest(data);
    }

    #endregion

    /// <summary>
    /// 消息回调
    /// </summary>
    /// <param name="data"></param>
    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        pos = UnityTools.Parse(strs[0]);
        rot = UnityTools.Parse(strs[1]);
        foward = float.Parse(strs[2]);
        isSyncRemotePlayer = true;
    }


    #region 同步其他玩家信息



    /// <summary>
    /// 设置需要同步玩家信息
    /// </summary>
    /// <param name="remotePlayerTransform"></param>
    public MoveRequest SetRemotePlayer(Transform remotePlayerTransform)
    {
        this.remotePlayerTransform = remotePlayerTransform;
        this.remotePlayerAnim = this.remotePlayerTransform.GetComponent<Animator>();
        return this;
    }

    /// <summary>
    /// 异步设置其他玩家信息
    /// </summary>
    private void SyncRemotePlayer()
    {
        remotePlayerTransform.position = pos;
        remotePlayerTransform.eulerAngles = rot;
        remotePlayerAnim.SetFloat("Forward", foward);
    }
    #endregion
}
