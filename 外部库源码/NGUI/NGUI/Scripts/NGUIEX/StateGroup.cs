using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityToolbag;

public class StateGroup : MonoBehaviour
{
    [Serializable]
    public class Group
    {
        public string name;

        public ColorChange[] colorChange;
        public SpriteChange[] spriteChanges;
        public GameObject[] activeGo;

        public StateGroup mgr { get; set; }

        public void Enter()
        {
            if (colorChange != null)
            {
                foreach (var change in colorChange)
                {
                    change.Enter();
                }
            }

            if (spriteChanges != null)
            {
                foreach (var spriteChange in spriteChanges)
                {
                    spriteChange.Enter();
                }
            }

            if (activeGo != null)
            {
                foreach (var o in activeGo)
                {
                    o.SetActive(true);
                }
                if (mgr != null)
                {
                    foreach (var o in mgr.ativeGoList)
                    {
                        if (o.activeSelf && !activeGo.Contains(o))
                        {
                            o.SetActive(false);
                        }
                    }
                }
            }
        }

        public void Leave()
        {
            if (colorChange != null)
            {
                foreach (var change in colorChange)
                {
                    change.Leave();
                }
            }

            if (activeGo != null)
            {
                foreach (var o in activeGo)
                {
                    o.SetActive(false);
                }
            }
        }
    }

    [Serializable]
    public class ColorChange
    {
        public UIWidget widget;
        public Color color = Color.white;

        private Color orgColor;

        public void Enter()
        {
            orgColor = widget.color;
            widget.color = color;
        }

        public void Leave()
        {
            widget.color = orgColor;
        }
    }

    [Serializable]
    public class SpriteChange
    {
        public UISprite sprite;
        public string spriteName;
        private string orgSpriteName;

        public void Enter()
        {
            orgSpriteName = sprite.spriteName;
            sprite.spriteName = spriteName;
        }

        public void Leave()
        {
            sprite.spriteName = orgSpriteName;
        }
    }
    [Reorderable]
    public Group[] states;
    public int startState = 0;
    public Group currState { get; private set; }

    private List<GameObject> ativeGoList = new List<GameObject>();

    void Awake()
    {
        foreach (var state in states)
        {
            ativeGoList.AddRange(state.activeGo);
            state.mgr = this;
        }
    }

    void OnEnable()
    {
        if (startState != -1)
        {
            ChangeState(startState);
        }
        else if (currState != null)
        {
            ChangeState(currState);
        }
        else
        {
            for (int i = 0; i < states.Length; i++)
            {
                states[i].Leave();
            }
        }
    }

    public void ChangeState(string stateName)
    {
        foreach (var s in states)
        {
            if (s.name == stateName)
            {
                ChangeState(s);
            }
        }
    }

    public void ChangeState(int index)
    {
        if (index < 0 || index >= states.Length)
        {
            return;
        }

        var targetState = states[index];

        ChangeState(targetState);
    }

    public void ChangeState(Group targetState)
    {
        if (currState != null)
        {
            currState.Leave();
        }
        currState = targetState;
        if (targetState != null)
        {
            targetState.Enter();
        }
    }

#if UNITY_EDITOR
    [Space][Space]
    public string testStateName;

    [ContextMenu("Test State")]
    public void TestState()
    {
        ChangeState(testStateName);
        NGUITools.SetDirty(gameObject);
    }
#endif
}