using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            if (order.TargetGameObject.Resources.ContainsKey(i.Key) == false || order.TargetGameObject.Resources[i.Key].Value - i.Value < 0)
            {
                toGive = false;
                break;
            }
        }

        // Have all resources to take.
        bool toTake = true;

        foreach (var i in order.Resources)
        {
            if (Resources.ContainsKey(i.Key) == false || Resources[i.Key].Value + i.Value > Resources[i.Key].Max)
            {
                toTake = false;
                break;
            }
        }

        if (toGive && toTake)
        {
            foreach (var i in order.Resources)
            {
                order.TargetGameObject.Resources[i.Key].Remove(i.Value);
                Resources[i.Key].Add(i.Value);
            }
        }

        Orders.Pop();
    }

    protected virtual void OnOrderMove()
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

    protected virtual void OnOrderPatrol()
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
                    if (Resources.ContainsKey(i.Name) == false || Resources[i.Name].Value - i.Count < 0)
                    {
                        toConsume = false;
                        break;
                    }
                }

                // Have all resources to produce.
                bool toProduce = true;

                foreach (var i in recipe.ToConsume)
                {
                    if (Resources.ContainsKey(i.Name) == false || Resources[i.Name].Value + i.Count > Resources[i.Name].Max)
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
                        Resources[i.Name].Remove(i.Count);
                    }

                    foreach (var i in recipe.ToProduce)
                    {
                        Resources[i.Name].Add(i.Count);
                    }
                }
            }

            order.Timer.Reset();
        }
    }

    protected virtual void OnOrderStop()
    {
        Orders.Clear();
    }

    protected virtual void OnOrderUnload()
    {
    }
    #endregion

    public OrderContainer Orders { get; private set; }

    public Dictionary<OrderType, UnityAction> OrderHandlers { get; private set; }

    public ResourceContainer Resources { get; private set; }

    public List<Recipe> Recipes { get; private set; }
}
