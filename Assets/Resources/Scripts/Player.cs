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
    }

    protected virtual void Update()
    {
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

    public Order CreateOrderConstruction(MyGameObject myGameObject)
    {
        float minDistance = float.MaxValue;
        MyGameObject closest = null;

        foreach (MyGameObject underConstruction in FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (underConstruction.State != MyGameObjectState.UnderConstruction)
            {
                continue;
            }

            if (myGameObject.Is(underConstruction, DiplomacyState.Ally) == false)
            {
                continue;
            }

            float distance = myGameObject.DistanceTo(underConstruction);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = underConstruction;
            }
        }

        if (closest == null)
        {
            return null;
        }

        return Order.Construct(closest, myGameObject.GetComponent<Constructor>().ResourceUsage);
    }

    public Order CreateOrderGather(MyGameObject myGameObject)
    {
        MyResource closest = null;
        float distance = float.MaxValue;

        foreach (MyResource myResource in FindObjectsByType<MyResource>(FindObjectsSortMode.None)) // TODO: Find storage for resource.
        {
            float magnitude = (myGameObject.Position - myResource.Position).magnitude;

            if (magnitude < distance)
            {
                closest = myResource;
                distance = magnitude;
            }
        }

        if (closest == null)
        {
            return null;
        }

        return Order.Gather(closest);
    }

    public Order CreateOrderTransport(MyGameObject myGameObject)
    {
        foreach (ConsumerProducerRequest consumer in Consumers.Items) // TODO: Return closest object.
        {
            foreach (ConsumerProducerRequest producer in Producers.Items) // TODO: Return closest object.
            {
                if (producer.MyGameObject == consumer.MyGameObject)
                {
                    continue;
                }

                if (producer.Name != consumer.Name)
                {
                    continue;
                }

                Dictionary<string, int> resources = new Dictionary<string, int>()
                {
                    { producer.Name, producer.Value },
                };

                Consumers.MoveToEnd();
                Producers.MoveToEnd();

                return Order.Transport(producer.MyGameObject, consumer.MyGameObject, resources, myGameObject.LoadTime);
            }
        }

        return null;
    }

    public void RegisterConsumer(MyGameObject myGameObject, string name, int value)
    {
        Register(Consumers, myGameObject, name, value);
    }

    public void UnregisterConsumer(MyGameObject myGameObject, string name)
    {
        Unregister(Consumers, myGameObject, name);
    }

    public void RegisterProducer(MyGameObject myGameObject, string name, int value)
    {
        Register(Producers, myGameObject, name, value);
    }

    public void UnregisterProducer(MyGameObject myGameObject, string name)
    {
        Unregister(Producers, myGameObject, name);
    }

    private void Register(ConsumerProducerContainer container, MyGameObject myGameObject, string name, int value)
    {
        container.Add(myGameObject, name, value);
    }

    private void Unregister(ConsumerProducerContainer container, MyGameObject myGameObject, string name)
    {
        container.Remove(myGameObject, name);
    }

    [field: SerializeField]
    public Sprite SelectionSprite { get; set; }

    public HashSet<MyGameObject> Selected { get; } = new HashSet<MyGameObject>();

    public Dictionary<KeyCode, HashSet<MyGameObject>> Groups { get; } = new Dictionary<KeyCode, HashSet<MyGameObject>>();

    public TechnologyTree TechnologyTree { get; } = new TechnologyTree();

    public Dictionary<Player, DiplomacyState> Diplomacy { get; } = new Dictionary<Player, DiplomacyState>();

    public AchievementContainer Achievements { get; } = new AchievementContainer();

    public Stats Stats { get; } = new Stats();

    public ConsumerProducerContainer Consumers { get; } = new ConsumerProducerContainer();

    public ConsumerProducerContainer Producers { get; } = new ConsumerProducerContainer();
}
