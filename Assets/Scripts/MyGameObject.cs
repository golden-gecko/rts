using System.Collections.Generic;
using UnityEngine;

public class MyGameObject : MonoBehaviour
{
    protected virtual void Start()
    {
        var whitelist = new HashSet<OrderType> { OrderType.Idle, OrderType.Stop };

        Orders = new OrderContainer(whitelist);
        Resources = new Dictionary<string, Resource>();
    }

    protected virtual void Update()
    {
        if (Orders.Count > 0)
        {
            var order = Orders.First();

            if (Orders.Contains(order.Type))
            {
                switch (order.Type)
                {
                    case OrderType.Idle:
                        OnIdleOrder();
                        break;

                    case OrderType.Move:
                        OnMoveOrder();
                        break;

                    case OrderType.Patrol:
                        OnPatrolOrder();
                        break;

                    case OrderType.Produce:
                        OnProduceOrder();
                        break;

                    case OrderType.Stop:
                        OnStopOrder();
                        break;
                }
            }
            else
            {
                Orders.Pop();
            }
        }
        else
        {
            Idle();
        }
    }

    #region Orders
    public void Idle()
    {
        // TODO: Restore.
        // Orders.Add(new Order(OrderType.Idle));
    }

    public void Move(MyGameObject target)
    {
        Orders.Add(new Order(OrderType.Move, target));
    }

    public void Move(Vector3 target)
    {
        Orders.Add(new Order(OrderType.Move, target));
    }

    public void Patrol(Vector3 target)
    {
        Orders.Add(new Order(OrderType.Patrol, target));
        Orders.Add(new Order(OrderType.Patrol, transform.position));
    }

    public void Patrol(MyGameObject target)
    {
        Orders.Add(new Order(OrderType.Patrol, target));
        Orders.Add(new Order(OrderType.Patrol, transform.position));
    }

    public void Produce()
    {
    }

    public void Stop()
    {
        Orders.Insert(0, new Order(OrderType.Stop));
    }
    #endregion

    #region Handlers

    void OnIdleOrder()
    {
        var objects = GameObject.FindGameObjectsWithTag("Resource");

        foreach (var item in objects)
        {
            Orders.Add(new Order(OrderType.Move, item.transform.position));

            break;
        }

        Orders.Pop();
    }

    void OnMoveOrder()
    {
        var order = Orders.First();

        var distanceToTarget = (order.TargetPosition - transform.position).magnitude;
        var distanceToTravel = 10 * Time.deltaTime;

        if (distanceToTarget > distanceToTravel)
        {
            var direction = order.TargetPosition - transform.position;
            direction.y = 0;
            direction.Normalize();

            transform.LookAt(order.TargetPosition);
            transform.Translate(Vector3.forward * distanceToTravel);
        }
        else
        {
            var direction = order.TargetPosition - transform.position;
            direction.y = 0;
            direction.Normalize();

            transform.LookAt(order.TargetPosition);
            transform.position = order.TargetPosition;

            Orders.Pop();
        }
    }

    void OnPatrolOrder()
    {
        var order = Orders.First();

        var distanceToTarget = (order.TargetPosition - transform.position).magnitude;
        var distanceToTravel = 10 * Time.deltaTime;

        if (distanceToTarget > distanceToTravel)
        {
            var direction = order.TargetPosition - transform.position;
            direction.y = 0;
            direction.Normalize();

            transform.LookAt(order.TargetPosition);
            transform.Translate(Vector3.forward * distanceToTravel);
        }
        else
        {
            var direction = order.TargetPosition - transform.position;
            direction.y = 0;
            direction.Normalize();

            transform.LookAt(order.TargetPosition);
            transform.position = order.TargetPosition;

            Orders.MoveToEnd();
        }
    }

    void OnProduceOrder()
    {
        var order = Orders.First();

        if (order.TargetGameObject == null)
        {
            Orders.Pop();
        }
        else
        {
            order.Timer.Update(Time.deltaTime);

            if (order.Timer.Finished)
            {
                Resources["Wood"].Value += 1;

                order.Timer.Reset();

                Orders.Pop();
            }
        }
    }

    void OnStopOrder()
    {
        Orders.Clear();
    }
    #endregion

    public OrderContainer Orders { get; private set; }

    public Dictionary<string, Resource> Resources { get; private set; }
}
