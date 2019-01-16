using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RoleData
{
    public RoleType RoleType { get; private set; }

    public GameObject RolePrefab { get; private set; }

    public GameObject ArrowPrefab { get; private set; }

    public Vector3 SpawnPosition { get; private set; }

    public GameObject Effect { get; private set; }

    public RoleData(RoleType roleType,string rolePath,string arrowPath,string effectPath,Transform spawnPos)
    {
        this.RoleType = roleType;
        this.RolePrefab = Resources.Load(rolePath) as GameObject;
        this.ArrowPrefab = Resources.Load(arrowPath) as GameObject;
        this.Effect = Resources.Load(effectPath) as GameObject;
        ArrowPrefab.GetComponent<Arrow>().effect = this.Effect;
        this.SpawnPosition = spawnPos.position;
    }

}
