/********************************************************************************
 *	文件名：	EnvironmentManager.cs
 *	全路径：	\Script\Logic\Environment\EnvironmentManager.cs
 *	创建人：	李嘉
 *	创建时间：2017-02-14
 *
 *	功能说明：游戏中的环境管理器
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Games;
using Games.GlobeDefine;
using Games.Table;
public class EnvironmentManager : MonoBehaviour
{
    private GameObject[] m_EnvNode = new GameObject[(int)SCENE_ENVIRONMENT_TYPE.NUM];
    
	public Tab_Environment CurEvnTable {get;private set;}		//缓存当前的环境对应的表格
    
    public FakeGlobalFog Fog { get; private set; }
    public string FogRes { get; private set; }
    public GameObject CameraRockEffect { get; private set; }
    public ColorCorrectionLUTCtrl ColorLUTCtrl { get; private set; }

    public List<int> ScreenEffectList = new List<int>();

    public void SetEnvirNode(int nEnvirType, GameObject sceneNode)
    {
        if (nEnvirType <= (int)SCENE_ENVIRONMENT_TYPE.INVALID || nEnvirType > (int)SCENE_ENVIRONMENT_TYPE.NUM)
        {
            return;
        }

        m_EnvNode[nEnvirType] = sceneNode;
    }

    private GameObject GetSceneNode(Tab_Environment env)
    {
        if (null == env)
        {
            return null;
        }

        return GetEnvirNode(env.EnvirType);
    }

    public GameObject GetEnvirNode(int nEnvirType)
    {
        if (nEnvirType <= (int)SCENE_ENVIRONMENT_TYPE.INVALID || nEnvirType > (int)SCENE_ENVIRONMENT_TYPE.NUM)
        {
            return null;
        }

        return m_EnvNode[nEnvirType];
    }


    //清空所有环境状态
    public void ClearEnv()
    {
        StopAllCoroutines();
        if (Fog != null)
        {
            Destroy(Fog.gameObject);
        }
        Fog = null;
        FogRes = null;
        if (CameraRockEffect != null)
        {
            Destroy(CameraRockEffect);
        }
        if (ColorLUTCtrl != null)
        {
            ColorLUTCtrl.enabled = false;
        }
        CameraRockEffect = null;
        ClearEvnNode();
		CurEvnTable = null;
    }

    //清空环境节点
    private void ClearEvnNode()
    {
        for (int i = 0; i < m_EnvNode.Length; ++i)
        {
            m_EnvNode[i] = null;
        }
    }

    //隐藏环境节点
    private void HideAllEnvNode()
    {
        for (int i = 0; i < m_EnvNode.Length; ++i)
        {
            if (null != m_EnvNode[i])
            {
                m_EnvNode[i].SetActive(false);
            }
        }
    }
    //当前环境节点、控制显示隐藏
    public void SetCurEnvNodeVisible(bool visible)
    {
        for (int i = 0; i < m_EnvNode.Length; i++)
        {
            if (m_EnvNode[i]!=null&&i==CurEvnTable.EnvirType)
            {
                m_EnvNode[i].SetActive(visible);
            }
        }
    }
    //切换到某个环境,第二个参数确定是否添加一个新的列表

    private Coroutine switchingEffect;
    private Tab_Environment switchingTargetEnv;

    public void Switch(int nEnvID, bool immediate = false)
    {
        Tab_Environment _env = TableManager.GetEnvironmentByID(nEnvID, 0);
        if (null == _env)
        {
            //没有找到对应的环境ID，所有环境因素全部隐藏
            ChangeEnv(null);
            return;
        }

        if (_env.EnvirType <= (int)SCENE_ENVIRONMENT_TYPE.INVALID || _env.EnvirType >= (int)SCENE_ENVIRONMENT_TYPE.NUM)
        {
            ChangeEnv(null);
            return;
        }
        
		//不一样，则进行环境变换
		if (CurEvnTable != _env)
		{
		    bool changeByCor = false;
            if (CurEvnTable == null || CurEvnTable.EnvirType != _env.EnvirType)
            {
                //如果两个环境的资源类型变了，则需要播放切换特效
                if (null != GameManager.CurScene && null != GameManager.CurScene.EffectCamera)
                {
                    //播放特效
                    if (!immediate)
                    {
                        changeByCor = true;
                        StopAllCoroutines();
                        SetSwitchTargetEnv(_env);
                        if (Display.IsRtSwitchEffect())
                        {
                            switchingEffect = StartCoroutine(_SwitchEffectRT(CurEvnTable, _env));
                        }
                        else
                        {
                            switchingEffect = StartCoroutine(_SwitchEffect(CurEvnTable, _env));
                        }
                    }

                }
            }

		    if (!changeByCor)
		    {
                ChangeEnv(_env);
            }
		}
    }

    public bool IsSwitching()
    {
        return switchingEffect != null;
    }

    public void FinishSwitching()
    {
        if (switchingEffect != null)
        {
            StopCoroutine(switchingEffect);
        }
        if (switchingTargetEnv != null)
        {
            ChangeEnv(switchingTargetEnv,true,true);
            ClearSwitchTargetEnv();
        }
    }

    private IEnumerator _SwitchEffect(Tab_Environment last, Tab_Environment cur)
    {
        GameManager.CurScene.EffectCamera.SetScreenEffect(SCREEN_EFFECT.CHANG_ENV2, true);

        if (SwitchEffect.Ins != null)
        {
            yield return SwitchEffect.Ins.FadeIn();
        }
        ChangeEnv(cur);
        yield return new WaitForSeconds(0.3f);
        if (SwitchEffect.Ins != null)
        {
            yield return SwitchEffect.Ins.FadeOut();
        }

        GameManager.CurScene.EffectCamera.SetScreenEffect(SCREEN_EFFECT.CHANG_ENV2, false);

        ClearSwitchTargetEnv();
    }

    private IEnumerator _SwitchEffectRT(Tab_Environment last,Tab_Environment cur)
    {
        if (SwitchEffectMgr.Ins == null)
        {
            yield return _SwitchEffect(last, cur);
            yield break;
        }
        if (SwitchEffectMgr.Ins.CurEffect == null)
        {
            yield return _SwitchEffect(last, cur);
            yield break;
        }
        SwitchEffectMgr.Ins.CurEffect.Stop();
        yield return null;

        GameObject src = GetSceneNode(last);
        GameObject target = GetSceneNode(cur);

        if (src == null || target == null)
        {
            yield break;
        }

        Coroutine coroutine = SwitchEffectMgr.Ins.CurEffect.Begin(src.transform,target.transform);
        if (coroutine == null)
        {
            //失败了，用全屏特效
            //立即切换
            //PlayFullScreenEffect();
            yield return _SwitchEffect(last, cur);
        }
        else
        {
            ChangeEnv(cur,false);
            yield return coroutine;
        }

        ClearSwitchTargetEnv();
    }

    private void ClearSwitchTargetEnv()
    {
        switchingTargetEnv = null;
        //Debug.LogError("clear evn target");
    }

    private void SetSwitchTargetEnv(Tab_Environment target)
    {
        switchingTargetEnv = target;
        //Debug.LogError("set evn target");
    }

    public void PlayFullScreenEffect()
    {
        GameManager.CurScene.EffectCamera.SetScreenEffect(SCREEN_EFFECT.CHANG_ENV, true);
        StartCoroutine(OnChangeEnvEffectFinish());
    }

    //实际对场景进行效果切换
    private void ChangeEnv(Tab_Environment env,bool modifyActive = true,bool force = false)
    {
        //传空则隐藏所有环境节点
        if (null == env)
        {
            //隐藏天气节点
            HideAllEnvNode();

            //恢复场景音乐为默认
            if (null != GameManager.CurScene)
            {
                GameManager.CurScene.PlaySceneMusic();
            }

            return;
        }

        //如果一样则不进行切换
        if (env == CurEvnTable && !force)
        {
            return;
        }

        CurEvnTable = env;
        GameManager.PlayerDataPool.TreasureMap.SwitchEnv(); //dk 通知寻宝切换环境


        //播放切环境时候的Stinger
        if (env.SwitchSoundID > GlobeVar.INVALID_ID)
		{
			if (null != GameManager.SoundManager)
			{
				GameManager.SoundManager.PlaySoundEffect(env.SwitchSoundID);
			}
		}

        if (modifyActive)
        {
            //变换白天黑夜场景
            for (int i = 0; i < m_EnvNode.Length; ++i)
            {
                if (i == env.EnvirType)
                {
                    if (null != m_EnvNode[i])
                    {
                        m_EnvNode[i].SetActive(true);
                    }
                }
                else
                {
                    if (null != m_EnvNode[i])
                    {
                        m_EnvNode[i].SetActive(false);
                    }
                }
            }
        }

        //切换场景音乐
        if (null != GameManager.SoundManager)
        {
            Tab_Sounds _envSound = TableManager.GetSoundsByID(env.SoundID, 0);
            if (null != _envSound)
            {
                GameManager.SoundManager.PlayBGMusic(env.SoundID, _envSound.FadeOutTime, _envSound.FadeInTime);
            }
			else
			{
				//恢复场景音乐为默认
				if (null != GameManager.CurScene)
				{
					GameManager.CurScene.PlaySceneMusic();
				}
			}
        }

        if (string.IsNullOrEmpty(env.FogEffect))
        {
            //切换雾气
            if (Fog != null)
            {
                Fog.FadeOut();
            }
            Fog = null;
            FogRes = null;
        }
        else
        {
            if (env.FogEffect != FogRes)
            {
                if (Fog != null)
                {
                    Fog.FadeOut();
                }
                Fog = null;
                FogRes = null;
                Object res = AssetManager.LoadResource("Bundle/Effect/" + env.FogEffect);
                if (res == null)
                {
                    LogModule.WarningLog(string.Format("Load Fog Effect failed:{0}.", env.FogEffect));
                }
                else
                {
                    GameObject go = Instantiate(res) as GameObject;
                    if (go != null)
                    {
                        Fog = go.GetComponent<FakeGlobalFog>();
                        FogRes = env.FogEffect;
                        if (Fog != null)
                        {
                            Fog.FadeIn();
                        }
                        else
                        {
                            LogModule.WarningLog("Can not find FakeGlobalFog.");
                        }
                    }
                }
            }
        }

        //震屏效果
        if (CameraRockEffect != null)
        {
            Destroy(CameraRockEffect);
            CameraRockEffect = null;
        }
        if (!string.IsNullOrEmpty(env.CameraRockEffect))
        {
            Object res = AssetManager.LoadResource("Bundle/Effect/" + env.CameraRockEffect);
            if (res == null)
            {
                LogModule.WarningLog(string.Format("Load Camera Rock Effect failed:{0}.", env.CameraRockEffect));
            }
            else
            {
                CameraRockEffect = Instantiate(res) as GameObject;
            }
        }

        //颜色矫正
        if (!string.IsNullOrEmpty(env.ColorCorrectionLUT))
        {
            if (ColorLUTCtrl == null)
            {
                ColorLUTCtrl = Utils.TryAddComponent<ColorCorrectionLUTCtrl>(gameObject);
            }

            Texture2D tex = AssetManager.LoadResource("LUT/" + env.ColorCorrectionLUT) as Texture2D;
            if (tex != null)
            {
                ColorLUTCtrl.lut = tex;
                ColorLUTCtrl.ReloadTexture();
                ColorLUTCtrl.enabled = true;
            }
            else
            {
                //LogModule.DebugLog("Load LUT texture failed." + env.ColorCorrectionLUT);
            }
        }
        else
        {
            if (ColorLUTCtrl != null)
            {
                ColorLUTCtrl.enabled = false;
            }
        }

        //激活的屏幕特效
        InitScreenEffect(env);

        //切换受场景影响的NPC
        if (null != GameManager.CurScene && (GameManager.CurScene.IsLobby()
            || GameManager.CurScene.IsFunctionScene()
            || GameManager.CurScene.IsRealTimeScene()
            || GameManager.CurScene.IsHouseScene()))
        {
            LobbyScene _ls = GameManager.CurScene as LobbyScene;
            if (null != _ls)
            {
                _ls.RefreshCommonNPC();
            }
        }
        else if (GameManager.CurScene != null && GameManager.CurScene.IsStoryCsScene())
        {
            //灵气采集点在剧情场景，需要刷新采集点
            GameManager.CurScene.RefreshCommonNPC();
        }

        // 更新人妖界切换按钮图标
        if (null != PlayerFrameController.Instance())
        {
            PlayerFrameController.Instance().UpdateSwitchIcon(CurEvnTable);
        }

        // 庭院 功能 多人场景记录玩家当前环境
        if (GameManager.CurScene != null && 
            (GameManager.CurScene.IsLobby() 
                || GameManager.CurScene.IsFunctionScene() 
                || GameManager.CurScene.IsRealTimeScene() 
                || GameManager.CurScene.IsAsyncPVPScene()
                || GameManager.CurScene.IsHouseScene()) &&
            false == GameManager.CurScene.IsStoryScene())
        {
            CG_RT_CHANGE_ENVIRONMENT_PAK pak = new CG_RT_CHANGE_ENVIRONMENT_PAK();
            pak.data.Environment = CurEvnTable.EnvirType;
            pak.SendPacket();
        }

        NPCMiscManager.CreateMiscNpc(GameManager.CurScene);
        
    }

    private void InitScreenEffect(Tab_Environment env)
    {
        if (env == null)
        {
            return;
        }

        int scrEffCount = env.getScreenEffCount();
        List<int> lastEffList = new List<int>(ScreenEffectList);
        ScreenEffectList.Clear();

        for (int i = 0; i < scrEffCount; i++)
        {
            int scrEffId = env.GetScreenEffbyIndex(i);
            if (scrEffId != -1)
            {
                if (lastEffList.Contains(scrEffId))
                {
                    lastEffList.Remove(scrEffId);
                }
                ScreenEffectList.Add(scrEffId);
            }
        }

        //关掉上一环境的屏幕特效
        foreach (var lastScrEffect in lastEffList)
        {
            Scene scene = GameManager.CurScene;
            if (scene == null)
            {
                continue;
            }
            scene.EffectCamera.SetScreenEffect((SCREEN_EFFECT)lastScrEffect, false);
        }
        //激活当前环境要激活的屏幕特效
        foreach (var eff in ScreenEffectList)
        {
            Scene scene = GameManager.CurScene;
            if (scene == null)
            {
                continue;
            }
            scene.EffectCamera.SetScreenEffect((SCREEN_EFFECT)eff, true);
        }
    }

    private IEnumerator OnChangeEnvEffectFinish()
	{
		yield return new WaitForSeconds(1.55f);

		if (null != GameManager.CurScene && null != GameManager.CurScene.EffectCamera)
		{
			GameManager.CurScene.EffectCamera.SetScreenEffect(SCREEN_EFFECT.CHANG_ENV, false);
		}
	}
    
    //恢复场景默认环境
    public void SetDefault()
    {
        //获取当前场景的环境ID
        Tab_SceneClass _tabSceneClass = TableManager.GetSceneClassByID(GameManager.RunningScene, 0);
        if (null == _tabSceneClass)
        {
            return;
        }

        //如果当前场景没有默认环境ID，则隐藏掉所有节点
        if (GlobeVar.INVALID_ID == _tabSceneClass.DefaultEnv)
        {
            ChangeEnv(null);
        }

        Tab_Environment _tabEnv = TableManager.GetEnvironmentByID(_tabSceneClass.DefaultEnv, 0);
        if (null == _tabEnv)
        {
            return;
        }

        if (_tabEnv != CurEvnTable)
        {
            ChangeEnv(_tabEnv);
        }
    }

    public string GetCurrEnvColorCorrectionLUT()
    {
        if (CurEvnTable == null)
        {
            return null;
        }

        return CurEvnTable.ColorCorrectionLUT;
    }
}
