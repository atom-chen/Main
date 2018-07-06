// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

//#define ENABLE_CAPTURE_DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Yoga;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
    internal delegate void OnStylesResolved(ICustomStyle styles);

    // pseudo states are used for common states of a widget
    // they are addressable from CSS via the pseudo state syntax ":selected" for example
    // while css class list can solve the same problem, pseudo states are a fast commonly agreed upon path for common cases.
    [Flags]
    internal enum PseudoStates
    {
        Active    = 1 << 0,     // control is currently pressed in the case of a button
        Hover     = 1 << 1,     // mouse is over control, set and removed from dispatcher automatically
        Checked   = 1 << 3,     // usually associated with toggles of some kind to change visible style
        Selected  = 1 << 4,     // selected, used to denote the current selected state and associate a visual style from CSS
        Disabled  = 1 << 5,     // control will not respond to user input
        Focus     = 1 << 6,     // control has the keyboard focus. This is activated deactivated by the dispatcher automatically
    }

    public enum PickingMode
    {
        Position, // todo better name
        Ignore
    }

    internal class VisualElementListPool
    {
        static ObjectPool<List<VisualElement>> pool = new ObjectPool<List<VisualElement>>(20);

        public static List<VisualElement> Copy(List<VisualElement> elements)
        {
            var result = pool.Get();

            result.AddRange(elements);

            return result;
        }

        public static List<VisualElement> Get(int initialCapacity = 0)
        {
            List<VisualElement> result = pool.Get();

            if (initialCapacity > 0 && result.Capacity < initialCapacity)
            {
                result.Capacity = initialCapacity;
            }
            return result;
        }

        public static void Release(List<VisualElement> elements)
        {
            elements.Clear();
            pool.Release(elements);
        }
    }
    public partial class VisualElement : Focusable, ITransform
    {
        public class VisualElementFactory : UxmlFactory<VisualElement, VisualElementUxmlTraits> {}

        public class VisualElementUxmlTraits : UxmlTraits
        {
            UxmlStringAttributeDescription m_Name;
            UxmlEnumAttributeDescription<PickingMode> m_PickingMode;
            protected UxmlIntAttributeDescription m_FocusIndex;

            public VisualElementUxmlTraits()
            {
                m_Name = new UxmlStringAttributeDescription { name = "name" };
                m_PickingMode = new UxmlEnumAttributeDescription<PickingMode> { name = "pickingMode" };
                m_FocusIndex = new UxmlIntAttributeDescription { name = "focusIndex", defaultValue = VisualElement.defaultFocusIndex };
            }

            public override IEnumerable<UxmlAttributeDescription> uxmlAttributesDescription
            {
                get
                {
                    foreach (var attr in base.uxmlAttributesDescription)
                    {
                        yield return attr;
                    }

                    yield return m_Name;
                    yield return m_PickingMode;
                    yield return m_FocusIndex;
                }
            }

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield return new UxmlChildElementDescription(typeof(VisualElement)); }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ve.name = m_Name.GetValueFromBag(bag);
                ve.pickingMode = m_PickingMode.GetValueFromBag(bag);
                ve.focusIndex = m_FocusIndex.GetValueFromBag(bag);
            }
        }

        public static readonly int defaultFocusIndex = -1;

        private static uint s_NextId;

        string m_Name;
        HashSet<string> m_ClassList;
        string m_TypeName;
        string m_FullTypeName;

        // Used for view data persistence, like expanded states.
        private string m_PersistenceKey;
        public string persistenceKey
        {
            get { return m_PersistenceKey; }
            set
            {
                if (m_PersistenceKey != value)
                {
                    m_PersistenceKey = value;

                    if (!string.IsNullOrEmpty(value))
                        Dirty(ChangeType.PersistentData);
                }
            }
        }

        // It seems worse to have unwanted/unpredictable persistence
        // than to expect it and not have it. This internal check, set
        // by Panel.ValidatePersistentDataOnSubTree() controls whether
        // persistency is enabled or not. Persistence will be disabled
        // on any VisualElement that does not have a persistenceKey, but
        // it will also be disabled if any of its parents does not have
        // a persistenceKey.
        internal bool enablePersistence { get; private set; }

        public object userData { get; set; }

        public override bool canGrabFocus { get { return visible && enabledInHierarchy && base.canGrabFocus; } }

        public override FocusController focusController
        {
            get { return panel == null ? null : panel.focusController; }
        }

        private RenderData m_RenderData;
        internal RenderData renderData
        {
            get { return m_RenderData ?? (m_RenderData = new RenderData()); }
        }

        Vector3 m_Position = Vector3.zero;
        Quaternion m_Rotation = Quaternion.identity;
        Vector3 m_Scale = Vector3.one;

        public ITransform transform
        {
            get { return this; }
        }

        Vector3 ITransform.position
        {
            get
            {
                return m_Position;
            }
            set
            {
                if (m_Position == value)
                    return;
                m_Position = value;
                Dirty(ChangeType.Transform);
            }
        }

        Quaternion ITransform.rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                if (m_Rotation == value)
                    return;
                m_Rotation = value;
                Dirty(ChangeType.Transform);
            }
        }

        Vector3 ITransform.scale
        {
            get
            {
                return m_Scale;
            }
            set
            {
                if (m_Scale == value)
                    return;
                m_Scale = value;
                Dirty(ChangeType.Transform);
            }
        }

        Matrix4x4 ITransform.matrix
        {
            get { return Matrix4x4.TRS(m_Position, m_Rotation, m_Scale); }
        }

        Rect m_Layout;

        // This will replace the Rect position
        // origin and size relative to parent
        public Rect layout
        {
            get
            {
                var result = m_Layout;
                if (yogaNode != null && style.positionType.value != PositionType.Manual)
                {
                    result.x = yogaNode.LayoutX;
                    result.y = yogaNode.LayoutY;
                    result.width = yogaNode.LayoutWidth;
                    result.height = yogaNode.LayoutHeight;
                }
                return result;
            }
            set
            {
                if (yogaNode == null)
                {
                    yogaNode = new YogaNode();
                }

                // Same position value while type is already manual should not trigger any layout change, return early
                if (style.positionType.value == PositionType.Manual && m_Layout == value)
                    return;

                // set results so we can read straight back in get without waiting for a pass
                m_Layout = value;

                // mark as inline so that they do not get overridden if needed.
                IStyle styleAccess = this;
                styleAccess.positionType = PositionType.Manual;
                styleAccess.marginLeft = 0.0f;
                styleAccess.marginRight = 0.0f;
                styleAccess.marginBottom = 0.0f;
                styleAccess.marginTop = 0.0f;
                styleAccess.positionLeft = value.x;
                styleAccess.positionTop = value.y;
                styleAccess.positionRight = float.NaN;
                styleAccess.positionBottom = float.NaN;
                styleAccess.width = value.width;
                styleAccess.height = value.height;

                Dirty(ChangeType.Transform);
            }
        }

        public Rect contentRect
        {
            get
            {
                var spacing = new Spacing(m_Style.paddingLeft,
                        m_Style.paddingTop,
                        m_Style.paddingRight,
                        m_Style.paddingBottom);

                return paddingRect - spacing;
            }
        }

        protected Rect paddingRect
        {
            get
            {
                var spacing = new Spacing(style.borderLeftWidth,
                        style.borderTopWidth,
                        style.borderRightWidth,
                        style.borderBottomWidth);

                return rect - spacing;
            }
        }

        /// <summary>
        /// AABB after applying the world transform to <c>rect</c>.
        /// </summary>
        public Rect worldBound
        {
            get
            {
                var g = worldTransform;
                var min = GUIUtility.Internal_MultiplyPoint(new Vector3(rect.min.x, rect.min.y, 1), g);
                var max = GUIUtility.Internal_MultiplyPoint(new Vector3(rect.max.x, rect.max.y, 1), g);

                // We assume that the transform performs translation/scaling without rotation.
                return Rect.MinMaxRect(Math.Min(min.x, max.x), Math.Min(min.y, max.y), Math.Max(min.x, max.x), Math.Max(min.y, max.y));
            }
        }

        /// <summary>
        /// AABB after applying the transform to the rect, but before applying the layout translation.
        /// </summary>
        public Rect localBound
        {
            get
            {
                var g = transform.matrix;
                var min = GUIUtility.Internal_MultiplyPoint(layout.min, g);
                var max = GUIUtility.Internal_MultiplyPoint(layout.max, g);

                // We assume that the transform performs translation/scaling without rotation.
                return Rect.MinMaxRect(Math.Min(min.x, max.x), Math.Min(min.y, max.y), Math.Max(min.x, max.x), Math.Max(min.y, max.y));
            }
        }

        internal Rect rect
        {
            get
            {
                return new Rect(0.0f, 0.0f, layout.width, layout.height);
            }
        }

        internal bool isWorldTransformDirty { get; set; } = true;

        /// <summary>
        /// Returns a matrix that cumulates the following operations (in order):
        /// -Local Scaling
        /// -Local Rotation
        /// -Local Translation
        /// -Layout Translation
        /// -Parent <c>worldTransform</c> (recursive definition - consider identity when there is no parent)
        /// </summary>
        /// <remarks>
        /// Multiplying the <c>layout</c> rect by this matrix is incorrect because it already contains the translation.
        /// </remarks>
        public Matrix4x4 worldTransform
        {
            get
            {
                if (IsDirty(ChangeType.Transform))
                {
                    var offset = Matrix4x4.Translate(new Vector3(layout.x, layout.y, 0));
                    if (shadow.parent != null)
                    {
                        renderData.worldTransForm = shadow.parent.worldTransform * offset * transform.matrix;
                    }
                    else
                    {
                        renderData.worldTransForm = offset * transform.matrix;
                    }
                    ClearDirty(ChangeType.Transform);
                }
                return renderData.worldTransForm;
            }
        }

        // which pseudo states would change the current VE styles if added
        internal PseudoStates triggerPseudoMask;
        // which pseudo states would change the current VE styles if removed
        internal PseudoStates dependencyPseudoMask;

        private PseudoStates m_PseudoStates;
        internal PseudoStates pseudoStates
        {
            get { return m_PseudoStates; }
            set
            {
                if (m_PseudoStates != value)
                {
                    m_PseudoStates = value;

                    if ((triggerPseudoMask & m_PseudoStates) != 0
                        || (dependencyPseudoMask & ~m_PseudoStates) != 0)
                    {
                        Dirty(ChangeType.Styles);
                    }
                }
            }
        }

        public PickingMode pickingMode { get; set; }

        // does not guarantee uniqueness
        public string name
        {
            get { return m_Name; }
            set
            {
                if (m_Name == value)
                    return;
                m_Name = value;
                Dirty(ChangeType.Styles);
            }
        }

        internal string fullTypeName
        {
            get
            {
                if (string.IsNullOrEmpty(m_FullTypeName))
                    m_FullTypeName = GetType().FullName;
                return m_FullTypeName;
            }
        }

        internal string typeName
        {
            get
            {
                if (string.IsNullOrEmpty(m_TypeName))
                {
                    var type = GetType();
                    bool isGeneric = false;
                    isGeneric = type.IsGenericType;

                    m_TypeName = isGeneric ? type.Name.Remove(type.Name.IndexOf('`')) : type.Name;
                }

                return m_TypeName;
            }
        }

        // Set and pass in values to be used for layout
        internal YogaNode yogaNode { get; private set; }

        // shared style object, cannot be changed by the user
        internal VisualElementStylesData m_SharedStyle = VisualElementStylesData.none;
        // user-defined style object, if not set, is the same reference as m_SharedStyles
        internal VisualElementStylesData m_Style = VisualElementStylesData.none;

        protected virtual void OnStyleResolved(ICustomStyle style)
        {
            // push all non inlined layout things up
            FinalizeLayout();
        }

        internal VisualElementStylesData sharedStyle
        {
            get
            {
                return m_SharedStyle;
            }
        }

        internal VisualElementStylesData effectiveStyle
        {
            get
            {
                return m_Style;
            }
        }

        internal bool hasInlineStyle
        {
            get
            {
                return m_Style != m_SharedStyle;
            }
        }

        VisualElementStylesData inlineStyle
        {
            get
            {
                if (!hasInlineStyle)
                {
                    var inline = new VisualElementStylesData(false);
                    inline.Apply(m_SharedStyle, StylePropertyApplyMode.Copy);
                    m_Style = inline;
                }
                return m_Style;
            }
        }

        // Opacity is not fully supported so it's hidden from public API for now
        internal float opacity
        {
            get
            {
                return style.opacity.value;
            }
            set
            {
                style.opacity = value;
            }
        }

        internal readonly uint controlid;

        public VisualElement()
        {
            controlid = ++s_NextId;

            shadow = new Hierarchy(this);

            m_ClassList = new HashSet<string>();
            m_FullTypeName = string.Empty;
            m_TypeName = string.Empty;
            SetEnabled(true);

            // Make element non focusable by default.
            focusIndex = defaultFocusIndex;

            name = string.Empty;
            yogaNode = new YogaNode();
            changesNeeded = ChangeType.All;
            clippingOptions = ClippingOptions.ClipContents;
        }

        protected internal override void ExecuteDefaultAction(EventBase evt)
        {
            base.ExecuteDefaultAction(evt);

            if (evt.GetEventTypeId() == MouseOverEvent.TypeId() || evt.GetEventTypeId() == MouseOutEvent.TypeId())
            {
                UpdateCursorStyle(evt.GetEventTypeId());
            }
            else if (evt.GetEventTypeId() == MouseEnterEvent.TypeId())
            {
                pseudoStates |= PseudoStates.Hover;
            }
            else if (evt.GetEventTypeId() == MouseLeaveEvent.TypeId())
            {
                pseudoStates &= ~PseudoStates.Hover;
            }
            else if (evt.GetEventTypeId() == BlurEvent.TypeId())
            {
                pseudoStates = pseudoStates & ~PseudoStates.Focus;
            }
            else if (evt.GetEventTypeId() == FocusEvent.TypeId())
            {
                pseudoStates = pseudoStates | PseudoStates.Focus;
            }
        }

        public sealed override void Focus()
        {
            if (!canGrabFocus && shadow.parent != null)
            {
                shadow.parent.Focus();
            }
            else
            {
                base.Focus();
            }
        }

        internal void SetPanel(BaseVisualElementPanel p)
        {
            if (panel == p)
                return;

            //We now gather all Elements in order to dispatch events in an efficient manner
            List<VisualElement> elements = VisualElementListPool.Get();
            try
            {
                elements.Add(this);
                GatherAllChildren(elements);

                foreach (var e in elements)
                {
                    e.ChangePanel(p);
                }
            }
            finally
            {
                VisualElementListPool.Release(elements);
            }
        }

        // This should never be called directly
        // This needs to be virtual for GraphView.cs
        // We could avoid this by having a single PanelChangeEvent
        // containing both the previous and the new panel values.
        internal virtual void ChangePanel(BaseVisualElementPanel p)
        {
            if (panel != null)
            {
                using (var e = DetachFromPanelEvent.GetPooled())
                {
                    e.target = this;
                    UIElementsUtility.eventDispatcher.DispatchEvent(e, panel);
                }
            }

            elementPanel = p;

            if (panel != null)
            {
                using (var e = AttachToPanelEvent.GetPooled())
                {
                    e.target = this;
                    UIElementsUtility.eventDispatcher.DispatchEvent(e, panel);
                }
            }

            Dirty(ChangeType.Styles);
        }

        // in the case of a Topology change the target is the parent of the removed or added element
        // TODO write test suite for this method. in particular, any change type should implicitly
        private ChangeType changesNeeded;

        // helper when change impact are known and we need to propagate down into children
        private void PropagateToChildren(ChangeType type)
        {
            // when touching we grass fire in the tree but stop when we are already touched.
            // this is a key point of the on demand dirty that keeps it from exploding.
            if ((type & changesNeeded) == type)
                return;

            changesNeeded |= type;

            // only those propagate to children
            type = type & (ChangeType.Styles | ChangeType.Transform);
            if (type == 0)
                return;

            // propagate to children
            if (m_Children != null)
            {
                foreach (var child in m_Children)
                {
                    // recurse down
                    child.PropagateToChildren(type);
                }
            }
        }

        private void PropagateChangesToParents()
        {
            ChangeType parentChanges = 0;
            if (changesNeeded != 0)
            {
                // if we have any change at all, propagate the repaint flag
                // this is somehow an implementation detail but this is the only flag that is checked in the app tick
                // this means that styles, layout, transform, etc. will be processed just before painting a panel
                // a small downside is that we might repaint more often that necessary
                // another solution would be to process all flags sequentially in the tick to check if a repaint is needed
                parentChanges |= ChangeType.Repaint;

                if ((changesNeeded & ChangeType.Styles) > 0)
                {
                    // if this visual element needs its styles recomputed, propagate this specific flags for its parents
                    // it is less expensive than a full styles re-pass
                    parentChanges |= ChangeType.StylesPath;
                }

                if ((changesNeeded & (ChangeType.PersistentData | ChangeType.PersistentDataPath)) > 0)
                {
                    // Parents do not need their OnPersistentDataReady() called but they need to call still
                    // propagate it to their children so it gets back to us.
                    parentChanges |= ChangeType.PersistentDataPath;
                }
            }

            var current = shadow.parent;
            while (current != null)
            {
                if ((current.changesNeeded & parentChanges) == parentChanges)
                    break;

                current.changesNeeded |= parentChanges;
                current = current.shadow.parent;
            }
        }

        public void Dirty(ChangeType type)
        {
            // when touching we grass fire in the tree but stop when we are already touched.
            // this is a key point of the on demand dirty that keeps it from exploding.
            if ((type & changesNeeded) == type)
                return;

            if ((type & ChangeType.Layout) == ChangeType.Layout)
            {
                if (yogaNode != null && yogaNode.IsMeasureDefined)
                {
                    yogaNode.MarkDirty();
                }
                type |= ChangeType.Repaint;
            }

            if (((type & ChangeType.Transform) == ChangeType.Transform) && (elementPanel != null))
            {
                elementPanel.hasDirtyTransform = true;
            }

            PropagateToChildren(type);

            PropagateChangesToParents();
        }

        internal bool AnyDirty()
        {
            return changesNeeded != 0;
        }

        public bool IsDirty(ChangeType type)
        {
            return (changesNeeded & type) == type;
        }

        public bool AnyDirty(ChangeType type)
        {
            return (changesNeeded & type) > 0;
        }

        public void ClearDirty(ChangeType type)
        {
            changesNeeded &= ~type;
        }

        [Obsolete("enabled is deprecated. Use SetEnabled as setter, and enabledSelf/enabledInHierarchy as getters.", true)]
        public virtual bool enabled
        {
            get { return enabledInHierarchy; }
            set { SetEnabled(value); }
        }

        private bool m_Enabled;


        //TODO: Make private once VisualContainer is merged with VisualElement
        protected internal bool SetEnabledFromHierarchy(bool state)
        {
            //returns false if state hasn't changed
            if (state == ((pseudoStates & PseudoStates.Disabled) != PseudoStates.Disabled))
                return false;

            if (state && m_Enabled && (parent == null || parent.enabledInHierarchy))
                pseudoStates &= ~PseudoStates.Disabled;
            else
                pseudoStates |= PseudoStates.Disabled;

            return true;
        }

        //Returns true if 'this' can be enabled relative to the enabled state of its panel
        public bool enabledInHierarchy
        {
            get { return (pseudoStates & PseudoStates.Disabled) != PseudoStates.Disabled; }
        }

        //Returns the local enabled state
        public bool enabledSelf
        {
            get { return m_Enabled; }
        }

        public void SetEnabled(bool value)
        {
            if (m_Enabled != value)
            {
                m_Enabled = value;

                PropagateEnabledToChildren(value);
            }
        }

        void PropagateEnabledToChildren(bool value)
        {
            if (SetEnabledFromHierarchy(value))
            {
                for (int i = 0; i < shadow.childCount; ++i)
                {
                    shadow[i].PropagateEnabledToChildren(value);
                }
            }
        }

        public bool visible
        {
            get
            {
                return style.visibility.GetSpecifiedValueOrDefault(Visibility.Visible) == Visibility.Visible;
            }
            set
            {
                // Note: this could causes an allocation because styles are copy-on-write
                // we might want to remove this setter altogether
                // so everything goes through style.visibility (and then it's documented in a single place)
                style.visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public virtual void DoRepaint()
        {
            var painter = elementPanel.stylePainter;
            painter.DrawBackground(this);
            painter.DrawBorder(this);
        }

        internal virtual void DoRepaint(IStylePainter painter)
        {
            if (visible == false)
            {
                return;
            }
            DoRepaint();
        }

        private void GetFullHierarchicalPersistenceKey(StringBuilder key)
        {
            const string keySeparator = "__";

            if (parent != null)
                parent.GetFullHierarchicalPersistenceKey(key);

            if (!string.IsNullOrEmpty(persistenceKey))
            {
                key.Append(keySeparator);
                key.Append(persistenceKey);
            }
        }

        public string GetFullHierarchicalPersistenceKey()
        {
            StringBuilder key = new StringBuilder();

            GetFullHierarchicalPersistenceKey(key);

            return key.ToString();
        }

        public T GetOrCreatePersistentData<T>(object existing, string key) where T : class, new()
        {
            Debug.Assert(elementPanel != null, "VisualElement.elementPanel is null! Cannot load persistent data.");

            var persistentData = elementPanel == null || elementPanel.getViewDataDictionary == null ? null : elementPanel.getViewDataDictionary();

            // If persistency is disable (no data, no key, no key one of the parents), just return the
            // existing data or create a local one if none exists.
            if (persistentData == null || string.IsNullOrEmpty(persistenceKey) || enablePersistence == false)
            {
                if (existing != null)
                    return existing as T;

                return new T();
            }

            string keyWithType = key + "__" + typeof(T).ToString();

            if (!persistentData.ContainsKey(keyWithType))
                persistentData.Set(keyWithType, new T());

            return persistentData.Get<T>(keyWithType);
        }

        public T GetOrCreatePersistentData<T>(ScriptableObject existing, string key) where T : ScriptableObject
        {
            Debug.Assert(elementPanel != null, "VisualElement.elementPanel is null! Cannot load persistent data.");

            var persistentData = elementPanel == null || elementPanel.getViewDataDictionary == null ? null : elementPanel.getViewDataDictionary();

            // If persistency is disable (no data, no key, no key one of the parents), just return the
            // existing data or create a local one if none exists.
            if (persistentData == null || string.IsNullOrEmpty(persistenceKey) || enablePersistence == false)
            {
                if (existing != null)
                    return existing as T;

                return ScriptableObject.CreateInstance<T>();
            }

            string keyWithType = key + "__" + typeof(T).ToString();

            if (!persistentData.ContainsKey(keyWithType))
                persistentData.Set(keyWithType, ScriptableObject.CreateInstance<T>());

            return persistentData.GetScriptable<T>(keyWithType);
        }

        public void OverwriteFromPersistedData(object obj, string key)
        {
            Debug.Assert(elementPanel != null, "VisualElement.elementPanel is null! Cannot load persistent data.");

            var persistentData = elementPanel == null || elementPanel.getViewDataDictionary == null ? null : elementPanel.getViewDataDictionary();

            // If persistency is disable (no data, no key, no key one of the parents), just return the
            // existing data or create a local one if none exists.
            if (persistentData == null || string.IsNullOrEmpty(persistenceKey) || enablePersistence == false)
            {
                return;
            }

            string keyWithType = key + "__" + obj.GetType();

            if (!persistentData.ContainsKey(keyWithType))
            {
                persistentData.Set(keyWithType, obj);
                return;
            }

            persistentData.Overwrite(obj, keyWithType);
        }

        public void SavePersistentData()
        {
            if (elementPanel != null && elementPanel.savePersistentViewData != null && !string.IsNullOrEmpty(persistenceKey))
                elementPanel.savePersistentViewData();
        }

        internal bool IsPersitenceSupportedOnChildren()
        {
            // We relax here the requirement that ALL parents of a VisualElement
            // need to have a persistenceKey for persistence to work. Plain
            // VisualElements are likely to be used just for layouting and
            // grouping and requiring a key on each element is a bit tedious.
            if (this.GetType() == typeof(VisualElement))
                return true;

            if (string.IsNullOrEmpty(persistenceKey))
                return false;

            return true;
        }

        internal void OnPersistentDataReady(bool enablePersistence)
        {
            this.enablePersistence = enablePersistence;
            OnPersistentDataReady();
        }

        public virtual void OnPersistentDataReady() {}

        // position should be in local space
        // override to customize intersection between point and shape
        public virtual bool ContainsPoint(Vector2 localPoint)
        {
            return rect.Contains(localPoint);
        }

        public virtual bool Overlaps(Rect rectangle)
        {
            return rect.Overlaps(rectangle, true);
        }

        public enum MeasureMode
        {
            Undefined = YogaMeasureMode.Undefined,
            Exactly = YogaMeasureMode.Exactly,
            AtMost = YogaMeasureMode.AtMost
        }

        private bool m_RequireMeasureFunction = false;
        internal bool requireMeasureFunction
        {
            get { return m_RequireMeasureFunction; }
            set
            {
                m_RequireMeasureFunction = value;
                if (m_RequireMeasureFunction && !yogaNode.IsMeasureDefined)
                {
                    yogaNode.SetMeasureFunction(Measure);
                }
                else if (!m_RequireMeasureFunction && yogaNode.IsMeasureDefined)
                {
                    yogaNode.SetMeasureFunction(null);
                }
            }
        }

        protected internal virtual Vector2 DoMeasure(float width, MeasureMode widthMode, float height, MeasureMode heightMode)
        {
            return new Vector2(float.NaN, float.NaN);
        }

        internal YogaSize Measure(YogaNode node, float width, YogaMeasureMode widthMode, float height, YogaMeasureMode heightMode)
        {
            Debug.Assert(node == yogaNode, "YogaNode instance mismatch");
            Vector2 size = DoMeasure(width, (MeasureMode)widthMode, height, (MeasureMode)heightMode);
            return MeasureOutput.Make(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));
        }

        public void SetSize(Vector2 size)
        {
            var pos = layout;
            pos.width = size.x;
            pos.height = size.y;
            layout = pos;
        }

        internal const Align DefaultAlignContent = Align.FlexStart;
        internal const Align DefaultAlignItems = Align.Stretch;

        void FinalizeLayout()
        {
            yogaNode.Flex = style.flex.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.FlexBasis = style.flexBasis.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.FlexGrow = style.flexGrow.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.FlexShrink = style.flexShrink.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.Left = style.positionLeft.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.Top = style.positionTop.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.Right = style.positionRight.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.Bottom = style.positionBottom.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.MarginLeft = style.marginLeft.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.MarginTop = style.marginTop.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.MarginRight = style.marginRight.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.MarginBottom = style.marginBottom.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.PaddingLeft = style.paddingLeft.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.PaddingTop = style.paddingTop.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.PaddingRight = style.paddingRight.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.PaddingBottom = style.paddingBottom.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.BorderLeftWidth = style.borderLeft.GetSpecifiedValueOrDefault(style.borderLeftWidth.GetSpecifiedValueOrDefault(float.NaN));
            yogaNode.BorderTopWidth = style.borderTop.GetSpecifiedValueOrDefault(style.borderTopWidth.GetSpecifiedValueOrDefault(float.NaN));
            yogaNode.BorderRightWidth = style.borderRight.GetSpecifiedValueOrDefault(style.borderRightWidth.GetSpecifiedValueOrDefault(float.NaN));
            yogaNode.BorderBottomWidth = style.borderBottom.GetSpecifiedValueOrDefault(style.borderBottomWidth.GetSpecifiedValueOrDefault(float.NaN));
            yogaNode.Width = style.width.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.Height = style.height.GetSpecifiedValueOrDefault(float.NaN);

            PositionType posType = style.positionType;
            switch (posType)
            {
                case PositionType.Absolute:
                case PositionType.Manual:
                    yogaNode.PositionType = YogaPositionType.Absolute;
                    break;
                case PositionType.Relative:
                    yogaNode.PositionType = YogaPositionType.Relative;
                    break;
            }

            yogaNode.Overflow = (YogaOverflow)(style.overflow.value);
            yogaNode.AlignSelf = (YogaAlign)(style.alignSelf.value);
            yogaNode.MaxWidth = style.maxWidth.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.MaxHeight = style.maxHeight.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.MinWidth = style.minWidth.GetSpecifiedValueOrDefault(float.NaN);
            yogaNode.MinHeight = style.minHeight.GetSpecifiedValueOrDefault(float.NaN);

            // Note: the following applies to VisualContainer only
            // but it won't cause any trouble and we avoid making this method virtual
            yogaNode.FlexDirection = (YogaFlexDirection)style.flexDirection.value;
            yogaNode.AlignContent = (YogaAlign)style.alignContent.GetSpecifiedValueOrDefault(DefaultAlignContent);
            yogaNode.AlignItems = (YogaAlign)style.alignItems.GetSpecifiedValueOrDefault(DefaultAlignItems);
            yogaNode.JustifyContent = (YogaJustify)style.justifyContent.value;
            yogaNode.Wrap = (YogaWrap)style.flexWrap.value;

            Dirty(ChangeType.Layout);
            Dirty(ChangeType.Transform);
        }

        internal event OnStylesResolved onStylesResolved;

        // for internal use only, used by asset instantiation to push local styles
        // likely can be replaced by merging VisualContainer and VisualElement
        // and then storing the inline sheet in the list held by VisualContainer
        internal void SetInlineStyles(VisualElementStylesData inlineStyle)
        {
            Debug.Assert(!inlineStyle.isShared);
            inlineStyle.Apply(m_Style, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity);
            m_Style = inlineStyle;
        }

        internal void SetSharedStyles(VisualElementStylesData sharedStyle)
        {
            Debug.Assert(sharedStyle.isShared);

            ClearDirty(ChangeType.StylesPath | ChangeType.Styles);
            if (sharedStyle == m_SharedStyle)
            {
                return;
            }

            if (hasInlineStyle)
            {
                m_Style.Apply(sharedStyle, StylePropertyApplyMode.CopyIfNotInline);
            }
            else
            {
                m_Style = sharedStyle;
            }

            m_SharedStyle = sharedStyle;

            if (onStylesResolved != null)
            {
                onStylesResolved(m_Style);
            }
            OnStyleResolved(m_Style);
            Dirty(ChangeType.Repaint);
        }

        public void ResetPositionProperties()
        {
            if (!hasInlineStyle)
            {
                return;
            }
            VisualElementStylesData styleAccess = inlineStyle;
            styleAccess.positionType = StyleValue<int>.nil;
            styleAccess.marginLeft = StyleValue<float>.nil;
            styleAccess.marginRight = StyleValue<float>.nil;
            styleAccess.marginBottom = StyleValue<float>.nil;
            styleAccess.marginTop = StyleValue<float>.nil;
            styleAccess.positionLeft = StyleValue<float>.nil;
            styleAccess.positionTop = StyleValue<float>.nil;
            styleAccess.positionRight = StyleValue<float>.nil;
            styleAccess.positionBottom = StyleValue<float>.nil;
            styleAccess.width = StyleValue<float>.nil;
            styleAccess.height = StyleValue<float>.nil;

            // Make sure to retrieve shared styles from the shared style sheet and update CSSNode again
            m_Style.Apply(sharedStyle, StylePropertyApplyMode.CopyIfNotInline);
            FinalizeLayout();

            Dirty(ChangeType.Layout);
        }

        public override string ToString()
        {
            return GetType().Name + " " + name + " " + layout + " world rect: " + worldBound;
        }

        // WARNING returning the HashSet means it could be modified, be careful
        internal IEnumerable<string> GetClasses()
        {
            return m_ClassList;
        }

        public void ClearClassList()
        {
            if (m_ClassList != null && m_ClassList.Count > 0)
            {
                m_ClassList.Clear();
                Dirty(ChangeType.Styles);
            }
        }

        public void AddToClassList(string className)
        {
            if (m_ClassList == null)
                m_ClassList = new HashSet<string>();

            if (m_ClassList.Add(className))
            {
                Dirty(ChangeType.Styles);
            }
        }

        public void RemoveFromClassList(string className)
        {
            if (m_ClassList != null && m_ClassList.Remove(className))
            {
                Dirty(ChangeType.Styles);
            }
        }

        public void ToggleInClassList(string className)
        {
            if (ClassListContains(className))
                RemoveFromClassList(className);
            else
                AddToClassList(className);
        }

        public void EnableInClassList(string className, bool enable)
        {
            if (enable)
                AddToClassList(className);
            else
                RemoveFromClassList(className);
        }

        public bool ClassListContains(string cls)
        {
            return m_ClassList != null && m_ClassList.Contains(cls);
        }

        public object FindAncestorUserData()
        {
            VisualElement p = parent;

            while (p != null)
            {
                if (p.userData != null)
                    return p.userData;
                p = p.parent;
            }

            return null;
        }

        private void UpdateCursorStyle(long eventType)
        {
            if (elementPanel != null)
            {
                if (eventType == MouseOverEvent.TypeId())
                {
                    elementPanel.cursorManager.SetCursor(style.cursor.value);
                }
                else
                {
                    elementPanel.cursorManager.ResetCursor();
                }
            }
        }
    }

    public static class VisualElementExtensions
    {
        // transforms a point assumed in Panel space to the referential inside of the element bound (local)
        public static Vector2 WorldToLocal(this VisualElement ele, Vector2 p)
        {
            return ele.worldTransform.inverse.MultiplyPoint3x4((Vector3)p);
        }

        // transforms a point to Panel space referential
        public static Vector2 LocalToWorld(this VisualElement ele, Vector2 p)
        {
            return (Vector2)ele.worldTransform.MultiplyPoint3x4((Vector3)p);
        }

        // transforms a rect assumed in Panel space to the referential inside of the element bound (local)
        public static Rect WorldToLocal(this VisualElement ele, Rect r)
        {
            var inv = ele.worldTransform.inverse;
            Vector2 position = inv.MultiplyPoint3x4((Vector2)r.position);
            r.position = position;
            r.size = inv.MultiplyVector(r.size);
            return r;
        }

        // transforms a rect to Panel space referential
        public static Rect LocalToWorld(this VisualElement ele, Rect r)
        {
            var toWorldMatrix = ele.worldTransform;
            r.position = toWorldMatrix.MultiplyPoint3x4(r.position);
            r.size = toWorldMatrix.MultiplyVector(r.size);
            return r;
        }

        // transform point from the local space of one element to to the local space of another
        public static Vector2 ChangeCoordinatesTo(this VisualElement src, VisualElement dest, Vector2 point)
        {
            return dest.WorldToLocal(src.LocalToWorld(point));
        }

        // transform Rect from the local space of one element to to the local space of another
        public static Rect ChangeCoordinatesTo(this VisualElement src, VisualElement dest, Rect rect)
        {
            return dest.WorldToLocal(src.LocalToWorld(rect));
        }

        public static void StretchToParentSize(this VisualElement elem)
        {
            IStyle styleAccess = elem.style;
            styleAccess.positionType = PositionType.Absolute;
            styleAccess.positionLeft = 0.0f;
            styleAccess.positionTop = 0.0f;
            styleAccess.positionRight = 0.0f;
            styleAccess.positionBottom = 0.0f;
        }

        public static void StretchToParentWidth(this VisualElement elem)
        {
            IStyle styleAccess = elem.style;
            styleAccess.positionType = PositionType.Absolute;
            styleAccess.positionLeft = 0.0f;
            styleAccess.positionRight = 0.0f;
        }

        public static void AddManipulator(this VisualElement ele, IManipulator manipulator)
        {
            if (manipulator != null)
            {
                manipulator.target = ele;
            }
        }

        public static void RemoveManipulator(this VisualElement ele, IManipulator manipulator)
        {
            if (manipulator != null)
            {
                manipulator.target = null;
            }
        }
    }

    internal static class StylePainterExtensionMethods
    {
        internal static TextureStylePainterParameters GetDefaultTextureParameters(this IStylePainter painter, VisualElement ve)
        {
            IStyle style = ve.style;
            var painterParams = new TextureStylePainterParameters
            {
                rect = GUIUtility.AlignRectToDevice(ve.rect),
                uv = new Rect(0, 0, 1, 1),
                color = Color.white,
                texture = style.backgroundImage,
                scaleMode = style.backgroundSize,
                sliceLeft = style.sliceLeft,
                sliceTop = style.sliceTop,
                sliceRight = style.sliceRight,
                sliceBottom = style.sliceBottom
            };
            painter.SetBorderFromStyle(ref painterParams.border, style);
            return painterParams;
        }

        internal static RectStylePainterParameters GetDefaultRectParameters(this IStylePainter painter, VisualElement ve)
        {
            IStyle style = ve.style;
            var painterParams = new RectStylePainterParameters
            {
                rect = GUIUtility.AlignRectToDevice(ve.rect),
                color = style.backgroundColor,
            };
            painter.SetBorderFromStyle(ref painterParams.border, style);
            return painterParams;
        }

        internal static TextStylePainterParameters GetDefaultTextParameters(this IStylePainter painter, BaseTextElement te)
        {
            IStyle style = te.style;
            var painterParams = new TextStylePainterParameters
            {
                rect = te.contentRect,
                text = te.text,
                font = style.font,
                fontSize = style.fontSize,
                fontStyle = style.fontStyle,
                fontColor = style.textColor.GetSpecifiedValueOrDefault(Color.black),
                anchor = style.textAlignment,
                wordWrap = style.wordWrap,
                wordWrapWidth = style.wordWrap ? te.contentRect.width : 0.0f,
                richText = false,
                clipping = style.textClipping
            };
            return painterParams;
        }

        internal static CursorPositionStylePainterParameters GetDefaultCursorPositionParameters(this IStylePainter painter, BaseTextElement te)
        {
            IStyle style = te.style;
            var painterParams = new CursorPositionStylePainterParameters() {
                rect = te.contentRect,
                text = te.text,
                font = style.font,
                fontSize = style.fontSize,
                fontStyle = style.fontStyle,
                anchor = style.textAlignment,
                wordWrapWidth = style.wordWrap ? te.contentRect.width : 0.0f,
                richText = false,
                cursorIndex = 0
            };
            return painterParams;
        }

        internal static void DrawBackground(this IStylePainter painter, VisualElement ve)
        {
            IStyle style = ve.style;
            if (style.backgroundColor != Color.clear)
            {
                var painterParams = painter.GetDefaultRectParameters(ve);
                painterParams.border.SetWidth(0.0f);
                painter.DrawRect(painterParams);
            }

            if (style.backgroundImage.value != null)
            {
                var painterParams = painter.GetDefaultTextureParameters(ve);
                painterParams.border.SetWidth(0.0f);
                painter.DrawTexture(painterParams);
            }
        }

        internal static void DrawBorder(this IStylePainter painter, VisualElement ve)
        {
            IStyle style = ve.style;
            if (style.borderColor != Color.clear && (style.borderLeftWidth > 0.0f || style.borderTopWidth > 0.0f || style.borderRightWidth > 0.0f || style.borderBottomWidth > 0.0f))
            {
                var painterParams = painter.GetDefaultRectParameters(ve);
                painterParams.color = style.borderColor;
                painter.DrawRect(painterParams);
            }
        }

        internal static void DrawText(this IStylePainter painter, BaseTextElement te)
        {
            if (!string.IsNullOrEmpty(te.text) && te.contentRect.width > 0.0f && te.contentRect.height > 0.0f)
            {
                painter.DrawText(painter.GetDefaultTextParameters(te));
            }
        }

        internal static void SetBorderFromStyle(this IStylePainter painter, ref BorderParameters border, IStyle style)
        {
            border.SetWidth(style.borderTopWidth, style.borderRightWidth, style.borderBottomWidth, style.borderLeftWidth);
            border.SetRadius(style.borderTopLeftRadius, style.borderTopRightRadius, style.borderBottomRightRadius, style.borderBottomLeftRadius);
        }
    }
}
