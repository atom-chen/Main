// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.Debugger
{
    class PickingData
    {
        private readonly List<UIElementsDebugger.ViewPanel> m_Panels;
        private GUIContent[] m_Labels;
        public Rect screenRect;

        public PickingData()
        {
            m_Panels = new List<UIElementsDebugger.ViewPanel>();
            Refresh();
        }

        internal bool Draw(ref UIElementsDebugger.ViewPanel? selectedPanel, Rect dataScreenRect)
        {
            foreach (UIElementsDebugger.ViewPanel panel in m_Panels)
            {
                Rect sp = panel.View.screenPosition;
                sp.x -= dataScreenRect.xMin;
                sp.y -= dataScreenRect.yMin;

                if (GUI.Button(sp, string.Format("{0}({1})", panel.Panel.visualTree.name, panel.View.GetInstanceID()), EditorStyles.miniButton))
                {
                    selectedPanel = panel;
                    return true;
                }
                DrawRect(sp, Color.white);
            }
            return false;
        }

        public static void DrawRect(Rect sp, Color c)
        {
            sp.xMin++;
            sp.xMax--;
            sp.yMin++;
            sp.yMax--;

            HandleUtility.ApplyWireMaterial();
            GL.PushMatrix();
            GL.Begin(GL.LINES);

            GL.Color(c);
            GL.Vertex3(sp.xMin, sp.yMin, 0);
            GL.Color(c);
            GL.Vertex3(sp.xMax, sp.yMin, 0);

            GL.Color(c);
            GL.Vertex3(sp.xMax, sp.yMin, 0);
            GL.Color(c);
            GL.Vertex3(sp.xMax, sp.yMax, 0);

            GL.Color(c);
            GL.Vertex3(sp.xMax, sp.yMax, 0);
            GL.Color(c);
            GL.Vertex3(sp.xMin, sp.yMax, 0);

            GL.Color(c);
            GL.Vertex3(sp.xMin, sp.yMax, 0);
            GL.Color(c);
            GL.Vertex3(sp.xMin, sp.yMin, 0);
            GL.End();
            GL.PopMatrix();
        }

        public void Refresh()
        {
            m_Panels.Clear();

            var it = UIElementsUtility.GetPanelsIterator();
            List<GUIView> guiViews = new List<GUIView>();
            GUIViewDebuggerHelper.GetViews(guiViews);
            bool setMax = false;
            Rect screen = new Rect(float.MaxValue, float.MaxValue, 0, 0);
            while (it.MoveNext())
            {
                GUIView view = guiViews.FirstOrDefault(v => v.GetInstanceID() == it.Current.Key);
                if (view == null)
                    continue;

                m_Panels.Add(new UIElementsDebugger.ViewPanel
                {
                    Panel = it.Current.Value,
                    View = view
                });

                if (screen.xMin > view.screenPosition.xMin)
                    screen.xMin = view.screenPosition.xMin;
                if (screen.yMin > view.screenPosition.yMin)
                    screen.yMin = view.screenPosition.yMin;

                if (screen.xMax < view.screenPosition.xMax || !setMax)
                    screen.xMax = view.screenPosition.xMax;
                if (screen.yMax < view.screenPosition.yMax || !setMax)
                    screen.yMax = view.screenPosition.yMax;
                setMax = true;
            }

            m_Labels = new GUIContent[m_Panels.Count + 1];
            m_Labels[0] = EditorGUIUtility.TrTextContent("Select a panel");
            for (int i = 0; i < m_Panels.Count; i++)
                m_Labels[i + 1] = new GUIContent(GetName(m_Panels[i]));

            screenRect = screen;
        }

        internal static string GetName(UIElementsDebugger.ViewPanel viewPanel)
        {
            var hostview = viewPanel.View as HostView;
            if (hostview != null)
            {
                var win = hostview.actualView;
                if (win != null)
                {
                    if (!String.IsNullOrEmpty(win.name))
                        return win.name;
                    return win.GetType().Name;
                }
                if (!String.IsNullOrEmpty(hostview.name))
                    return hostview.name;
            }
            return viewPanel.Panel.visualTree.name;
        }

        private int m_Selected;
        private static int s_PopupHash = "EditorPopup".GetHashCode();

        public void DoSelectDropDown(Action onSelect)
        {
            var label = m_Selected >= 0 && m_Selected < m_Labels.Length ? m_Labels[m_Selected] : GUIContent.Temp("<NONE>");
            if (GUILayout.Button(label, EditorStyles.popup))
            {
                Refresh();
                Rect rect = EditorGUILayout.GetControlRect(false, EditorGUI.kSingleLineHeight, EditorStyles.popup);
                var controlID = GUIUtility.GetControlID(s_PopupHash, FocusType.Keyboard, rect);
                var position = EditorGUI.IndentedRect(rect);
                position.y += EditorGUI.kSingleLineHeight;
                EditorGUI.PopupCallbackInfo.instance = new EditorGUI.PopupCallbackInfo(controlID);
                EditorUtility.DisplayCustomMenu(position, m_Labels, m_Selected, (data, options, selected) =>
                    {
                        m_Selected = selected;
                        onSelect();
                    }, null);
                GUIUtility.keyboardControl = controlID;
            }
        }

        internal UIElementsDebugger.ViewPanel? Selected
        {
            get
            {
                if (m_Selected == 0 || m_Selected > m_Panels.Count)
                    return null;
                return m_Panels[m_Selected - 1];
            }
        }

        public bool TryRestoreSelectedWindow(string lastWindowTitle)
        {
            for (int i = 0; i < m_Labels.Length; i++)
            {
                if (m_Labels[i].text == lastWindowTitle)
                {
                    m_Selected = i;
                    return true;
                }
            }
            return false;
        }

        public bool TrySelectWindow(EditorWindow searchedWindow)
        {
            if (searchedWindow.rootVisualContainer == null)
                return false;

            IPanel searchedPanel = searchedWindow.rootVisualContainer.panel;
            for (int i = 0; i < m_Panels.Count; i++)
            {
                var p = m_Panels[i];
                if (p.Panel == searchedPanel)
                {
                    m_Selected = i + 1;
                    return true;
                }
            }

            return false;
        }
    }
}
