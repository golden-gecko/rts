using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public Order CreateOrderConstruction()
    {
        foreach (MyGameObject myGameObject in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Not very efficient. Refactor into raycast.
        {
            if (myGameObject.State == MyGameObjectState.UnderConstruction)
            {
                return new Order(OrderType.Construct, "", PrefabConstructionType.Structure, myGameObject.ConstructionTime, 0, myGameObject, myGameObject.Position);
            }
        }

        return null;
    }

    public Order CreateOrderTransport()
    {
        foreach (ConsumerProducerRequest producer in Producers.Items)
        {
            foreach (ConsumerProducerRequest consumer in Consumers.Items)
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

                return new Order(OrderType.Transport, producer.MyGameObject, consumer.MyGameObject, resources);
            }
        }

        return null;
    }

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

    public ConsumerProducerContainer Consumers { get; private set; } = new();

    public ConsumerProducerContainer Producers { get; private set; } = new();
}
