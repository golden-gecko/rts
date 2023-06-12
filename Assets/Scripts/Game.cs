using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class Request
{
    public Request(MyGameObject myGameObject, string name, int value)
    {
        GameObject = myGameObject;
        Name = name;
        Value = value;
    }

    public MyGameObject GameObject { get; }
    
    public string Name { get; }
    
    public int Value { get; }
}

public class ConsumerProducerContainer
{
    public ConsumerProducerContainer()
    {
        Items = new List<Request>();
    }

    public void Add(MyGameObject myGameObject, string name, int value)
    {
        Items.Add(new Request(myGameObject, name, value));
    }

    public int Count { get => Items.Count; }

    public List<Request> Items { get; }
}

public class Game : MonoBehaviour
{
    public Order CreateTransportOrder()
    {
        foreach (var producer in Producers.Items)
        {
            foreach (var consumer in Consumers.Items)
            {
                if (producer.GameObject == consumer.GameObject)
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

                return new Order(OrderType.Transport, producer.GameObject, consumer.GameObject, resources);
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
