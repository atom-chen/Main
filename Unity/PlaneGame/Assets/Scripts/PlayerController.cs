using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 玩家脚本
 */
class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {


    public Transform m_SpotSpawn;

    private Boundary m_Boundary;//移动边界
    private Rigidbody m_PlayerRigidbody;
    private float m_MoveSpeed = 5;//移动速度
    private float m_Tilt=4;//倾斜系数

    private Vector3 m_InitPos = new Vector3(0, 0, 1);
    void OnEnable()
    {
        this.transform.position = m_InitPos;
    }

	void Start () {

        m_PlayerRigidbody = GetComponent<Rigidbody>();

        m_Boundary = new Boundary();
        m_Boundary.xMin = -6;
        m_Boundary.xMax = 6;
        m_Boundary.zMin = 1;
        m_Boundary.zMax = 9;
	}

	void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);//移动速度
        if(m_PlayerRigidbody!=null)
        {
            m_PlayerRigidbody.velocity = movement * m_MoveSpeed;
            m_PlayerRigidbody.position = new Vector3(Mathf.Clamp(m_PlayerRigidbody.position.x, m_Boundary.xMin, m_Boundary.xMax), 0
, Mathf.Clamp(m_PlayerRigidbody.position.z, m_Boundary.zMin, m_Boundary.zMax));
            m_PlayerRigidbody.rotation=Quaternion.Euler(0,0,m_PlayerRigidbody.velocity.x*-m_Tilt);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.TAG_ENEMY)
        {
            GameManager.Instance.KillPlayer();
            
        }
    }


}
