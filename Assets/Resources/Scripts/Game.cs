using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Player cpu = GameObject.Find("CPU").GetComponent<Player>();
        Player gaia = GameObject.Find("Gaia").GetComponent<Player>();
        Player human = GameObject.Find("Human").GetComponent<Player>();

        Consumers[cpu] = new ConsumerProducerContainer();
        Consumers[gaia] = new ConsumerProducerContainer();
        Consumers[human] = new ConsumerProducerContainer();

        Producers[cpu] = new ConsumerProducerContainer();
        Producers[gaia] = new ConsumerProducerContainer();
        Producers[human] = new ConsumerProducerContainer();

        Diplomacy[cpu] = new Dictionary<Player, DiplomacyState>();
        Diplomacy[gaia] = new Dictionary<Player, DiplomacyState>();
        Diplomacy[human] = new Dictionary<Player, DiplomacyState>();

        Diplomacy[cpu][cpu] = DiplomacyState.Ally;
        Diplomacy[cpu][gaia] = DiplomacyState.Neutral;
        Diplomacy[cpu][human] = DiplomacyState.Enemy;

        Diplomacy[gaia][cpu] = DiplomacyState.Neutral;
        Diplomacy[gaia][gaia] = DiplomacyState.Ally;
        Diplomacy[gaia][human] = DiplomacyState.Neutral;

        Diplomacy[human][cpu] = DiplomacyState.Enemy;
        Diplomacy[human][gaia] = DiplomacyState.Neutral;
        Diplomacy[human][human] = DiplomacyState.Ally;
    }

    public Order CreataAttackJob(MyGameObject myGameObject)
    {
        foreach (MyGameObject target in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Optimize.
        {
            if (myGameObject.IsInAttackRange(target.Position) && myGameObject.IsEnemy(target))
            {
                return Order.Attack(target);
            }
        }

        return null;
    }

    public Order CreateOrderConstruction(MyGameObject myGameObject)
    {
        foreach (MyGameObject underConstruction in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Optimize.
        {
            if (underConstruction.Player != this)
            {
                continue;
            }

            if (underConstruction.State != MyGameObjectState.UnderConstruction)
            {
                continue;
            }

            return Order.Construct(underConstruction, myGameObject.ConstructionTime);
        }

        return null;
    }

    public Order CreateOrderTransport(MyGameObject myGameObject)
    {
        foreach (ConsumerProducerRequest producer in Producers[myGameObject.Player].Items)
        {
            foreach (ConsumerProducerRequest consumer in Consumers[myGameObject.Player].Items)
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

                Consumers[myGameObject.Player].MoveToEnd();
                Producers[myGameObject.Player].MoveToEnd();

                return Order.Transport(producer.MyGameObject, consumer.MyGameObject, resources, myGameObject.LoadTime);
            }
        }

        return null;
    }

    public void RegisterConsumer(MyGameObject myGameObject, string name, int value)
    {
        Player[] players = GameObject.Find("Players").GetComponentsInChildren<Player>();

        if (myGameObject.Player.Gatherable)
        {
            foreach (Player player in players)
            {
                Consumers[player].Add(myGameObject, name, value);
            }
        }
        else
        {
            Consumers[myGameObject.Player].Add(myGameObject, name, value);
        }
    }

    public void UnregisterConsumer(MyGameObject myGameObject, string name)
    {
        Player[] players = GameObject.Find("Players").GetComponentsInChildren<Player>();

        if (myGameObject.Player.Gatherable)
        {
            foreach (Player player in players)
            {
                Consumers[player].Remove(myGameObject, name);
            }
        }
        else
        {
            Consumers[myGameObject.Player].Remove(myGameObject, name);
        }
    }

    public void RegisterProducer(MyGameObject myGameObject, string name, int value)
    {
        Player[] players = GameObject.Find("Players").GetComponentsInChildren<Player>();

        if (myGameObject.Player.Gatherable)
        {
            foreach (Player player in players)
            {
                Producers[player].Add(myGameObject, name, value);
            }
        }
        else
        {
            Producers[myGameObject.Player].Add(myGameObject, name, value);
        }
    }

    public void UnregisterProducer(MyGameObject myGameObject, string name)
    {
        Player[] players = GameObject.Find("Players").GetComponentsInChildren<Player>();

        if (myGameObject.Player.Gatherable)
        {
            foreach (Player player in players)
            {
                Producers[player].Remove(myGameObject, name);
            }
        }
        else
        {
            Producers[myGameObject.Player].Remove(myGameObject, name);
        }
    }

    public Dictionary<Player, ConsumerProducerContainer> Consumers { get; } = new Dictionary<Player, ConsumerProducerContainer>();

    public Dictionary<Player, ConsumerProducerContainer> Producers { get; } = new Dictionary<Player, ConsumerProducerContainer>();

    public Dictionary<Player, Dictionary<Player, DiplomacyState>> Diplomacy { get; } = new Dictionary<Player, Dictionary<Player, DiplomacyState>>();
}
