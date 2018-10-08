using System;
using System.Collections;
using System.Collections.Generic;
using Games.GlobeDefine;
using Games.Table;
using UnityEngine;

public class HouseCardSetEffectUpdater : MonoBehaviour
{
    [SerializeField] private float mTimeGap = GlobeVar.YARD_INTIMACY_EFFECT_TIME_GAP;
    [SerializeField] private float mEffectDist = GlobeVar.YARD_INTIMACY_DIST;

    private Dictionary<UInt64, HouseScene.SceneCardInfo> mCards;
    private float mLastTime;

    public void Setup(Dictionary<UInt64, HouseScene.SceneCardInfo> cards)
    {
        mCards = cards;
        mLastTime = 0f;
    }

    // Update is called once per frame
    void Update ()
    {
        if (mCards == null)
            return;

        if (Time.time - mLastTime < mTimeGap)
            return;

        mLastTime = Time.time;

        HashSet<HouseScene.SceneCardInfo> set = new HashSet<HouseScene.SceneCardInfo>();

        foreach (var pc in mCards)
        {
            if (!pc.Value.Valid())
                continue;

            if (set.Contains(pc.Value))
                continue;

            Tab_CardSet tSet = TableManager.GetCardSetByID(pc.Value.card.CardId, 0);
            if (tSet == null)
                continue;

            bool found = false;
            foreach (var pc2 in mCards)
            {
                if (!pc.Value.Valid())
                    continue;

                for (int i = 0; i < tSet.getCardIdCount(); ++i)
                {
                    if (tSet.GetCardIdbyIndex(i) != GlobeVar.INVALID_ID &&
                        tSet.GetCardIdbyIndex(i) == pc2.Value.card.CardId)
                    {
                        var dist = pc.Value.Position - pc2.Value.Position;
                        if (dist.magnitude <= mEffectDist)
                        {
                            set.Add(pc.Value);
                            found = true;
                        }
                    }
                }
            }

            if (found)
                set.Add(pc.Value);
        }

        foreach (var c in set)
        {
            c.objCard.PlayEffect(GlobeVar.YARD_INTIMACY_EFFECT);
        }
    }
}
