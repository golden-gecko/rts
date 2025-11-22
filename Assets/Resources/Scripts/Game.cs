using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public Order CreateOrderConstruction(MyGameObject myGameObject)
    {
        foreach (MyGameObject underConstruction in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Not very efficient. Refactor into raycast.
        {
            if (underConstruction.State == MyGameObjectState.UnderConstruction)
            {
                return Order.Construct(underConstruction, myGameObject.ConstructionTime);
            }
        }

        return null;
    }

    public Order CreateOrderTransport(MyGameObject myGameObject)
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

                return Order.Transport(producer.MyGameObject, consumer.MyGameObject, resources, myGameObject.LoadTime);
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

    public ConsumerProducerContainer Consumers { get; private set; } = new ConsumerProducerContainer();

    public ConsumerProducerContainer Producers { get; private set; } = new ConsumerProducerContainer();
}
