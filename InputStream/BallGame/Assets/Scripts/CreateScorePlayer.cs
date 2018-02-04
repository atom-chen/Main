using UnityEngine;
using System.Collections;

public class CreateScorePlayer : MonoBehaviour {
    public GameObject disscorePlane;
    public void OnCollisionStay(Collision collision)
    {
        //如果不处于默认状态
        if(Staticer.STATUS != Staticer.NORMAL)
        {
            return;
        }
        if(collision.gameObject.tag=="ball")
        {
            Instantiate(disscorePlane,collision.gameObject.transform.position,new Quaternion(-0.7f,0,0,0.7f));
            Staticer.STATUS = Staticer.SAVED;
        }
    }
}
