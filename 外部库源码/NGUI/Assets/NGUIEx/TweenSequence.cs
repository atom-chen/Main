using System.Collections;
using UnityEngine;
using UnityToolbag;

public class TweenSequence : MonoBehaviour
{
    [Reorderable]
    public UITweener[] tweens;

    public bool autoPlay = true;
    public float delay = 0f;
    public bool ignoreTimeScale = true;
    public UITweener.Style style = UITweener.Style.Once;

    void Awake()
    {
        foreach (var tween in tweens)
        {
            if (tween == null)
            {
                continue;
            }
            tween.enabled = false;
        }
    }

    void OnEnable()
    {
        if (autoPlay)
        {
            Play(true);
        }
    }

    void OnDisable()
    {
        foreach (var tween in tweens)
        {
            if (tween == null)
            {
                continue;
            }
            tween.enabled = false;
        }
    }

    public void Play(bool forward = true)
    {
        enabled = true;
        StopAllCoroutines();
        StartCoroutine(_Play(forward));
    }

    IEnumerator _Play(bool forward)
    {
        float timer = 0f;
        while (timer < delay)
        {
            if (ignoreTimeScale)
            {
                timer += Time.unscaledDeltaTime;
            }
            else
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }
        if (style == UITweener.Style.Once)
        {
            yield return _PlayDir(forward);
        }
        else if (style == UITweener.Style.Loop)
        {
            while (true)
            {
                yield return _PlayDir(forward);
            }
        }
        else if (style == UITweener.Style.PingPong)
        {
            yield return _PlayDir(forward);
            yield return _PlayDir(!forward);
        }
        enabled = false;
    }

    IEnumerator _PlayDir(bool forward)
    {
        for (int i = 0; i < tweens.Length; i++)
        {
            int index = forward ? i : tweens.Length - i - 1;
            UITweener tween = tweens[index];
            if (tween == null)
            {
                continue;
            }
            tween.Play(forward);
            tween.ResetToBeginning();
            while (tween.enabled)
            {
                yield return null;
            }
        }
    }
}