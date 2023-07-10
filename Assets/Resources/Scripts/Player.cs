using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Order CreataAttackJob(MyGameObject myGameObject)
    {
        foreach (MyGameObject target in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Optimize.
        {
            if (myGameObject.IsInRange(target.Position, myGameObject.MissileRange) && myGameObject.IsEnemy(target)) // TODO: Create IsInAttackRange method.
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

    [SerializeField]
    public Sprite Selection;

    public ConsumerProducerContainer Consumers { get; } = new ConsumerProducerContainer();

    public ConsumerProducerContainer Producers { get; } = new ConsumerProducerContainer();
}
