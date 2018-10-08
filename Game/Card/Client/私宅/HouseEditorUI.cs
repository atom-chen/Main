using System;
using System.Collections;
using System.Collections.Generic;
using Games;
using Games.GlobeDefine;
using Games.LogicObj;
using Games.Table;
using UnityEngine;


public class HouseEditorUI : UIControllerBase<HouseEditorUI>
{
    public PageWrapController mCardListWrap;
    public TabController mBagTabCtrl;
    public HouseHeadMenu mHeadMenu;
    public UIToggle mShowMainPlayer;
    public UILabel mPage;

    private List<Card> mList = null;

    private CardSortType[] SortTypeMap = { CardSortType.Star, CardSortType.NewGet, CardSortType.NewBattle };

    private HouseScene mScene;
    
    void OnEnable()
    {
        SetInstance(this);

        mScene = GameManager.CurScene as HouseScene;
        if (mScene != null)
        {
            mScene.DelSceneAddCard += OnAddCardInScene;
            mScene.DelSceneDelCard += OnDelCardInScene;
        }

        if (null != GameManager.PlayerDataPool.PlayerCardBag)
        {
            mList = GameManager.PlayerDataPool.PlayerCardBag.CardList;
            if (null != mList)
            {
                mCardListWrap.Init(mList.Count, OnUpdateItem, OnUpdatePage);
            }
        }

        SortList(CardSortType.Star);
        mBagTabCtrl.delTabChanged = OnCardFilterChanged;
    }

    void OnDisable()
    {
        if (mScene != null)
        {
            mScene.DelSceneAddCard -= OnAddCardInScene;
            mScene.DelSceneDelCard -= OnDelCardInScene;
        }
    }

    void OnCardFilterChanged(TabButton button)
    {
        if (button.Index < 0 || button.Index >= SortTypeMap.Length)
            return;

        var curSortType = SortTypeMap[button.Index];
        SortList(curSortType);
    }

    void SortList(CardSortType sortType)
    {
        if (sortType == CardSortType.Invalid)
            return;

        Comparison<Card> sortFunc = CardTool.GetSortFunc(sortType);
        if (sortFunc == null)
            return;

        if (null != mList)
        {
            mList.Sort(sortFunc);
        }

        mCardListWrap.Refresh();
    }

    public void OnAddCardInScene(HouseScene.SceneCardInfo ci)
    {
        mCardListWrap.Refresh();
    }

    public void OnDelCardInScene(UInt64 cardGuid)
    {
        mCardListWrap.Refresh();
    }

    private void OnUpdateItem(GameObject item, int index)
    {
        //if (item == null || null == mList)
        //{
        //    return;
        //}

        //if (index < 0 || index >= mList.Count)
        //{
        //    item.SetActive(false);
        //    return;
        //}

        //Card card = mList[index];
        //if (card == null || !card.IsValid())
        //{
        //    item.SetActive(false);
        //    return;
        //}

        //var itemLogic = item.GetComponent<BattleCardItem>();
        //if (itemLogic == null)
        //{
        //    item.SetActive(false);
        //    return;
        //}

        //item.SetActive(true);
        //itemLogic.Refresh(card);
        //itemLogic.onClickCard = OnClickCard;
        //itemLogic.onDragCard = OnDragCard;
        //itemLogic.onDraggingCard = OnDraggingCard;

        //if (mScene != null)
        //{
        //    bool has = mScene.HasCard(card.Guid);
        //    itemLogic.SetSelected(has);
        //}
    }

    private void OnUpdatePage(int cur, int total)
    {
        mPage.text = string.Format("{0}/{1}", cur, total);
    }

    public void OnClickPageUp()
    {
        mCardListWrap.PageUp();
    }

    public void OnClickPageDown()
    {
        mCardListWrap.PageDown();
    }

    void OnClickCard(Card card, BattleCardItem item)
    {
        if (card == null)
            return;

        var scene = GameManager.CurScene as HouseScene;
        if (scene == null)
            return;

        if (scene.HasCard(card.Guid))
        {
            scene.TakeCard(card.Guid);
            item.SetSelected(false);
            Utils.CenterNotice(StrDictionary.GetDicByID(7329, card.GetName()));
        }
        //else
        //{
        //    if (scene.CanPutCard(card))
        //    {
        //        Vector3 pos = ObjManager.MainPlayer.Position;
        //        Vector3 rot = ObjManager.MainPlayer.transform.eulerAngles;
        //        scene.PutCard(card, pos, rot, false);
        //        item.SetSelected(true);
        //    }
        //    else
        //    {
        //        Utils.CenterNotice("9999");
        //    }
        //}
    }

    //public void OnDragCard(Card card, BattleCardItem item, bool start)
    //{
    //    var scene = GameManager.CurScene as HouseScene;
    //    if (scene == null)
    //        return;
        
    //    if (start)
    //    {
    //        scene.DragStart(card);
    //    }
    //    else
    //    {
    //        scene.DragEnd(card);
    //    }
    //}

    public void OnDraggingCard(Card card, BattleCardItem item)
    {
    }

    public void OnCloseClick()
    {
        //var scene = GameManager.CurScene as HouseScene;
        //if (scene == null)
        //{
        //    return;
        //}
        //scene.SwitchMode(HouseScene.HouseMode.NORMAL);
    }

    public void OnClickShowMainCharacter()
    {
        if (ObjManager.MainPlayer != null)
        {
            ObjManager.MainPlayer.SetVisible(ObjVisibleLayer.Story, mShowMainPlayer.value);
        }
    }

    //void Update()
    //{
    //    var scene = GameManager.CurScene as HouseScene;
    //    if (scene == null || scene.InMode(HouseScene.HouseMode.NORMAL) || scene.EditCard == null)
    //    {
    //        mHeadMenu.SetTarget(null);
    //        return;
    //    }
        
    //    mHeadMenu.SetTarget(scene.EditCard.ObjTransform);
    //}
}