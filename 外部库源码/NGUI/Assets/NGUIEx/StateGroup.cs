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

        public void Init()
        {
            foreach (var change in colorChange)
            {
                change.Init();
            }
            foreach (var spriteChange in spriteChanges)
            {
                spriteChange.Init();
            }
        }

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

        public void Init()
        {
            if (widget != null)
            {
                orgColor = widget.color;
            }
        }

        public void Enter()
        {
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

        public void Init()
        {
            if (sprite != null)
            {
                orgSpriteName = sprite.spriteName;
            }
        }

        public void Enter()
        {
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
        InitActiveGos();
        foreach (var @group in states)
        {
            group.Init();
        }
    }

    private void InitActiveGos()
    {
        ativeGoList.Clear();
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
        InitActiveGos();
        ChangeState(testStateName);
        NGUITools.SetDirty(gameObject);
    }
#endif
}