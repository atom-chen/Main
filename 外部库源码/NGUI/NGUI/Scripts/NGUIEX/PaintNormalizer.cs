using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games;
using Games.Table;

public class PaintNormalizer : MonoBehaviour
{
    [SerializeField] UITexture mUITexture;
    [SerializeField] int mCharModel = -1;
    [SerializeField] string mPaint;
    [SerializeField] int mNormCfg = -1;

    [Header("每个界面对立绘的定制比例")]
    [SerializeField] float mCustomFactor = 1f;
    [Header("纹理默认大小")]
    [SerializeField] int mOriginWidth = 32;
    [SerializeField] int mOriginHeight = 32;

    void Awake()
    {
        if (mUITexture == null)
        {
            mUITexture = gameObject.GetComponent<UITexture>();
        }
    }

    public void Setup(int charModel)
    {
        Tab_CharModel tab = TableManager.GetCharModelByID(charModel, 0);
        if (tab == null)
        {
            LogModule.ErrorLog("char model " + charModel + " not exists @ PaintNormalizer.Setup");
            return;
        }

        Setup(tab.Painting, tab.PaintNormCfg);
    }

    public void Setup(string tex, int normCfg)
    {
        mPaint = tex;
        mNormCfg = normCfg;

        if (mUITexture == null)
            return;

        mUITexture.mainTexture = Utils.LoadTexture(mPaint);

        if (mNormCfg != -1)
        {
            Tab_PaintNormCfg pn = TableManager.GetPaintNormCfgByID(mNormCfg, 0);
            if (pn == null)
            {
                LogModule.ErrorLog("paint norm cfg " + normCfg + " not exists @ PaintNormalizer.Setup");
                return;
            }

            float factor = mCustomFactor * pn.NormalizedScale;

            mUITexture.width = (int)(pn.Width * factor);
            mUITexture.height = (int)(pn.Height * factor);

            Vector3 pos = Vector3.zero;
            pos.x = pos.x + pn.OffsetX * factor;
            pos.z = pos.y + pn.OffsetY * factor;
            mUITexture.transform.localPosition = pos;
        }
        else
        {
            mUITexture.width = (int)(mOriginWidth*mCustomFactor);
            mUITexture.height = (int)(mOriginHeight*mCustomFactor);
            mUITexture.transform.localPosition = Vector3.zero;
        }
    }

    [ContextMenu("RefreshWithPaintAndNormCfg")]
    void RefreshWithPaintAndNormCfg()
    {
        Setup(mPaint, mNormCfg);
    }

    [ContextMenu("RefreshWithCharModel")]
    void RefreshWithCharModel()
    {
        Setup(mCharModel);
    }
}
