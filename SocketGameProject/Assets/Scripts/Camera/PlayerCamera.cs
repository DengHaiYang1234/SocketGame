using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform target; //跟随目标
    private Vector3 offest = new Vector3(0, 28.07f, -25.27f); //相机偏移
    private float smoothing = 2f;
	// Use this for initialization
	void Start ()
    {
		
	}

    private void Update()
    {
        GameObject obj = GameFacade.Instance.GetCurrentRoleObj();
        

        if (obj == null)
        {
            transform.position = new Vector3(15.6f, 45.12f, -29.45f);
            transform.rotation = Quaternion.Euler(43.809f, -25.021f, -1.822f);
            return;
        }
        else
        {
            target = GameFacade.Instance.GetCurrentRoleObj().transform;
            Vector3 targetPosition = target.position + offest; //目标位置
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);//相机位置
            transform.LookAt(target);
        }


    }
}
