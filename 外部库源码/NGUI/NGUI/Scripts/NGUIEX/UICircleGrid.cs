using UnityEngine;
using System.Collections.Generic;
using System;

public class UICircleGrid : UIWidgetContainer
{
	public delegate void OnReposition ();

	public enum Sorting
	{
		None,
		Alphabetic,
		Custom,
        Number,
	}

	/// <summary>
	/// How to sort the grid's elements.
	/// </summary>

	public Sorting sorting = Sorting.None;

    public float angleOffset = 0f;
    public float centerRadius = 100f;
    public float ringBetween = 10f;

    /// <summary>
    /// Maximum children per line.
    /// If the arrangement is horizontal, this denotes the number of columns.
    /// If the arrangement is vertical, this stands for the number of rows.
    /// </summary>

    public int maxPerLine = 0;

	/// <summary>
	/// Whether the grid will smoothly animate its children into the correct place.
	/// </summary>

	public bool animateSmoothly = false;

	/// <summary>
	/// Whether to ignore the disabled children or to treat them as being present.
	/// </summary>

	public bool hideInactive = false;

	/// <summary>
	/// Whether the parent container will be notified of the grid's changes.
	/// </summary>

	public bool keepWithinPanel = false;

	/// <summary>
	/// Callback triggered when the grid repositions its contents.
	/// </summary>

	public OnReposition onReposition;

	/// <summary>
	/// Custom sort delegate, used when the sorting method is set to 'custom'.
	/// </summary>

	public System.Comparison<Transform> onCustomSort;

	// Use the 'sorting' property instead
	[HideInInspector][SerializeField] bool sorted = false;

	protected bool mReposition = false;
	protected UIPanel mPanel;
	protected bool mInitDone = false;

	/// <summary>
	/// Reposition the children on the next Update().
	/// </summary>

	public bool repositionNow { set { if (value) { mReposition = true; enabled = true; } } }

	/// <summary>
	/// Get the current list of the grid's children.
	/// </summary>

	public List<Transform> GetChildList ()
	{
		Transform myTrans = transform;
		List<Transform> list = new List<Transform>();

		for (int i = 0; i < myTrans.childCount; ++i)
		{
			Transform t = myTrans.GetChild(i);
			if (!hideInactive || (t && NGUITools.GetActive(t.gameObject)))
				list.Add(t);
		}

        // Sort the list using the desired sorting logic
        if (sorting == Sorting.Alphabetic) list.Sort(SortByName);
        else if (onCustomSort != null) list.Sort(onCustomSort);
        else if (sorting == Sorting.Number) list.Sort(SortNumber);
        else Sort(list);

        return list;
	}

	/// <summary>
	/// Convenience method: get the child at the specified index.
	/// Note that if you plan on calling this function more than once, it's faster to get the entire list using GetChildList() instead.
	/// </summary>

	public Transform GetChild (int index)
	{
		List<Transform> list = GetChildList();
		return (index < list.Count) ? list[index] : null;
	}

	/// <summary>
	/// Get the index of the specified item.
	/// </summary>

	public int GetIndex (Transform trans) { return GetChildList().IndexOf(trans); }

	/// <summary>
	/// Convenience method -- add a new child.
	/// </summary>

	public void AddChild (Transform trans) { AddChild(trans, true); }

	/// <summary>
	/// Convenience method -- add a new child.
	/// Note that if you plan on adding multiple objects, it's faster to GetChildList() and modify that instead.
	/// </summary>

	public void AddChild (Transform trans, bool sort)
	{
		if (trans != null)
		{
			trans.parent = transform;
			ResetPosition(GetChildList());
		}
	}

    /// <summary>
    /// Remove the specified child from the list.
    /// Note that if you plan on removing multiple objects, it's faster to GetChildList() and modify that instead.
    /// </summary>

    public bool RemoveChild (Transform t)
	{
		List<Transform> list = GetChildList();

		if (list.Remove(t))
		{
			ResetPosition(list);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Initialize the grid. Executed only once.
	/// </summary>

	protected virtual void Init ()
	{
		mInitDone = true;
		mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
	}

	/// <summary>
	/// Cache everything and reset the initial position of all children.
	/// </summary>

	protected virtual void Start ()
	{
		if (!mInitDone) Init();
		bool smooth = animateSmoothly;
		animateSmoothly = false;
		Reposition();
		animateSmoothly = smooth;
		enabled = false;
	}

	/// <summary>
	/// Reset the position if necessary, then disable the component.
	/// </summary>

	protected virtual void Update ()
	{
		Reposition();
		enabled = false;
	}

	/// <summary>
	/// Reposition the content on inspector validation.
	/// </summary>

	void OnValidate () { if (!Application.isPlaying && NGUITools.GetActive(this)) Reposition(); }

	// Various generic sorting functions
	static public int SortByName (Transform a, Transform b) { return string.Compare(a.name, b.name); }
    static public int SortNumber (Transform a, Transform b)
    {
        int num_a = 0;
        bool ret = Int32.TryParse(a.name, out num_a);

        int num_b = 0;
        Int32.TryParse(b.name, out num_b);

        if (num_a < num_b)
        {
            return -1;
        }
        else if (num_a == num_b)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
	/// <summary>
	/// You can override this function, but in most cases it's easier to just set the onCustomSort delegate instead.
	/// </summary>

	protected virtual void Sort (List<Transform> list) { }

	/// <summary>
	/// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
	/// </summary>

	[ContextMenu("Execute")]
	public virtual void Reposition ()
	{
		if (Application.isPlaying && !mInitDone && NGUITools.GetActive(gameObject)) Init();

		// Legacy functionality
		if (sorted)
		{
			sorted = false;
			if (sorting == Sorting.None)
				sorting = Sorting.Alphabetic;
			NGUITools.SetDirty(this);
		}

		// Get the list of children in their current order
		List<Transform> list = GetChildList();

		// Reset the position and order of all objects in the list
		ResetPosition(list);

		// Constrain everything to be within the panel's bounds
		if (keepWithinPanel) ConstrainWithinPanel();

		// Notify the listener
		if (onReposition != null)
			onReposition();
	}

	/// <summary>
	/// Constrain the grid's content to be within the panel's bounds.
	/// </summary>

	public void ConstrainWithinPanel ()
	{
		if (mPanel != null)
		{
			mPanel.ConstrainTargetToBounds(transform, true);
			UIScrollView sv = mPanel.GetComponent<UIScrollView>();
			if (sv != null) sv.UpdateScrollbars(true);
		}
	}

	/// <summary>
	/// Reset the position of all child objects based on the order of items in the list.
	/// </summary>

	protected virtual void ResetPosition (List<Transform> list)
	{
		mReposition = false;

		int x = 0;
		int y = 0;

	    int nCircleItemCount = maxPerLine > 0 ? maxPerLine : list.Count;
	    float rRadius = centerRadius;

        // Re-add the children in the same order we have them in and position them accordingly
        for (int i = 0, imax = list.Count; i < imax; ++i)
		{
			Transform t = list[i];

			Vector3 pos = t.localPosition;
			float depth = pos.z;

            pos = new Vector3(
                rRadius * Mathf.Sin(Mathf.PI * 2 / nCircleItemCount * i + angleOffset / Mathf.PI * 2), 
                rRadius * Mathf.Cos(Mathf.PI * 2 / nCircleItemCount * i + angleOffset / Mathf.PI * 2), 
                depth);

			if (animateSmoothly && Application.isPlaying && Vector3.SqrMagnitude(t.localPosition - pos) >= 0.0001f)
			{
				SpringPosition sp = SpringPosition.Begin(t.gameObject, pos, 15f);
				sp.updateScrollView = true;
				sp.ignoreTimeScale = true;
			}
			else t.localPosition = pos;

			if (++x >= maxPerLine && maxPerLine > 0)
			{
				x = 0;
				++y;

                rRadius += ringBetween;
			}
		}
	}
}
