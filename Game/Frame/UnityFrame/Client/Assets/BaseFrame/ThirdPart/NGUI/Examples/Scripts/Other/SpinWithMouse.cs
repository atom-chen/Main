using UnityEngine;

[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
	public Transform target;
	public float speed = 1f;

	Transform mTrans;

    bool isHoldOn = false;
    public bool SetHoldOn
    {
        get { return isHoldOn; }
        set { isHoldOn = value; }
    }

    public delegate bool delCanDrag();
    public delCanDrag deleCanDrag = null;

	void Start ()
	{
		mTrans = transform;
	}

	void OnDrag (Vector2 delta)
	{
        if(isHoldOn)
        {
            return;
        }

	    if (deleCanDrag != null)
	    {
	        if (false == deleCanDrag())
	        {
	            return;
	        }
	    }

		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;

		if (target != null)
		{
			target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * target.localRotation;
		}
		else
		{
			mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * mTrans.localRotation;
		}
    }
}