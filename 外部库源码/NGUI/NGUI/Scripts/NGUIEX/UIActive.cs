using System;
using System.Collections;
using UnityEngine;


public class UIActive : MonoBehaviour
{
    public float delay = 0f;
    public float liveTime = 1f;
    public GameObject target;

    void OnEnable()
    {
        StartCoroutine(_Play());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        if (target != null && target.activeSelf)
        {
            target.SetActive(false);
        }
    }

    IEnumerator _Play()
    {
        yield return new WaitForSeconds(delay);
        if (target != null)
        {
            target.SetActive(true);
        }
        if (liveTime > 0f)
        {
            yield return new WaitForSeconds(liveTime);
            if (target != null)
            {
                target.SetActive(false);
            }
        }
    } 
}