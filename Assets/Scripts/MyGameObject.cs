using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class MyGameObject : MonoBehaviour
{
    protected virtual void Start()
    {
        var whitelist = new HashSet<OrderType> { OrderType.Idle, OrderType.Stop };

        Orders = new OrderContainer(whitelist);
        Resources = new Dictionary<string, Resource>();
        Recipes = new List<Recipe>();

        OrderHandlers = new Dictionary<OrderType, UnityAction>()
        {
            { OrderType.Idle, OnIdleOrder },
            { OrderType.Move, OnMoveOrder },
            { OrderType.Patrol, OnPatrolOrder },
            { OrderType.Produce, OnProduceOrder },
            { OrderType.Stop, OnStopOrder },
        };
    }

    protected virtual void Update()
    {
        if (Orders.Count > 0)
        {
            var order = Orders.First();

            if (Orders.Contains(order.Type))
            {
                if (OrderHandlers.ContainsKey(order.Type))
                {
                    OrderHandlers[order.Type]();
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
        Orders.Add(new Order(OrderType.Idle));
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
        // TODO: Make it virtual and take timer value from object.
        Orders.Add(new Order(OrderType.Produce, 3));
    }

    public void Stop()
    {
        Orders.Insert(0, new Order(OrderType.Stop));
    }
    #endregion

    #region Handlers

    protected virtual void OnIdleOrder()
    {
    }

    protected virtual void OnMoveOrder()
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

    protected virtual void OnPatrolOrder()
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

    protected virtual void OnProduceOrder()
    {
        var order = Orders.First();

        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished)
        {
            foreach (var item in Resources)
            {
                item.Value.Add(1);
            }

            order.Timer.Reset();
        }
    }

    protected virtual void OnStopOrder()
    {
        Orders.Clear();
    }
    #endregion

    public OrderContainer Orders { get; private set; }

    public Dictionary<OrderType, UnityAction> OrderHandlers { get; private set; }

    public Dictionary<string, Resource> Resources { get; private set; }

    public List<Recipe> Recipes { get; private set; }
}
