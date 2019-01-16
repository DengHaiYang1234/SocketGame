using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Arrow : MonoBehaviour
{
    private float speed = 15;
    private Rigidbody rig;
    public RoleType roleType;

    public GameObject effect;

    public bool isLocal = false;
    
	// Use this for initialization
	void Start ()
	{
	    rig = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update ()
	{
        //局部坐标
	    rig.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameFacade.Instance.PlayNoramSound(AudioMAnager.Sound_ShootPerson,1,false);
            if (isLocal)
            {
                bool playerIsLocal = other.GetComponent<PlayerInfo>().isLocal;
                if (isLocal != playerIsLocal)
                {
                    GameFacade.Instance.SendAttack(Random.Range(10, 20));
                }
            }
        }
        else
        {
            GameFacade.Instance.PlayNoramSound(AudioMAnager.Sound_Miss, 1, false);
        }
        GameObject.Instantiate(effect, transform.position, transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}
