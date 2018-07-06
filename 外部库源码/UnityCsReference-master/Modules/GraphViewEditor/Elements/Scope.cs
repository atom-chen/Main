// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.Yoga;

namespace UnityEditor.Experimental.UIElements.GraphView
{
    public partial class Scope : GraphElement
    {
        private VisualElement m_MainContainer;
        private VisualElement m_HeaderContainer;
        private ScopeContentContainer m_ContentContainer;

        private List<GraphElement> m_ContainedElements = new List<GraphElement>();

        private bool m_IsUpdatingGeometryFromContent; // To avoid recursive calls to UpdateGeometryFromContent

        internal bool hasPendingGeometryUpdate { get; private set; }

        private Vector2 m_Position = Vector2.zero;

        private bool m_AutoUpdateGeometry = true;

        public bool autoUpdateGeometry
        {
            get { return m_AutoUpdateGeometry; }
            set
            {
                if (m_AutoUpdateGeometry == value)
                    return;

                m_AutoUpdateGeometry = value;

                if (m_AutoUpdateGeometry)
                {
                    ScheduleUpdateGeometryFromContent();
                }
            }
        }

        public VisualElement headerContainer { get { return m_HeaderContainer; } }
        public IEnumerable<GraphElement> containedElements { get { return m_ContainedElements; } }
        public Rect containedElementsRect { get { return m_ContentContainer.ChangeCoordinatesTo(this, m_ContentContainer.rect); } }

        public Scope()
        {
            var visualTree = EditorGUIUtility.Load("UXML/GraphView/Scope.uxml") as VisualTreeAsset;

            AddStyleSheetPath("StyleSheets/GraphView/Scope.uss");

            m_MainContainer = visualTree.CloneTree(null);
            m_MainContainer.AddToClassList("mainContainer");

            m_HeaderContainer = m_MainContainer.Q(name: "headerContainer");

            VisualElement contentContainerPlaceholder = m_MainContainer.Q(name: "contentContainerPlaceholder");

            m_ContentContainer = new ScopeContentContainer();
            m_ContentContainer.containedElements = containedElements;
            contentContainerPlaceholder.Add(m_ContentContainer);

            Add(m_MainContainer);

            ClearClassList();
            AddToClassList("scope");

            clippingOptions = ClippingOptions.ClipAndCacheContents;

            style.positionType = PositionType.Absolute;
            m_ContentContainer.RegisterCallback<GeometryChangedEvent>(OnSubElementGeometryChanged);
        }

        private void OnSubElementGeometryChanged(EventBase e)
        {
            ScheduleUpdateGeometryFromContent();
        }

        internal static bool IsValidSize(Vector2 size)
        {
            return !Single.IsNaN(size.x + size.y) && size.x > 0 && size.y > 0;
        }

        internal static bool IsValidRect(Rect rect)
        {
            return !Single.IsNaN(rect.x + rect.y + rect.width + rect.height) && rect.width > 0 && rect.height > 0;
        }

        public override bool HitTest(Vector2 localPoint)
        {
            Vector2 mappedPoint = this.ChangeCoordinatesTo(m_HeaderContainer, localPoint);

            return m_HeaderContainer.ContainsPoint(mappedPoint);
        }

        public override bool Overlaps(Rect rectangle)
        {
            Rect mappedRect = this.ChangeCoordinatesTo(m_HeaderContainer, rectangle);

            return m_HeaderContainer.Overlaps(mappedRect);
        }

        public bool ContainsElement(GraphElement element)
        {
            return (m_ContainedElements != null) ? m_ContainedElements.Contains(element) : false;
        }

        public virtual bool AcceptsElement(GraphElement element, ref string reasonWhyNotAccepted)
        {
            if (element.GetType() == typeof(Scope))
            {
                reasonWhyNotAccepted = "Nested scope is not supported yet.";
                return false;
            }

            return true;
        }

        public void AddElement(GraphElement element)
        {
            if (element == null)
                throw new ArgumentException("Cannot add null element");

            if (containedElements.Contains(element))
            {
                throw new ArgumentException("The specified element is already contained in this scope.");
            }

            string reasonWhyNotAccepted = "Cannot add the specified element to this scope.";

            if (!AcceptsElement(element, ref reasonWhyNotAccepted))
            {
                throw new ArgumentException(reasonWhyNotAccepted);
            }

            // Removes the element from its current scope
            Scope currentScope = element.GetContainingScope();

            if (currentScope != null)
            {
                currentScope.RemoveElement(element);
            }

            m_ContainedElements.Add(element);

            // To update the scope geometry whenever the added element's geometry changes
            element.RegisterCallback<GeometryChangedEvent>(OnSubElementGeometryChanged);
            ScheduleUpdateGeometryFromContent();

            OnElementAdded(element);
        }

        protected virtual void OnElementAdded(GraphElement element)
        {
        }

        public void RemoveElement(GraphElement element)
        {
            if (element == null)
                throw new ArgumentException("Cannot remove null element from this scope");

            if (!m_ContainedElements.Contains(element))
                throw new ArgumentException("This element is not contained in this scope");

            element.UnregisterCallback<GeometryChangedEvent>(OnSubElementGeometryChanged);
            m_ContainedElements.Remove(element);
            ScheduleUpdateGeometryFromContent();

            OnElementRemoved(element);
        }

        protected virtual void OnElementRemoved(GraphElement element)
        {
        }

        private void MoveElements(Vector2 delta)
        {
            foreach (GraphElement subElement in containedElements)
            {
                Rect newGeometry = subElement.GetPosition();

                newGeometry.position += delta;
                subElement.SetPosition(newGeometry);
            }
        }

        protected void ScheduleUpdateGeometryFromContent()
        {
            if (hasPendingGeometryUpdate || !m_AutoUpdateGeometry)
                return;

            hasPendingGeometryUpdate = true;
            schedule.Execute(t => UpdateGeometryFromContent());
        }

        void MarkYogaNodeSeen(YogaNode node)
        {
            node.MarkLayoutSeen();

            for (int i = 0; i < node.Count; i++)
            {
                MarkYogaNodeSeen(node[i]);
            }
        }

        public void UpdateGeometryFromContent()
        {
            hasPendingGeometryUpdate = false;

            if (panel == null || m_IsUpdatingGeometryFromContent)
            {
                return;
            }

            m_IsUpdatingGeometryFromContent = true;

            try
            {
                GraphView graphView = GetFirstAncestorOfType<GraphView>();
                VisualElement viewport = graphView.contentViewContainer;

                // Dirty the layout of the content container to recompute the content bounding rect
                m_ContentContainer.yogaNode.MarkDirty();

                // Foce the layout to be computed right away
                this.yogaNode.CalculateLayout();

                MarkYogaNodeSeen(yogaNode);

                if (m_ContainedElements.Count > 0)
                {
                    // Match the top lef corner of the content container to the top left corner of the bounding box of the contained elements
                    Rect elemRectInLocalSpace = containedElementsRect;

                    float xOffset = elemRectInLocalSpace.x;
                    float yOffset = elemRectInLocalSpace.y;
                    Vector2 newPosition = m_ContentContainer.contentRectInViewportSpace.position - new Vector2(xOffset, yOffset);
                    Rect newGeom = GetPosition();

                    newGeom.position = newPosition;
                    SetScopePositionOnly(newGeom);
                }
            }
            finally
            {
                m_IsUpdatingGeometryFromContent = false;
            }
        }

        public override Rect GetPosition()
        {
            return new Rect(m_Position.x, m_Position.y, layout.width, layout.height);
        }

        public override void SetPosition(Rect newPos)
        {
            if (!IsValidRect(newPos) || m_Position == newPos.position)
                return;

            if (m_ContainedElements.Count == 0)
            {
                SetScopePositionOnly(newPos);
            }
            else
            {
                Vector2 delta = newPos.position - m_Position;

                m_Position = newPos.position;

                // Moves the contained elements by the same displacement
                MoveElements(delta);
            }
        }

        private void SetScopePositionOnly(Rect newPos)
        {
            m_Position = newPos.position;
            style.positionType = PositionType.Absolute;
            style.positionLeft = newPos.x;
            style.positionTop = newPos.y;
        }
    }
}
