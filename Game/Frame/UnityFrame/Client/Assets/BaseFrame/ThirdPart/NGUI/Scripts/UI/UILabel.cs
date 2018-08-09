//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using Alignment = NGUIText.Alignment;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Label")]
public class UILabel : UIWidget
{
    public enum Effect
    {
        None,
        Shadow,
        Outline,
        Outline8,
    }

    public enum Overflow
    {
        ShrinkContent,
        ClampContent,
        ResizeFreely,
        ResizeHeight,
    }

    public enum Crispness
    {
        Never,
        OnDesktop,
        Always,
    }

    /// <summary>
    /// Whether the label will keep its content crisp even when shrunk.
    /// You may want to turn this off on mobile devices.
    /// </summary>

    public Crispness keepCrispWhenShrunk = Crispness.Never;

    [HideInInspector]
    [SerializeField]
    Font mTrueTypeFont;
    [HideInInspector]
    [SerializeField]
    UIFont mFont;
#if !UNITY_3_5
    [MultilineAttribute(6)]
#endif
    [HideInInspector]
    [SerializeField]
    string mText = "";
    [HideInInspector]
    [SerializeField]
    int mFontSize = 16;
    [HideInInspector]
    [SerializeField]
    FontStyle mFontStyle = FontStyle.Normal;
    [HideInInspector]
    [SerializeField]
    Alignment mAlignment = Alignment.Automatic;
    [HideInInspector]
    [SerializeField]
    bool mEncoding = true;
    [HideInInspector]
    [SerializeField]
    int mMaxLineCount = 0; // 0 denotes unlimited
    [HideInInspector]
    [SerializeField]
    Effect mEffectStyle = Effect.None;
    [HideInInspector]
    [SerializeField]
    Color mEffectColor = Color.black;
    [HideInInspector]
    [SerializeField]
    NGUIText.SymbolStyle mSymbols = NGUIText.SymbolStyle.Normal;
    [HideInInspector]
    [SerializeField]
    Vector2 mEffectDistance = Vector2.one;
    [HideInInspector]
    [SerializeField]
    Overflow mOverflow = Overflow.ShrinkContent;
    [HideInInspector]
    [SerializeField]
    Material mMaterial;
    [HideInInspector]
    [SerializeField]
    bool mApplyGradient = false;
    [HideInInspector]
    [SerializeField]
    Color mGradientTop = Color.white;
    [HideInInspector]
    [SerializeField]
    Color mGradientBottom = new Color(0.7f, 0.7f, 0.7f);
    [HideInInspector]
    [SerializeField]
    int mSpacingX = 0;
    [HideInInspector]
    [SerializeField]
    int mSpacingY = 0;
    [HideInInspector]
    [SerializeField]
    bool mUseFloatSpacing = false;
    [HideInInspector]
    [SerializeField]
    float mFloatSpacingX = 0;
    [HideInInspector]
    [SerializeField]
    float mFloatSpacingY = 0;
    [HideInInspector]
    [SerializeField]
    bool mOverflowEllipsis = false;

    // Obsolete values
    [HideInInspector]
    [SerializeField]
    bool mShrinkToFit = false;
    [HideInInspector]
    [SerializeField]
    int mMaxLineWidth = 0;
    [HideInInspector]
    [SerializeField]
    int mMaxLineHeight = 0;
    [HideInInspector]
    [SerializeField]
    float mLineWidth = 0;
    [HideInInspector]
    [SerializeField]
    bool mMultiline = true;
    [HideInInspector]
    [SerializeField]
    bool mUseDicTable = false;
    [HideInInspector]
    [SerializeField]
    int mDicID = -1;
    [HideInInspector]
    [SerializeField]
    private bool mSupportDictSymbol = false;
    [HideInInspector]
    [SerializeField]
    bool mUseSymbol = false;
    [HideInInspector]
    [SerializeField]
    UIAtlas[] mSymbolAtlas = null;
    [HideInInspector]
    [SerializeField]
    bool mPraseLink = false;
    [HideInInspector]
    [SerializeField]
    GameObject mLinkItem = null;
    [HideInInspector]
    [SerializeField]
    bool mForceColor = true;
    [HideInInspector]
    [SerializeField]
    private bool m_bFixPos = false;

    [System.NonSerialized]
    Font mActiveTTF = null;
    [System.NonSerialized]
    float mDensity = 1f;
    [System.NonSerialized]
    bool mShouldBeProcessed = true;
    [System.NonSerialized]
    string mProcessedText = null;
    [System.NonSerialized]
    bool mPremultiply = false;
    [System.NonSerialized]
    Vector2 mCalculatedSize = Vector2.zero;
    [System.NonSerialized]
    float mScale = 1f;
    [System.NonSerialized]
    int mFinalFontSize = 0;
    [System.NonSerialized]
    int mLastWidth = 0;
    [System.NonSerialized]
    int mLastHeight = 0;

    public bool UseDictionary()
    {
        return mUseDicTable;
    }

    public int CurDicID()
    {
        return mDicID;
    }
    /// <summary>
    /// Font size after modifications got taken into consideration such as shrinking content.
    /// </summary>

    public int finalFontSize
    {
        get
        {
            if (trueTypeFont) return Mathf.RoundToInt(mScale * mFinalFontSize);
            return Mathf.RoundToInt(mFinalFontSize * mScale);
        }
    }

    /// <summary>
    /// Function used to determine if something has changed (and thus the geometry must be rebuilt)
    /// </summary>

    bool shouldBeProcessed
    {
        get
        {
            return mShouldBeProcessed;
        }
        set
        {
            if (value)
            {
                mChanged = true;
                mShouldBeProcessed = true;
            }
            else
            {
                mShouldBeProcessed = false;
            }
        }
    }

    /// <summary>
    /// Whether the rectangle is anchored horizontally.
    /// </summary>

    public override bool isAnchoredHorizontally { get { return base.isAnchoredHorizontally || mOverflow == Overflow.ResizeFreely; } }

    /// <summary>
    /// Whether the rectangle is anchored vertically.
    /// </summary>

    public override bool isAnchoredVertically
    {
        get
        {
            return base.isAnchoredVertically ||
                mOverflow == Overflow.ResizeFreely ||
                mOverflow == Overflow.ResizeHeight;
        }
    }

    /// <summary>
    /// Retrieve the material used by the font.
    /// </summary>

    public override Material material
    {
        get
        {
            if (mMaterial != null) return mMaterial;
            if (mFont != null) return mFont.material;
            if (mTrueTypeFont != null) return mTrueTypeFont.material;
            return null;
        }
        set
        {
            if (mMaterial != value)
            {
                RemoveFromPanel();
                mMaterial = value;
                MarkAsChanged();
            }
        }
    }

    [Obsolete("Use UILabel.bitmapFont instead")]
    public UIFont font { get { return bitmapFont; } set { bitmapFont = value; } }

    /// <summary>
    /// Set the font used by this label.
    /// </summary>

    public UIFont bitmapFont
    {
        get
        {
            return mFont;
        }
        set
        {
            if (mFont != value)
            {
                RemoveFromPanel();
                mFont = value;
                mTrueTypeFont = null;
                MarkAsChanged();
            }
        }
    }

    /// <summary>
    /// Set the font used by this label.
    /// </summary>

    public Font trueTypeFont
    {
        get
        {
            if (mTrueTypeFont != null) return mTrueTypeFont;
            return (mFont != null ? mFont.dynamicFont : null);
        }
        set
        {
            if (mTrueTypeFont != value)
            {
                SetActiveFont(null);
                RemoveFromPanel();
                mTrueTypeFont = value;
                shouldBeProcessed = true;
                mFont = null;
                SetActiveFont(value);
                ProcessAndRequest();
                if (mActiveTTF != null)
                    base.MarkAsChanged();
            }
        }
    }

    /// <summary>
    /// Ambiguous helper function.
    /// </summary>

    public UnityEngine.Object ambigiousFont
    {
        get
        {
            return (UnityEngine.Object)mFont ?? (UnityEngine.Object)mTrueTypeFont;
        }
        set
        {
            UIFont bf = value as UIFont;
            if (bf != null) bitmapFont = bf;
            else trueTypeFont = value as Font;
        }
    }


    /// <summary>
    /// Text that's being displayed by the label.
    /// </summary>

    public string text
    {
        get
        {
            return mText;
        }
        set
        {
            if (mText == value) return;
            mText = Regex.Replace(mText, @"\p{Cs}", "");//屏蔽系统表情
            ClearSymbols();

            if (string.IsNullOrEmpty(value))
            {
                if (!string.IsNullOrEmpty(mText))
                {
                    mText = "";
                    MarkAsChanged();
                    ProcessAndRequest();
                }
            }
            else if (mText != value)
            {
                if (mForceColor)
                {
                    mText = Games.Utils.ParseForceColor(value);
                }
                else
                {
                    mText = value;
                }

                if (mSupportDictSymbol)
                {
                    mText = mText.Replace("#r", "\n");
                }

                MarkAsChanged();
                ProcessAndRequest();
            }

            if (autoResizeBoxCollider) ResizeCollider();
            PraseSymbolAndLink();
        }
    }

    public void ClearSymbols()
    {
        if (mSymbolList == null) return;
        foreach (var v in mSymbolList)
        {
            if (v == null) continue;
            Destroy(v.gameObject);
        }

        mSymbolList.Clear();
    }

    /// <summary>
    /// Default font size.
    /// </summary>

    public int defaultFontSize { get { return (trueTypeFont != null) ? mFontSize : (mFont != null ? mFont.defaultSize : 16); } }

    /// <summary>
    /// Active font size used by the label.
    /// </summary>

    public int fontSize
    {
        get
        {
            return mFontSize;
        }
        set
        {
            value = Mathf.Clamp(value, 0, 256);

            if (mFontSize != value)
            {
                mFontSize = value;
                shouldBeProcessed = true;
                ProcessAndRequest();
            }
        }
    }

    /// <summary>
    /// Dynamic font style used by the label.
    /// </summary>

    public FontStyle fontStyle
    {
        get
        {
            return mFontStyle;
        }
        set
        {
            if (mFontStyle != value)
            {
                mFontStyle = value;
                shouldBeProcessed = true;
                ProcessAndRequest();
            }
        }
    }

    /// <summary>
    /// Text alignment option.
    /// </summary>

    public Alignment alignment
    {
        get
        {
            return mAlignment;
        }
        set
        {
            if (mAlignment != value)
            {
                mAlignment = value;
                shouldBeProcessed = true;

                ProcessAndRequest();

                if (mUseSymbol
                    && null != mSymbolAtlas
                    && null != mSymbolList
                    && 0 < mSymbolList.Count)
                {
                    UpdateNGUIText();
                    PraseSymbolAndLink();
                }

            }
        }
    }

    /// <summary>
    /// Whether a gradient will be applied.
    /// </summary>

    public bool applyGradient
    {
        get
        {
            return mApplyGradient;
        }
        set
        {
            if (mApplyGradient != value)
            {
                mApplyGradient = value;
                MarkAsChanged();
            }
        }
    }

    /// <summary>
    /// Top gradient color.
    /// </summary>

    public Color gradientTop
    {
        get
        {
            return mGradientTop;
        }
        set
        {
            if (mGradientTop != value)
            {
                mGradientTop = value;
                if (mApplyGradient) MarkAsChanged();
            }
        }
    }

    /// <summary>
    /// Bottom gradient color.
    /// </summary>

    public Color gradientBottom
    {
        get
        {
            return mGradientBottom;
        }
        set
        {
            if (mGradientBottom != value)
            {
                mGradientBottom = value;
                if (mApplyGradient) MarkAsChanged();
            }
        }
    }

    /// <summary>
    /// Additional horizontal spacing between characters when printing text.
    /// </summary>

    public int spacingX
    {
        get
        {
            return mSpacingX;
        }
        set
        {
            if (mSpacingX != value)
            {
                mSpacingX = value;
                MarkAsChanged();
            }
        }
    }

    /// <summary>
    /// Additional vertical spacing between lines when printing text.
    /// </summary>

    public int spacingY
    {
        get
        {
            return mSpacingY;
        }
        set
        {
            if (mSpacingY != value)
            {
                mSpacingY = value;
                MarkAsChanged();
            }
        }
    }

    /// <summary>
    /// Whether this label will use float text spacing values, instead of integers.
    /// </summary>

    public bool useFloatSpacing
    {
        get
        {
            return mUseFloatSpacing;
        }
        set
        {
            if (mUseFloatSpacing != value)
            {
                mUseFloatSpacing = value;
                shouldBeProcessed = true;
            }
        }
    }

    /// <summary>
    /// Additional horizontal spacing between characters when printing text.
    /// For this to have any effect useFloatSpacing must be true.
    /// </summary>

    public float floatSpacingX
    {
        get
        {
            return mFloatSpacingX;
        }
        set
        {
            if (!Mathf.Approximately(mFloatSpacingX, value))
            {
                mFloatSpacingX = value;
                MarkAsChanged();
            }
        }
    }

    /// <summary>
    /// Additional vertical spacing between lines when printing text.
    /// For this to have any effect useFloatSpacing must be true.
    /// </summary>

    public float floatSpacingY
    {
        get
        {
            return mFloatSpacingY;
        }
        set
        {
            if (!Mathf.Approximately(mFloatSpacingY, value))
            {
                mFloatSpacingY = value;
                MarkAsChanged();
            }
        }
    }

    /// <summary>
    /// Convenience property to get the used y spacing.
    /// </summary>

    public float effectiveSpacingY
    {
        get
        {
            return mUseFloatSpacing ? mFloatSpacingY : mSpacingY;
        }
    }

    /// <summary>
    /// Convenience property to get the used x spacing.
    /// </summary>

    public float effectiveSpacingX
    {
        get
        {
            return mUseFloatSpacing ? mFloatSpacingX : mSpacingX;
        }
    }

    /// <summary>
    /// Whether to append "..." at the end of clamped text that didn't fit.
    /// </summary>

    public bool overflowEllipsis
    {
        get
        {
            return mOverflowEllipsis;
        }
        set
        {
            if (mOverflowEllipsis != value)
            {
                mOverflowEllipsis = value;
                MarkAsChanged();
            }
        }
    }

    /// <summary>
    /// Whether the label will use the printed size instead of font size when printing the label.
    /// It's a dynamic font feature that will ensure that the text is crisp when shrunk.
    /// </summary>

    bool keepCrisp
    {
        get
        {
            if (trueTypeFont != null && keepCrispWhenShrunk != Crispness.Never)
            {
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_WP_8_1 || UNITY_BLACKBERRY
                return (keepCrispWhenShrunk == Crispness.Always);
#else
				return true;
#endif
            }
            return false;
        }
    }

    /// <summary>
    /// Whether this label will support color encoding in the format of [RRGGBB] and new line in the form of a "\\n" string.
    /// </summary>

    public bool supportEncoding
    {
        get
        {
            return mEncoding;
        }
        set
        {
            if (mEncoding != value)
            {
                mEncoding = value;
                shouldBeProcessed = true;
            }
        }
    }

    /// <summary>
    /// Style used for symbols.
    /// </summary>

    public NGUIText.SymbolStyle symbolStyle
    {
        get
        {
            return mSymbols;
        }
        set
        {
            if (mSymbols != value)
            {
                mSymbols = value;
                shouldBeProcessed = true;
            }
        }
    }

    /// <summary>
    /// Overflow method controls the label's behaviour when its content doesn't fit the bounds.
    /// </summary>

    public Overflow overflowMethod
    {
        get
        {
            return mOverflow;
        }
        set
        {
            if (mOverflow != value)
            {
                mOverflow = value;
                shouldBeProcessed = true;
            }
        }
    }

    /// <summary>
    /// Maximum width of the label in pixels.
    /// </summary>

    [System.Obsolete("Use 'width' instead")]
    public int lineWidth
    {
        get { return width; }
        set { width = value; }
    }

    /// <summary>
    /// Maximum height of the label in pixels.
    /// </summary>

    [System.Obsolete("Use 'height' instead")]
    public int lineHeight
    {
        get { return height; }
        set { height = value; }
    }

    /// <summary>
    /// Whether the label supports multiple lines.
    /// </summary>

    public bool multiLine
    {
        get
        {
            return mMaxLineCount != 1;
        }
        set
        {
            if ((mMaxLineCount != 1) != value)
            {
                mMaxLineCount = (value ? 0 : 1);
                shouldBeProcessed = true;
            }
        }
    }

    /// <summary>
    /// Process the label's text before returning its corners.
    /// </summary>

    public override Vector3[] localCorners
    {
        get
        {
            if (shouldBeProcessed) ProcessText();
            return base.localCorners;
        }
    }

    /// <summary>
    /// Process the label's text before returning its corners.
    /// </summary>

    public override Vector3[] worldCorners
    {
        get
        {
            if (shouldBeProcessed) ProcessText();
            return base.worldCorners;
        }
    }

    /// <summary>
    /// Process the label's text before returning its drawing dimensions.
    /// </summary>

    public override Vector4 drawingDimensions
    {
        get
        {
            if (shouldBeProcessed) ProcessText();
            return base.drawingDimensions;
        }
    }

    /// <summary>
    /// The max number of lines to be displayed for the label
    /// </summary>

    public int maxLineCount
    {
        get
        {
            return mMaxLineCount;
        }
        set
        {
            if (mMaxLineCount != value)
            {
                mMaxLineCount = Mathf.Max(value, 0);
                shouldBeProcessed = true;
                if (overflowMethod == Overflow.ShrinkContent) MakePixelPerfect();
            }
        }
    }

    /// <summary>
    /// What effect is used by the label.
    /// </summary>

    public Effect effectStyle
    {
        get
        {
            return mEffectStyle;
        }
        set
        {
            if (mEffectStyle != value)
            {
                mEffectStyle = value;
                shouldBeProcessed = true;
            }
        }
    }

    /// <summary>
    /// Color used by the effect, if it's enabled.
    /// </summary>

    public Color effectColor
    {
        get
        {
            return mEffectColor;
        }
        set
        {
            if (mEffectColor != value)
            {
                mEffectColor = value;
                if (mEffectStyle != Effect.None) shouldBeProcessed = true;
            }
        }
    }

    /// <summary>
    /// Effect distance in pixels.
    /// </summary>

    public Vector2 effectDistance
    {
        get
        {
            return mEffectDistance;
        }
        set
        {
            if (mEffectDistance != value)
            {
                mEffectDistance = value;
                shouldBeProcessed = true;
            }
        }
    }

    /// <summary>
    /// Whether the label will automatically shrink its size in order to fit the maximum line width.
    /// </summary>

    [System.Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
    public bool shrinkToFit
    {
        get
        {
            return mOverflow == Overflow.ShrinkContent;
        }
        set
        {
            if (value)
            {
                overflowMethod = Overflow.ShrinkContent;
            }
        }
    }

    /// <summary>
    /// Returns the processed version of 'text', with new line characters, line wrapping, etc.
    /// </summary>

    public string processedText
    {
        get
        {
            if (mLastWidth != mWidth || mLastHeight != mHeight)
            {
                mLastWidth = mWidth;
                mLastHeight = mHeight;
                mShouldBeProcessed = true;
            }

            // Process the text if necessary
            if (shouldBeProcessed) ProcessText();
            return mProcessedText;
        }
    }

    /// <summary>
    /// Actual printed size of the text, in pixels.
    /// </summary>

    public Vector2 printedSize
    {
        get
        {
            if (shouldBeProcessed) ProcessText();
            return mCalculatedSize;
        }
    }

    /// <summary>
    /// Local size of the widget, in pixels.
    /// </summary>

    public override Vector2 localSize
    {
        get
        {
            if (shouldBeProcessed) ProcessText();
            return base.localSize;
        }
    }

    /// <summary>
    /// Whether the label has a valid font.
    /// </summary>

    bool isValid { get { return mFont != null || mTrueTypeFont != null; } }

    static BetterList<UILabel> mList = new BetterList<UILabel>();
    static Dictionary<Font, int> mFontUsage = new Dictionary<Font, int>();

    // 居中显示时缓存文字中的表情
    List<UISprite> mSymbolList = new List<UISprite>();


    /// <summary>
    /// Register the font texture change listener.
    /// </summary>

    protected override void OnInit()
    {
        base.OnInit();
        mList.Add(this);
        SetActiveFont(trueTypeFont);

    }

    /// <summary>
    /// Remove the font texture change listener.
    /// </summary>

    protected override void OnDisable()
    {
        SetActiveFont(null);
        mList.Remove(this);
        base.OnDisable();
    }

    /// <summary>
    /// Set the active font, correctly setting and clearing callbacks.
    /// </summary>

    protected void SetActiveFont(Font fnt)
    {
        if (mActiveTTF != fnt)
        {
            Font font = mActiveTTF;

            if (font != null)
            {
                int usage;

                if (mFontUsage.TryGetValue(font, out usage))
                {
                    usage = Mathf.Max(0, --usage);

                    if (usage == 0)
                    {
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
						font.textureRebuildCallback = null;
#endif
                        mFontUsage.Remove(font);
                    }
                    else mFontUsage[font] = usage;
                }
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
				else font.textureRebuildCallback = null;
#endif
            }

            mActiveTTF = fnt;
            font = fnt;

            if (font != null)
            {
                int usage = 0;

                // Font hasn't been used yet? Register a change delegate callback
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
				if (!mFontUsage.TryGetValue(font, out usage))
					font.textureRebuildCallback = delegate() { OnFontChanged(font); };
#endif
#if UNITY_FLASH
				mFontUsage[font] = usage + 1;
#else
                mFontUsage[font] = ++usage;
#endif
            }
        }
    }

    /// <summary>
    /// Notification called when the Unity's font's texture gets rebuilt.
    /// Unity's font has a nice tendency to simply discard other characters when the texture's dimensions change.
    /// By requesting them inside the notification callback, we immediately force them back in.
    /// Originally I was subscribing each label to the font individually, but as it turned out
    /// mono's delegate system causes an insane amount of memory allocations when += or -= to a delegate.
    /// So... queue yet another work-around.
    /// </summary>

    static void OnFontChanged(Font font)
    {
        for (int i = 0; i < mList.size; ++i)
        {
            UILabel lbl = mList[i];

            if (lbl != null)
            {
                Font fnt = lbl.trueTypeFont;

                if (fnt == font)
                {
                    fnt.RequestCharactersInTexture(lbl.mText, lbl.mFinalFontSize, lbl.mFontStyle);
                    lbl.MarkAsChanged();

                    if (lbl.panel == null)
                        lbl.CreatePanel();

                    if (mTempDrawcalls == null)
                        mTempDrawcalls = new List<UIDrawCall>();

                    if (lbl.drawCall != null && !mTempDrawcalls.Contains(lbl.drawCall))
                        mTempDrawcalls.Add(lbl.drawCall);
                }
            }
        }

        if (mTempDrawcalls != null)
        {
            for (int i = 0, imax = mTempDrawcalls.Count; i < imax; ++i)
            {
                UIDrawCall dc = mTempDrawcalls[i];
                if (dc.panel != null) dc.panel.FillDrawCall(dc);
            }
            mTempDrawcalls.Clear();
        }
    }

    static List<UIDrawCall> mTempDrawcalls;

    /// <summary>
    /// Get the sides of the rectangle relative to the specified transform.
    /// The order is left, top, right, bottom.
    /// </summary>

    public override Vector3[] GetSides(Transform relativeTo)
    {
        if (shouldBeProcessed) ProcessText();
        return base.GetSides(relativeTo);
    }

    /// <summary>
    /// Upgrading labels is a bit different.
    /// </summary>

    protected override void UpgradeFrom265()
    {
        ProcessText(true, true);

        if (mShrinkToFit)
        {
            overflowMethod = Overflow.ShrinkContent;
            mMaxLineCount = 0;
        }

        if (mMaxLineWidth != 0)
        {
            width = mMaxLineWidth;
            overflowMethod = mMaxLineCount > 0 ? Overflow.ResizeHeight : Overflow.ShrinkContent;
        }
        else overflowMethod = Overflow.ResizeFreely;

        if (mMaxLineHeight != 0)
            height = mMaxLineHeight;

        if (mFont != null)
        {
            int min = mFont.defaultSize;
            if (height < min) height = min;
            fontSize = min;
        }

        mMaxLineWidth = 0;
        mMaxLineHeight = 0;
        mShrinkToFit = false;

        NGUITools.UpdateWidgetCollider(gameObject, true);
    }

    /// <summary>
    /// If the label is anchored it should not auto-resize.
    /// </summary>

    protected override void OnAnchor()
    {
        if (mOverflow == Overflow.ResizeFreely)
        {
            if (isFullyAnchored)
                mOverflow = Overflow.ShrinkContent;
        }
        else if (mOverflow == Overflow.ResizeHeight)
        {
            if (topAnchor.target != null && bottomAnchor.target != null)
            {
                if (GetComponent<UIDragScrollView>() == null)
                {
                    mOverflow = Overflow.ShrinkContent;
                }
            }
        }
        base.OnAnchor();
    }

    /// <summary>
    /// Request the needed characters in the texture.
    /// </summary>

    void ProcessAndRequest()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && !NGUITools.GetActive(this)) return;
        if (!mAllowProcessing) return;
#endif
        if (ambigiousFont != null) ProcessText();
    }

#if UNITY_EDITOR
    // Used to ensure that we don't process font more than once inside OnValidate function below
    [System.NonSerialized]
    bool mAllowProcessing = true;
    [System.NonSerialized]
    bool mUsingTTF = true;

    /// <summary>
    /// Validate the properties.
    /// </summary>

    protected override void OnValidate()
    {
        base.OnValidate();

        if (NGUITools.GetActive(this))
        {
            Font ttf = mTrueTypeFont;
            UIFont fnt = mFont;

            // If the true type font was not used before, but now it is, clear the font reference
            if (!mUsingTTF && ttf != null) fnt = null;
            else if (mUsingTTF && fnt != null) ttf = null;

            mFont = null;
            mTrueTypeFont = null;
            mAllowProcessing = false;
            SetActiveFont(null);

            if (fnt != null)
            {
                bitmapFont = fnt;
                mUsingTTF = false;
            }
            else if (ttf != null)
            {
                trueTypeFont = ttf;
                mUsingTTF = true;
            }

            shouldBeProcessed = true;
            mAllowProcessing = true;
            ProcessAndRequest();
            if (autoResizeBoxCollider) ResizeCollider();
        }
    }
#endif

#if !UNITY_4_3 && !UNITY_4_5 && !UNITY_4_6
    static bool mTexRebuildAdded = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!mTexRebuildAdded)
        {
            mTexRebuildAdded = true;
            Font.textureRebuilt += OnFontChanged;
        }
    }
#endif

    /// <summary>
    /// Determine start-up values.
    /// </summary>

    protected override void OnStart()
    {
        base.OnStart();

        if (mUseDicTable && mDicID >= 0)
        {
            text = StrDictionary.GetDicByID(mDicID);
        }

        // Legacy support
        if (mLineWidth > 0f)
        {
            mMaxLineWidth = Mathf.RoundToInt(mLineWidth);
            mLineWidth = 0f;
        }

        if (!mMultiline)
        {
            mMaxLineCount = 1;
            mMultiline = true;
        }

        // Whether this is a premultiplied alpha shader
        mPremultiply = (material != null && material.shader != null && material.shader.name.Contains("Premultiplied"));

        // Request the text within the font
        ProcessAndRequest();
    }

    /// <summary>
    /// UILabel needs additional processing when something changes.
    /// </summary>

    public override void MarkAsChanged()
    {
        shouldBeProcessed = true;
        base.MarkAsChanged();
    }

    /// <summary>
    /// Process the raw text, called when something changes.
    /// </summary>

    public void ProcessText() { ProcessText(false, true); }

    /// <summary>
    /// Process the raw text, called when something changes.
    /// </summary>

    void ProcessText(bool legacyMode, bool full)
    {
        if (!isValid) return;

        mChanged = true;
        shouldBeProcessed = false;

        float regionX = mDrawRegion.z - mDrawRegion.x;
        float regionY = mDrawRegion.w - mDrawRegion.y;

        NGUIText.rectWidth = legacyMode ? (mMaxLineWidth != 0 ? mMaxLineWidth : 1000000) : width;
        NGUIText.rectHeight = legacyMode ? (mMaxLineHeight != 0 ? mMaxLineHeight : 1000000) : height;
        NGUIText.regionWidth = (regionX != 1f) ? Mathf.RoundToInt(NGUIText.rectWidth * regionX) : NGUIText.rectWidth;
        NGUIText.regionHeight = (regionY != 1f) ? Mathf.RoundToInt(NGUIText.rectHeight * regionY) : NGUIText.rectHeight;

        mFinalFontSize = Mathf.Abs(legacyMode ? Mathf.RoundToInt(cachedTransform.localScale.x) : defaultFontSize);
        mScale = 1f;

        if (NGUIText.regionWidth < 1 || NGUIText.regionHeight < 0)
        {
            mProcessedText = "";
            return;
        }

        bool isDynamic = (trueTypeFont != null);

        if (isDynamic && keepCrisp)
        {
            UIRoot rt = root;
            if (rt != null) mDensity = (rt != null) ? rt.pixelSizeAdjustment : 1f;
        }
        else mDensity = 1f;

        if (full) UpdateNGUIText();

        if (mOverflow == Overflow.ResizeFreely)
        {
            NGUIText.rectWidth = 1000000;
            NGUIText.regionWidth = 1000000;
        }

        if (mOverflow == Overflow.ResizeFreely || mOverflow == Overflow.ResizeHeight)
        {
            NGUIText.rectHeight = 1000000;
            NGUIText.regionHeight = 1000000;
        }

        if (mFinalFontSize > 0)
        {
            bool adjustSize = keepCrisp;

            for (int ps = mFinalFontSize; ps > 0; --ps)
            {
                // Adjust either the size, or the scale
                if (adjustSize)
                {
                    mFinalFontSize = ps;
                    NGUIText.fontSize = mFinalFontSize;
                }
                else
                {
                    mScale = (float)ps / mFinalFontSize;
                    NGUIText.fontScale = isDynamic ? mScale : ((float)mFontSize / mFont.defaultSize) * mScale;
                }

                NGUIText.Update(false);

                // Wrap the text
                bool fits = NGUIText.WrapText(mText, out mProcessedText, true, false,
                    mOverflowEllipsis && mOverflow == Overflow.ClampContent);

                if (mOverflow == Overflow.ShrinkContent && !fits)
                {
                    if (--ps > 1) continue;
                    else break;
                }
                else if (mOverflow == Overflow.ResizeFreely)
                {
                    mCalculatedSize = NGUIText.CalculatePrintedSize(mProcessedText);

                    mWidth = Mathf.Max(minWidth, Mathf.RoundToInt(mCalculatedSize.x));
                    if (regionX != 1f) mWidth = Mathf.RoundToInt(mWidth / regionX);
                    mHeight = Mathf.Max(minHeight, Mathf.RoundToInt(mCalculatedSize.y));
                    if (regionY != 1f) mHeight = Mathf.RoundToInt(mHeight / regionY);

                    if ((mWidth & 1) == 1) ++mWidth;
                    if ((mHeight & 1) == 1) ++mHeight;
                }
                else if (mOverflow == Overflow.ResizeHeight)
                {
                    mCalculatedSize = NGUIText.CalculatePrintedSize(mProcessedText);
                    mHeight = Mathf.Max(minHeight, Mathf.RoundToInt(mCalculatedSize.y));
                    if (regionY != 1f) mHeight = Mathf.RoundToInt(mHeight / regionY);
                    if ((mHeight & 1) == 1) ++mHeight;
                }
                else
                {
                    mCalculatedSize = NGUIText.CalculatePrintedSize(mProcessedText);
                }

                // Upgrade to the new system
                if (legacyMode)
                {
                    width = Mathf.RoundToInt(mCalculatedSize.x);
                    height = Mathf.RoundToInt(mCalculatedSize.y);
                    cachedTransform.localScale = Vector3.one;
                }
                break;
            }
        }
        else
        {
            cachedTransform.localScale = Vector3.one;
            mProcessedText = "";
            mScale = 1f;
        }

        if (full)
        {
            NGUIText.bitmapFont = null;
            NGUIText.dynamicFont = null;
        }
    }

    /// <summary>
    /// Text is pixel-perfect when its scale matches the size.
    /// </summary>

    public override void MakePixelPerfect()
    {
        if (ambigiousFont != null)
        {
            Vector3 pos = cachedTransform.localPosition;
            pos.x = Mathf.RoundToInt(pos.x);
            pos.y = Mathf.RoundToInt(pos.y);
            pos.z = Mathf.RoundToInt(pos.z);

            cachedTransform.localPosition = pos;
            cachedTransform.localScale = Vector3.one;

            if (mOverflow == Overflow.ResizeFreely)
            {
                AssumeNaturalSize();
            }
            else
            {
                int w = width;
                int h = height;

                Overflow over = mOverflow;
                if (over != Overflow.ResizeHeight) mWidth = 100000;
                mHeight = 100000;

                mOverflow = Overflow.ShrinkContent;
                ProcessText(false, true);
                mOverflow = over;

                int minX = Mathf.RoundToInt(mCalculatedSize.x);
                int minY = Mathf.RoundToInt(mCalculatedSize.y);

                minX = Mathf.Max(minX, base.minWidth);
                minY = Mathf.Max(minY, base.minHeight);

                if ((minX & 1) == 1) ++minX;
                if ((minY & 1) == 1) ++minY;

                mWidth = Mathf.Max(w, minX);
                mHeight = Mathf.Max(h, minY);

                MarkAsChanged();
            }
        }
        else base.MakePixelPerfect();
    }

    /// <summary>
    /// Make the label assume its natural size.
    /// </summary>

    public void AssumeNaturalSize()
    {
        if (ambigiousFont != null)
        {
            mWidth = 100000;
            mHeight = 100000;
            ProcessText(false, true);
            mWidth = Mathf.RoundToInt(mCalculatedSize.x);
            mHeight = Mathf.RoundToInt(mCalculatedSize.y);
            if ((mWidth & 1) == 1) ++mWidth;
            if ((mHeight & 1) == 1) ++mHeight;
            MarkAsChanged();
        }
    }

    [System.Obsolete("Use UILabel.GetCharacterAtPosition instead")]
    public int GetCharacterIndex(Vector3 worldPos) { return GetCharacterIndexAtPosition(worldPos, false); }

    [System.Obsolete("Use UILabel.GetCharacterAtPosition instead")]
    public int GetCharacterIndex(Vector2 localPos) { return GetCharacterIndexAtPosition(localPos, false); }

    static BetterList<Vector3> mTempVerts = new BetterList<Vector3>();
    static BetterList<int> mTempIndices = new BetterList<int>();

    /// <summary>
    /// Return the index of the character at the specified world position.
    /// </summary>

    public int GetCharacterIndexAtPosition(Vector3 worldPos, bool precise)
    {
        Vector2 localPos = cachedTransform.InverseTransformPoint(worldPos);
        return GetCharacterIndexAtPosition(localPos, precise);
    }

    /// <summary>
    /// Return the index of the character at the specified local position.
    /// </summary>

    public int GetCharacterIndexAtPosition(Vector2 localPos, bool precise)
    {
        if (isValid)
        {
            string text = processedText;
            if (string.IsNullOrEmpty(text)) return 0;

            UpdateNGUIText();

            if (precise) NGUIText.PrintExactCharacterPositions(text, mTempVerts, mTempIndices);
            else NGUIText.PrintApproximateCharacterPositions(text, mTempVerts, mTempIndices);

            if (mTempVerts.size > 0)
            {
                ApplyOffset(mTempVerts, 0);
                int retVal = precise ?
                    NGUIText.GetExactCharacterIndex(mTempVerts, mTempIndices, localPos) :
                    NGUIText.GetApproximateCharacterIndex(mTempVerts, mTempIndices, localPos);

                mTempVerts.Clear();
                mTempIndices.Clear();

                NGUIText.bitmapFont = null;
                NGUIText.dynamicFont = null;
                return retVal;
            }

            NGUIText.bitmapFont = null;
            NGUIText.dynamicFont = null;
        }
        return 0;
    }

    /// <summary>
    /// Retrieve the word directly below the specified world-space position.
    /// </summary>

    public string GetWordAtPosition(Vector3 worldPos)
    {
        int index = GetCharacterIndexAtPosition(worldPos, true);
        return GetWordAtCharacterIndex(index);
    }

    /// <summary>
    /// Retrieve the word directly below the specified relative-to-label position.
    /// </summary>

    public string GetWordAtPosition(Vector2 localPos)
    {
        int index = GetCharacterIndexAtPosition(localPos, true);
        return GetWordAtCharacterIndex(index);
    }

    /// <summary>
    /// Retrieve the word right under the specified character index.
    /// </summary>

    public string GetWordAtCharacterIndex(int characterIndex)
    {
        if (characterIndex != -1 && characterIndex < mText.Length)
        {
#if UNITY_FLASH
			int wordStart = LastIndexOfAny(mText, new char[] { ' ', '\n' }, characterIndex) + 1;
			int wordEnd = IndexOfAny(mText, new char[] { ' ', '\n', ',', '.' }, characterIndex);
#else
            int wordStart = mText.LastIndexOfAny(new char[] { ' ', '\n' }, characterIndex) + 1;
            int wordEnd = mText.IndexOfAny(new char[] { ' ', '\n', ',', '.' }, characterIndex);
#endif
            if (wordEnd == -1) wordEnd = mText.Length;

            if (wordStart != wordEnd)
            {
                int len = wordEnd - wordStart;

                if (len > 0)
                {
                    string word = mText.Substring(wordStart, len);
                    return NGUIText.StripSymbols(word);
                }
            }
        }
        return null;
    }

#if UNITY_FLASH
	/// <summary>
	/// Flash is fail IRL: http://www.tasharen.com/forum/index.php?topic=11390.0
	/// </summary>

	int LastIndexOfAny (string input, char[] any, int start)
	{
		if (start >= 0 && input.Length > 0 && any.Length > 0 && start < input.Length)
		{
			for (int w = start; w >= 0; w--)
			{
				for (int r = 0; r < any.Length; r++)
				{
					if (any[r] == input[w])
					{
						return w;
					}
				}
			}
		}
		return -1;
	}

	/// <summary>
	/// Flash is fail IRL: http://www.tasharen.com/forum/index.php?topic=11390.0
	/// </summary>

	int IndexOfAny (string input, char[] any, int start)
	{
		if (start >= 0 && input.Length > 0 && any.Length > 0 && start < input.Length)
		{
			for (int w = start; w < input.Length; w++)
			{
				for (int r = 0; r < any.Length; r++)
				{
					if (any[r] == input[w])
					{
						return w;
					}
				}
			}
		}
		return -1;
	}
#endif

    /// <summary>
    /// Retrieve the URL directly below the specified world-space position.
    /// </summary>

    public string GetUrlAtPosition(Vector3 worldPos) { return GetUrlAtCharacterIndex(GetCharacterIndexAtPosition(worldPos, true)); }

    /// <summary>
    /// Retrieve the URL directly below the specified relative-to-label position.
    /// </summary>

    public string GetUrlAtPosition(Vector2 localPos) { return GetUrlAtCharacterIndex(GetCharacterIndexAtPosition(localPos, true)); }

    /// <summary>
    /// Retrieve the URL right under the specified character index.
    /// </summary>

    public string GetUrlAtCharacterIndex(int characterIndex)
    {
        if (characterIndex != -1 && characterIndex < mText.Length - 6)
        {
            int linkStart;

            // LastIndexOf() fails if the string happens to begin with the expected text
            if (mText[characterIndex] == '[' &&
                mText[characterIndex + 1] == 'u' &&
                mText[characterIndex + 2] == 'r' &&
                mText[characterIndex + 3] == 'l' &&
                mText[characterIndex + 4] == '=')
            {
                linkStart = characterIndex;
            }
            else linkStart = mText.LastIndexOf("[url=", characterIndex);

            if (linkStart == -1) return null;

            linkStart += 5;
            int linkEnd = mText.IndexOf("]", linkStart);
            if (linkEnd == -1) return null;

            int urlEnd = mText.IndexOf("[/url]", linkEnd);
            if (urlEnd == -1 || characterIndex <= urlEnd)
                return mText.Substring(linkStart, linkEnd - linkStart);
        }
        return null;
    }

    /// <summary>
    /// Get the index of the character on the line directly above or below the current index.
    /// </summary>

    public int GetCharacterIndex(int currentIndex, KeyCode key)
    {
        if (isValid)
        {
            string text = processedText;
            if (string.IsNullOrEmpty(text)) return 0;

            int def = defaultFontSize;
            UpdateNGUIText();

            NGUIText.PrintApproximateCharacterPositions(text, mTempVerts, mTempIndices);

            if (mTempVerts.size > 0)
            {
                ApplyOffset(mTempVerts, 0);

                for (int i = 0; i < mTempIndices.size; ++i)
                {
                    if (mTempIndices[i] == currentIndex)
                    {
                        // Determine position on the line above or below this character
                        Vector2 localPos = mTempVerts[i];

                        if (key == KeyCode.UpArrow) localPos.y += def + effectiveSpacingY;
                        else if (key == KeyCode.DownArrow) localPos.y -= def + effectiveSpacingY;
                        else if (key == KeyCode.Home) localPos.x -= 1000f;
                        else if (key == KeyCode.End) localPos.x += 1000f;

                        // Find the closest character to this position
                        int retVal = NGUIText.GetApproximateCharacterIndex(mTempVerts, mTempIndices, localPos);
                        if (retVal == currentIndex) break;

                        mTempVerts.Clear();
                        mTempIndices.Clear();
                        return retVal;
                    }
                }
                mTempVerts.Clear();
                mTempIndices.Clear();
            }

            NGUIText.bitmapFont = null;
            NGUIText.dynamicFont = null;

            // If the selection doesn't move, then we're at the top or bottom-most line
            if (key == KeyCode.UpArrow || key == KeyCode.Home) return 0;
            if (key == KeyCode.DownArrow || key == KeyCode.End) return text.Length;
        }
        return currentIndex;
    }

    /// <summary>
    /// Fill the specified geometry buffer with vertices that would highlight the current selection.
    /// </summary>

    public void PrintOverlay(int start, int end, UIGeometry caret, UIGeometry highlight, Color caretColor, Color highlightColor)
    {
        if (caret != null) caret.Clear();
        if (highlight != null) highlight.Clear();
        if (!isValid) return;

        string text = processedText;
        UpdateNGUIText();

        int startingCaretVerts = caret.verts.size;
        Vector2 center = new Vector2(0.5f, 0.5f);
        float alpha = finalAlpha;

        // If we have a highlight to work with, fill the buffer
        if (highlight != null && start != end)
        {
            int startingVertices = highlight.verts.size;
            NGUIText.PrintCaretAndSelection(text, start, end, caret.verts, highlight.verts);

            if (highlight.verts.size > startingVertices)
            {
                ApplyOffset(highlight.verts, startingVertices);

                Color32 c = new Color(highlightColor.r, highlightColor.g, highlightColor.b, highlightColor.a * alpha);

                for (int i = startingVertices; i < highlight.verts.size; ++i)
                {
                    highlight.uvs.Add(center);
                    highlight.cols.Add(c);
                }
            }
        }
        else NGUIText.PrintCaretAndSelection(text, start, end, caret.verts, null);

        // Fill the caret UVs and colors
        ApplyOffset(caret.verts, startingCaretVerts);
        Color32 cc = new Color(caretColor.r, caretColor.g, caretColor.b, caretColor.a * alpha);

        for (int i = startingCaretVerts; i < caret.verts.size; ++i)
        {
            caret.uvs.Add(center);
            caret.cols.Add(cc);
        }

        NGUIText.bitmapFont = null;
        NGUIText.dynamicFont = null;
    }

    /// <summary>
    /// Draw the label.
    /// </summary>

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        if (!isValid) return;

        int offset = verts.size;
        Color col = color;
        col.a = finalAlpha;

        if (mFont != null && mFont.premultipliedAlphaShader) col = NGUITools.ApplyPMA(col);

        if (QualitySettings.activeColorSpace == ColorSpace.Linear)
        {
            col.r = Mathf.GammaToLinearSpace(col.r);
            col.g = Mathf.GammaToLinearSpace(col.g);
            col.b = Mathf.GammaToLinearSpace(col.b);
            //col.a = Mathf.GammaToLinearSpace(col.a);
        }

        string text = processedText;
        int start = verts.size;

        UpdateNGUIText();

        NGUIText.tint = col;
        NGUIText.Print(text, verts, uvs, cols);
        NGUIText.bitmapFont = null;
        NGUIText.dynamicFont = null;

        // Center the content within the label vertically
        Vector2 pos = ApplyOffset(verts, start);

        // Effects don't work with packed fonts
        if (mFont != null && mFont.packedFontShader) return;

        // Apply an effect if one was requested
        if (effectStyle != Effect.None)
        {
            int end = verts.size;
            pos.x = mEffectDistance.x;
            pos.y = mEffectDistance.y;

            ApplyShadow(verts, uvs, cols, offset, end, pos.x, -pos.y);

            if ((effectStyle == Effect.Outline) || (effectStyle == Effect.Outline8))
            {
                offset = end;
                end = verts.size;

                ApplyShadow(verts, uvs, cols, offset, end, -pos.x, pos.y);

                offset = end;
                end = verts.size;

                ApplyShadow(verts, uvs, cols, offset, end, pos.x, pos.y);

                offset = end;
                end = verts.size;

                ApplyShadow(verts, uvs, cols, offset, end, -pos.x, -pos.y);

                if (effectStyle == Effect.Outline8)
                {
                    offset = end;
                    end = verts.size;

                    ApplyShadow(verts, uvs, cols, offset, end, -pos.x, 0);

                    offset = end;
                    end = verts.size;

                    ApplyShadow(verts, uvs, cols, offset, end, pos.x, 0);

                    offset = end;
                    end = verts.size;

                    ApplyShadow(verts, uvs, cols, offset, end, 0, pos.y);

                    offset = end;
                    end = verts.size;

                    ApplyShadow(verts, uvs, cols, offset, end, 0, -pos.y);
                }
            }
        }

        if (onPostFill != null)
            onPostFill(this, offset, verts, uvs, cols);

        if (m_symbolCallBack != null)
        {
            m_symbolCallBack();
        }
    }

    private System.Action m_symbolCallBack;
    public void SetSymbolOffset(System.Action callback)
    {
        m_symbolCallBack = callback;
    }


    /// <summary>
    /// Align the vertices, making the label positioned correctly based on the pivot.
    /// Returns the offset that was applied.
    /// </summary>

    public Vector2 ApplyOffset(BetterList<Vector3> verts, int start)
    {
        Vector2 po = pivotOffset;

        float fx = Mathf.Lerp(0f, -mWidth, po.x);
        float fy = Mathf.Lerp(mHeight, 0f, po.y) + Mathf.Lerp((mCalculatedSize.y - mHeight), 0f, po.y);

        fx = Mathf.Round(fx);
        fy = Mathf.Round(fy);

#if UNITY_FLASH
		for (int i = start; i < verts.size; ++i)
		{
			Vector3 buff = verts.buffer[i];
			buff.x += fx;
			buff.y += fy;
			verts.buffer[i] = buff;
		}
#else
        for (int i = start; i < verts.size; ++i)
        {
            verts.buffer[i].x += fx;
            verts.buffer[i].y += fy;
        }
#endif
        return new Vector2(fx, fy);
    }

    /// <summary>
    /// Apply a shadow effect to the buffer.
    /// </summary>

    public void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
    {
        Color c = mEffectColor;
        c.a *= finalAlpha;
        Color32 col = (bitmapFont != null && bitmapFont.premultipliedAlphaShader) ? NGUITools.ApplyPMA(c) : c;

        for (int i = start; i < end; ++i)
        {
            verts.Add(verts.buffer[i]);
            uvs.Add(uvs.buffer[i]);
            cols.Add(cols.buffer[i]);

            Vector3 v = verts.buffer[i];
            v.x += x;
            v.y += y;
            verts.buffer[i] = v;

            Color32 uc = cols.buffer[i];

            if (uc.a == 255)
            {
                cols.buffer[i] = col;
            }
            else
            {
                Color fc = c;
                fc.a = (uc.a / 255f * c.a);
                cols.buffer[i] = (bitmapFont != null && bitmapFont.premultipliedAlphaShader) ? NGUITools.ApplyPMA(fc) : fc;
            }
        }
    }

    /// <summary>
    /// Calculate the character index offset necessary in order to print the end of the specified text.
    /// </summary>

    public int CalculateOffsetToFit(string text)
    {
        UpdateNGUIText();
        NGUIText.encoding = false;
        NGUIText.symbolStyle = NGUIText.SymbolStyle.None;
        int offset = NGUIText.CalculateOffsetToFit(text);
        NGUIText.bitmapFont = null;
        NGUIText.dynamicFont = null;
        return offset;
    }

    /// <summary>
    /// Convenience function, in case you wanted to associate progress bar, slider or scroll bar's
    /// OnValueChanged function in inspector with a label.
    /// </summary>

    public void SetCurrentProgress()
    {
        if (UIProgressBar.current != null)
            text = UIProgressBar.current.value.ToString("F");
    }

    /// <summary>
    /// Convenience function, in case you wanted to associate progress bar, slider or scroll bar's
    /// OnValueChanged function in inspector with a label.
    /// </summary>

    public void SetCurrentPercent()
    {
        if (UIProgressBar.current != null)
            text = Mathf.RoundToInt(UIProgressBar.current.value * 100f) + "%";
    }

    /// <summary>
    /// Convenience function, in case you wanted to automatically set some label's text
    /// by selecting a value in the UIPopupList.
    /// </summary>

    public void SetCurrentSelection()
    {
        if (UIPopupList.current != null)
        {
            text = UIPopupList.current.isLocalized ?
                Localization.Get(UIPopupList.current.value) :
                UIPopupList.current.value;
        }
    }

    /// <summary>
    /// Convenience function -- wrap the current text given the label's settings and unlimited height.
    /// </summary>

    public bool Wrap(string text, out string final) { return Wrap(text, out final, 1000000); }

    /// <summary>
    /// Convenience function -- wrap the current text given the label's settings and the given height.
    /// </summary>

    public bool Wrap(string text, out string final, int height)
    {
        UpdateNGUIText();
        NGUIText.rectHeight = height;
        NGUIText.regionHeight = height;
        bool retVal = NGUIText.WrapText(text, out final);
        NGUIText.bitmapFont = null;
        NGUIText.dynamicFont = null;
        return retVal;
    }

    /// <summary>
    /// Update NGUIText.current with all the properties from this label.
    /// </summary>

    public void UpdateNGUIText()
    {
        Font ttf = trueTypeFont;
        bool isDynamic = (ttf != null);

        NGUIText.fontSize = mFinalFontSize;
        NGUIText.fontStyle = mFontStyle;
        NGUIText.rectWidth = mWidth;
        NGUIText.rectHeight = mHeight;
        NGUIText.regionWidth = Mathf.RoundToInt(mWidth * (mDrawRegion.z - mDrawRegion.x));
        NGUIText.regionHeight = Mathf.RoundToInt(mHeight * (mDrawRegion.w - mDrawRegion.y));
        NGUIText.gradient = mApplyGradient && (mFont == null || !mFont.packedFontShader);
        NGUIText.gradientTop = mGradientTop;
        NGUIText.gradientBottom = mGradientBottom;
        NGUIText.encoding = mEncoding;
        NGUIText.premultiply = mPremultiply;
        NGUIText.symbolStyle = mSymbols;
        NGUIText.maxLines = mMaxLineCount;
        NGUIText.spacingX = effectiveSpacingX;
        NGUIText.spacingY = effectiveSpacingY;
        NGUIText.fontScale = isDynamic ? mScale : ((float)mFontSize / mFont.defaultSize) * mScale;

        if (mFont != null)
        {
            NGUIText.bitmapFont = mFont;

            for (;;)
            {
                UIFont fnt = NGUIText.bitmapFont.replacement;
                if (fnt == null) break;
                NGUIText.bitmapFont = fnt;
            }

            if (NGUIText.bitmapFont.isDynamic)
            {
                NGUIText.dynamicFont = NGUIText.bitmapFont.dynamicFont;
                NGUIText.bitmapFont = null;
            }
            else NGUIText.dynamicFont = null;
        }
        else
        {
            NGUIText.dynamicFont = ttf;
            NGUIText.bitmapFont = null;
        }

        if (isDynamic && keepCrisp)
        {
            UIRoot rt = root;
            if (rt != null) NGUIText.pixelDensity = (rt != null) ? rt.pixelSizeAdjustment : 1f;
        }
        else NGUIText.pixelDensity = 1f;

        if (mDensity != NGUIText.pixelDensity)
        {
            ProcessText(false, false);
            NGUIText.rectWidth = mWidth;
            NGUIText.rectHeight = mHeight;
            NGUIText.regionWidth = Mathf.RoundToInt(mWidth * (mDrawRegion.z - mDrawRegion.x));
            NGUIText.regionHeight = Mathf.RoundToInt(mHeight * (mDrawRegion.w - mDrawRegion.y));
        }

        if (alignment == Alignment.Automatic)
        {
            Pivot p = pivot;

            if (p == Pivot.Left || p == Pivot.TopLeft || p == Pivot.BottomLeft)
            {
                NGUIText.alignment = Alignment.Left;
            }
            else if (p == Pivot.Right || p == Pivot.TopRight || p == Pivot.BottomRight)
            {
                NGUIText.alignment = Alignment.Right;
            }
            else NGUIText.alignment = Alignment.Center;
        }
        else NGUIText.alignment = alignment;

        NGUIText.Update();
    }

    void OnApplicationPause(bool paused)
    {
        if (!paused && mTrueTypeFont != null) Invalidate(false);
    }

    void PraseSymbolAndLink()
    {
        if ((mUseSymbol == false || mSymbolAtlas == null))
        {
            return;
        }
        // Utils.CleanGrid(cachedGameObject);


        while ((mUseSymbol && mSymbolAtlas != null && mText.Contains("&")) ||
            (mPraseLink && mLinkItem != null && mText.Contains("<a,") && mText.Contains(",a>")))
        {
            int symbolindex = mText.IndexOf("&");
            int linkindex = mText.IndexOf("<a,");

            if (symbolindex != -1 && linkindex == -1)
            {
                PraseSymbol();
            }
            else if (symbolindex == -1 && linkindex != -1)
            {
                // PraseLink();
                return;
            }
            else
            {
                if (symbolindex < linkindex)
                {
                    PraseSymbol();
                }
                else
                {
                    // PraseLink();
                    return;
                }
            }
        }

        // 居中显示时表情识别第二步 设置表情坐标
        // 通过检测mSymbolList来判断是否需要进行
        // 左上角显示以后或许也可以统一为该方法
        if (mSymbolList.Count > 0)
        {
            // 遍历缓存的表情 通过查找占位符的索引来确定表情的坐标
            // 用本label设置NGUIText
            UpdateNGUIText();
            NGUIText.PrintExactCharacterPositions(mText, mTempVerts, mTempIndices);
            ApplyOffset(mTempVerts, 0);
            for (int i = 0, findstart = 0; i < mSymbolList.Count; i++)
            {
                // 查找占位符位置 每找到一次findstart加1
                // 后续处理和之前解析表情一致
                int index = mText.IndexOf("　", findstart);
                findstart = index + 1;

                int nIdx = mTempIndices.IndexOf(index) * 2;

                bool isSmallEmoj = false;
                if (nIdx >= 0 && mTempVerts.size > nIdx)
                {
                    for (int j = 0; j < mSymbolAtlas[0].spriteList.Count; j++)
                    {
                        if (mSymbolAtlas[0].spriteList[j].name == mSymbolList[i].spriteName)
                        {
                            isSmallEmoj = true;                          
                            break; 
                        }
                    }
                    if (isSmallEmoj)
                    {
                        isSmallEmoj = false;
                        mSymbolList[i].transform.localPosition = new Vector3(mTempVerts[nIdx].x, mTempVerts[nIdx].y, 0);
                    }
                    else
                    {
                        if (!(pivot == Pivot.Top || pivot == Pivot.Bottom || pivot == Pivot.TopRight || pivot == Pivot.BottomLeft))
                        {
                            mSymbolList[i].pivot = Pivot.TopLeft;
                            mSymbolList[i].transform.localPosition = new Vector3(mTempVerts[nIdx].x, 0, 0);
                        }
                        else
                        {
                            mSymbolList[i].transform.localPosition = new Vector3(mTempVerts[nIdx].x - mSymbolList[i].width / 3, mTempVerts[nIdx].y - mSymbolList[i].height / 4, 0);
                        }
                    }                  
                    //Debug.Log("vtemp " + nIdx + ", " + mTempVerts[nIdx] + "," + mTempVerts[nIdx+1]);
                }
            }
            mTempVerts.Clear();
            mTempIndices.Clear();
        }
    }

    void PraseSymbol()
    {
        if (false == mUseSymbol || mSymbolAtlas == null)
        {
            return;
        }

        // 与符号'&'作为表情标识
        int index = mText.IndexOf("&");
        bool bRet = false;
        bool isOther = false;
        // 与符号'&'后1-6位均作为表情名字来识别 若字数少的先被识别 则会进入下一个表情的识别
        // 所以需要注意避免表情名是abc和abcde的情况 此时会率先识别abc abcde不会被识别
        // 后续看需求是否要变为数字
        for (int textnum = 1; textnum <= 6; textnum++)
        {
            if (index + textnum >= mText.Length)
            {
                break;
            }
            // 以表情&abc为例
            // szSymbol为&abc 用于字符串拼接 szSymbolName为abc 用于比较图集内图素名
            string szSymbol = mText.Substring(index, textnum + 1);
            string szSymbolName = mText.Substring(index + 1, textnum);
            if (mSymbolAtlas.Length > 1 && mSymbolAtlas[1] != null)//动态表情
            {
                for (int i = 0; i < mSymbolAtlas[1].spriteList.Count; i++)
                {
                    if (mSymbolAtlas[1].spriteList[i].name == szSymbolName)
                    {
                        UpdateNGUIText();
                        if (!(pivot == Pivot.Top || pivot == Pivot.Bottom || pivot == Pivot.TopRight || pivot == Pivot.BottomLeft))
                        {
                            isOther = true;
                        }
                        // 创建表情sprite
                        UISprite sprite = NGUITools.AddWidget<UISprite>(cachedGameObject);
                        sprite.atlas = mSymbolAtlas[1];
                        sprite.spriteName = mSymbolAtlas[1].spriteList[i].name;
                        sprite.type = UIBasicSprite.Type.Sliced;
                        // 表情长宽自由设置
                        if (!m_bFixPos)
                        {
                            sprite.width = mSymbolAtlas[1].spriteList[i].width;
                            sprite.height = mSymbolAtlas[1].spriteList[i].height;
                        }
                        else
                        {
                            if (mSymbolAtlas[1].spriteList[i].width > 100)
                            {
                                sprite.width = 100;
                                sprite.height = (int)((float)mSymbolAtlas[1].spriteList[i].width / (float)mSymbolAtlas[1].spriteList[i].height * 100.0f);
                            }
                            else
                            {
                                sprite.width = 70;
                                sprite.height = 70;
                            }
                        }

                        sprite.gameObject.AddComponent<UIDragScrollView>();

                        // 锚点定位左下 方便后面设置位置
                        sprite.pivot = Pivot.Center;
                        sprite.depth = depth + 1;

                        {
                            //增加可点击 Lijianpeng
                            //这个表情商店专属的功能还是尽早分出去吧。。。
                            UIButton Btn = sprite.gameObject.AddComponent<UIButton>();
                            //BigEmojClickable Clickable = sprite.gameObject.AddComponent<BigEmojClickable>();
                            //Clickable.m_SpName = sprite.spriteName;
                            //EventDelegate.Add(Btn.onClick, Clickable.OnClick);
                            BoxCollider BC = sprite.gameObject.AddComponent<BoxCollider>();
                            Vector3 Size = new Vector3(width, sprite.height);
                            BC.size = Size;
                        }

                        UISpriteAnimation UIani = sprite.gameObject.AddComponent<UISpriteAnimation>();//NGUI自带的切换sprite
                        if (UIani == null)
                        {
                            return;
                        }
                        UIani.framesPerSecond = 3;
                        UIani.namePrefix = sprite.spriteName.Substring(0, sprite.spriteName.Length - 1);
                        UIani.loop = true;
                        UIani.autoPlay = true;
                        UIani.autoReset = true;
                        UIani.Play();
                        // 居中显示时解析方式和左上角显示不同 因为随着每次变化mText内容 文字坐标均可能发生变化 所以需要循环两次
                        // 第一次把所有表情识别为占位符 并把表情缓存到mSymbolList中 即本逻辑
                        // 第二次设置mSymbolList中表情的坐标 在PraseSymbolAndLink大循环完成后再进行
                        string szSymbolLeft = mText.Substring(0, index);
                        string szSymbolRight = mText.Substring(index + szSymbol.Length);
                        mText = string.Format("{0}　{1}", szSymbolLeft, szSymbolRight);

                        mSymbolList.Add(sprite);

                        if (m_bFixPos)
                        {
                            Vector3 vec;
                            vec.x = sprite.width / 2;//mSymbolAtlas[1].spriteList[i].width / 2;
                            vec.y = sprite.height / 2;//mSymbolAtlas[1].spriteList[i].height / 2;
                            vec.z = 0;
                            sprite.transform.localPosition = vec;
                        }
                        try
                        {
                            if (isOther)
                            {
                                if (!m_bFixPos)
                                {
                                    Vector3 vec;
                                    vec.x = mSymbolAtlas[1].spriteList[i].width / 2;
                                    vec.y = -mSymbolAtlas[1].spriteList[i].height / 2;
                                    vec.z = 0;
                                    sprite.transform.localPosition = vec;
                                    isOther = false;
                                }
                            }
                            else
                            {
                                // Debug.Log("!Other");
                                //                                 Vector3 vec;
                                //                                 vec.x = -mSymbolAtlas[1].spriteList[i].width / 2;
                                //                                 vec.y = -24;
                                //                                 vec.z = 0;
                                //                                 sprite.transform.localPosition = vec;
                                //                                 isOther = false;
                            }
                            Transform trans = sprite.transform.parent;//父节点
                            if (trans == null)
                            {
                                return;
                            }
                            Transform Ptrans = trans.parent;//Uilabel的父节点
                            if (Ptrans == null)
                            {
                                return;
                            }
                            Transform transBg = Ptrans.gameObject.transform.Find("Bg");
                            if (transBg == null)
                            {
                                return;
                            }
                            transBg.gameObject.SetActive(false);//将背景板关闭
                        }
                        catch (Exception e)
                        {
                            //LogModule.DebugLog("The corresponding node was not found");
                            throw e;
                        }
                        mProcessedText = "";
                        return;//大表情不做换行处理

                        // }                      
                    }
                }
            }
            for (int i = 0; i < mSymbolAtlas[0].spriteList.Count; i++)
            {
                // 遍历所有图素
                if (mSymbolAtlas[0].spriteList[i].name == szSymbolName)
                {
                    // 用本label设置NGUIText
                    UpdateNGUIText();

                    if (pivot == Pivot.Top || pivot == Pivot.Bottom || pivot == Pivot.TopRight || pivot == Pivot.BottomLeft)
                    {
                        // 创建表情sprite
                        UISprite sprite = NGUITools.AddWidget<UISprite>(cachedGameObject);
                        sprite.atlas = mSymbolAtlas[0];
                        sprite.spriteName = mSymbolAtlas[0].spriteList[i].name;
                        sprite.type = UIBasicSprite.Type.Sliced;
                        // 表情长宽和字号相同大小 当做一个汉字来处理
                        sprite.width = fontSize;
                        sprite.height = fontSize;
                        // 锚点定位左下 方便后面设置位置
                        sprite.pivot = Pivot.BottomLeft;
                        sprite.depth = depth + 1;

                        // 居中显示时解析方式和左上角显示不同 因为随着每次变化mText内容 文字坐标均可能发生变化 所以需要循环两次
                        // 第一次把所有表情识别为占位符 并把表情缓存到mSymbolList中 即本逻辑
                        // 第二次设置mSymbolList中表情的坐标 在PraseSymbolAndLink大循环完成后再进行
                        string szSymbolLeft = mText.Substring(0, index);
                        string szSymbolRight = mText.Substring(index + szSymbol.Length);
                        mText = string.Format("{0}　{1}", szSymbolLeft, szSymbolRight);

                        mSymbolList.Add(sprite);
                    }
                    else
                    {
                        // 取szSymbol(&abc)左边的部分来判断在本label下的打印尺寸
                        string szSymbolLeft = mText.Substring(0, index);
                        Vector2 pos = NGUIText.CalculatePrintedSize(szSymbolLeft);
                        pos -= new Vector2(0, spacingY);
                        if (pos.x == 0 && pos.y == 0)
                        {
                            // 第一个字就是表情 特殊处理一下
                            pos = new Vector2(0, fontSize - spacingY);
                        }
                        else if (pos.y > fontSize + spacingY)
                        {
                            // 超过一行 x坐标重新计算 y坐标不变
                            // 目前计算方法为从表情位置开始往回倒退 直到y坐标小于表情位置的y坐标
                            // 主要为了获取表情位于该行第几个字符 没找到NGUI的相关接口 只好硬算了
                            int indexoffset = 1;
                            while (NGUIText.CalculatePrintedSize(mText.Substring(0, index - indexoffset)).y >= pos.y)
                            {
                                indexoffset += 1;
                            }

                            float posx = NGUIText.CalculatePrintedSize(mText.Substring(index - indexoffset, indexoffset)).x;

                            pos = new Vector2(posx + spacingX, pos.y);
                        }

                        if (pos.x + fontSize + spacingX > width)
                        {
                            // 本来不超过一行 加上一个表情的宽度会超过一行 表情会出现在下一行最左边 x坐标直接置为0 y加一个字体大小 即进入一行
                            pos = new Vector2(0, pos.y + fontSize + spacingY);
                        }

                        // 目前使用表情解析的label均为定宽 所以x坐标永远不会超过宽度 根据表情锚点设置位置
                        Vector3 vSpritePos = new Vector3(pos.x, -pos.y, 0);
                        if (pivot == Pivot.Left && mOverflow == Overflow.ClampContent)
                        {
                            vSpritePos += new Vector3(0, fontSize / 2.0f, 0);
                        }

                        if (mOverflow == Overflow.ClampContent && Mathf.Abs(vSpritePos.y) > height)
                        {
                            // 超过范围 不显示表情
                        }
                        else
                        {
                            // 创建表情sprite
                            UISprite sprite = NGUITools.AddWidget<UISprite>(cachedGameObject);
                            sprite.atlas = mSymbolAtlas[0];
                            sprite.spriteName = mSymbolAtlas[0].spriteList[i].name;
                            sprite.type = UIBasicSprite.Type.Sliced;
                            // 表情长宽和字号相同大小 当做一个汉字来处理
                            sprite.width = fontSize;
                            sprite.height = fontSize;
                            // 锚点定位左下 方便后面设置位置

                            sprite.pivot = Pivot.BottomLeft;
                            sprite.transform.localPosition = vSpritePos;

                            sprite.depth = depth + 1;

                            mSymbolList.Add(sprite);
                        }

                        // 表情位置用中文全角空格占位 表情已设置为汉字大小 即和空格等长宽
                        string szSymbolRight = mText.Substring(index + szSymbol.Length);
                        mText = string.Format("{0}　{1}", szSymbolLeft, szSymbolRight);
                    }

                    MarkAsChanged();

                    bRet = true;
                    break;
                }
            }

            if (bRet)
            {
                break;
            }
        }

        if (!bRet)
        {
            // 如果没找到 把标记去掉 防止死循环
            mText = mText.Substring(0, index) + mText.Substring(index + 1);
        }
    }

}
