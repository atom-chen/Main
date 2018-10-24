using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class StartScene:SceneBase
{
    public override SCENE_CODE SceneCode
    {
        get { return SCENE_CODE.LAUNCH; }
    }

    public override void OnLoadScene()
    {
        UIManager.ShowUI(UIInfo.LaunchUI);
    }

    public override void OnCloseScene()
    {
        UIManager.CloseUI(UIInfo.LaunchUI);
    }
    
}

