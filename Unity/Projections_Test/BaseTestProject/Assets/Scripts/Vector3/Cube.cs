using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Vector3 m_Dir;

    private Vector3 m_InitPos;
    private Transform m_Target;
    void Start()
    {

        m_Target = GameObject.Find("Sphere").transform;
        m_InitPos = transform.position;
    }

    void Update()
    {
        m_Dir = (m_Target.position - transform.position).normalized;//获取移动方向

        LookAt();
    }
    /// <summary>
    /// 直接对pos+=
    /// </summary>
    void PosAdd()
    {
        if (Vector3.Distance(transform.position, m_Target.position) > 0.01)
        {
            //PosAdd();//
            transform.position += m_Dir * Time.deltaTime;//向指定方向移动，速度为1
        }
        else
        {
            transform.position = m_Target.position;
        }
    }

    /// <summary>
    /// 通过线性插值的方式进行位移
    /// </summary>
    void Lerp()
    {
        if (Vector3.Distance(transform.position, m_Target.position) > 0.01)
        {
            //PosAdd();//
            transform.position = Vector3.Lerp(transform.position, m_Target.position, Time.deltaTime);//每次会对剩余的距离进行插值
        }
        else
        {
            transform.position = m_Target.position;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void LookAt()
    {
        Vector3 view = Vector3.Cross(transform.forward, m_Dir);//由前方转向射线方向
        float angle = Vector3.Angle(transform.forward, m_Dir);//自身前方和需要朝向方向的夹角
        if (view.y < 0)
        {
            angle = -angle;//小于0说明在左边
        }
        if (Mathf.Abs(angle) >= 5)
        {
            transform.Rotate(new Vector3(0, angle, 0));
        }
        transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);//匀速移动到目标位置
    }
}
