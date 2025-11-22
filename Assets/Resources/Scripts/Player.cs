using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected virtual void Awake()
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

    public bool IsAlly(Player player)
    {
        return Diplomacy[player] == DiplomacyState.Ally;
    }

    public bool IsEnemy(Player player)
    {
        return Diplomacy[player] == DiplomacyState.Enemy;
    }

    public void SelectGroup(KeyCode keyCode)
    {
        if (Groups.ContainsKey(keyCode))
        {
            if (HUD.Instance.IsShift() == false)
            {
                foreach (MyGameObject myGameObject in Selected)
                {
                    myGameObject.Select(false);
                }

                Selected.Clear();
            }

            foreach (MyGameObject myGameObject in Groups[keyCode])
            {
                myGameObject.Select(true);

                Selected.Add(myGameObject);
            }
        }
    }

    public void SetDiplomacy(Player player, DiplomacyState state)
    {
        Diplomacy[player] = state;
    }

    [SerializeField]
    public Sprite SelectionSprite;

    public bool Gatherable { get; protected set; } = false;

    public HashSet<MyGameObject> Selected { get; } = new HashSet<MyGameObject>();

    public Dictionary<KeyCode, HashSet<MyGameObject>> Groups { get; } = new Dictionary<KeyCode, HashSet<MyGameObject>>();

    public TechnologyTree TechnologyTree { get; } = new TechnologyTree();

    public Dictionary<Player, DiplomacyState> Diplomacy { get; } = new Dictionary<Player, DiplomacyState>();
}
