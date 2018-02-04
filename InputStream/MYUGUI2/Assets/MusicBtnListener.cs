using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicBtnListener : MonoBehaviour {
    public Button play;//播放按钮
    public Button next;//下一首
    public Button list;//音乐列表
    public Button sound;//声音按钮
    public Button backMenu;//返回主界面
    public GameObject MusicPlayer;//游戏主界面游戏对象
    public GameObject MusicList;//音乐列表游戏对象
    public bool IsSound;//是否正在设置声音
    public bool ShowList;//是否展开播放列表

	// Use this for initialization
	void Start () {
        //注册监听
        list.onClick.AddListener(OnListBtnClick);//给显示列表按钮注册监听
        backMenu.onClick.AddListener(OnListBtnClick);//给主菜单按钮注册监听
        sound.onClick.AddListener(OnSoundBtnClick);
        play.onClick.AddListener(OnPlayBtnClick);


	}
    //显示列表/返回主菜单按钮 监听
    void OnListBtnClick()
    {
        ShowList = !ShowList;//是否展开播放列表的逆操作
        MusicList.SetActive(ShowList);//根据showList值判定是否激活MusicList对象
    }
    //播放按钮
	void OnPlayBtnClick()
    {
        //播放音乐...
        Debug.Log("用户想要播放...");
    }
    //音量按钮
    void OnSoundBtnClick()
    {
        //改变状态位
        IsSound=!IsSound;
        //设置黑幕是否激活
        //1、拿到黑幕 2、获取游戏对象 3、设置
        sound.transform.GetChild(0).gameObject.SetActive(IsSound);
        //设置音量Slide是否显示
        sound.transform.GetChild(1).gameObject.SetActive(IsSound);

    }



	// Update is called once per frame
	void Update () {
	
	}
}
