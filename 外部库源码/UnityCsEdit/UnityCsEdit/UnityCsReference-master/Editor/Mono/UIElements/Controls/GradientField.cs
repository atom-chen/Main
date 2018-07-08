// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.Experimental.UIElements;
using Object = UnityEngine.Object;

namespace UnityEditor.Experimental.UIElements
{
    public class GradientField : BaseControl<Gradient>
    {
        public class GradientFieldFactory : UxmlFactory<GradientField, GradientFieldUxmlTraits> {}

        public class GradientFieldUxmlTraits : BaseControlUxmlTraits {}

        private bool m_ValueNull;
        Gradient m_Value;
        public override Gradient value
        {
            get
            {
                if (m_ValueNull) return null;
                Gradient gradientCopy = new Gradient();
                gradientCopy.colorKeys = m_Value.colorKeys;
                gradientCopy.alphaKeys = m_Value.alphaKeys;
                gradientCopy.mode = m_Value.mode;

                return m_Value;
            }
            set
            {
                if (value != null || !m_ValueNull)  // let's not reinitialize an initialized gradient
                {
                    m_ValueNull = value == null;
                    if (!m_ValueNull)
                    {
                        m_Value.colorKeys = value.colorKeys;
                        m_Value.alphaKeys = value.alphaKeys;
                        m_Value.mode = value.mode;
                    }
                    else // restore the internal gradient to the default state.
                    {
                        m_Value.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0), new GradientColorKey(Color.white, 1) };
                        m_Value.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) };
                        m_Value.mode = GradientMode.Blend;
                    }
                }

                UpdateGradientTexture();
            }
        }

        public GradientField()
        {
            VisualElement borderElement = new VisualElement() { name = "border", pickingMode = PickingMode.Ignore };
            Add(borderElement);

            m_Value = new Gradient();
        }

        protected internal override void ExecuteDefaultAction(EventBase evt)
        {
            base.ExecuteDefaultAction(evt);

            if ((evt as MouseDownEvent)?.button == (int)MouseButton.LeftMouse || (evt as KeyDownEvent)?.character == '\n')
                ShowGradientPicker();
            else if (evt.GetEventTypeId() == DetachFromPanelEvent.TypeId())
                OnDetach();
            else if (evt.GetEventTypeId() == AttachToPanelEvent.TypeId())
                OnAttach();
        }

        void OnDetach()
        {
            if (style.backgroundImage.value != null)
            {
                Object.DestroyImmediate(style.backgroundImage.value);
                style.backgroundImage = null;
            }
        }

        void OnAttach()
        {
            UpdateGradientTexture();
        }

        void ShowGradientPicker()
        {
            GradientPicker.Show(m_Value, true, OnGradientChanged);
        }

        public override void OnPersistentDataReady()
        {
            base.OnPersistentDataReady();
            UpdateGradientTexture();
        }

        void UpdateGradientTexture()
        {
            if (m_ValueNull)
            {
                style.backgroundImage = null;
            }
            else
            {
                Texture2D gradientTexture = UnityEditorInternal.GradientPreviewCache.GenerateGradientPreview(value, style.backgroundImage.value);

                style.backgroundImage = gradientTexture;

                Dirty(ChangeType.Repaint); // since the Texture2D object can be reused, force dirty because the backgroundImage change will only trigger the Dirty if the Texture2D objects are different.
            }
        }

        void OnGradientChanged(Gradient newValue)
        {
            SetValueAndNotify(newValue);

            GradientPreviewCache.ClearCache(); // needed because GradientEditor itself uses the cache and will no invalidate it on changes.
            Dirty(ChangeType.Repaint);
        }

        public override void SetValueAndNotify(Gradient newValue)
        {
            using (ChangeEvent<Gradient> evt = ChangeEvent<Gradient>.GetPooled(value, newValue))
            {
                evt.target = this;
                value = newValue;
                UIElementsUtility.eventDispatcher.DispatchEvent(evt, panel);
            }
        }

        public override void DoRepaint()
        {
            //Start by drawing the checkerboard background for alpha gradients.
            Texture2D backgroundTexture = GradientEditor.GetBackgroundTexture();
            var painter = elementPanel.stylePainter;
            var painterParams = painter.GetDefaultTextureParameters(this);
            painterParams.texture = backgroundTexture;
            painter.DrawTexture(painterParams);

            base.DoRepaint();
        }
    }
}
