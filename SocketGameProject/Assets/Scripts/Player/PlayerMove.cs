using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float forward = 0;

    private float speed = 3;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //若当前是处在Attack状态。那么就停止移动
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded") == false)
        {
            anim.SetFloat("Forward", 0);
            return;
        }


        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
        {
            //移动   世界坐标中移动
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.World);
            //转向
            transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));
            //动画
            float res = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
            forward = res;
            anim.SetFloat("Forward", res); //动画移动速度
        }

    }

}
