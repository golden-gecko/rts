using System.Collections.Generic;
using UnityEngine;

public class ConsumerProducerContainer
{
    public ConsumerProducerContainer()
    {
        Items = new Dictionary<MyGameObject, Dictionary<string, int>>();
    }

    public void Add(MyGameObject myGameObject, string name, int value)
    {
        if (Items.ContainsKey(myGameObject) == false)
        {
            Items[myGameObject] = new Dictionary<string, int>();
        }

        Items[myGameObject][name] = value;
    }

    public int Count { get => Items.Count; }

    public Dictionary<MyGameObject, Dictionary<string, int>> Items { get; }
}

public class Game : MonoBehaviour
{
    public Order CreateTransportOrder()
    {
        foreach (var producer in Producers.Items)
        {
            foreach (var consumer in Consumers.Items)
            {
                // Skip the same game objects in both lists.
                if (producer.Key == consumer.Key)
                {
                    continue;
                }

                foreach (var output in producer.Value)
                {
                    foreach (var input in consumer.Value)
                    {
                        // Skip different resources.
                        if (output.Key != input.Key)
                        {
                            continue;
                        }

                        var resources = new Dictionary<string, int>()
                        {
                            { input.Key, input.Value },
                        };

                        return new Order(OrderType.Transport, producer.Key, consumer.Key, resources);
                    }
                }
            }
        }

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
