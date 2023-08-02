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

        cpu.SetDiplomacy(cpu, DiplomacyState.Ally);
        cpu.SetDiplomacy(gaia, DiplomacyState.Neutral);
        cpu.SetDiplomacy(human, DiplomacyState.Enemy);

        gaia.SetDiplomacy(cpu, DiplomacyState.Neutral);
        gaia.SetDiplomacy(gaia, DiplomacyState.Neutral);
        gaia.SetDiplomacy(human, DiplomacyState.Neutral);

        human.SetDiplomacy(cpu, DiplomacyState.Enemy);
        human.SetDiplomacy(gaia, DiplomacyState.Neutral);
        human.SetDiplomacy(human, DiplomacyState.Ally);
    }

    public Order CreataAttackJob(MyGameObject myGameObject)
    {
        foreach (MyGameObject target in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Return closest object.
        {
            if (myGameObject.IsInAttackRange(target.Position) && myGameObject.IsEnemy(target) && target.GetComponent<Missile>() == null) // TODO: Better way of checking object type?
            {
                return Order.Attack(target);
            }
        }

        return null;
    }

    public Order CreateOrderConstruction(MyGameObject myGameObject)
    {
        foreach (MyGameObject underConstruction in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Return closest object.
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
        foreach (ConsumerProducerRequest producer in Producers[myGameObject.Player].Items) // TODO: Return closest object.
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

    private Player[] GetPlayers()
    {
        return GameObject.Find("Players").GetComponentsInChildren<Player>();
    }

    private void Register(Dictionary<Player, ConsumerProducerContainer> container, MyGameObject myGameObject, string name, int value)
    {
        Player[] players = GetPlayers();

        if (myGameObject.Player.Gatherable)
        {
            foreach (Player player in players)
            {
                container[player].Add(myGameObject, name, value);
            }
        }
        else
        {
            container[myGameObject.Player].Add(myGameObject, name, value);
        }
    }

    private void Unregister(Dictionary<Player, ConsumerProducerContainer> container, MyGameObject myGameObject, string name)
    {
        Player[] players = GetPlayers();

        if (myGameObject.Player.Gatherable)
        {
            foreach (Player player in players)
            {
                container[player].Remove(myGameObject, name);
            }
        }
        else
        {
            container[myGameObject.Player].Remove(myGameObject, name);
        }
    }

    public Dictionary<Player, ConsumerProducerContainer> Consumers { get; } = new Dictionary<Player, ConsumerProducerContainer>();

    public Dictionary<Player, ConsumerProducerContainer> Producers { get; } = new Dictionary<Player, ConsumerProducerContainer>();
}
