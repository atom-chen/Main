using UnityEngine;
using System.Collections;

public class UIAttack : MonoBehaviour {
    public GameObject normalAttack;
    public GameObject rangeAttack;
    public GameObject redAttack;
    public static UIAttack uiAttack;
    void Awake()
    {
        uiAttack = this;
    }

    public void TurnToFrist()
    {
        normalAttack.SetActive(false);
        rangeAttack.SetActive(false);
        redAttack.SetActive(true);
    }
    public void TurnToScend()
    {
        normalAttack.SetActive(true);
        rangeAttack.SetActive(true);
        redAttack.SetActive(false);
    }
}
