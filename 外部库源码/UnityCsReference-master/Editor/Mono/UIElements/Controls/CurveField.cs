// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;
using Object = UnityEngine.Object;

namespace UnityEditor.Experimental.UIElements
{
    public class CurveField : BaseControl<AnimationCurve>
    {
        public class CurveFieldFactory : UxmlFactory<CurveField, CurveFieldUxmlTraits> {}

        public class CurveFieldUxmlTraits : BaseControlUxmlTraits {}

        private const string k_CurveColorProperty = "curve-color";
        public Rect ranges { get; set; }

        StyleValue<Color> m_CurveColor;
        private Color curveColor
        {
            get
            {
                return m_CurveColor.GetSpecifiedValueOrDefault(Color.green);
            }
        }

        private bool m_ValueNull;
        private AnimationCurve m_Value;
        private bool m_TextureDirty;

        public enum RenderMode
        {
            Texture,
            Mesh,
            Default = Texture
        }

        RenderMode m_RenderMode = RenderMode.Default;

        public RenderMode renderMode
        {
            get { return m_RenderMode; }
            set
            {
                if (m_RenderMode != value)
                {
                    m_RenderMode = value;

                    m_Content = new CurveFieldContent();
                    if (renderMode == RenderMode.Mesh)
                    {
                        Insert(0, m_Content);
                    }
                    else
                    {
                        m_Content.RemoveFromHierarchy();
                        m_Content = null;
                    }

                    m_TextureDirty = true;
                }
            }
        }

        public override AnimationCurve value
        {
            get
            {
                if (m_ValueNull) return null;
                AnimationCurve curveCopy = new AnimationCurve();
                curveCopy.keys = m_Value.keys;
                curveCopy.preWrapMode = m_Value.preWrapMode;
                curveCopy.postWrapMode = m_Value.postWrapMode;

                return curveCopy;
            }
            set
            {
                //I need to have total ownership of the curve, I won't be able to know if it is changed outside. so I'm duplicating it.

                if (value != null || !m_ValueNull) // let's not reinitialize an initialized curve
                {
                    m_ValueNull = value == null;
                    if (!m_ValueNull)
                    {
                        m_Value.keys = value.keys;
                        m_Value.preWrapMode = value.preWrapMode;
                        m_Value.postWrapMode = value.postWrapMode;
                    }
                    else
                    {
                        m_Value.keys = new Keyframe[0];
                        m_Value.preWrapMode = WrapMode.Once;
                        m_Value.postWrapMode = WrapMode.Once;
                    }
                }
                m_TextureDirty = true;

                Dirty(ChangeType.Repaint);

                m_Content?.Dirty(ChangeType.Repaint);
            }
        }
        CurveFieldContent m_Content;

        public CurveField()
        {
            ranges = Rect.zero;

            m_Value = new AnimationCurve(new Keyframe[0]);

            VisualElement borderElement = new VisualElement() { name = "border", pickingMode = PickingMode.Ignore };
            Add(borderElement);
        }

        void OnDetach()
        {
            if (m_Mesh != null)
                Object.DestroyImmediate(m_Mesh);
            if (style.backgroundImage.value != null)
                Object.DestroyImmediate(style.backgroundImage.value);
            m_Mesh = null;
            style.backgroundImage = null;
            m_TextureDirty = true;
        }

        public override void SetValueAndNotify(AnimationCurve newValue)
        {
            using (ChangeEvent<AnimationCurve> evt = ChangeEvent<AnimationCurve>.GetPooled(value, newValue))
            {
                evt.target = this;
                value = newValue;
                UIElementsUtility.eventDispatcher.DispatchEvent(evt, panel);
            }
        }

        protected override void OnStyleResolved(ICustomStyle style)
        {
            base.OnStyleResolved(style);

            Color color = curveColor;
            style.ApplyCustomProperty(k_CurveColorProperty, ref m_CurveColor);

            if (color != curveColor && renderMode == RenderMode.Texture)
            {
                // The mesh texture is updated at each repaint, the standard texture should however be regenerated
                m_TextureDirty = true;
            }
        }

        void ShowCurveEditor()
        {
            if (!enabledInHierarchy)
                return;

            CurveEditorSettings settings = new CurveEditorSettings();
            if (m_Value == null)
                m_Value = new AnimationCurve();
            CurveEditorWindow.curve = m_Value;

            CurveEditorWindow.color = curveColor;
            CurveEditorWindow.instance.Show(OnCurveChanged, settings);
        }

        protected internal override void ExecuteDefaultAction(EventBase evt)
        {
            base.ExecuteDefaultAction(evt);

            if ((evt as MouseDownEvent)?.button == (int)MouseButton.LeftMouse || (evt as KeyDownEvent)?.character == '\n')
                ShowCurveEditor();
            else if (evt.GetEventTypeId() == DetachFromPanelEvent.TypeId())
                OnDetach();
            if (evt.GetEventTypeId() == GeometryChangedEvent.TypeId())
                m_TextureDirty = true;
        }

        void OnCurveChanged(AnimationCurve curve)
        {
            CurveEditorWindow.curve = m_Value;
            SetValueAndNotify(m_Value);
        }

        private void SendChangeEvent(AnimationCurve newValue)
        {
            using (ChangeEvent<AnimationCurve> evt = ChangeEvent<AnimationCurve>.GetPooled(value, newValue))
            {
                evt.target = this;
                value = newValue;
                UIElementsUtility.eventDispatcher.DispatchEvent(evt, panel);
            }
        }

        public override void OnPersistentDataReady()
        {
            base.OnPersistentDataReady();
            m_TextureDirty = true;
        }

        // Must be the same with AACurveField.shader
        const float k_EdgeWidth = 2;
        const float k_MinEdgeWidth = 1.75f;
        const float k_HalfWidth = k_EdgeWidth * 0.5f;
        const float k_VertexHalfWidth = k_HalfWidth + 1;

        const int k_HorizontalCurveResolution = 256;

        void FillCurveData()
        {
            AnimationCurve curve = value;

            if (m_Mesh == null)
            {
                m_Mesh = new Mesh();
                m_Mesh.hideFlags = HideFlags.HideAndDontSave;

                m_Content.SetMesh(m_Mesh);
            }

            if (curve.keys.Length < 2)
                return;
            Vector3[] vertices = m_Mesh.vertices;
            Vector3[] normals = m_Mesh.normals;
            if (vertices == null || vertices.Length != k_HorizontalCurveResolution * 2)
            {
                vertices = new Vector3[k_HorizontalCurveResolution * 2];
                normals = new Vector3[k_HorizontalCurveResolution * 2];
            }

            float startTime = 0;
            float endTime = curve.keys.Length > 0 ? curve.keys[curve.keys.Length - 1].time : 1.0f;
            float duration = endTime - startTime;

            float minValue = Mathf.Infinity;
            float maxValue = -Mathf.Infinity;

            float[] timeCache = new float[k_HorizontalCurveResolution];
            int keyCount = curve.keys.Length;
            int noKeySampleCount = k_HorizontalCurveResolution - keyCount;

            timeCache[0] = curve.keys[0].time;

            int usedSamples = 1;
            for (int k = 1; k < keyCount; ++k)
            {
                float sliceStartTime = timeCache[usedSamples - 1];
                float sliceEndTime = curve.keys[k].time;
                float sliceDuration = sliceEndTime - sliceStartTime;
                int sliceSampleCount = Mathf.FloorToInt((float)noKeySampleCount * sliceDuration / duration);
                if (k == keyCount - 1)
                {
                    sliceSampleCount = k_HorizontalCurveResolution - usedSamples - 1;
                }

                for (int i = 1; i < sliceSampleCount + 1; ++i)
                {
                    float time = sliceStartTime + i * sliceDuration / (sliceSampleCount + 1);
                    timeCache[usedSamples + i - 1] = time;
                }

                timeCache[usedSamples + sliceSampleCount] = curve.keys[k].time;
                usedSamples += sliceSampleCount + 1;
            }

            float[] valueCache = new float[k_HorizontalCurveResolution];

            for (int i = 0; i < k_HorizontalCurveResolution; ++i)
            {
                float ct = timeCache[i];

                float currentValue = curve.Evaluate(ct);

                if (currentValue > maxValue)
                {
                    maxValue = currentValue;
                }
                if (currentValue < minValue)
                {
                    minValue = currentValue;
                }

                valueCache[i] = currentValue;
            }

            Vector3 scale = new Vector3(m_Content.layout.width, m_Content.layout.height);

            vertices[0] = vertices[1] = Vector3.Scale(new Vector3(0, 1 - Mathf.InverseLerp(minValue, maxValue, valueCache[0]), 0), scale);

            Vector3 secondPoint = Vector3.Scale(new Vector3(1.0f / k_HorizontalCurveResolution, 1 - Mathf.InverseLerp(minValue, maxValue, valueCache[1]), 0), scale);
            Vector3 prevDir = (secondPoint - vertices[0]).normalized;

            Vector3 norm = new Vector3(prevDir.y, -prevDir.x, 1);

            normals[0] = -norm * k_VertexHalfWidth;
            normals[1] = norm * k_VertexHalfWidth;

            Vector3 currentPoint = secondPoint;

            for (int i = 1; i < k_HorizontalCurveResolution - 1; ++i)
            {
                vertices[i * 2] = vertices[i * 2 + 1] = currentPoint;

                Vector3 nextPoint = Vector3.Scale(new Vector3(Mathf.InverseLerp(startTime, endTime, timeCache[i + 1]), 1 - Mathf.InverseLerp(minValue, maxValue, valueCache[i + 1]), 0), scale);

                Vector3 nextDir = (nextPoint - currentPoint).normalized;
                Vector3 dir = (prevDir + nextDir).normalized;
                norm = new Vector3(dir.y, -dir.x, 1);
                normals[i * 2] = -norm * k_VertexHalfWidth;
                normals[i * 2 + 1] = norm * k_VertexHalfWidth;

                currentPoint = nextPoint;
                prevDir = nextDir;
            }

            vertices[(k_HorizontalCurveResolution - 1) * 2] = vertices[(k_HorizontalCurveResolution - 1) * 2 + 1] = currentPoint;

            norm = new Vector3(prevDir.y, -prevDir.x, 1);
            normals[(k_HorizontalCurveResolution - 1) * 2] = -norm * k_VertexHalfWidth;
            normals[(k_HorizontalCurveResolution - 1) * 2 + 1] = norm * k_VertexHalfWidth;

            m_Mesh.vertices = vertices;
            m_Mesh.normals = normals;

            //fill triangle indices as it is a triangle strip
            int[] indices = new int[(k_HorizontalCurveResolution * 2 - 2) * 3];

            for (int i = 0; i < k_HorizontalCurveResolution * 2 - 2; ++i)
            {
                if ((i % 2) == 0)
                {
                    indices[i * 3] = i;
                    indices[i * 3 + 1] = i + 1;
                    indices[i * 3 + 2] = i + 2;
                }
                else
                {
                    indices[i * 3] = i + 1;
                    indices[i * 3 + 1] = i;
                    indices[i * 3 + 2] = i + 2;
                }
            }

            m_Mesh.triangles = indices;
        }

        void SetupMeshRepaint()
        {
            if (m_TextureDirty || m_Mesh == null)
            {
                m_TextureDirty = false;
                style.backgroundImage = null;

                FillCurveData();
            }
            m_Content.curveColor = curveColor;
        }

        void SetupStandardRepaint()
        {
            if (!m_TextureDirty) return;

            m_TextureDirty = false;

            int previewWidth = (int)layout.width;
            int previewHeight = (int)layout.height;

            Rect rangeRect = new Rect(0, 0, 1, 1);

            if (ranges.width > 0 && ranges.height > 0)
            {
                rangeRect = ranges;
            }
            else if (!m_ValueNull && m_Value.keys.Length > 1)
            {
                float xMin = Mathf.Infinity;
                float yMin = Mathf.Infinity;
                float xMax = -Mathf.Infinity;
                float yMax = -Mathf.Infinity;

                for (int i = 0; i < m_Value.keys.Length; ++i)
                {
                    float y = m_Value.keys[i].value;
                    float x = m_Value.keys[i].time;
                    if (xMin > x)
                    {
                        xMin = x;
                    }
                    if (xMax < x)
                    {
                        xMax = x;
                    }
                    if (yMin > y)
                    {
                        yMin = y;
                    }
                    if (yMax < y)
                    {
                        yMax = y;
                    }
                }

                if (yMin == yMax)
                {
                    yMax = yMin + 1;
                }
                if (xMin == xMax)
                {
                    xMax = xMin + 1;
                }

                rangeRect = Rect.MinMaxRect(xMin, yMin, xMax, yMax);
            }

            if (previewHeight > 0 && previewWidth > 0)
            {
                if (!m_ValueNull)
                {
                    style.backgroundImage = AnimationCurvePreviewCache.GenerateCurvePreview(
                            previewWidth,
                            previewHeight,
                            rangeRect,
                            m_Value,
                            curveColor,
                            style.backgroundImage.value);
                }
                else
                {
                    style.backgroundImage = null;
                }
            }
        }

        Mesh m_Mesh = null;
        public override void DoRepaint()
        {
            if (renderMode == RenderMode.Mesh)
            {
                SetupMeshRepaint();
            }
            else
            {
                SetupStandardRepaint();
            }

            base.DoRepaint();
        }

        class CurveFieldContent : VisualElement
        {
            Material m_Mat;
            Mesh m_Mesh;

            public Color curveColor { get; set; }

            public void SetMesh(Mesh mesh)
            {
                m_Mesh = mesh;
            }

            public CurveFieldContent()
            {
                pickingMode = PickingMode.Ignore;
            }

            protected internal override void ExecuteDefaultAction(EventBase evt)
            {
                base.ExecuteDefaultAction(evt);

                if (evt.GetEventTypeId() == DetachFromPanelEvent.TypeId())
                    OnDetach();
            }

            void OnDetach()
            {
                Object.DestroyImmediate(m_Mat);
                m_Mat = null;
            }

            public override void DoRepaint()
            {
                base.DoRepaint();

                if (m_Mesh != null)
                {
                    if (m_Mat == null)
                    {
                        m_Mat = new Material(EditorGUIUtility.LoadRequired("Shaders/UIElements/AACurveField.shader") as Shader);

                        m_Mat.hideFlags = HideFlags.HideAndDontSave;
                    }

                    float scale = worldTransform.MultiplyVector(Vector3.one).x;

                    float realWidth = CurveField.k_EdgeWidth;
                    if (realWidth * scale < CurveField.k_MinEdgeWidth)
                    {
                        realWidth = CurveField.k_MinEdgeWidth / scale;
                    }

                    // Send the view zoom factor so that the antialias width do not grow when zooming in.
                    m_Mat.SetFloat("_ZoomFactor", scale * realWidth / CurveField.k_EdgeWidth * EditorGUIUtility.pixelsPerPoint);

                    // Send the view zoom correction so that the vertex shader can scale the edge triangles when below m_MinWidth.
                    m_Mat.SetFloat("_ZoomCorrection", realWidth / CurveField.k_EdgeWidth);

                    m_Mat.SetColor("_Color", (QualitySettings.activeColorSpace == ColorSpace.Linear) ? curveColor.gamma : curveColor);
                    m_Mat.SetPass(0);

                    Graphics.DrawMeshNow(m_Mesh, Matrix4x4.identity);
                }
            }
        }
    }
}
