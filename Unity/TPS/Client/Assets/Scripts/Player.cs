﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runRight;
    public AnimationClip runLeft;
}

public class Player : MonoBehaviour {
    private Transform m_PlayerTramsform;//transform组件
    private Animation m_PlayerAnimation;//玩家animation
    public Anim m_Anim;//所有动画


    float m_Vertical;                          //竖直方向输入
    float m_Horizontal;                          //水平方向输入
    float m_MouseX;                              //鼠标水平位置
    float m_MoveSpeed=5.0f;                      //玩家移动速度
    float m_RotateSpeed = 100.0f;                  //玩家旋转速度
    void Start()
    {
        m_PlayerTramsform = this.transform;
        m_PlayerAnimation = GetComponentInChildren<Animation>();
        if(m_PlayerAnimation!=null)
        {
            m_PlayerAnimation.clip = m_Anim.idle;
            m_PlayerAnimation.Play();
        }
    }
    void Update()
    {
        m_Vertical=Input.GetAxis("Vertical");
        m_Horizontal = Input.GetAxis("Horizontal");
        m_MouseX=Input.GetAxis("Mouse X");
        TransLate();
        Rotate();
        PlayAnimation();
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

    void PlayAnimation()
    {
        if (m_Vertical==0 && m_Horizontal==0)
        {
            m_PlayerAnimation.CrossFade(m_Anim.idle.name, 0.3f);
        }
        else if (m_Horizontal > 0)
        {
            m_PlayerAnimation.CrossFade(m_Anim.runRight.name, 0.3f);
        }
        else if (m_Horizontal < 0)
        {
            m_PlayerAnimation.CrossFade(m_Anim.runLeft.name, 0.3f);
        }
        else if (m_Vertical > 0)
        {
            m_PlayerAnimation.CrossFade(m_Anim.runForward.name, 0.3f);
        }
        else if (m_Vertical < 0)
        {
            m_PlayerAnimation.CrossFade(m_Anim.runBackward.name, 0.3f);
        }
    }
}
