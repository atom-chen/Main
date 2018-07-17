using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public delegate void OnClose();
//数字键盘
public class DigitalKeyboard : MonoBehaviour {
    
    public Transform offset;
    public Transform numberRoot;
    private Vector3 leftPosPoint = new Vector3(-320,0,0);
    private Vector3 rightPosPoint = new Vector3(320,0,0);
    private int mCurrValue;
    public static OnClose OnDigitalKeyboardClose;

    public TweenAlpha tweenAlpha;
    public TweenPosition tweenPosition;

    public static DigitalKeyboard Ins;

    public enum KeyboardPosPointEnum
    {
        ENUM_LEFT,//左侧
        ENUM_RIGHT,//右侧
        ENUM_CUSTOM//自定义坐标
    }

    public delegate void DigitalKeyboardSetEvent(int value);
    private static DigitalKeyboardSetEvent mDigitalKeyboardSetEvent = null;

    public delegate void DigitalKeyboardOnSubmit();
    public static DigitalKeyboardOnSubmit mDigitalKeyboardOnSubmit = null;

    private static KeyboardPosPointEnum mPosEnum = KeyboardPosPointEnum.ENUM_LEFT;
    private static Vector3 mCustomPos;
    private static int mMaxValue;
    private static int mDefaultValue;
    
    void Awake()
    {
        Ins = this;
    }

    void Start ()
    {
        int count = numberRoot.childCount;
        for (int i = 0; i < count; ++i)
        {
            UIEventListener.Get(numberRoot.GetChild(i).gameObject).onClick = OnNumberKeyClick;
        }
	}
    private void OnEnable()
    {
        mCurrValue = 0;
        if (mPosEnum == KeyboardPosPointEnum.ENUM_LEFT)
        {
            offset.localPosition = leftPosPoint;
        }
        else if (mPosEnum == KeyboardPosPointEnum.ENUM_RIGHT)
        {
            offset.localPosition = rightPosPoint;
        }
        else
        {
            offset.localPosition = mCustomPos;
        }
    }

    void OnDestroy()
    {
        Ins = null;
        mDigitalKeyboardOnSubmit = null;
    }

    //点击播放恢复动画后关闭
    public void OnClickTweenClose()
    {
        tweenPosition.PlayReverse();
        tweenPosition.AddOnFinished(()=> {
            RunDelegate();
            UIManager.CloseUI(UIInfo.DigitalKeyboard);
        });
        tweenAlpha.PlayReverse();
    }

    /// <summary>
    /// 点击数字按钮      
    /// </summary>
    /// <param name="go"></param>
    void OnNumberKeyClick(GameObject go)
    {
        int value = 0;
        if (int.TryParse(go.name, out value))
        {
            AddNumber(value);
       
        }
    }
    void AddNumber(int value)
    {
        if (mCurrValue >= mMaxValue) return;

        if (mDefaultValue > 0)
        {
            //如果输入框内有默认值 输入时则直接覆盖（不保留）
            mDefaultValue = 0;
        }

        if (value >= 0 && value < 10)
        {
            int cacheValue = value + mCurrValue * 10;
            if (cacheValue > mMaxValue)
            {
                mCurrValue = mMaxValue;
            }
            else
            {
                mCurrValue = cacheValue;
            }
            RunCallback();
        }
    }
    public void OnRemoveNumberClick()
    {
        if (mDefaultValue > 0)
        {
            //如果输入框内有默认值 删除时则保留之前的值
            mCurrValue = mDefaultValue;
            mDefaultValue = 0;
        }

        if (mCurrValue > 0)
        {
            mCurrValue /= 10;
            RunCallback();
        }
        
    }

    void RunCallback()
    {
        if (mDigitalKeyboardSetEvent != null)
        {
            mDigitalKeyboardSetEvent(mCurrValue);
        }
    }

    public void OnCloseBtnClick()
    {
        RunDelegate();
        UIManager.CloseUI(UIInfo.DigitalKeyboard);
    }

    public void OnSubmitClick()
    {
        if (mDigitalKeyboardOnSubmit != null)
        {
            mDigitalKeyboardOnSubmit();
        }
        RunDelegate();
        UIManager.CloseUI(UIInfo.DigitalKeyboard);
    }
    private void RunDelegate()
    {
        try
        {
            System.Delegate[] delegates = OnDigitalKeyboardClose.GetInvocationList(); //返回委托挂接的方法，通过他可以控制委托方法执行顺序
            foreach (OnClose item in delegates)
            {
                item();
            }

        }
        catch (Exception ex)
        {

        }
        finally
        {
            ClearDel();
        }
    }


    void ClearDel()
    {
        try
        {
            System.Delegate[] delegates = OnDigitalKeyboardClose.GetInvocationList(); //返回委托挂接的方法，通过他可以控制委托方法执行顺序
            //执行后清空所有委托
            if (delegates != null)
            {
                for (int i = 0; i < delegates.Length; i++)
                {
                    OnDigitalKeyboardClose -= delegates[i] as OnClose;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public static void Show(int maxValue, KeyboardPosPointEnum posEnum, DigitalKeyboardSetEvent digitalKeyboardSetEvent, int defaultValue = 0,Vector3 customPos = new Vector3())
    {
        UIManager.UILayer layer = UIManager.Instance().GetUILayer(UIInfo.DigitalKeyboard.uiType);
        if (layer != null)
        {
            if (layer.IsUIShow(UIInfo.DigitalKeyboard.path))
            {
                return;
            }
            else
            {
                UIManager.ShowUI(UIInfo.DigitalKeyboard);
                mPosEnum = posEnum;
                mDigitalKeyboardSetEvent = digitalKeyboardSetEvent;
                mMaxValue = maxValue;
                mCustomPos = customPos;
                if (defaultValue >= 0)
                {
                    mDefaultValue = defaultValue;
                }
                else
                {
                    mDefaultValue = 0;
                }
                
            }
        }
    }


}
