using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        TechnologyTree.Load();

        for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha9; i++)
        {
            Groups[i] = new HashSet<MyGameObject>();
        }
    }

    public void AssignGroup(KeyCode keyCode)
    {
        if (Groups.ContainsKey(keyCode))
        {
            Groups[keyCode] = new HashSet<MyGameObject>(Selected);
        }
    }

    public void SelectGroup(KeyCode keyCode)
    {
        if (Groups.ContainsKey(keyCode))
        {
            if (IsMulti() == false)
            {
                foreach (MyGameObject myGameObject in Selected)
                {
                    myGameObject.Select(false);
                }
            }

            Selected = new HashSet<MyGameObject>(Groups[keyCode]);

            foreach (MyGameObject myGameObject in Selected)
            {
                myGameObject.Select(true);
            }
        }
    }

    private bool IsMulti() // TODO: Refactor.
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    [SerializeField]
    public Sprite SelectionSprite;

    public HashSet<MyGameObject> Selected { get; private set; } = new HashSet<MyGameObject>();

    public Dictionary<KeyCode, HashSet<MyGameObject>> Groups { get; } = new Dictionary<KeyCode, HashSet<MyGameObject>>();

    public TechnologyTree TechnologyTree { get; } = new TechnologyTree();
}
