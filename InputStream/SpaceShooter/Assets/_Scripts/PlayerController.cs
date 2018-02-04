using UnityEngine;
using System.Collections;
[System.Serializable]
public class Boundary{
    public float xMin, xMax, zMin, zMax;
}
public class PlayerController : MonoBehaviour {
    //移动速度
    public float speed = 5;
    //边界类的对象
    public Boundary boundary;
    //翻转幅度
    public float tilt = 4;
    //攻击CD 单位s
    public const float fireRate = 0.35f;
    public GameObject shot;
    public Transform shotSpawn;
    //攻击计时器
    private float nextFire = 0;
    Rigidbody rb;
    void FixedUpdate()
    {
        //得到水平，竖直方向输入
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //计算刚体速度向量
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        //设置速度 
        if(rb!=null)
        {
            rb.velocity = movement * speed;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x,boundary.xMin,boundary.xMax),0,Mathf.Clamp(rb.position.z
                ,boundary.zMin,boundary.zMax));
            rb.rotation=Quaternion.Euler(0,0,rb.velocity.x*-tilt);
        }
    }
	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        nextFire+=Time.deltaTime;
        if(Input.GetButton("Fire1")&& nextFire>=fireRate)
        {
            //发射
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            nextFire = 0;
            //播放音效
            GetComponent<AudioSource>().Play();
        }
        
    }
}

