using System;
using UnityEngine;

public class TweenCounter : UITweener
{
    public UILabel label;
    public float from;
    public float to;
    public bool integer = true;

    private float m_value;
    public float value
    {
        get
        {
            return m_value;
        }
        private set
        {
            m_value = value;
            OnValueUpdate();
        }
    }

    void OnEnable()
    {
        if (label == null)
        {
            label = GetComponent<UILabel>();
        }
    }

    public delegate string StrFormatter(float time);
    public StrFormatter formatter;

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = Mathf.Lerp(from, to, factor);
    }

    public override void SetStartToCurrentValue() { from = value; }
    public override void SetEndToCurrentValue() { to = value; }

    private void OnValueUpdate()
    {
        if (label != null)
        {
            if (formatter != null)
            {
                label.text = formatter(value);
            }
            else
            {
                if (integer)
                {
                    label.text = ((int)value).ToString();
                }
                else
                {
                    label.text = value.ToString();
                }
            }
        }
    }

    static public TweenCounter Begin(GameObject go, float duration, float from,float to)
    {
        if (go == null)
        {
            return null;
        }

        TweenCounter comp = UITweener.Begin<TweenCounter>(go, duration);
        comp.from = from;
        comp.to = to;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
}
