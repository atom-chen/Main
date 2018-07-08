// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
    public abstract class BaseTextElement : VisualElement
    {
        public class BaseTextElementUxmlTraits : VisualElementUxmlTraits
        {
            UxmlStringAttributeDescription m_Text;

            public BaseTextElementUxmlTraits()
            {
                m_Text = new UxmlStringAttributeDescription { name = "text" };
            }

            public override IEnumerable<UxmlAttributeDescription> uxmlAttributesDescription
            {
                get
                {
                    foreach (var attr in base.uxmlAttributesDescription)
                    {
                        yield return attr;
                    }

                    yield return m_Text;
                }
            }

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((BaseTextElement)ve).text = m_Text.GetValueFromBag(bag);
            }
        }

        internal const string k_TextElementClass = "textElement";
        public BaseTextElement()
        {
            requireMeasureFunction = true;
            AddToClassList(k_TextElementClass);
        }

        [SerializeField]
        private string m_Text;
        public virtual string text
        {
            get { return m_Text ?? String.Empty; }
            set
            {
                if (m_Text == value)
                    return;

                m_Text = value;
                Dirty(ChangeType.Layout);

                if (!string.IsNullOrEmpty(persistenceKey))
                    SavePersistentData();
            }
        }

        public override void DoRepaint()
        {
            var painter = elementPanel.stylePainter;
            painter.DrawBackground(this);
            painter.DrawBorder(this);
            painter.DrawText(this);
        }

        protected internal override Vector2 DoMeasure(float width, MeasureMode widthMode, float height, MeasureMode heightMode)
        {
            float measuredWidth = float.NaN;
            float measuredHeight = float.NaN;

            Font usedFont = style.font;
            if (text == null || usedFont == null)
                return new Vector2(measuredWidth, measuredHeight);

            var stylePainter = elementPanel.stylePainter;
            if (widthMode == MeasureMode.Exactly)
            {
                measuredWidth = width;
            }
            else
            {
                var textParams = stylePainter.GetDefaultTextParameters(this);
                textParams.text = text;
                textParams.font = usedFont;
                textParams.wordWrapWidth = 0.0f;
                textParams.wordWrap = false;
                textParams.richText = true;

                measuredWidth = stylePainter.ComputeTextWidth(textParams);

                if (widthMode == MeasureMode.AtMost)
                {
                    measuredWidth = Mathf.Min(measuredWidth, width);
                }
            }

            if (heightMode == MeasureMode.Exactly)
            {
                measuredHeight = height;
            }
            else
            {
                var textParams = stylePainter.GetDefaultTextParameters(this);
                textParams.text = text;
                textParams.font = usedFont;
                textParams.wordWrapWidth = measuredWidth;
                textParams.richText = true;

                measuredHeight = stylePainter.ComputeTextHeight(textParams);

                if (heightMode == MeasureMode.AtMost)
                {
                    measuredHeight = Mathf.Min(measuredHeight, height);
                }
            }
            return new Vector2(measuredWidth, measuredHeight);
        }
    }
}
