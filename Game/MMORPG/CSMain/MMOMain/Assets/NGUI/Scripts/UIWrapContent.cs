//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This script makes it possible for a scroll view to wrap its content, creating endless scroll views.
/// Usage: simply attach this script underneath your scroll view where you would normally place a UIGrid:
/// 
/// + Scroll View
/// |- UIWrappedContent
/// |-- Item 1
/// |-- Item 2
/// |-- Item 3
/// </summary>

[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class UIWrapContent : MonoBehaviour
{
	public delegate void OnInitializeItem (GameObject go, int wrapIndex, int realIndex);

	/// <summary>
	/// Width or height of the child items for positioning purposes.
	/// </summary>

	public int itemSize = 100;

    public int itemHeight = 100;

	/// <summary>
	/// Whether the content will be automatically culled. Enabling this will improve performance in scroll views that contain a lot of items.
	/// </summary>

	public bool cullContent = true;

	/// <summary>
	/// Minimum allowed index for items. If "min" is equal to "max" then there is no limit.
	/// For vertical scroll views indices increment with the Y position (towards top of the screen).
	/// </summary>

	public int minIndex = 0;

	/// <summary>
	/// Maximum allowed index for items. If "min" is equal to "max" then there is no limit.
	/// For vertical scroll views indices increment with the Y position (towards top of the screen).
	/// </summary>

	public int maxIndex = 0;

    //列数
    public int column = 1;

    /// <summary>
    /// Callback that will be called every time an item needs to have its content updated.
    /// The 'wrapIndex' is the index within the child list, and 'realIndex' is the index using position logic.
    /// </summary>

    public OnInitializeItem onInitializeItem;

	protected Transform mTrans;
	protected UIPanel mPanel;
	protected UIScrollView mScroll;
	protected bool mHorizontal = false;
	protected bool mFirstTime = true;
	protected BetterList<Transform> mChildren = new BetterList<Transform>();

	/// <summary>
	/// Initialize everything and register a callback with the UIPanel to be notified when the clipping region moves.
	/// </summary>

    public void Init()
    {
        mTrans = null;
        mPanel = null;
        mScroll = null;
        mHorizontal = false;
        mFirstTime = true;
        mChildren.Clear();

        SortBasedOnScrollMovement();
        WrapContent();
        if (mScroll != null) mScroll.GetComponent<UIPanel>().onClipMove = OnMove;
        mFirstTime = false;
    }

	/// <summary>
	/// Callback triggered by the UIPanel when its clipping region moves (for example when it's being scrolled).
	/// </summary>

	protected virtual void OnMove (UIPanel panel) { WrapContent(); }

	/// <summary>
	/// Immediately reposition all children.
	/// </summary>

	[ContextMenu("Sort Based on Scroll Movement")]
	public virtual void SortBasedOnScrollMovement ()
	{
		if (!CacheScrollView()) return;

		// Cache all children and place them in order
		mChildren.Clear();
		for (int i = 0; i < mTrans.childCount; ++i)
			mChildren.Add(mTrans.GetChild(i));

		// Sort the list of children so that they are in order
		if (mHorizontal) mChildren.Sort(UIGrid.SortHorizontal);
		else mChildren.Sort(UIGrid.SortVertical);
		ResetChildPositions();
	}

	/// <summary>
	/// Immediately reposition all children, sorting them alphabetically.
	/// </summary>

	[ContextMenu("Sort Alphabetically")]
	public virtual void SortAlphabetically ()
	{
		if (!CacheScrollView()) return;

		// Cache all children and place them in order
		mChildren.Clear();
		for (int i = 0; i < mTrans.childCount; ++i)
			mChildren.Add(mTrans.GetChild(i));

		// Sort the list of children so that they are in order
		mChildren.Sort(UIGrid.SortByName);
		ResetChildPositions();
	}

	/// <summary>
	/// Cache the scroll view and return 'false' if the scroll view is not found.
	/// </summary>

	protected bool CacheScrollView ()
	{
		mTrans = transform;
		mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
		mScroll = mPanel.GetComponent<UIScrollView>();
		if (mScroll == null) return false;
		if (mScroll.movement == UIScrollView.Movement.Horizontal) mHorizontal = true;
		else if (mScroll.movement == UIScrollView.Movement.Vertical) mHorizontal = false;
		else return false;
		return true;
	}

	/// <summary>
	/// Helper function that resets the position of all the children.
	/// </summary>

	protected virtual void ResetChildPositions ()
	{
        int col = 0;
        int row = 0;

        for (int i = 0, imax = mChildren.size; i < imax; ++i)
		{
			Transform t = mChildren[i];
            if (mHorizontal)
            {
                t.localPosition = new Vector3(row * itemSize, -col * itemHeight, 0f);
            }
            else
            {
                t.localPosition = new Vector3(col * itemSize, -row * itemHeight, 0f);
            }
            col++;
            if (col >= column)
            {
                col = 0;
                row++;
            }
        }
	}

	/// <summary>
	/// Wrap all content, repositioning all children as needed.
	/// </summary>

	public virtual void WrapContent ()
	{
		
		Vector3[] corners = mPanel.worldCorners;
		
		for (int i = 0; i < 4; ++i)
		{
			Vector3 v = corners[i];
			v = mTrans.InverseTransformPoint(v);
			corners[i] = v;
		}
		
		Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
		bool allWithinRange = true;


		if (mHorizontal)
		{
            float offset = itemSize * mChildren.size / column;
            float extents = offset * 0.5f;

            float min = corners[0].x - itemSize;
			float max = corners[2].x + itemSize;

			for (int i = 0, imax = mChildren.size; i < imax; ++i)
			{
				Transform t = mChildren[i];
				float distance = t.localPosition.x - center.x;

				if (distance < -extents)
				{
					Vector3 pos = t.localPosition;
					pos.x += offset;
					distance = pos.x - center.x;
				    int realIndex = CalcRealIndex(pos);

					if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
					{
						t.localPosition = pos;
						UpdateItem(t, i);
					}
					else allWithinRange = false;
				}
				else if (distance > extents)
				{
					Vector3 pos = t.localPosition;
					pos.x -= offset;
					distance = pos.x - center.x;
                    int realIndex = CalcRealIndex(pos);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
					{
						t.localPosition = pos;
						UpdateItem(t, i);
					}
					else allWithinRange = false;
				}
				else if (mFirstTime) UpdateItem(t, i);

				if (cullContent)
				{
					distance += mPanel.clipOffset.x - mTrans.localPosition.x;
					if (!UICamera.IsPressed(t.gameObject))
						NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
				}
			}
		}
		else
		{
            float offset = itemHeight * mChildren.size / column;
            float extents = offset * 0.5f;

            float min = corners[0].y - itemHeight;
			float max = corners[2].y + itemHeight;

			for (int i = 0, imax = mChildren.size; i < imax; ++i)
			{
				Transform t = mChildren[i];
				float distance = t.localPosition.y - center.y;

				if (distance < -extents)
				{
					Vector3 pos = t.localPosition;
					pos.y += offset;
					distance = pos.y - center.y;
                    int realIndex = CalcRealIndex(pos);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
					{
						t.localPosition = pos;
						UpdateItem(t, i);
					}
					else allWithinRange = false;
				}
				else if (distance > extents)
				{
					Vector3 pos = t.localPosition;
					pos.y -= offset;
					distance = pos.y - center.y;
                    int realIndex = CalcRealIndex(pos);

                    if (minIndex == maxIndex || (minIndex <= realIndex && realIndex <= maxIndex))
					{
						t.localPosition = pos;
						UpdateItem(t, i);
					}
					else allWithinRange = false;
				}
				else if (mFirstTime) UpdateItem(t, i);

				if (cullContent)
				{
					distance += mPanel.clipOffset.y - mTrans.localPosition.y;
					if (!UICamera.IsPressed(t.gameObject))
						NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
				}
			}
		}
		mScroll.restrictWithinPanel = !allWithinRange;
	}

	/// <summary>
	/// Sanity checks.
	/// </summary>

	void OnValidate ()
	{
		if (maxIndex < minIndex)
			maxIndex = minIndex;
		if (minIndex > maxIndex)
			maxIndex = minIndex;
	}

	/// <summary>
	/// Want to update the content of items as they are scrolled? Override this function.
	/// </summary>

	protected virtual void UpdateItem (Transform item, int index)
	{
		if (onInitializeItem != null)
		{
			onInitializeItem(item.gameObject, index, CalcRealIndex(item.localPosition));
		}
	}

    private int CalcRealIndex(Vector3 pos)
    {
        if (mScroll.movement == UIScrollView.Movement.Vertical)
        {
            return Mathf.RoundToInt(pos.y/itemHeight)*column - Mathf.RoundToInt(pos.x/itemSize);
        }
        else
        {
            return Mathf.RoundToInt(pos.x / itemSize) * column + Mathf.RoundToInt(pos.y / itemHeight);
        }
    }

    public void UpdateAllItem()
    {
        for (int i = 0, imax = mChildren.size; i < imax; ++i)
        {
            Transform t = mChildren[i];
            UpdateItem(t, i);
        }
    }

    public int GetChildCount()
    {
        return mChildren.size;
    }

    //让指定项在页内，仅设定坐标，若要内容更新，在此后需要WrapContent
    public void SetItemOnPage(int realIndex)
    {
        if (realIndex < 0 || mChildren.size == 0)
            return;
        SetOnPage(realIndex / mChildren.size);
    }
    //指定页,仅设定坐标，若要内容更新，在此后需要WrapContent
    protected void SetOnPage(int nPage)
    {
        if (nPage <0 || mChildren.size == 0 || column == 0)
            return;
        int pageWidth = 0;
        int pageHeight = 0;
        if (mHorizontal)
        {
            pageWidth = (mChildren.size / column) * itemSize;
            pageHeight = column * itemHeight;
        }
        else
        {
            pageWidth = column * itemSize;
            pageHeight = (mChildren.size / column) * itemHeight;
        }
        int col = 0;
        int row = 0;
        for (int i = 0, imax = mChildren.size; i < imax; ++i)
        {
            Transform t = mChildren[i];
            if (mHorizontal)
            {
                t.localPosition = new Vector3(row * itemSize + nPage * pageWidth, -col * itemHeight, 0f);
            }
            else
            {
                t.localPosition = new Vector3(col * itemSize, -row * itemHeight -nPage * pageHeight, 0f);
            }
            col++;
            if (col >= column)
            {
                col = 0;
                row++;
            }
        }
    }
}
