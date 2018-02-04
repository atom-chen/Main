using UnityEngine;
using System.Collections;

public class PlayAWard : MonoBehaviour {
    public GameObject singleSwordGo;
    public GameObject dualSwordGo;
    public GameObject gunGo;
    //武器存在时间
    public float exitTime = 5;
    private float dualSwordTimer = 0;
    private float gunTimer = 0;

    public void SetAward(AwardType type)
    {
        if(type==AwardType.DualSword)
        {
            TurnToSword();
        }else if(type==AwardType.Gun)
        {
            TurnToGun();
        }
    }
    void Update()
    {
        if(dualSwordTimer>0)
        {
            dualSwordTimer -= Time.deltaTime;
            if(dualSwordTimer<=0)
            {
                TurnToSingleSword();
            }
        }else if(gunTimer>0)
        {
            gunTimer -= Time.deltaTime;
            if(gunTimer<=0)
            {
                TurnToSingleSword();
            }
        }
    }

    void TurnToSword()
    {
        singleSwordGo.SetActive(false);
        gunGo.SetActive(false);
        dualSwordGo.SetActive(true);
        dualSwordTimer = exitTime;
        gunTimer = 0;
        UIAttack.uiAttack.TurnToScend();
        
    }
    void TurnToGun()
    {
        singleSwordGo.SetActive(false);
        gunGo.SetActive(true);
        dualSwordGo.SetActive(false);
        gunTimer = exitTime;
        dualSwordTimer = 0;
        UIAttack.uiAttack.TurnToFrist();
    }
    void TurnToSingleSword()
    {
        singleSwordGo.SetActive(true);
        gunGo.SetActive(false);
        dualSwordGo.SetActive(false);
        gunTimer = 0;
        dualSwordTimer = 0;
        UIAttack.uiAttack.TurnToScend();
    }
}
