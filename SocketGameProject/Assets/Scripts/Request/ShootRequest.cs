using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ShootRequest : BaseRequest
{
    public PlayerManager playerManager;
    private bool isShoot = false;

    private RoleType roleType;
    private Vector3 pos;
    private Vector3 rotation;

    private void Update()
    {
        if (isShoot)
        {
            playerManager.RemoteShoot(roleType, pos, rotation);
            isShoot = false;
        }
    }

    public override void Awake()
    {
        actionCode = ActionCode.Shoot;
        requestCode = RequestCode.Game;
        base.Awake();
    }

    public void SendRequest(RoleType type,Vector3 pos,Vector3 rotation)
    {
        string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}",(int)type,pos.x,pos.y,pos.z,rotation.x,rotation.y,rotation.z);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        RoleType roleType = (RoleType) int.Parse(strs[0]); 
        Vector3 pos = UnityTools.Parse(strs[1]);
        Vector3 rotation = UnityTools.Parse(strs[2]);
        isShoot = true;
        this.roleType = roleType;
        this.pos = pos;
        this.rotation = rotation;
    }

}
