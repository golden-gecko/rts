using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected virtual void Awake()
    {
        for (KeyCode i = KeyCode.Alpha0; i <= KeyCode.Alpha9; i++)
        {
            SelectionGroups[i] = new SelectionGroup();
        }

        TechnologyTree.Load();

        Achievements.Player = this;

        jobHandlers[OrderType.Attack] = new JobHandlerAttack();
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
        if (SelectionGroups.ContainsKey(keyCode))
        {
            SelectionGroups[keyCode].Items = new HashSet<MyGameObject>(Selection.Items);
        }
    }

    public bool Is(Player player, DiplomacyState state)
    {
        return Diplomacy[player] == state;
    }

    public void SelectGroup(KeyCode keyCode, bool append)
    {
        if (append == false)
        {
            Selection.Clear();
        }

        if (SelectionGroups.ContainsKey(keyCode))
        {
            Selection.Add(SelectionGroups[keyCode].Items);
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

    public MyResource GetResource(MyGameObject myGameObject, string resource = "")
    {
        MyResource closest = null;
        float distance = float.MaxValue;

        foreach (MyResource myResource in Object.FindObjectsByType<MyResource>(FindObjectsSortMode.None))
        {
            if (myResource.Working == false)
            {
                continue;
            }

            if (myResource == myGameObject)
            {
                continue;
            }

            if (myResource.Gatherable == false)
            {
                continue;
            }

            if (resource != "" && myResource.GetComponent<Storage>().Resources.Storage(resource) <= 0)
            {
                continue;
            }

            float magnitude = (myGameObject.Position - myResource.Position).magnitude;

            if (magnitude < distance)
            {
                closest = myResource;
                distance = magnitude;
            }
        }

        return closest;
    }

    public MyGameObject GetStorage(MyGameObject myGameObject, Resource resource)
    {
        MyGameObject closest = null;
        float distance = float.MaxValue;

        foreach (Storage storage in Object.FindObjectsByType<Storage>(FindObjectsSortMode.None))
        {
            MyGameObject parent = storage.GetComponent<MyGameObject>();

            if (parent == null)
            {
                continue;
            }

            if (parent.Working == false)
            {
                continue;
            }

            if (parent == myGameObject)
            {
                continue;
            }

            string[] resourcesFromStorage = new string[] { resource.Name };
            string[] resourcesFromCapacity = storage.Resources.Items.Where(x => x.In && x.Full == false).Select(x => x.Name).ToArray();
            string[] match = resourcesFromStorage.Intersect(resourcesFromCapacity).ToArray();

            if (match.Length <= 0)
            {
                continue;
            }

            float magnitude = (myGameObject.Position - parent.Position).magnitude;

            if (magnitude < distance)
            {
                closest = parent;
                distance = magnitude;
            }
        }

        return closest;
    }

    public MyGameObject GetStorage(MyGameObject myGameObject, MyResource myResource)
    {
        MyGameObject closest = null;
        float distance = float.MaxValue;

        foreach (Storage storage in Object.FindObjectsByType<Storage>(FindObjectsSortMode.None))
        {
            MyGameObject parent = storage.GetComponent<MyGameObject>();

            if (parent == null)
            {
                continue;
            }

            if (parent == myGameObject)
            {
                continue;
            }

            if (parent == myResource)
            {
                continue;
            }

            string[] resourcesFromStorage = myResource.GetComponent<Storage>().Resources.Items.Where(x => x.Out && x.Empty == false).Select(x => x.Name).ToArray();
            string[] resourcesFromCapacity = storage.Resources.Items.Where(x => x.In && x.Full == false).Select(x => x.Name).ToArray();
            string[] match = resourcesFromStorage.Intersect(resourcesFromCapacity).ToArray();

            if (match.Length <= 0)
            {
                continue;
            }

            float magnitude = (myGameObject.Position - parent.Position).magnitude;

            if (magnitude < distance)
            {
                closest = parent;
                distance = magnitude;
            }
        }

        return closest;
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
        Selection.RemoveEmpty();

        foreach (SelectionGroup group in SelectionGroups.Values)
        {
            group.RemoveEmpty();
        }
    }

    [field: SerializeField]
    public Sprite SelectionSprite { get; set; }

    [field: SerializeField]
    public string Name { get; set; }

    public SelectionGroup Selection { get; } = new SelectionGroup();

    public Dictionary<KeyCode, SelectionGroup> SelectionGroups { get; } = new Dictionary<KeyCode, SelectionGroup>();

    public TechnologyTree TechnologyTree { get; } = new TechnologyTree();

    public Dictionary<Player, DiplomacyState> Diplomacy { get; } = new Dictionary<Player, DiplomacyState>();

    public AchievementContainer Achievements { get; } = new AchievementContainer();

    public Stats Stats { get; } = new Stats();

    public ResourceRequestContainer Consumers { get; } = new ResourceRequestContainer();

    public ResourceRequestContainer Producers { get; } = new ResourceRequestContainer();

    private Dictionary<OrderType, JobHandler> jobHandlers = new Dictionary<OrderType, JobHandler>();
}
