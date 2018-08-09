using System.Collections;
using System.Collections.Generic;
using Games.GlobeDefine;
using Games.Table;
using ProtobufPacket;
using UnityEngine;

namespace Games.LogicObj
{
    partial class Obj_Char
    {
        public BattleRoleData BattleRoleData;
        private bool m_IsAlive;
        private Dictionary<string, SkillLogic> m_LoadedSkills = new Dictionary<string, SkillLogic>();
        private SkillLogic m_CurSkill;

        public SkillLogic CurSkill
        {
            get { return m_CurSkill; }
            set { m_CurSkill = value; }
        }

        public bool IsShowInActQ { get; set; }

        private BattleHeadInfoLogic m_BattleHeadInfoLogic;

        public BattleHeadInfoLogic BattleHeadInfoLogic
        {
            get { return m_BattleHeadInfoLogic; }
        }

        private bool m_IsShowHeadInfo = false;

        private UIBossHeadInfo m_BossHeadInfo;

        public void InitForBattle(BattleRoleData roleData)
        {
            BattleRoleData = roleData;
            m_IsAlive = true;

            //战斗始终显示阴影片
            ShadowTrans.gameObject.SetActive(true);
            if (roleData != null && roleData.IsHeroCard())
            {
                m_HeroBattleAnimTab = TableManager.GetHeroBattleAnimByID(roleData.HeroId, 0);
                if (m_HeroBattleAnimTab != null)
                {
                    ChangeDefAnim(m_HeroBattleAnimTab.IdleStandAnim, true);
                }
            }
        }

        public void InitNameBoardForBattle()
        {
            m_BattleHeadInfoLogic = null;

            BattleController ctrl = BattleController.CurBattleController;
            if (ctrl == null)
            {
                return;
            }
            if (BattleRoleData == null)
            {
                return;
            }

            //boss,加载特殊血条，普通角色，加载常规血条
            Tab_RoleBaseAttr tabRoleBase = TableManager.GetRoleBaseAttrByID(BattleRoleData.RoleBaseID, 0);
            if (tabRoleBase == null)
            {
                return;
            }

            if (tabRoleBase.Type == (int) RoleType.Boss)
            {
                UIBossHeadInfo.shouldShow = true;
                UIManager.ShowUI(UIInfo.UIBossHeadInfo, OnOpenBossHeadInfo, null);
            }

            UIPathData info = BattleRoleData.Side == ctrl.OurSide ? UIInfo.BattleHeadInfo : UIInfo.BattleHeadInfoEnemy;
            AssetManager.LoadHeadInfoPrefab(info, gameObject, info.bundleName, OnLoadHeadInfo);
        }

        private void OnOpenBossHeadInfo(bool bSuccess, object o)
        {
            if (gameObject == null)
            {
                UIManager.CloseUI(UIInfo.UIBossHeadInfo);
                return;
            }
            if (UIBossHeadInfo.Ins == null)
            {
                return;
            }
            m_BossHeadInfo = UIBossHeadInfo.Ins;
            UIBossHeadInfo.Ins.Refresh(BattleRoleData);
        }

        void OnLoadHeadInfo(GameObject resHeadInfo)
        {
            if (resHeadInfo == null)
            {
                return;
            }

            if (this == null || gameObject == null)
            {
                return;
            }

            HeadInfo = resHeadInfo;
            m_BattleHeadInfoLogic = resHeadInfo.GetComponent<BattleHeadInfoLogic>();
            if (m_BattleHeadInfoLogic == null)
            {
                return;
            }
            m_BattleHeadInfoLogic.SetActive(m_IsShowHeadInfo);
            m_BattleHeadInfoLogic.Init();
            m_BattleHeadInfoLogic.Refresh(BattleRoleData);

            NameBoard nameBoardLogic = resHeadInfo.GetComponent<NameBoard>();
            if (nameBoardLogic != null)
            {
                nameBoardLogic.IsBattleNameBoard = true;
            }
        }

        public void SetBattleNameBoardActive(bool active)
        {
            m_IsShowHeadInfo = active;
            if (m_BattleHeadInfoLogic != null)
            {
                m_BattleHeadInfoLogic.SetActive(active);
            }
        }

        public bool CastSkill(int skillID, List<Hit> hits, int targetID)
        {
            Tab_SkillEx tab = TableManager.GetSkillExByID(skillID, 0);

            if (tab == null)
            {
                LogModule.WarningLog("No Such Skill ID:" + skillID);
                return false;
            }

            //if (string.IsNullOrEmpty(tab.SkillAnim))
            //{
            //    return false;
            //}

            //SkillLogic logic = LoadSkillLogic(skillID, tab.SkillAnim);
            string skilAnim = GetRealSkillAnim(tab);
            if (string.IsNullOrEmpty(skilAnim))
            {
                return false;
            }
            SkillLogic logic = LoadSkillLogic(skillID, skilAnim);
            if (logic == null)
            {
                return false;
            }

            StopSkill();

            logic.Caster = this;
            if (targetID != -1)
            {
                logic.TargetSelect = BattleController.CurBattleController.GetObjByRoleID(targetID);
            }
            logic.HitInfos = new List<Hit>(hits);
            logic.SkillID = skillID;
            logic.SkillExTab = tab;
            logic.processHit = ProcessHit;
            m_CurSkill = logic;
            logic.Cast();
            return true;
        }

        private void ProcessHit(Hit hit, int index)
        {
            if (BattleController.CurBattleController == null)
            {
                return;
            }
            if (hit == null || hit.hitResults == null)
            {
                return;
            }
            if (index == -1)
            {
                foreach (var result in hit.hitResults)
                {
                    BattleController.CurBattleController.ExcuteBattleEvents(result.events);
                }
            }
            else
            {
                if (index >= 0 && index < hit.hitResults.Count)
                {
                    BattleController.CurBattleController.ExcuteBattleEvents(
                        hit.hitResults[index].events);
                }
            }
        }

        public SkillLogic LoadSkillLogic(int skillID, string skillAnim = null)
        {
            if (skillAnim == null)
            {
                var tab = TableManager.GetSkillExByID(skillID, 0);

                if (tab == null)
                {
                    LogModule.WarningLog("No Such Skill ID:" + skillID);
                    return null;
                }

                skillAnim = GetRealSkillAnim(tab);
            }
            if (string.IsNullOrEmpty(skillAnim))
            {
                return null;
            }
            SkillLogic outLogic = null;
            if (m_LoadedSkills.TryGetValue(skillAnim, out outLogic))
            {
                return outLogic;
            }

            //Load
            GameObject res = AssetManager.LoadResource("Skill/" + skillAnim) as GameObject;
            if (res == null)
            {
                LogModule.WarningLog(string.Format("Load skill {0} failed,No Such Skill Anim :{1}", skillID, skillAnim));
                return null;
            }
            GameObject go = Instantiate(res, Vector3.zero, Quaternion.identity) as GameObject;
            if (go == null)
            {
                return null;
            }
#if UNITY_EDITOR
            go.name = "Skill[" + skillID + "]" + skillAnim;
#endif
            go.transform.SetParent(transform);
            SkillLogic logic = go.GetComponent<SkillLogic>();
            if (logic == null)
            {
                LogModule.WarningLog("BattleSkill prefab don not have SkillBehaviour!,SkillID:" + skillID);
                Destroy(go);
                return null;
            }
            m_LoadedSkills[skillAnim] = logic;
            //m_LoadedSkills.Add(skillID, logic);
            return logic;
        }

        public bool IsSkillDone()
        {
            if (m_CurSkill == null)
                return true;
            return m_CurSkill.IsSkillDone();
        }

        public void Dodge(Obj from)
        {
            //LogModule.DebugLog("Dodge");
        }

        public void BeKilled(int killerID)
        {
            m_IsAlive = false;
            Die(killerID);
            if (ObjMinionLogic != null)
            {
                ObjMinionLogic.OwnerBeKilled(killerID);
            }
        }

        public void BeHit(HitEvent e)
        {
            if (BattleRoleData == null || e == null)
            {
                return;
            }

            HitType hitType = (HitType) e.hitType;

            switch (hitType)
            {
                case HitType.Damage:
                case HitType.CritDamage:
                case HitType.EnvEnhance:
                case HitType.EnvResist:
                case HitType.EnvEnhanceCrit:
                case HitType.EnvResistCrit:
                {
                    if (e.val > 0)
                    {
                        _BeDamage(e);
                    }
                    break;
                }
                case HitType.Heal:
                {
                    if (e.val > 0)
                    {
                        BattleRoleData.HP += e.val;
                        PlayHitText(e);
                        if (BattleController.CurBattleController != null)
                        {
                            BattleController.CurBattleController.StatistHeal(e.senderID,e.val);
                        }
                    }
                    break;
                }
                case HitType.GainSp:
                {
                    if (e.val > 0)
                    {
                        BattleRoleData.SP += e.val;
                    }
                    break;
                }
                case HitType.CostSp:
                {
                    if (e.val > 0)
                    {
                        BattleRoleData.SP -= e.val;
                    }
                    break;
                }
                case HitType.Interrupt:
                {
                    PlayAnim(e.val);
                    break;
                }
                case HitType.Sacrifice:
                {
                    BattleRoleData.HP -= e.val;
                    PlayHitText(e);
                    break;
                }
                case HitType.Immue:
                case HitType.IncAp:
                case HitType.DescAp:
                case HitType.Absorb:
                case HitType.Dispell:
                case HitType.Tips:
                case HitType.ActGain:
                {
                    PlayHitText(e);
                    break;
                }
                case HitType.Guard:
                {
                    StartCoroutine(_Guard(e));
                    break;
                }
                case HitType.PassiveEffected:
                {
                    _PassiveEffected(e);
                    break;
                }
                case HitType.BombBlast:
                {
                    _BombBlast(e);
                    break;
                }
                case HitType.MaxHPChange:
                {
                    _MaxHPChange(e);
                    break;
                }
                case HitType.ShiledChange:
                {
                    _ShiledChange(e);
                    break;
                }
            }
        }

        private void _BeDamage(HitEvent e)
        {
            if (BattleRoleData == null || e == null)
            {
                return;
            }

            Tab_CharModel tab = GetBaseCharModelTab();
            if (tab != null)
            {
                PlayAnim(tab.HitAnimId);
            }
            PlayHurtSound();
            BattleRoleData.HP -= e.val;
            //yield return new WaitForSeconds(0.1f);
            PlayHitText(e);

            if (BattleController.CurBattleController != null)
            {
                BattleController.CurBattleController.StatistDamage(e.targetID, e.senderID, e.val);
            }
        }

        //private IEnumerator _ReflactDamage(HitEvent e)
        //{
        //    BattleController ctrl = BattleController.CurBattleController;
        //    if (ctrl == null)
        //    {
        //        yield break;
        //    }
        //    Obj_Char sender = ctrl.GetObjByRoleID(e.senderID);
        //    if (sender != null)
        //    {
        //        //动画
        //        sender.PlayAnim((int) CHAR_ANIM_ID.Skill_0);
        //        //特效飞过去
        //    }
        //    _BeDamage(e);
        //}

        private IEnumerator _Guard(HitEvent e)
        {
            BattleController ctrl = BattleController.CurBattleController;
            if (ctrl == null)
            {
                yield break;
            }
            Obj_Char guarder = ctrl.GetObjByRoleID(e.senderID);
            if (guarder != null)
            {
                //闪过去   
                Vector3 dir = ObjTransform.forward;
                Vector3 targetPos = dir + ObjTransform.position;

                guarder.ObjTransform.position = targetPos;
                guarder.ObjTransform.rotation = ObjTransform.rotation;
            }
            if (guarder != null)
            {
                yield return new WaitForSeconds(1f);
                //闪回来
                Transform pos = BattlePos.CurBattlePos.GetPos(guarder.BattleRoleData);
                guarder.ObjTransform.position = pos.position;
                guarder.ObjTransform.rotation = pos.rotation;
            }
        }

        private void _PassiveEffected(HitEvent e)
        {
            Tab_SkillEx tab = TableManager.GetSkillExByID(e.val, 0);
            if (tab == null)
            {
                return;
            }
            Tab_SkillBase tabBase = TableManager.GetSkillBaseByID(tab.BaseID, 0);
            if (tabBase == null)
            {
                return;
            }
            if (tabBase.PassiveEffectId != -1)
            {
                PlayEffect(tabBase.PassiveEffectId);
            }
            if (tabBase.IsShowPassiveName == 1 && !string.IsNullOrEmpty(tabBase.Name))
            {
                PlayFlyText(tabBase.Name, FlyTextMgr.TextType.Impact, false);
            }
        }

        private void _BombBlast(HitEvent e)
        {
            PlayEffect(e.val);
        }

        private void _MaxHPChange(HitEvent e)
        {
            if (BattleRoleData == null)
            {
                return;
            }

            if (BattleRoleData.MaxHP > e.val)
            {
                //血上限降低
                PlayFlyText(StrDictionary.GetDicByID(6967), FlyTextMgr.TextType.Impact);
            }

            //血上限变化
            BattleRoleData.MaxHP = e.val;
            BattleRoleData.MaxHpReduce = e.param1;

            RefreshHP();

            if (e.param0 > 0)
            {
                BattleRoleData.HP -= e.param0;
                //血上限下降导致当前血量下降，伤害冒字
                PlayFlyText(e.param0.ToString(), FlyTextMgr.TextType.Damage);
            }

        }

        private void _ShiledChange(HitEvent e)
        {
            if (BattleRoleData == null)
            {
                return;
            }
            BattleRoleData.Shild = e.val;
            RefreshHP();
        }

        protected void PlayHitText(HitEvent e)
        {

            HitType hitType = (HitType) e.hitType;
            if (hitType == HitType.Damage)
            {
                PlayFlyText(e.val.ToString(), FlyTextMgr.TextType.Damage);
            }
            else if (hitType == HitType.Heal)
            {
                PlayFlyText(e.val.ToString(), FlyTextMgr.TextType.Heal, false);
            }
            else if (hitType == HitType.CritDamage)
            {
                PlayFlyText(e.val.ToString(), FlyTextMgr.TextType.DamageCrit);
            }
            else if (hitType == HitType.EnvEnhance)
            {
                PlayFlyText(e.val.ToString(), FlyTextMgr.TextType.DamageEnvEnhance);
            }
            else if (hitType == HitType.EnvResist)
            {
                PlayFlyText(e.val.ToString(), FlyTextMgr.TextType.DamageEnvResist);
            }
            else if (hitType == HitType.Sacrifice)
            {
                PlayFlyText(e.val.ToString(), FlyTextMgr.TextType.Damage, false);
            }
            else if (hitType == HitType.Immue)
            {
                PlayFlyText(StrDictionary.GetDicByID(5837), FlyTextMgr.TextType.Impact, false);
            }
            else if (hitType == HitType.IncAp)
            {
                PlayFlyText(StrDictionary.GetDicByID(5838), FlyTextMgr.TextType.Impact, false);
            }
            else if (hitType == HitType.DescAp)
            {
                PlayFlyText(StrDictionary.GetDicByID(5839), FlyTextMgr.TextType.Impact);
            }
            else if (hitType == HitType.Absorb)
            {
                PlayFlyText(StrDictionary.GetDicByID(5840), FlyTextMgr.TextType.Impact, false);
            }
            else if (hitType == HitType.Dispell)
            {
                PlayFlyText(StrDictionary.GetDicByID(5841), FlyTextMgr.TextType.Impact);
            }
            else if (hitType == HitType.Tips)
            {
                PlayFlyText(StrDictionary.GetDicByID(e.val), FlyTextMgr.TextType.Impact, false);
            }
            else if (hitType == HitType.EnvEnhanceCrit)
            {
                PlayFlyText(e.val.ToString(), FlyTextMgr.TextType.DamageEnvEnhanceCrit);
            }
            else if (hitType == HitType.EnvResistCrit)
            {
                PlayFlyText(e.val.ToString(), FlyTextMgr.TextType.DamageEnvResistCrit);
            }
            else if (hitType == HitType.ActGain)
            {
                if (e.val > 0)
                {
                    PlayFlyText(StrDictionary.GetDicByID(e.val), FlyTextMgr.TextType.Impact, false);
                }
            }
            else
            {
                LogModule.WarningLog("not support type !!!!" + hitType);
            }
        }

        public void PlayFlyText(string text, FlyTextMgr.TextType textType, bool isNegtive = true)
        {
            BattleScene curScene = GameManager.CurScene as BattleScene;
            if (curScene == null)
            {
                return;
            }

            if (curScene.FlyTextMgr == null)
            {
                return;
            }

            if (this == null)
            {
                return;
            }
            //冒字
            Vector3 offset = new Vector3(0f, this.DamageBoardHeight, 0f);

            bool isOther = false;
            if (isNegtive)
            {
                isOther = BattleRoleData.Side == BattleController.CurBattleController.OurSide;
            }
            else
            {
                isOther = BattleRoleData.Side != BattleController.CurBattleController.OurSide;
            }

            curScene.FlyTextMgr.Play(text, ObjTransform, offset, textType, isOther);
        }

        public void RefreshHeadInfo()
        {
            if (m_BattleHeadInfoLogic != null)
                m_BattleHeadInfoLogic.Refresh(BattleRoleData);

            if (m_BossHeadInfo != null)
            {
                m_BossHeadInfo.Refresh(BattleRoleData);
            }
        }

        public void ChangeImpacts(List<ImpactInfo> newVal)
        {
            BattleRoleData.Impacts = newVal;
            RefreshBuff();
        }

        //是否已死亡，还在表现死亡动画
        public bool IsAlive()
        {
            return m_IsAlive;
        }

        public void SetAlive(bool val)
        {
            m_IsAlive = val;
        }

        //战斗中死亡
        public void Die(int killer)
        {
            //停止技能，如果有没执行完的事件，立即支持掉
            StopSkill();

            //立即移除管理            
            if (BattleController.CurBattleController != null && BattleRoleData != null)
            {
                BattleController.CurBattleController.RemoveRoleID(BattleRoleData.RoleID);
                BattleController.CurBattleController.OnRoleDead(this);
            }

            //隐藏头顶血条
            SetBattleNameBoardActive(false);
            PlayDead();
        }

        public void RefreshHP()
        {
            if (m_BattleHeadInfoLogic != null)
                m_BattleHeadInfoLogic.RefreshHP(BattleRoleData);

            if (m_BossHeadInfo != null)
            {
                m_BossHeadInfo.RefreshHP(BattleRoleData);
            }
        }

        //public void RefreshAP()
        //{
        //    if (m_BattleHeadInfoLogic != null)
        //        m_BattleHeadInfoLogic.RefreshAP(BattleRoleData);
        //}

        private void RefreshBuff()
        {
            if (m_BattleHeadInfoLogic != null)
                m_BattleHeadInfoLogic.RefreshBuff(BattleRoleData);

            if (m_BossHeadInfo != null)
            {
                m_BossHeadInfo.RefreshBuff(BattleRoleData);
            }
        }

        public void OnUseSkill(int skillExId)
        {
            if (m_BattleHeadInfoLogic != null)
            {
                m_BattleHeadInfoLogic.OnUseSkill(skillExId);
            }
        }

        public void OnSkillEnd()
        {
            if (m_BattleHeadInfoLogic != null)
            {
                m_BattleHeadInfoLogic.OnSkillEnd();
            }
            //StopAnim();
            //PlayAnim((int)CHAR_ANIM_ID.Stand);
        }

        public void StopSkill()
        {
            if (m_CurSkill != null && !m_CurSkill.IsSkillDone())
            {
                m_CurSkill.StopSkill();
            }
            m_CurSkill = null;
        }

        //public void OnSelected(bool sel)
        //{
        //    if (m_BattleHeadInfoLogic == null)
        //    {
        //        return;
        //    }
        //    m_BattleHeadInfoLogic.Selected(sel);
        //}

        public void SetSelectable(bool flag, Obj_Char attacker)
        {
            if (m_BattleHeadInfoLogic == null)
            {
                return;
            }
            m_BattleHeadInfoLogic.SetSelectable(flag, attacker);
        }

        public void SyncAttrs(RoleSync attrs)
        {
            if (BattleRoleData == null)
            {
                return;
            }
            BattleRoleData.Sync(attrs);
            RefreshHeadInfo();
        }

        public void OnImpactModify(List<ImpactInfo> org)
        {
            if (BattleRoleData == null || BattleRoleData.Impacts == null)
            {
                return;
            }

            List<int> fadeOuts = new List<int>();
            List<int> fadeIns = new List<int>();

            if (org != null)
            {
                //buff数量有限,不会很费
                foreach (var cur in BattleRoleData.Impacts)
                {
                    if (org.Find((o) => o.id == cur.id) == null)
                    {
                        fadeIns.Add(cur.impactID);
                    }
                }
            }
            else
            {
                foreach (var cur in BattleRoleData.Impacts)
                {
                    fadeIns.Add(cur.impactID);
                }
            }


            if (org != null)
            {
                foreach (var old in org)
                {
                    if (BattleRoleData.Impacts.Find((o) => o.id == old.id) == null)
                    {
                        fadeOuts.Add(old.impactID);
                    }
                }
            }

            foreach (var d in fadeOuts)
            {
                OnImpactFadeOut(d);
            }

            foreach (var a in fadeIns)
            {
                OnImpactFadeIn(a);
            }

        }

        public void AddImpact(ImpactInfo info)
        {
            if (BattleRoleData == null || BattleRoleData.Impacts == null)
            {
                return;
            }
            if (info == null)
            {
                return;
            }

            bool found = false;
            foreach (var impactInfo in BattleRoleData.Impacts)
            {
                if (impactInfo.id == info.id)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                BattleRoleData.Impacts.Add(info);
            }
            OnImpactFadeIn(info.impactID);
            RefreshBuff();
        }

        public void UpdateImpact(ImpactInfo info)
        {
            if (info == null)
            {
                return;
            }
            if (BattleRoleData == null || BattleRoleData.Impacts == null)
            {
                return;
            }
            bool found = false;
            int count = BattleRoleData.Impacts.Count;
            for (int i = 0; i < count; i++)
            {
                if (BattleRoleData.Impacts[i].id == info.id)
                {
                    BattleRoleData.Impacts[i] = info;
                    found = true;
                    RefreshBuff();
                    break;
                }
            }
            if (!found)
            {
                AddImpact(info);
            }
        }

        public void RemoveImpact(int buffId)
        {
            if (BattleRoleData == null || BattleRoleData.Impacts == null)
            {
                return;
            }
            for (int i = 0; i < BattleRoleData.Impacts.Count; i++)
            {
                ImpactInfo impact = BattleRoleData.Impacts[i];
                if (impact != null && impact.id == buffId)
                {
                    OnImpactFadeOut(impact.impactID);
                    BattleRoleData.Impacts.RemoveAt(i);
                    break;
                }
            }
            RefreshBuff();
        }

        private void OnImpactFadeOut(int impactId)
        {
            Tab_Impact tab = TableManager.GetImpactByID(impactId, 0);
            if (tab == null)
                return;

            //播放特效
            if (tab.EffectID != -1)
            {
                StopEffect(tab.EffectID);
            }

            if (tab.FadeOutEffectID != -1)
            {
                PlayEffect(tab.FadeOutEffectID);
            }
        }

        private void OnImpactFadeIn(int impactId)
        {
            Tab_Impact tab = TableManager.GetImpactByID(impactId, 0);
            if (tab == null)
                return;

            //播放特效
            if (tab.EffectID != -1)
            {
                PlayEffect(tab.EffectID);
            }

            if (tab.FadeInEffectID != -1)
            {
                PlayEffect(tab.FadeInEffectID);
            }
        }

        public bool IsNothingness()
        {
            return BattleRoleData.IsNothingness();
        }

        public bool IsCursed()
        {
            return BattleRoleData.IsCursed();
        }

        public bool IsSilence()
        {
            return BattleRoleData.IsSilence();
        }

        public bool IsAlwaysEnvEnhance()
        {
            return BattleRoleData.IsAlwaysEnvEnhance();
        }

        public bool IsAlwaysDay()
        {
            return BattleRoleData.IsAlwaysDay();
        }

        public bool IsAlwaysNight()
        {
            return BattleRoleData.IsAlwaysNight();
        }

        public bool HasImpact(int impactId, int senderId = -1)
        {
            if (BattleRoleData == null)
            {
                return false;
            }

            return BattleRoleData.HasImpact(impactId, senderId);
        }


        private string GetRealSkillAnim(Tab_SkillEx tabSkillEx)
        {
            if (null == tabSkillEx)
            {
                return string.Empty;
            }

            if (!PlayerPreferenceData.IsPlaySkillTV
                || (null != BattleController.CurBattleController
                    && (BattleController.CurBattleController is MultiplayerCtrl)))

            {
                return (string.IsNullOrEmpty(tabSkillEx.SkillSimpleAnim)
                    ? tabSkillEx.SkillAnim
                    : tabSkillEx.SkillSimpleAnim);
            }

            return tabSkillEx.SkillAnim;
        }

        #region 战斗主角动画

        private Tab_HeroBattleAnim m_HeroBattleAnimTab;
        

        public bool IsIdle_Standing()
        {
            if (m_HeroBattleAnimTab == null) return false;
            return m_HeroBattleAnimTab.IdleStandAnim == CurDefaultAnimId;
        }

        public bool IsBattle_Standing()
        {
            if (m_HeroBattleAnimTab == null) return false;
            return m_HeroBattleAnimTab.BattleStandAnim == CurDefaultAnimId;
        }
        private void ChancePlayAnim(int animId, int chance)
        {
            if (UnityEngine.Random.Range(0, 100) <= chance)
            {
                PlayAnim(animId);
            }
        }

        //特定行为反馈
        public bool CanPlayHeroBehaviourAnim()
        {
            if (AnimLogic == null)
            {
                return false;
            }
            if (m_HeroBattleAnimTab == null)
            {
                return false;
            }
            //战斗待机不能播放
            if (IsBattle_Standing())
            {
                return false;
            }
            if (HasChangeAvatar())
            {
                return false;
            }
            //已经在播其他动画，不能播
            if (AnimLogic.IsPlaying(m_HeroBattleAnimTab.OnOurUseSkillAnim))
            {
                return false;
            }
            if (AnimLogic.IsPlaying(m_HeroBattleAnimTab.OnEnemyDeadAnim))
            {
                return false;
            }
            if (AnimLogic.IsPlaying(m_HeroBattleAnimTab.OnOurDeadAnim))
            {
                return false;
            }
            if (AnimLogic.IsPlaying(m_HeroBattleAnimTab.BattleStartAnim))
            {
                return false;
            }

            return true;
        }

        public void HeroOnOurUseSkill()
        {
            if (!CanPlayHeroBehaviourAnim())
            {
                return;
            }
            if (m_HeroBattleAnimTab == null)
            {
                return;
            }
            ChancePlayAnim(m_HeroBattleAnimTab.OnOurUseSkillAnim, m_HeroBattleAnimTab.OnOurUseSkillChance);
        }

        public void HeroOnOurDead()
        {
            if (!CanPlayHeroBehaviourAnim())
            {
                return;
            }
            if (m_HeroBattleAnimTab == null)
            {
                return;
            }
            ChancePlayAnim(m_HeroBattleAnimTab.OnOurDeadAnim, m_HeroBattleAnimTab.OnOurDeadAnimChance);
        }

        public void HeroOnEnemyDead()
        {
            if (!CanPlayHeroBehaviourAnim())
            {
                return;
            }
            if (m_HeroBattleAnimTab == null)
            {
                return;
            }
            ChancePlayAnim(m_HeroBattleAnimTab.OnEnemyDeadAnim, m_HeroBattleAnimTab.OnEnemyDeadChance);
        }

        public void HeroOnBattleStart()
        {
            //始终播放
            if (m_HeroBattleAnimTab == null)
            {
                return;
            }
            PlayAnim(m_HeroBattleAnimTab.BattleStartAnim);
        }
        ///////////////////////////////////////////

        //怒气变化反馈
        public void CheckCanUseSkillEffect(int lastSp,int sp)
        {
            if (m_HeroBattleAnimTab == null)
            {
                return;
            }

            if (lastSp == sp)
            {
                return;
            }

            if (lastSp < 1000 && sp >= 1000)
            {
                PlayEffect(m_HeroBattleAnimTab.CanUseSkillEffect);
            }
            else if(lastSp >= 1000 && sp < 1000)
            {
                StopEffect(m_HeroBattleAnimTab.CanUseSkillEffect);
            }
        }

        public void CheckSwitchState(int lastSp, int sp)
        {
            if (BattleRoleData == null)
            {
                return;
            }
            if (!BattleRoleData.IsHeroCard())
            {
                return;
            }
            if (m_HeroBattleAnimTab == null)
            {
                return;
            }
            if (HasChangeAvatar())
            {
                return;
            }
            if (lastSp == sp)
            {
                return;
            }

            if (lastSp < 1000 && sp >= 1000)
            {
                //直接切换
                ChangeDefAnim(m_HeroBattleAnimTab.BattleStandAnim, true);
                PlayDefaultAnim();
            }
            else if (lastSp >= 1000 && sp < 1000)
            {
                //直接切换
                ChangeDefAnim(m_HeroBattleAnimTab.IdleStandAnim, true);
                PlayDefaultAnim();
            }
        }

        public void PlayHeroWin()
        {
            if (m_HeroBattleAnimTab == null)
            {
                return;
            }
            ChangeDefAnim(m_HeroBattleAnimTab.IdleStandAnim, true);
            PlayAnim(m_HeroBattleAnimTab.WinAnim);
        }

        public void PlayHeroLose()
        {
            if (m_HeroBattleAnimTab == null)
            {
                return;
            }
            ChangeDefAnim(m_HeroBattleAnimTab.IdleStandAnim, true);
            PlayAnim(m_HeroBattleAnimTab.LoseAnim);
        }

    #endregion
    }
}


