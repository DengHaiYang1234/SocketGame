using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    public GameObject arrowPreafab;
    private Transform leftHandTrans;
    private PlayerManager playerManger;

    private Vector3 shootDir;

    private void Start()
    {
        anim = GetComponent<Animator>();
        leftHandTrans =
            transform.Find(
                "Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand");
    }

    private void Update()
    {
        //如果当前处于移动状态。那么可以攻击
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded")) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray =  Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit);
                if (isCollider)
                {
                    Vector3 targetPoint = hit.point; //获取鼠标点击的位置
                    targetPoint.y = this.transform.position.y;
                    shootDir = targetPoint - transform.position; //面向的方向
                    transform.rotation = Quaternion.LookRotation(shootDir);

                    anim.SetTrigger("Attack"); //播放Attack动画
                    Invoke("Shoot", 0.5f); //延迟执行
                }
            }
        }
    }

    public void SetPlayerManager(PlayerManager playerManger)
    {
        this.playerManger = playerManger;
    }
     
    private void Shoot()
    {
        playerManger.Shoot(arrowPreafab, leftHandTrans.position, Quaternion.LookRotation(shootDir));
        

    }

}
