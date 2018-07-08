// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.UIElements
{
    [Serializable]
    internal class VisualElementAsset : IUxmlAttributes, ISerializationCallbackReceiver
    {
        [SerializeField]
        private string m_Name;

        [SerializeField]
        private int m_Id;

        // TODO 2018.3 [Obsolete("Use GetPropertyString(\"name\", null) instead.")]
        public string name
        {
            get { return GetPropertyString("name", null); }
            set { SetOrAddProperty("name", value); }
        }

        public int id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        [SerializeField]
        private int m_ParentId;

        public int parentId
        {
            get { return m_ParentId; }
            set { m_ParentId = value; }
        }

        [SerializeField]
        private int m_RuleIndex;

        public int ruleIndex
        {
            get { return m_RuleIndex; }
            set { m_RuleIndex = value; }
        }

        [SerializeField]
        private string m_Text;

        // TODO 2018.3 [Obsolete("Use GetPropertyString(\"text\", null) instead.")]
        public string text
        {
            get { return GetPropertyString("text", null); }
            set { SetOrAddProperty("text", value); }
        }

        [SerializeField]
        private PickingMode m_PickingMode;

        // TODO 2018.3 [Obsolete("Use GetPropertyString(\"pickingMode\", null) instead.")]
        public string pickingMode
        {
            get { return GetPropertyString("pickingMode", null); }
            set { SetOrAddProperty("pickingMode", value); }
        }

        [SerializeField]
        private string m_FullTypeName;

        public string fullTypeName
        {
            get { return m_FullTypeName; }
            set { m_FullTypeName = value; }
        }

        [SerializeField]
        private string[] m_Classes;

        public string[] classes
        {
            get { return m_Classes; }
            set { m_Classes = value; }
        }

        [SerializeField]
        private List<string> m_Stylesheets;

        public List<string> stylesheets
        {
            get { return m_Stylesheets == null ? (m_Stylesheets = new List<string>()) : m_Stylesheets; }
            set { m_Stylesheets = value; }
        }

        [SerializeField]
        private List<string> m_Properties;

        public VisualElementAsset(string fullTypeName)
        {
            m_FullTypeName = fullTypeName;
            m_Name = String.Empty;
            m_Text = String.Empty;
            m_PickingMode = PickingMode.Position;
        }

        public VisualElement Create(CreationContext ctx)
        {
            List<IUxmlFactory> factoryList;
            if (!VisualElementFactoryRegistry.TryGetValue(fullTypeName, out factoryList))
            {
                Debug.LogErrorFormat("Element '{0}' has no registered factory method.", fullTypeName);
                return new Label(string.Format("Unknown type: '{0}'", fullTypeName));
            }

            IUxmlFactory factory = null;
            foreach (IUxmlFactory f in factoryList)
            {
                if (f.AcceptsAttributeBag(this))
                {
                    factory = f;
                    break;
                }
            }

            if (factory == null)
            {
                Debug.LogErrorFormat("Element '{0}' has a no factory that accept the set of XML attributes specified.", fullTypeName);
                return new Label(string.Format("Type with no factory: '{0}'", fullTypeName));
            }

            if (factory is UxmlRootElementFactory)
            {
                return null;
            }

            VisualElement res = factory.Create(this, ctx);
            if (res == null)
            {
                Debug.LogErrorFormat("The factory of Visual Element Type '{0}' has returned a null object", fullTypeName);
                return new Label(string.Format("The factory of Visual Element Type '{0}' has returned a null object", fullTypeName));
            }

            if (classes != null)
            {
                for (int i = 0; i < classes.Length; i++)
                    res.AddToClassList(classes[i]);
            }

            if (stylesheets != null)
            {
                for (int i = 0; i < stylesheets.Count; i++)
                    res.AddStyleSheetPath(stylesheets[i]);
            }

            return res;
        }

        public void OnBeforeSerialize() {}

        public void OnAfterDeserialize()
        {
            // These properties were previously treated in a special way.
            // Now they are trated like all other properties. Put them in
            // the property list.
            if (!m_Properties.Contains("name"))
            {
                AddProperty("name", m_Name);
            }
            if (!m_Properties.Contains("text"))
            {
                AddProperty("text", m_Text);
            }
            if (!m_Properties.Contains("pickingMode"))
            {
                AddProperty("pickingMode", m_PickingMode.ToString());
            }
        }

        public void AddProperty(string propertyName, string propertyValue)
        {
            SetOrAddProperty(propertyName, propertyValue);
        }

        void SetOrAddProperty(string propertyName, string propertyValue)
        {
            if (m_Properties == null)
                m_Properties = new List<string>();

            for (int i = 0; i < m_Properties.Count - 1; i += 2)
            {
                if (m_Properties[i] == propertyName)
                {
                    m_Properties[i + 1] = propertyValue;
                    return;
                }
            }

            m_Properties.Add(propertyName);
            m_Properties.Add(propertyValue);
        }

        // TODO 2018.3 [Obsolete("Use GetPropertyString(string propertyName, string defaultValue)")]
        public virtual string GetPropertyString(string propertyName)
        {
            return GetPropertyString(propertyName, null);
        }

        public virtual string GetPropertyString(string propertyName, string defaultValue)
        {
            if (m_Properties == null)
                return defaultValue;

            for (int i = 0; i < m_Properties.Count - 1; i += 2)
            {
                if (m_Properties[i] == propertyName)
                    return m_Properties[i + 1];
            }

            return defaultValue;
        }

        public int GetPropertyInt(string propertyName, int defaultValue)
        {
            var v = GetPropertyString(propertyName, null);
            int l;
            if (v == null || !int.TryParse(v, out l))
                return defaultValue;

            return l;
        }

        public long GetPropertyLong(string propertyName, long defaultValue)
        {
            var v = GetPropertyString(propertyName, null);
            long l;
            if (v == null || !long.TryParse(v, out l))
                return defaultValue;
            return l;
        }

        public bool GetPropertyBool(string propertyName, bool defaultValue)
        {
            var v = GetPropertyString(propertyName, null);
            bool l;
            if (v == null || !bool.TryParse(v, out l))
                return defaultValue;

            return l;
        }

        public Color GetPropertyColor(string propertyName, Color defaultValue)
        {
            var v = GetPropertyString(propertyName, null);
            Color l;
            if (v == null || !ColorUtility.TryParseHtmlString(v, out l))
                return defaultValue;
            return l;
        }

        public float GetPropertyFloat(string propertyName, float defaultValue)
        {
            var v = GetPropertyString(propertyName, null);
            float l;
            if (v == null || !float.TryParse(v, out l))
                return defaultValue;
            return l;
        }

        public double GetPropertyDouble(string propertyName, double defaultValue)
        {
            var v = GetPropertyString(propertyName, null);
            double l;
            if (v == null || !double.TryParse(v, out l))
                return defaultValue;
            return l;
        }

        public T GetPropertyEnum<T>(string propertyName, T defaultValue)
        {
            var v = GetPropertyString(propertyName, null);
            if (v == null || !Enum.IsDefined(typeof(T), v))
                return defaultValue;

            var l = (T)Enum.Parse(typeof(T), v);
            return l;
        }
    }
}
