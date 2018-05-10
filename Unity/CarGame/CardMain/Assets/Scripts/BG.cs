using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BG : MonoBehaviour {
  private AudioSource m_Audio;
  private int m_PreIndex = 0;
  private static BG _Instance;
  void Awake()
  {
    _Instance = this;
  }
  public static BG Instance
  {
    get
    {
      return _Instance;
    }
  }

	void Start () {
    DontDestroyOnLoad(this.gameObject);
    m_Audio = this.GetComponentInChildren<AudioSource>();
	}

  public void OnSwitchScene(int index)
  {
    if(index==2)
    {
      m_Audio.Stop();
      AudioClip music = Resources.Load<AudioClip>("Music/2");
      m_Audio.clip = music;
      StartCoroutine(PlaySound());
    }
    else if ((index == 1|| index==0) && m_PreIndex == 2)
    {
      m_Audio.Stop();
      AudioClip music = Resources.Load<AudioClip>("Music/BG");
      m_Audio.clip = music;
      StartCoroutine(PlaySound());
    }
    
    m_PreIndex = index;
  }

  IEnumerator PlaySound()
  {
    yield return new WaitForSeconds(1.0f);
    m_Audio.Play();
  }
}
