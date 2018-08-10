

using UnityEngine;

public class EnableStateGroup : StateGroup
{
    public UISprite icon;
    public UILabel lb;
    public Color lbGray = new Color(212 / 255.0f, 200 / 255.0f, 195 / 255.0f,1.0f);

    void OnEnable()
    {
        ChangeState("Enable");
    }

    void OnDisable()
    {
        ChangeState("Disable");
    }

    [ContextMenu("QuickBuildAsButton")]
    public void QuickBuild()
    {
        states = new Group[]
        {
            new Group()
            {
                name = "Enable",
                colorChange = new ColorChange[]
                {
                    new ColorChange()
                    {
                        widget = icon,
                        color = Color.white,
                    },
                    new ColorChange()
                    {
                        widget = lb,
                        color = Color.white,
                    }
                }
            },
            new Group()
            {
                name = "Disable",
                colorChange = new ColorChange[]
                {
                    new ColorChange()
                    {
                        widget = icon,
                        color = new Color(0f,1f,1f,1f),
                    },
                    new ColorChange()
                    {
                        widget = lb,
                        color = lbGray,
                    }
                }
            }
        };
    }
}