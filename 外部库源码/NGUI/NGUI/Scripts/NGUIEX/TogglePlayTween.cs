using System.Collections;
using Games;
using UnityEngine;

public class TogglePlayTween : MonoBehaviour
{    
    public UITweener tween;
    public int loopTimes = 1;
    [Range(0f,10f)]
    public float intervalTime = 0f;
    public float loopIntervalTime = 0f;
    public bool autoPlay = false;
    public bool forwardAtFinal = false;

    void OnEnable()
    {
        if (autoPlay)
        {
            Play();
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
        enabled = false;
        tween.enabled = false;
    }

    public void Play()
    {
        if (tween == null)
        {
            return;
        }
        enabled = true;
        StopAllCoroutines();
        StartCoroutine(_Play());
    }

    public void Stop(bool reset = false)
    {
        if (reset)
        {
            tween.PlayForward();
            tween.ResetToBeginning();
        }
        enabled = false;
    }

    IEnumerator _Play()
    {
        for (int i = 0; i < loopTimes; i++)
        {
            tween.PlayForward();
            while (tween.enabled)
            {
                yield return null;
            }
            
            float timer = intervalTime;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            tween.Toggle();
            while (tween.enabled)
            {
                yield return null;
            }
            timer = loopIntervalTime;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        if (forwardAtFinal)
        {
            tween.PlayForward();
            while (tween.enabled)
            {
                yield return null;
            }
        }
        enabled = false;
    }

    public static TogglePlayTween Blink(UITweener tween,int times = 1,float intervalTime = 0f,bool forwardAtFinal = false)
    {
        TogglePlayTween player = Utils.TryAddComponent<TogglePlayTween>(tween.gameObject);
        if (player == null)
        {
            return null;
        }
        player.tween = tween;
        player.loopTimes = times;
        player.intervalTime = intervalTime;
        player.forwardAtFinal = forwardAtFinal;
        player.Play();
        return player;
    }
}