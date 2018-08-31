using UnityEngine;
using Games;
using Games.Table;
using Games.GlobeDefine;

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

    [Space]
    [Space]
    public bool shadow = false;
    public Vector3 shadowOffset = new Vector3(-4.9f, -14.23f, 0f);
    public Vector3 shadowScale = new Vector3(1.05f, 1.05f, 1.05f);
    public Color shadowColor = new Color(0.152f, 0.152f, 0.152f, 0.623f);
    [Space]

    private UITexture shadowTex;

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

    //剧情回忆界面专用设置函数
    public void SetupStoryMemory(string painting, int normcfg)
    {
        if (normcfg <= GlobeVar.INVALID_ID)
        {
            return;
        }

        mPaint = painting;
        mNormCfg = normcfg;

        if (mUITexture == null)
            return;

        mUITexture.mainTexture = Utils.LoadTexture(mPaint);
        //float scale = mCustomFactor;
        if (mNormCfg != GlobeVar.INVALID_ID)
        {
            Tab_PaintNormCfg pn = TableManager.GetPaintNormCfgByID(mNormCfg, 0);
            if (pn == null)
            {
                LogModule.ErrorLog("paint norm cfg " + mNormCfg + " not exists @ PaintNormalizer.Setup");
                return;
            }

            float factor = mCustomFactor * pn.NormalizedScale;
            //设置BoardLeft和BoardRight
            Vector4 border = mUITexture.border;
            border.x = pn.DrawCardBorderLeft;
            border.z = pn.DrawCardBorderRight;
            border.y = pn.StoryMemoryBorderBottom;
            border.w = pn.StoryMemoryBorderTop;
            mUITexture.border = border;

            mUITexture.width = (int)(pn.Width * factor);
            mUITexture.height = (int)(pn.Height * factor);

            Vector3 pos = Vector3.zero;
            pos.x = pos.x + pn.OffsetX * factor;
            pos.y = pos.y + pn.OffsetY * factor;
            mUITexture.transform.localPosition = pos;


            //scale = factor;
        }
        else
        {
            mUITexture.width = (int)(mOriginWidth * mCustomFactor);
            mUITexture.height = (int)(mOriginHeight * mCustomFactor);
            mUITexture.transform.localPosition = Vector3.zero;
        }
    }

    //十连抽卡专用设置函数
    //第一个参数是CharModel的ID，第二个参数是卡牌在十连抽中的槽位
    public void SetupDrawCardCombo(Tab_CharModel tCharModel, int nSlot)
    {
        if (null == tCharModel)
        {
            return;
        }
        SetupWithBorder(tCharModel.Painting, tCharModel.PaintNormCfg, nSlot);
    }

    public void SetupWithBorder(string painting, int normcfg, int nSlot)
    {
        mPaint = painting;
        mNormCfg = normcfg;

        if (mUITexture == null)
            return;

        mUITexture.mainTexture = Utils.LoadTexture(mPaint);
        float scale = mCustomFactor;
        if (mNormCfg != GlobeVar.INVALID_ID)
        {
            Tab_PaintNormCfg pn = TableManager.GetPaintNormCfgByID(mNormCfg, 0);
            if (pn == null)
            {
                LogModule.ErrorLog("paint norm cfg " + mNormCfg + " not exists @ PaintNormalizer.Setup");
                return;
            }

            //检查slot索引是否正确
            if (nSlot < 0 || nSlot >= pn.getDrawCardOffsetYCount())
            {
                return;
            }

            float factor = mCustomFactor * pn.DrawCardScale;

            mUITexture.width = pn.Width;
            mUITexture.height = pn.Height;

            Vector3 pos = Vector3.zero;
            pos.x = pos.x + pn.DrawCardOffsetX;
            pos.y = pos.y + pn.GetDrawCardOffsetYbyIndex(nSlot);
            mUITexture.transform.localPosition = pos;

            //设置BoardLeft和BoardRight
            Vector4 border = mUITexture.border;
            border.x = pn.DrawCardBorderLeft;
            border.z = pn.DrawCardBorderRight;
            mUITexture.border = border;

            scale = factor;
            mUITexture.gameObject.transform.localScale = scale * Vector3.one;
        }
        else
        {
            mUITexture.width = (int)(mOriginWidth * mCustomFactor);
            mUITexture.height = (int)(mOriginHeight * mCustomFactor);
            mUITexture.transform.localPosition = Vector3.zero;
        }
    }

    public void Setup(string tex, int normCfg)
    {
        mPaint = tex;
        mNormCfg = normCfg;

        if (mUITexture == null)
            return;

        mUITexture.mainTexture = Utils.LoadTexture(mPaint);
        float scale = mCustomFactor;
        if (mNormCfg != GlobeVar.INVALID_ID)
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
            pos.y = pos.y + pn.OffsetY * factor;
            mUITexture.transform.localPosition = pos;

            scale = factor;
        }
        else
        {
            mUITexture.width = (int)(mOriginWidth * mCustomFactor);
            mUITexture.height = (int)(mOriginHeight * mCustomFactor);
            mUITexture.transform.localPosition = Vector3.zero;
        }

        if (shadow)
        {
            SetupShadow(scale);
        }
    }

    public void SetupShadow(float scale)
    {
        if (shadowTex == null)
        {
            GameObject shadowGo = new GameObject("shadow");
            shadowGo.layer = mUITexture.gameObject.layer;
            shadowTex = shadowGo.AddComponent<UITexture>();
            shadowGo.transform.SetParent(mUITexture.transform);
            shadowTex.color = shadowColor;
        }

        Transform trans = shadowTex.transform;
        trans.localPosition = shadowOffset * scale;
        trans.localScale = shadowScale * scale;
        trans.localRotation = Quaternion.identity;

        shadowTex.mainTexture = mUITexture.mainTexture;
        shadowTex.width = mUITexture.width;
        shadowTex.height = mUITexture.height;
        shadowTex.depth = mUITexture.depth - 1;
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

    public UITexture UIMainTexture { get { return mUITexture; } }
}
