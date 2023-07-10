using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    protected virtual void Awake()
    {
        Assert.IsNotNull(Selection);
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

    public ConsumerProducerContainer Consumers { get; } = new ConsumerProducerContainer();

    public ConsumerProducerContainer Producers { get; } = new ConsumerProducerContainer();

    [SerializeField]
    public Sprite Selection;
}
