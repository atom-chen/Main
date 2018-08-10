using UnityEngine;


public class UIPlayEffect : MonoBehaviour
{
    public enum LayerType
    {
        UI = 5,
        TopUI = 15,
        TopTopUI = 16,
        HeadInfo = 22,
    }

    public string effName;
    public float duration = -1f;
    public LayerType layer = LayerType.TopUI;
    public float delay = 0.05f;
    public Transform parent;
    public bool autoplay = true;

    private GameObject effGo;

    void OnEnable()
    {
        if (autoplay)
        {
            if (delay > 0.0001f)
            {
                Invoke("Play", delay);
            }
            else
            {
                Play();
            }
        }
    }

    void OnDisable()
    {
        CancelInvoke("Play");
        if (effGo != null)
        {
            Destroy(effGo);
        }
    }

    public void Play()
    {
        if (effGo != null)
        {
            Stop();
        }
        effGo = UIManager.PlayEffect(effName, parent != null ? parent : transform, new Vector3(0f, 0f, 0f), duration, (int)layer);
    }

    public void Stop()
    {
        if (effGo != null)
        {
            UIManager.StopEffect(effGo);
        }
        effGo = null;
    }

    public bool IsPlaying()
    {
        return effGo != null;
    }
    public GameObject GetEffGo
    {
        get { return effGo; }
    }
}