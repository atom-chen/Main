using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireController : MonoBehaviour {
    private Transform m_FirePos;
    private const float m_CD = 0.1f;
    private float m_NextFire = 0;
    public AudioClip m_AudioClip;

    private MeshRenderer m_Render;
    private Transform m_FireRenderTrans;

    private int AttackState = 1;

    public delegate void OnFire1(Transform fireTransform);
    public delegate void OnFire2(Transform fireTransform, RaycastHit ray);
    public static event OnFire1 onFire;
    public static event OnFire2 onHitColl;
	void Start () 
    {
        m_FirePos = transform.Find("FirePos");
        m_Render = m_FirePos.GetComponentInChildren<MeshRenderer>();
        m_FireRenderTrans = m_Render.transform;
        m_Render.enabled = false;
	}
	

	void Update () 
    {
        Debug.DrawRay(m_FirePos.position, m_FirePos.forward * 10.0f, Color.red);
        m_NextFire += Time.deltaTime;
        if (Input.GetMouseButton(0) && m_NextFire>=m_CD)
        {
            Attack();
            m_NextFire = 0;
        }
	}

    public void Attack()
    {
        switch(AttackState)
        {
            case 0:
                Attack0();
                break;
            case 1:
                Attack1();
                break;
        }
        if(onFire!=null)
        {
            onFire(m_FirePos);
        }
        AudioManager.Instance.PlayGun(m_FirePos.position);
        StartCoroutine(ShowMuzzFlash());
    }
    void Attack0()
    {
        Transform bullet = BulletPool.Instance.GetBullet();
        bullet.position = m_FirePos.position;
        bullet.rotation = m_FirePos.rotation;
        bullet.gameObject.SetActive(true);
    }

    void Attack1()
    {
        RaycastHit ray;
        if(Physics.Raycast(m_FirePos.position,m_FirePos.forward,out ray,10.0f))
        {
            switch (ray.collider.tag)
            {
                case "MONSTER":
                    Vector3 para = ray.point;
                    ray.collider.gameObject.SendMessage("OnDamage", para, SendMessageOptions.DontRequireReceiver);
                    break;
                case "BARREL":
                    Barrel ba=ray.collider.GetComponent<Barrel>();
                    if(ba!=null)
                    {
                        ba.OnDamage(m_FirePos.position, ray.point);
                    }
                    break;
            }
            if (onHitColl != null)
            {
                onHitColl(m_FirePos, ray);
            }
        }

    }

    IEnumerator ShowMuzzFlash()
    {
        //随机大小
        float scale = Random.Range(1.0f, 2.0f);
        m_FireRenderTrans.localScale = Vector3.one * scale;

        //随机旋转
        Quaternion rotate = Quaternion.Euler(0, 0, Random.Range(0, 360));
        m_FireRenderTrans.rotation = rotate;

        m_Render.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.05f,0.25f));
        m_Render.enabled = false;
    }
}
