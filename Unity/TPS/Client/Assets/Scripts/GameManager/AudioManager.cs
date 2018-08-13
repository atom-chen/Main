using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager _Instance;
    public static AudioManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    private bool m_bIsCfxMute = false; //是否静音
    private float m_Audio=1.0f;

    private Dictionary<string, AudioClip> m_AudioMap = new Dictionary<string, AudioClip>();
    void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        AudioClip gun = Resources.Load<AudioClip>("Sounds/gun");
        if(gun!=null)
        {
            m_AudioMap.Add("gun", gun);
        }
    }

    public void PlaySfx(AudioClip audio,Vector3 pos,float volume=1)
    {
        if(m_bIsCfxMute)
        {
            return;
        }
        GameObject obj = new GameObject("Sfx");
        obj.transform.position = pos;
        AudioSource source=obj.AddComponent<AudioSource>();
        source.clip = audio;
        source.minDistance = 10.0f;
        source.maxDistance = 30.0f;
        source.volume = m_Audio * volume;
        source.Play();

        Destroy(obj,audio.length);
    }

    public void PlayGun(Vector3 pos, float volume = 1)
    {
        AudioClip gun;
        if(m_AudioMap.TryGetValue("gun",out gun))
        {
            PlaySfx(gun, pos,volume);
        }
    }
}
