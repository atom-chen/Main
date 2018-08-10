using Games;
using UnityEngine;

public class TimerLabel : MonoBehaviour
{
    public UILabel label;

    public float time = 3f;
    [Range(0.1f,3f)]
    public float interval = 1f;

    public bool autoStart = true;
    public bool intervalPlayTween = false;
    public bool intervalPlaySpriteAnim = false;

    public delegate void OnFinish();
    public OnFinish onFinish;

    public delegate void OnInterval();
    public OnInterval onInterval;

    public delegate string StrFormatter(float time);
    public StrFormatter formatter;

    private float timer;
    private float leftTime;
    private bool isRunning = false;
    private TweenGroup tweener;
    private UISpriteAnimation[] anims;

    public float current { get { return leftTime;} }

    void OnEnable()
    {
        if (intervalPlayTween)
        {
            tweener = Utils.TryAddComponent<TweenGroup>(gameObject);
        }
        if (intervalPlaySpriteAnim)
        {
            anims = GetComponentsInChildren<UISpriteAnimation>();
        }
        if (autoStart)
        {
            Restart();
        }
    }

    public void Restart()
    {
        interval = Mathf.Max(0.01f, interval);
        timer = interval;
        leftTime = time;
        isRunning = true;
        Interval();
    }

    void Update()
    {
        if (!isRunning)
        {
            return;
        }

        timer -= RealTime.deltaTime;
        if (timer <= 0f)
        {
            timer = interval;
            leftTime -= interval;
            Interval();
        }
        if (leftTime <= 0f)
        {
            if (onFinish != null)
            {
                onFinish();
            }
            isRunning = false;
        }
    }

    private void Interval()
    {
        if (onInterval != null)
        {
            onInterval();
        }
        if (label != null)
        {
            if (formatter != null)
            {
                label.text = formatter(leftTime);
            }
            else
            {
                label.text = ((int)leftTime).ToString();
            }
        }
        if (intervalPlayTween && tweener != null)
        {
            tweener.Play();
        }
        if (intervalPlaySpriteAnim)
        {
            foreach (var anim in anims)
            {
                anim.ResetToBeginning();
                anim.Play();
            }
        }
    }

    public void Puase()
    {
        isRunning = false;
    }

    public void Resume()
    {
        isRunning = true;
    }
}
