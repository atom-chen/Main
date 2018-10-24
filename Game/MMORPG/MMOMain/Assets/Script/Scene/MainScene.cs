using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class MainScene : SceneBase
{
    public override SCENE_CODE SceneCode
    {
        get { return SCENE_CODE.MAIN; }
    }
    public override void OnLoadScene()
    {
        UIManager.ShowUI(UIInfo._MainUI);
    }

    public override void OnCloseScene()
    {
        UIManager.CloseUI(UIInfo._MainUI);
    }
}

