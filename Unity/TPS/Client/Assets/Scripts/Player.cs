using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Transform m_PlayerTramsform;

    float m_Vertical;
    float m_Horizontal;
    float m_MouseX;
    float m_MoveSpeed=5.0f;
    float m_RotateSpeed = 100.0f;
    void Start()
    {
        m_PlayerTramsform = this.transform;
    }
    void Update()
    {
        m_Vertical=Input.GetAxis("Vertical");
        m_Horizontal = Input.GetAxis("Horizontal");
        m_MouseX=Input.GetAxis("Mouse X");
        TransLate();
        Rotate();
    }


    void TransLate()
    {
        if(m_Vertical!=0 || m_Horizontal!=0)
        {
            Vector3 moveDir = (Vector3.forward * m_Vertical + Vector3.right * m_Horizontal).normalized;
            m_PlayerTramsform.Translate(moveDir * m_MoveSpeed * Time.deltaTime, Space.Self);
        }
    }

    void Rotate()
    {
        if(m_MouseX!=0)
        {
            m_PlayerTramsform.Rotate(Time.deltaTime * m_RotateSpeed * Vector3.up*m_MouseX, Space.Self);
        }
    }
}
