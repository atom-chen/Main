using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.GlobeDefine;

public class HouseTargetEffectUpdater : MonoBehaviour
{
    private HouseScene.SceneCardInfo[] mList;
    private HouseScene.SceneCardInfo mTarget;

    private float mTime;
    private int mCurEffect;
    
    private Vector3 mLastValidPos;
    public Vector3 LastValidPos {get {return mLastValidPos; }}

    public void Setup(HouseScene.SceneCardInfo card, HouseScene.SceneCardInfo[] list)
    {
        if (card == null || list == null)
            return;
        mList = list;
        mTarget = card;
        mTime = 0;
        mCurEffect = GlobeVar.INVALID_ID;
        UpdateLastValidPos();
    }

    void UpdateLastValidPos()
    {
        if (mTarget != null && mTarget.Valid())
        {
            mLastValidPos = mTarget.objCard.Position;
        }
    }

    void Update ()
    {
        if (mTarget == null || !mTarget.Valid())
            return;

        if (Time.time - mTime > 0.2f)
        {
            mTime = Time.time;

            int newEffect = GlobeVar.YARD_MOVE_EFFECT_GREEN;
            foreach (var card in mList)
            {
                if (card == mTarget)
                    continue;
                if (card == null || !card.Valid() || mTarget == null || !mTarget.Valid())
                    continue;
                var distvec = card.Position - mTarget.Position;
                if (distvec.magnitude < card.YardDistRadius + mTarget.YardDistRadius)
                {
                    newEffect = GlobeVar.YARD_MOVE_EFFECT_RED;
                    break;
                }
            }

            // update the lastest valid position
            if (newEffect == GlobeVar.YARD_MOVE_EFFECT_GREEN)
            {
                UpdateLastValidPos();
            }

            // switch effect
            if (mCurEffect != newEffect)
            {
                if (mCurEffect != GlobeVar.INVALID_ID)
                {
                    mTarget.objCard.StopEffect(mCurEffect);
                }
                mCurEffect = newEffect;
                if (mCurEffect != GlobeVar.INVALID_ID)
                {
                    mTarget.objCard.PlayEffect(mCurEffect);
                }
            }
        }
    }

    void OnDestroy()
    {
        if (mCurEffect != GlobeVar.INVALID_ID && mTarget!= null && mTarget.objCard != null)
        {
            mTarget.objCard.StopEffect(mCurEffect);
        }
    }
}
