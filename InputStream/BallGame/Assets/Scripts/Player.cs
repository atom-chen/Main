using UnityEngine;
using System.Collections;
/*
 * 守门员AI
 */

public class Player : MonoBehaviour
{
    private BoxCollider playerBox;
    private Animation animation;

    // Use this for initialization
    void Start()
    {
        playerBox = this.GetComponent<BoxCollider>();
        animation = this.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Staticer.animationName == "down")
        {
            playerBox.size = new Vector3(0.1f, 0.1f, 0.1f);
        }
        else
        {
            playerBox.size = new Vector3(0.2f, 0.1f, 0.1f);
        }
        //若是可以播放动画
        if (Staticer.isPlay)
        {
            animation.CrossFade(Staticer.animationName);
            Staticer.isPlay = false;
        }
        if (Staticer.isSave && Staticer.animationName != null && Staticer.animationName != "down")
        {
            //拿到守门员位置
            Vector3 po = this.transform.position;
            //拿到球的落点
            Vector3 aimpo = Staticer.endPoint;

            if (animation[Staticer.animationName].normalizedTime <= 0.7)
            {
                //防止间隔过小
                if (Time.deltaTime != 0)
                {
                    //移动接球
                    this.transform.position = new Vector3(Mathf.Lerp(po.x, aimpo.x, 5 * Time.deltaTime), Mathf.Lerp(po.y, aimpo.y, 5 * Time.deltaTime), po.z);
                }
            }

        }

    }
}
