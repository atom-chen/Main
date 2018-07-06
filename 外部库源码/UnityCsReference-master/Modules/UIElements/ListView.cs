// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
    public class ListView : VisualElement
    {
        public class ListViewFactory : UxmlFactory<ListView, ListViewUxmlTraits> {}

        public class ListViewUxmlTraits : VisualElementUxmlTraits
        {
            UxmlIntAttributeDescription m_ItemHeight;

            public ListViewUxmlTraits()
            {
                m_ItemHeight = new UxmlIntAttributeDescription { name = "itemHeight", defaultValue = k_DefaultItemHeight };
            }

            public override IEnumerable<UxmlAttributeDescription> uxmlAttributesDescription
            {
                get
                {
                    foreach (var attr in base.uxmlAttributesDescription)
                    {
                        yield return attr;
                    }

                    yield return m_ItemHeight;
                }
            }

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((ListView)ve).itemHeight = m_ItemHeight.GetValueFromBag(bag);
            }
        }

        private class RecycledItem
        {
            public readonly VisualElement element;
            public int index;

            public RecycledItem(VisualElement element)
            {
                this.element = element;
            }

            internal void SetSelected(bool selected)
            {
                if (selected)
                    element.pseudoStates |= PseudoStates.Selected;
                else
                    element.pseudoStates &= ~PseudoStates.Selected;
            }
        }

        public event Action<object> onItemChosen;
        public event Action<List<object>> onSelectionChanged;

        private IList m_ItemsSource;
        public IList itemsSource
        {
            get { return m_ItemsSource; }
            set
            {
                m_ItemsSource = value;
                Refresh();
            }
        }

        Func<VisualElement> m_MakeItem;
        public Func<VisualElement> makeItem
        {
            get
            {
                return m_MakeItem;
            }
            set
            {
                m_MakeItem = value;
                Refresh();
            }
        }

        private Action<VisualElement, int> m_BindItem;
        public Action<VisualElement, int> bindItem
        {
            get
            {
                return m_BindItem;
            }
            set
            {
                m_BindItem = value;
                Refresh();
            }
        }

        private StyleValue<int> m_ItemHeight;
        public int itemHeight
        {
            get
            {
                return m_ItemHeight.GetSpecifiedValueOrDefault(k_DefaultItemHeight);
            }
            set
            {
                m_ItemHeight = value;
                Refresh();
            }
        }

        // Persisted.
        [SerializeField]
        private float m_ScrollOffset;

        // Persisted. It's why this can't be a HashSet(). :(
        [SerializeField]
        private List<int> m_SelectedIndices = new List<int>();

        public int selectedIndex
        {
            get { return m_SelectedIndices.Count == 0 ? -1 : m_SelectedIndices.First(); }
            set { SetSelection(value); }
        }

        public object selectedItem { get { return m_ItemsSource == null ? null : m_ItemsSource[selectedIndex]; } }

        public override VisualElement contentContainer { get { return m_ScrollView.contentContainer; } }

        public SelectionType selectionType { get; set; }

        private const int k_DefaultItemHeight = 30;
        private const string k_ItemHeightProperty = "-unity-item-height";

        private int m_FirstVisibleIndex;
        private Rect m_LastSize;
        private List<RecycledItem> m_Pool = new List<RecycledItem>();
        private ScrollView m_ScrollView;

        private const int m_ExtraVisibleItems = 2;
        private int m_VisibleItemCount;

        public ListView()
        {
            selectionType = SelectionType.Single;
            m_ScrollOffset = 0.0f;

            m_ScrollView = new ScrollView();
            m_ScrollView.StretchToParentSize();
            m_ScrollView.verticalScroller.valueChanged += OnScroll;
            shadow.Add(m_ScrollView);

            RegisterCallback<GeometryChangedEvent>(OnSizeChanged);

            m_ScrollView.contentContainer.RegisterCallback<MouseDownEvent>(OnClick);
            m_ScrollView.contentContainer.RegisterCallback<KeyDownEvent>(OnKeyDown);
            m_ScrollView.contentContainer.focusIndex = 0;

            schedule.Execute(() =>
                {
                    Dirty(ChangeType.Layout);
                    m_ScrollView.Focus();
                }).StartingIn(1);
        }

        public ListView(IList itemsSource, int itemHeight, Func<VisualElement> makeItem, Action<VisualElement, int> bindItem) : this()
        {
            m_ItemsSource = itemsSource;
            m_ItemHeight = itemHeight;
            m_MakeItem = makeItem;
            m_BindItem = bindItem;
        }

        public void OnKeyDown(KeyDownEvent evt)
        {
            if (!HasValidDataAndBindings())
                return;

            switch (evt.keyCode)
            {
                case KeyCode.UpArrow:
                    if (selectedIndex > 0)
                        selectedIndex = selectedIndex - 1;
                    break;
                case KeyCode.DownArrow:
                    if (selectedIndex + 1 < itemsSource.Count)
                        selectedIndex = selectedIndex + 1;
                    break;
                case KeyCode.Home:
                    selectedIndex = 0;
                    break;
                case KeyCode.End:
                    selectedIndex = itemsSource.Count - 1;
                    break;
                case KeyCode.PageDown:
                    selectedIndex = Math.Min(itemsSource.Count - 1, selectedIndex + (int)(m_LastSize.height / itemHeight));
                    break;
                case KeyCode.PageUp:
                    selectedIndex = Math.Max(0, selectedIndex - (int)(m_LastSize.height / itemHeight));
                    break;
            }
            ScrollToItem(selectedIndex);
        }

        public void ScrollToItem(int index)
        {
            if (!HasValidDataAndBindings())
                throw new InvalidOperationException("Can't scroll without valid source, bind method, or factory method.");

            if (m_VisibleItemCount == 0)
                return;
            if (m_FirstVisibleIndex > index)
            {
                m_ScrollView.scrollOffset = Vector2.up * itemHeight * index;
            }
            else // index >= first
            {
                int actualCount = (int)(m_LastSize.height / itemHeight);
                if (index < m_FirstVisibleIndex + actualCount)
                    return;
                m_ScrollView.scrollOffset = Vector2.up * itemHeight * (index - actualCount);
            }
        }

        private void OnClick(MouseDownEvent evt)
        {
            if (!HasValidDataAndBindings())
                return;

            if (evt.button != (int)MouseButton.LeftMouse)
                return;

            var clickedIndex = (int)(evt.localMousePosition.y / itemHeight);
            switch (evt.clickCount)
            {
                case 1:
                    if (selectionType == SelectionType.None)
                        return;

                    if (selectionType == SelectionType.Multiple && evt.ctrlKey)
                        if (m_SelectedIndices.Contains(clickedIndex))
                            RemoveFromSelection(clickedIndex);
                        else
                            AddToSelection(clickedIndex);
                    else // single
                        SetSelection(clickedIndex);
                    break;
                case 2:
                    if (onItemChosen == null)
                        return;

                    onItemChosen.Invoke(itemsSource[clickedIndex]);
                    break;
            }
        }

        protected void AddToSelection(int index)
        {
            if (!HasValidDataAndBindings())
                return;

            foreach (var recycledItem in m_Pool)
                if (recycledItem.index == index)
                    recycledItem.SetSelected(true);

            if (!m_SelectedIndices.Contains(index))
                m_SelectedIndices.Add(index);

            SelectionChanged();

            SavePersistentData();
        }

        protected void RemoveFromSelection(int index)
        {
            if (!HasValidDataAndBindings())
                return;

            foreach (var recycledItem in m_Pool)
                if (recycledItem.index == index)
                    recycledItem.SetSelected(false);

            if (m_SelectedIndices.Contains(index))
                m_SelectedIndices.Remove(index);

            SelectionChanged();

            SavePersistentData();
        }

        protected void SetSelection(int index)
        {
            if (!HasValidDataAndBindings())
                return;

            foreach (var recycledItem in m_Pool)
                recycledItem.SetSelected(recycledItem.index == index);
            m_SelectedIndices.Clear();
            if (index >= 0)
                m_SelectedIndices.Add(index);

            SelectionChanged();

            SavePersistentData();
        }

        private void SelectionChanged()
        {
            if (!HasValidDataAndBindings())
                return;

            if (onSelectionChanged == null)
                return;

            var selectedItems = new List<object>();
            foreach (var i in m_SelectedIndices)
                selectedItems.Add(itemsSource[i]);

            onSelectionChanged.Invoke(selectedItems);
        }

        protected void ClearSelection()
        {
            if (!HasValidDataAndBindings())
                return;

            foreach (var recycledItem in m_Pool)
                recycledItem.SetSelected(false);
            m_SelectedIndices.Clear();

            SelectionChanged();
        }

        public void ScrollTo(VisualElement visualElement)
        {
            m_ScrollView.ScrollTo(visualElement);
        }

        public override void OnPersistentDataReady()
        {
            base.OnPersistentDataReady();

            string key = GetFullHierarchicalPersistenceKey();

            OverwriteFromPersistedData(this, key);
        }

        private void OnScroll(float offset)
        {
            if (!HasValidDataAndBindings())
                return;

            m_ScrollOffset = offset;
            m_FirstVisibleIndex = (int)(offset / itemHeight);
            m_ScrollView.contentContainer.style.height = itemsSource.Count * itemHeight;

            for (var i = 0; i < m_Pool.Count && i + m_FirstVisibleIndex < itemsSource.Count; i++)
                Setup(m_Pool[i], i + m_FirstVisibleIndex);
        }

        private bool HasValidDataAndBindings()
        {
            return itemsSource != null && makeItem != null && bindItem != null;
        }

        public void Refresh()
        {
            m_Pool.Clear();
            m_ScrollView.Clear();

            m_ScrollView.contentContainer.style.width = m_ScrollView.contentViewport.layout.width;
            m_ScrollView.contentContainer.style.flex = 0;

            if (!HasValidDataAndBindings())
                return;

            // Resize ScrollView in case the collection has changed.
            m_ScrollView.contentContainer.style.height = itemsSource.Count * itemHeight;

            // Restore scroll offset and pre-emptively update the highValue
            // in case this is the initial restore from persistent data and
            // the ScrollView's OnGeometryChanged() didn't update the low
            // and highValues.
            m_ScrollView.verticalScroller.highValue = Mathf.Max(m_ScrollOffset, m_ScrollView.verticalScroller.highValue);
            m_ScrollView.verticalScroller.value = m_ScrollOffset;

            if (m_LastSize != m_ScrollView.layout)
                m_LastSize = m_ScrollView.layout;

            if (float.IsNaN(m_LastSize.height))
                return;

            m_ScrollView.contentContainer.style.height = itemsSource.Count * itemHeight;

            m_VisibleItemCount = (int)(m_LastSize.height / itemHeight) + m_ExtraVisibleItems;
            for (var i = m_FirstVisibleIndex; i < m_VisibleItemCount + m_FirstVisibleIndex; i++)
            {
                var item = makeItem();
                var recycledItem = new RecycledItem(item);
                m_Pool.Add(recycledItem);

                item.style.marginTop = 0;
                item.style.marginBottom = 0;
                item.style.positionType = PositionType.Absolute;
                item.style.positionLeft = 0;
                item.style.positionRight = 0;
                item.style.height = itemHeight;
                if (i < itemsSource.Count)
                {
                    item.style.visibility = Visibility.Visible;
                    Setup(recycledItem, i);
                }
                else
                {
                    item.style.visibility = Visibility.Hidden;
                }

                m_ScrollView.Add(item);
            }

            schedule.Execute(() => { Dirty(ChangeType.Layout); });
        }

        private void Setup(RecycledItem recycledItem, int newIndex)
        {
            Assert.IsTrue(newIndex < itemsSource.Count);
            recycledItem.index = newIndex;
            recycledItem.element.style.positionTop = recycledItem.index * itemHeight;
            recycledItem.element.style.positionBottom = (itemsSource.Count - recycledItem.index - 1) * itemHeight;
            bindItem(recycledItem.element, recycledItem.index);
            recycledItem.SetSelected(m_SelectedIndices.Contains(newIndex));
        }

        private void OnSizeChanged(GeometryChangedEvent evt)
        {
            if (!HasValidDataAndBindings())
                return;

            m_ScrollView.contentContainer.style.height = itemsSource.Count * itemHeight;

            if (m_LastSize == m_ScrollView.layout)
                return;

            Refresh();
        }

        protected override void OnStyleResolved(ICustomStyle styles)
        {
            base.OnStyleResolved(styles);

            styles.ApplyCustomProperty(k_ItemHeightProperty, ref m_ItemHeight);

            Refresh();
        }
    }
}
