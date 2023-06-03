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
        Resources = new ResourceContainer();
        Recipes = new List<Recipe>();

        OrderHandlers = new Dictionary<OrderType, UnityAction>()
        {
            { OrderType.Idle, OnOrderIdle },
            { OrderType.Load, OnOrderLoad },
            { OrderType.Move, OnOrderMove },
            { OrderType.Patrol, OnOrderPatrol },
            { OrderType.Produce, OnOrderProduce },
            { OrderType.Stop, OnOrderStop },
            { OrderType.Transport, OnOrderTransport },
            { OrderType.Unload, OnOrderUnload },
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

    public void Load(MyGameObject target, Dictionary<string, int> resources)
    {
        Orders.Add(new Order(OrderType.Load, target, resources));
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

    public void Transport(MyGameObject source, MyGameObject target, Dictionary<string, int> resources)
    {
        Orders.Add(new Order(OrderType.Transport, source, target, resources));
    }

    public void Unload(MyGameObject target, Dictionary<string, int> resources)
    {
        Orders.Add(new Order(OrderType.Unload, target, resources));
    }
    #endregion

    #region Handlers
    protected virtual void OnOrderIdle()
    {
    }

    protected virtual void OnOrderLoad()
    {
        var order = Orders.First();

        // Have all resources to give.
        bool toGive = true;

        foreach (var i in order.Resources)
        {
            if (order.TargetGameObject.Resources.CanRemove(i.Key, i.Value) == false)
            {
                toGive = false;
                break;
            }
        }

        // Have all resources to take.
        bool toTake = true;

        foreach (var i in order.Resources)
        {
            if (Resources.CanAdd(i.Key, i.Value) == false)
            {
                toTake = false;
                break;
            }
        }

        // Move resources.
        if (toGive && toTake)
        {
            foreach (var i in order.Resources)
            {
                order.TargetGameObject.Resources.Remove(i.Key, i.Value);
                Resources.Add(i.Key, i.Value);
            }
        }

        Orders.Pop();
    }

    protected virtual void OnOrderMove()
    {
        var order = Orders.First();
        Vector3 target;

        if (order.TargetGameObject != null)
        {
            target = order.TargetGameObject.transform.position;
        }
        else
        {
            target = order.TargetPosition;
        }

        var distanceToTarget = (target - transform.position).magnitude;
        var distanceToTravel = 10 * Time.deltaTime;

        if (distanceToTarget > distanceToTravel)
        {
            var direction = target - transform.position;
            direction.y = 0;
            direction.Normalize();

            transform.LookAt(target);
            transform.Translate(Vector3.forward * distanceToTravel);
        }
        else
        {
            var direction = target - transform.position;
            direction.y = 0;
            direction.Normalize();

            transform.LookAt(target);
            transform.position = target;

            Orders.Pop();
        }
    }

    protected virtual void OnOrderPatrol()
    {
        var order = Orders.First();

        Move(order.TargetPosition);
        Move(transform.position);

        Orders.MoveToEnd();
    }

    protected virtual void OnOrderProduce()
    {
        var order = Orders.First();

        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished)
        {
            foreach (var recipe in Recipes)
            {
                // Have all resources to consume.
                bool toConsume = true;

                foreach (var i in recipe.ToConsume)
                {
                    if (Resources.CanRemove(i.Name, i.Count) == false)
                    {
                        toConsume = false;
                        break;
                    }
                }

                // Have all resources to produce.
                bool toProduce = true;

                foreach (var i in recipe.ToConsume)
                {
                    if (Resources.CanAdd(i.Name, i.Count) == false)
                    {
                        toProduce = false;
                        break;
                    }
                }

                // Produce new resources.
                if (toConsume && toProduce)
                {
                    foreach (var i in recipe.ToConsume)
                    {
                        Resources.Remove(i.Name, i.Count);
                    }

                    foreach (var i in recipe.ToProduce)
                    {
                        Resources.Add(i.Name, i.Count);
                    }
                }
            }

            order.Timer.Reset();
        }

        Orders.MoveToEnd();
    }

    protected virtual void OnOrderStop()
    {
        Orders.Clear();
    }

    protected virtual void OnOrderTransport()
    {
        var order = Orders.First();

        Move(order.SourceGameObject);
        Load(order.SourceGameObject, order.Resources);
        Move(order.TargetGameObject);
        Unload(order.TargetGameObject, order.Resources);

        Orders.MoveToEnd();
    }

    protected virtual void OnOrderUnload()
    {
        var order = Orders.First();

        // Have all resources to give.
        bool toGive = true;

        foreach (var i in order.Resources)
        {
            if (Resources.CanRemove(i.Key, i.Value) == false)
            {
                toGive = false;
                break;
            }
        }

        // Have all resources to take.
        bool toTake = true;

        foreach (var i in order.Resources)
        {
            if (order.TargetGameObject.Resources.CanAdd(i.Key, i.Value) == false)
            {
                toTake = false;
                break;
            }
        }

        // Move resources.
        if (toGive && toTake)
        {
            foreach (var i in order.Resources)
            {
                Resources.Remove(i.Key, i.Value);
                order.TargetGameObject.Resources.Add(i.Key, i.Value);
            }
        }

        Orders.Pop();
    }
    #endregion

    public OrderContainer Orders { get; private set; }

    public Dictionary<OrderType, UnityAction> OrderHandlers { get; private set; }

    public ResourceContainer Resources { get; private set; }

    public List<Recipe> Recipes { get; private set; }
}
