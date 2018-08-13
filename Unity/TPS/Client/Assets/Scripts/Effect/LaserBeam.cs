﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private Transform tr;
    private LineRenderer m_LineRender;
    private RaycastHit m_Hit;

    void Start()
    {
        tr = transform;
        m_LineRender = this.GetComponent<LineRenderer>();
        m_LineRender.useWorldSpace = true;
        m_LineRender.enabled = false;
        m_LineRender.startWidth = 0.3f;
        m_LineRender.endWidth = 0.01f;
        FireController.onFire += OnFire;
        FireController.onHitColl += OnHit;
    }
    void OnDestroy()
    {
        FireController.onFire -= OnFire;
        FireController.onHitColl -= OnHit;
    }


    void OnFire(Transform firPos)
    {
        m_LineRender.SetPosition(0, firPos.position);
        m_LineRender.SetPosition(1, firPos.InverseTransformPoint(new Ray(firPos.position, firPos.forward).GetPoint(100.0f)));
        StartCoroutine(ShowLaserBeam());
    }

    void OnHit(Transform firPos,RaycastHit ray)
    {
        m_LineRender.SetPosition(1, firPos.InverseTransformPoint(ray.point));
    }

    IEnumerator ShowLaserBeam()
    {
        m_LineRender.enabled = true;
        yield return new WaitForSeconds(1.5f);
        m_LineRender.enabled = false;
    }
}
