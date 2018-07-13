using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JoyStick : MonoBehaviour
{
    public float MaxRadius = 100;

    public EventDelegate m_func;

    public bool CanXI = true; // 允许x正方向
    public bool CanXD = true; // 允许x负方向
    public bool CanYI = true; // 允许y正方向
    public bool CanYD = true; // 允许y负方向
    
    private TweenPosition m_Tween;
    private Transform m_Trans;
    private Vector3 m_OriginPos;
    private bool m_bPressed = false;

	// Use this for initialization
	void Start () {
        m_Trans = gameObject.transform;
        m_OriginPos = m_Trans.localPosition;
        m_Tween = gameObject.GetComponent<TweenPosition>();
        m_Tween.to = m_OriginPos;
    }

    void OnPress(bool press)
    {
        if (StoryCopyAssistantManager.IsAssistanting()) return;

        if (press)
        {
            PressJoyStick();
        }
        else
        {
            ReleaseJoyStick();
        }
    }

    void OnDrag(Vector2 delta)
    {
        if (StoryCopyAssistantManager.IsAssistanting()) return;

        if (!m_bPressed)
        {
            return;
        }
        if (GameManager.CurScene ==null)
        {
            return;
        }
        if (GameManager.CurScene.UIRoot.GetComponent<UIRoot>() == null)
        {
            return;
        }
        if (m_Trans == null)
        {
            return;
        }

        m_Trans.localPosition += (Vector3)delta * GameManager.CurScene.UIRoot.GetComponent<UIRoot>().pixelSizeAdjustment;

        Vector3 curLocalPos = m_Trans.localPosition;
        if (CanXI == false && curLocalPos.x - m_OriginPos.x > 0) curLocalPos.x = m_OriginPos.x;
        if (CanXD == false && curLocalPos.x - m_OriginPos.x < 0) curLocalPos.x = m_OriginPos.x;
        if (CanYI == false && curLocalPos.y - m_OriginPos.y > 0) curLocalPos.y = m_OriginPos.y;
        if (CanYD == false && curLocalPos.y - m_OriginPos.y < 0) curLocalPos.y = m_OriginPos.y;
        m_Trans.localPosition = curLocalPos;

        float fDis = Vector3.Distance(m_Trans.localPosition, m_OriginPos);
        if (fDis > MaxRadius)
        {
            float deltaX = 0;
            float deltaY = 0;

            if (m_Trans.localPosition.x == m_OriginPos.x)
            {
                deltaX = 0;
            }
            else if (m_Trans.localPosition.x - m_OriginPos.x != 0)
            {
                float k = (m_Trans.localPosition.y - m_OriginPos.y) / (m_Trans.localPosition.x - m_OriginPos.x);

                deltaX = Mathf.Sqrt(Mathf.Pow(MaxRadius, 2) / (Mathf.Pow(k, 2) + 1));
                deltaX *= m_Trans.localPosition.x > m_OriginPos.x ? 1 : -1;
            }

            deltaY = Mathf.Sqrt(Mathf.Pow(MaxRadius, 2) - Mathf.Pow(deltaX, 2));
            deltaY *= m_Trans.localPosition.y > m_OriginPos.y ? 1 : -1;

            m_Trans.localPosition = m_OriginPos + new Vector3(deltaX, deltaY, 0);
        }

        if (m_func != null)
        {
            float fHor;
            if (m_Trans.localPosition.x == m_OriginPos.x)
            {
                fHor = 0;
            }
            else
            {
                fHor = (float)Mathf.Abs(m_Trans.localPosition.x - m_OriginPos.x) / (float)(m_Trans.localPosition.x - m_OriginPos.x);
            }

            float fVer;
            if (m_Trans.localPosition.x == m_OriginPos.x)
            {
                if (m_Trans.localPosition.y > m_OriginPos.y)
                {
                    fVer = (m_Trans.localPosition.y - m_OriginPos.y) / MaxRadius;//1;
                }
                else if (m_Trans.localPosition.y == m_OriginPos.y)
                {
                    fVer = 0;
                }
                else
                {
                    fVer = (m_Trans.localPosition.y - m_OriginPos.y) / MaxRadius;//-1;
                }
            }
            else
            {
                fVer = (float)(m_Trans.localPosition.y - m_OriginPos.y) / (float)Mathf.Abs(m_Trans.localPosition.x - m_OriginPos.x);
            }

            m_func.parameters[0] = new EventDelegate.Parameter(fHor);
            m_func.parameters[1] = new EventDelegate.Parameter(fVer);
            m_func.Execute();
        }
    }

    void PressJoyStick()
    {
        if (m_Trans == null || m_Tween == null)
        {
            return;
        }

        if (!m_bPressed)
        {
            m_bPressed = true;
            LockPinch(m_bPressed);

            m_Tween.from = m_Trans.localPosition;
            m_Tween.ResetToBeginning();
        }
    }

    public void ReleaseJoyStick()
    {
        if (m_Trans == null || m_Tween == null)
        {
            return;
        }

        if (m_bPressed)
        {
            m_bPressed = false;
            LockPinch(m_bPressed);

            m_Tween.from = m_Trans.localPosition;
            m_Tween.method = UITweener.Method.EaseOut;
            m_Tween.PlayForward();

            if (m_func != null)
            {
                m_func.parameters[0] = new EventDelegate.Parameter(0);
                m_func.parameters[1] = new EventDelegate.Parameter(0);
                m_func.Execute();
            }
        }
    }

    public void ResetJoyStick()
    {
        if (m_Trans == null)
        {
            return;
        }

        m_bPressed = false;
        LockPinch(m_bPressed);
        m_Trans.localPosition = m_OriginPos;
        m_Tween.from = m_Trans.localPosition;
        m_Tween.ResetToBeginning();
    }

    void LockPinch(bool locked)
    {
        if (GameManager.CameraManager != null)
        {
            GameManager.CameraManager.IsPinchLock = locked;
        }
    }
}
