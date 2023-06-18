using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Order CreateConstruction()
    {
        return null;
    }

    public Order CreateTransport()
    {
        foreach (var producer in Producers.Items)
        {
            foreach (var consumer in Consumers.Items)
            {
                if (producer.MyGameObject == consumer.MyGameObject)
                {
                    continue;
                }

                if (producer.Name != consumer.Name)
                {
                    continue;
                }

                var resources = new Dictionary<string, int>()
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
    public Order CreateUnload()
    {
        return null;
    }

    void Start()
    {
        Consumers = new ConsumerProducerContainer();
        Producers = new ConsumerProducerContainer();
    }

    public ConsumerProducerContainer Consumers { get; private set; }

    public ConsumerProducerContainer Producers { get; private set; }
}
