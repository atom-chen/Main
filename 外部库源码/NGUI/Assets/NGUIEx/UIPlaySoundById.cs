using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class UIPlaySoundById : MonoBehaviour
{
    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        Custom,
        OnEnable,
        OnDisable,
    }

    public Trigger trigger = Trigger.OnClick;

    public int id;

    bool mIsOver = false;

    bool canPlay
    {
        get
        {
            if (!enabled) return false;
            UIButton btn = GetComponent<UIButton>();
            return (btn == null || btn.isEnabled);
        }
    }

    void OnEnable()
    {
        if (trigger == Trigger.OnEnable)
        {
            Play();
        }
    }

    void OnDisable()
    {
        if (trigger == Trigger.OnDisable)
            Play();
    }

    void OnHover(bool isOver)
    {
        if (trigger == Trigger.OnMouseOver)
        {
            if (mIsOver == isOver) return;
            mIsOver = isOver;
        }

        if (canPlay && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
            Play();
    }

    void OnPress(bool isPressed)
    {
        if (trigger == Trigger.OnPress)
        {
            if (mIsOver == isPressed) return;
            mIsOver = isPressed;
        }

        if (canPlay && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
            Play();
    }

    void OnClick()
    {
        if (canPlay && trigger == Trigger.OnClick)
            Play();
    }

    void OnSelect(bool isSelected)
    {
        if (canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
            OnHover(isSelected);
    }

    public void Play()
    {
        if (id != -1)
        {
            GameManager.SoundManager.PlaySoundEffect(id);
        }
    }
}
