using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Order CreateOrderConstruction()
    {
        return null;
    }

    public Order CreateOrderTransport(MyGameObject targetProducer = null, MyGameObject targetConsumer = null)
    {
        foreach (ConsumerProducerRequest producer in Producers.Items)
        {
            if (targetProducer != null && producer.MyGameObject != targetProducer)
            {
                continue;
            }

            foreach (ConsumerProducerRequest consumer in Consumers.Items)
            {
                if (targetConsumer != null && consumer.MyGameObject != targetConsumer)
                {
                    continue;
                }

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
    public Order CreateOrderUnload()
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
