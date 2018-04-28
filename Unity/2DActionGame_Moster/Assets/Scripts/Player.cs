using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float speed=10.0f;

    private Transform m_Transform;
    // Use this for initialization
    void Start()
    {
        m_Transform = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Transform.position =new Vector3(Time.deltaTime * speed + m_Transform.position.x, m_Transform.position.y, m_Transform.position.z);
    }
    //攻击
    public void Atack()
    {

    }
}
