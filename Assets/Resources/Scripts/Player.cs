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

        Achievements.Player = this;

        jobHandlers[OrderType.Construct] = new JobHandlerConstruct();
        jobHandlers[OrderType.Gather] = new JobHandlerGather();
        jobHandlers[OrderType.Transport] = new JobHandlerTransport();
        jobHandlers[OrderType.Unload] = new JobHandlerUnload();
    }

    protected virtual void Update()
    {
        RemoveEmptyObjectsFromSelection();

        Achievements.Update();
    }

    public void AssignGroup(KeyCode keyCode)
    {
        if (Groups.ContainsKey(keyCode))
        {
            Groups[keyCode] = new HashSet<MyGameObject>(Selected);
        }
    }

    public bool Is(Player player, DiplomacyState state)
    {
        return Diplomacy[player] == state;
    }

    public void SelectGroup(KeyCode keyCode)
    {
        if (Groups.ContainsKey(keyCode))
        {
            if (HUD.Instance.IsShift() == false)
            {
                HUD.Instance.SelectionClear();
            }

            foreach (MyGameObject myGameObject in Groups[keyCode])
            {
                HUD.Instance.Select(myGameObject);
            }
        }
    }

    public void SetDiplomacy(Player player, DiplomacyState state)
    {
        Diplomacy[player] = state;
    }

    public Order GetJob(MyGameObject myGameObject, OrderType orderType)
    {
        return jobHandlers.ContainsKey(orderType) ? jobHandlers[orderType].OnExecute(myGameObject) : null;
    }

    public void RegisterConsumer(MyGameObject myGameObject, string name, int value, ResourceDirection direction)
    {
        Register(Consumers, myGameObject, name, value, direction);
    }

    public void UnregisterConsumer(MyGameObject myGameObject, string name)
    {
        Unregister(Consumers, myGameObject, name);
    }

    public void RegisterProducer(MyGameObject myGameObject, string name, int value, ResourceDirection direction)
    {
        Register(Producers, myGameObject, name, value, direction);
    }

    public void UnregisterProducer(MyGameObject myGameObject, string name)
    {
        Unregister(Producers, myGameObject, name);
    }

    private void Register(ResourceRequestContainer container, MyGameObject myGameObject, string name, int value, ResourceDirection direction)
    {
        container.Add(myGameObject, name, value, direction);
    }

    private void Unregister(ResourceRequestContainer container, MyGameObject myGameObject, string name)
    {
        container.Remove(myGameObject, name);
    }

    private void RemoveEmptyObjectsFromSelection()
    {
        Selected.RemoveWhere(x => x == null);

        foreach (HashSet<MyGameObject> group in Groups.Values)
        {
            group.RemoveWhere(x => x == null);
        }
    }

    [field: SerializeField]
    public Sprite SelectionSprite { get; set; }

    [field: SerializeField]
    public string Name { get; set; }

    public HashSet<MyGameObject> Selected { get; } = new HashSet<MyGameObject>();

    public Dictionary<KeyCode, HashSet<MyGameObject>> Groups { get; } = new Dictionary<KeyCode, HashSet<MyGameObject>>();

    public TechnologyTree TechnologyTree { get; } = new TechnologyTree();

    public Dictionary<Player, DiplomacyState> Diplomacy { get; } = new Dictionary<Player, DiplomacyState>();

    public AchievementContainer Achievements { get; } = new AchievementContainer();

    public Stats Stats { get; } = new Stats();

    public ResourceRequestContainer Consumers { get; } = new ResourceRequestContainer();

    public ResourceRequestContainer Producers { get; } = new ResourceRequestContainer();

    private Dictionary<OrderType, JobHandler> jobHandlers = new Dictionary<OrderType, JobHandler>();
}
