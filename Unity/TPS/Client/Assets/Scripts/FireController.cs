using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireController : MonoBehaviour {
    private Transform m_FirePos;
    private const float m_CD = 0.25f;
    private float m_NextFire = 0;
    private AudioSource m_FireAudio;
    public AudioClip m_AudioClip;

    private MeshRenderer m_Render;
    private Transform m_FireRenderTrans;

	void Start () 
    {
        m_FirePos = transform.Find("FirePos");
        m_FireAudio = this.GetComponent<AudioSource>();
        m_Render = m_FirePos.GetComponentInChildren<MeshRenderer>();
        m_FireRenderTrans = m_Render.transform;
        m_Render.enabled = false;
	}
	

	void Update () 
    {
        m_NextFire += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && m_NextFire>=m_CD)
        {
            Attack();
            m_NextFire = 0;
        }
	}

    public void Attack()
    {
        Transform bullet = BulletPool.Instance.GetBullet();
        bullet.position = m_FirePos.position;
        bullet.rotation = m_FirePos.rotation;
        bullet.gameObject.SetActive(true);
        m_FireAudio.PlayOneShot(m_AudioClip, 0.9f);
        StartCoroutine(ShowMuzzFlash());
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
