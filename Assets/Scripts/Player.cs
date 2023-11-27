using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        for (KeyCode i = KeyCode.Alpha0; i <= KeyCode.Alpha9; i++)
        {
            SelectionGroups[i] = new SelectionGroup();
        }

        Achievements.Player = this;

        JobHandlers[OrderType.AttackObject] = new JobHandlerAttackObject();
        JobHandlers[OrderType.Construct] = new JobHandlerConstruct();
        JobHandlers[OrderType.GatherObject] = new JobHandlerGatherObject();
        JobHandlers[OrderType.Transport] = new JobHandlerTransport();
        JobHandlers[OrderType.Unload] = new JobHandlerUnload();
    }

    private void Start()
    {
        TechnologyTree.Load();
    }

    private void Update()
    {
        RemoveEmptyObjectsFromSelection();

        Achievements.Update();
    }

    public void AssignGroup(KeyCode keyCode)
    {
        if (SelectionGroups.ContainsKey(keyCode))
        {
            SelectionGroups[keyCode].Replace(Selection.Items);
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
        return JobHandlers.ContainsKey(orderType) ? JobHandlers[orderType].OnExecute(myGameObject) : null;
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

    public MyGameObject GetProducer(MyGameObject myGameObject, string resource, int value)
    {
        int max = 0;
        MyGameObject target = null;

        foreach (ResourceRequest i in Producers.Items)
        {
            if (i.MyGameObject == myGameObject)
            {
                continue;
            }

            if (i.Name != resource)
            {
                continue;
            }

            if (i.Value < value)
            {
                if (i.Value > max)
                {
                    max = i.Value;
                    target = i.MyGameObject;
                }

                continue;
            }
            else
            {
                return i.MyGameObject;
            }
        }

        return target;
    }

    public MyGameObject GetResource(MyGameObject myGameObject, string resource = "")
    {
        MyGameObject closest = null;
        float distance = float.MaxValue;

        foreach (MyGameObject myResource in FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (myResource == myGameObject)
            {
                continue;
            }

            Storage storage = myResource.GetComponentInChildren<Storage>();

            if (storage.Gatherable == false)
            {
                continue;
            }

            if (resource.Length > 0 && storage.Resources.Current(resource) <= 0)
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

    public MyGameObject GetStorage(MyGameObject myGameObject, string resource, int value) // TODO: Implement.
    {
        MyGameObject closest = null;
        float distance = float.MaxValue;

        foreach (ResourceRequest i in Consumers.Items)
        {
            if (i.MyGameObject == myGameObject)
            {
                continue;
            }

            if (i.Name != resource)
            {
                continue;
            }

            float magnitude = (myGameObject.Position - i.MyGameObject.Position).magnitude;

            if (magnitude < distance)
            {
                closest = i.MyGameObject;
                distance = magnitude;
            }
        }

        return closest;
    }

    public MyGameObject GetResourceToGather(MyGameObject myGameObject, string resource = "", int value = 0) // TODO: Implement.
    {
        MyGameObject closest = null;
        float distance = float.MaxValue;

        foreach (MyGameObject myResource in FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (myResource == myGameObject)
            {
                continue;
            }

            Storage storage = myResource.GetComponentInChildren<Storage>();

            if (storage == null || storage.Gatherable == false)
            {
                continue;
            }

            if (myResource.VisibilityState != MyGameObjectVisibilityState.Explored && myResource.VisibilityState != MyGameObjectVisibilityState.Visible)
            {
                continue;
            }

            float magnitude = (myGameObject.Position - myResource.Position).magnitude;

            if (magnitude < distance)
            {
                MyGameObject myStorage = null;

                foreach (Resource i in myResource.GetComponentInChildren<Storage>().Resources.Items) // TODO: Check each part.
                {
                    myStorage = myGameObject.Player.GetStorage(myGameObject, i.Name, i.Current);

                    if (myStorage != null)
                    {
                        break;
                    }
                }

                if (myStorage == null)
                {
                    continue;
                }

                closest = myResource;
                distance = magnitude;
            }
        }

        return closest;
    }

    public MyGameObject GetResourceToGatherInRange(MyGameObject myGameObject, float range, string resource = "", int value = 0) // TODO: Implement.
    {
        MyGameObject closest = null;
        float distance = float.MaxValue;

        foreach (MyGameObject myResource in FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (myResource == myGameObject)
            {
                continue;
            }

            Storage storage = myResource.GetComponentInChildren<Storage>();

            if (storage == null || storage.Gatherable == false)
            {
                continue;
            }

            if (myResource.VisibilityState != MyGameObjectVisibilityState.Explored && myResource.VisibilityState != MyGameObjectVisibilityState.Visible)
            {
                continue;
            }

            float magnitude = (myGameObject.Position - myResource.Position).magnitude;

            if (magnitude > range)
            {
                continue;
            }

            if (magnitude < distance)
            {
                MyGameObject myStorage = null;

                foreach (Resource i in myResource.GetComponentInChildren<Storage>().Resources.Items) // TODO: Check each part.
                {
                    myStorage = myGameObject.Player.GetStorage(myGameObject, i.Name, i.Current);

                    if (myStorage != null)
                    {
                        break;
                    }
                }

                if (myStorage == null)
                {
                    continue;
                }

                closest = myResource;
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
    public Sprite SelectionSprite { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }

    public SelectionGroup Selection { get; } = new SelectionGroup();

    public Dictionary<KeyCode, SelectionGroup> SelectionGroups { get; } = new Dictionary<KeyCode, SelectionGroup>();

    public TechnologyTree TechnologyTree { get; } = new TechnologyTree();

    public Dictionary<Player, DiplomacyState> Diplomacy { get; } = new Dictionary<Player, DiplomacyState>();

    public AchievementContainer Achievements { get; } = new AchievementContainer();

    public Stats Stats { get; } = new Stats();

    public ResourceRequestContainer Consumers { get; } = new ResourceRequestContainer();

    public ResourceRequestContainer Producers { get; } = new ResourceRequestContainer();

    private Dictionary<OrderType, JobHandler> JobHandlers { get; } = new Dictionary<OrderType, JobHandler>();
}
