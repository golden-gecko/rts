using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyGameObject : MonoBehaviour
{
    protected virtual void Start()
    {
        Orders = new OrderContainer();
        Resources = new ResourceContainer();
        Recipes = new List<Recipe>();

        OrderHandlers = new Dictionary<OrderType, UnityAction>()
        {
            { OrderType.Attack, OnOrderAttack },
            { OrderType.Construct, OnOrderConstruct },
            { OrderType.Follow, OnOrderFollow },
            { OrderType.Guard, OnOrderGuard },
            { OrderType.Idle, OnOrderIdle },
            { OrderType.Load, OnOrderLoad },
            { OrderType.Move, OnOrderMove },
            { OrderType.Patrol, OnOrderPatrol },
            { OrderType.Produce, OnOrderProduce },
            { OrderType.Research, OnOrderResearch },
            { OrderType.Stop, OnOrderStop },
            { OrderType.Transport, OnOrderTransport },
            { OrderType.Unload, OnOrderUnload },
        };

        Orders.Allow(OrderType.Idle);
        Orders.Allow(OrderType.Stop);
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
            // Idle();
        }
    }

    public void Select(bool status)
    {
        var selection = transform.Find("Selection");

        if (selection != null)
        {
            selection.gameObject.SetActive(status);
        }
    }

    public void Attack(Vector3 target)
    {
        Orders.Add(new Order(OrderType.Attack, target));
    }

    public void Attack(MyGameObject target)
    {
        Orders.Add(new Order(OrderType.Attack, target));
    }

    public void Idle()
    {
        Orders.Add(new Order(OrderType.Idle));
    }

    public void Load(MyGameObject target, Dictionary<string, int> resources)
    {
        Orders.Add(new Order(OrderType.Load, target, resources));
    }

    public void Move(Vector3 target)
    {
        Orders.Add(new Order(OrderType.Move, target));
    }

    public void Move(MyGameObject target)
    {
        Orders.Add(new Order(OrderType.Move, target));
    }

    public void Patrol(Vector3 target)
    {
        Orders.Add(new Order(OrderType.Patrol, target));
    }

    public void Patrol(MyGameObject target)
    {
        Orders.Add(new Order(OrderType.Patrol, target));
    }

    public void Produce()
    {
    }

    public void Stop()
    {
        Orders.Add(new Order(OrderType.Stop));
    }

    public void Transport(MyGameObject source, MyGameObject target, Dictionary<string, int> resources)
    {
        Orders.Add(new Order(OrderType.Transport, source, target, resources));
    }

    public void Unload(MyGameObject target, Dictionary<string, int> resources)
    {
        Orders.Add(new Order(OrderType.Unload, target, resources));
    }

    protected virtual void OnOrderAttack()
    {
        var order = Orders.First();
    }

    protected virtual void OnOrderConstruct()
    {
        var order = Orders.First();
    }

    protected virtual void OnOrderFollow()
    {
        var order = Orders.First();

        if (order.TargetGameObject == null)
        {
            Move(order.TargetPosition);
        }
        else
        {
            Move(order.TargetGameObject);
        }

        Orders.MoveToEnd();
    }

    protected virtual void OnOrderGuard()
    {
        var order = Orders.First();

        if (order.TargetGameObject == null)
        {
            Move(order.TargetPosition);
        }
        else
        {
            Move(order.TargetGameObject);
        }

        Orders.MoveToEnd();
    }

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
            MoveResources(order.TargetGameObject, this, order.Resources);
        }

        Orders.Pop();
    }

    protected virtual void OnOrderMove()
    {
        var order = Orders.First();

        Vector3 target;
        Vector3 position = transform.position;

        if (order.TargetGameObject != null)
        {
            target = order.TargetGameObject.Entrance;
        }
        else
        {
            target = order.TargetPosition;
        }

        target.y = 0;
        position.y = 0;

        var distanceToTarget = (target - transform.position).magnitude;
        var distanceToTravel = Speed * Time.deltaTime;

        if (distanceToTarget > distanceToTravel)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.forward * distanceToTravel);
        }
        else
        {
            transform.LookAt(target);
            transform.position = target;

            Orders.Pop();
        }
    }

    protected virtual void OnOrderPatrol()
    {
        var order = Orders.First();

        if (order.TargetGameObject == null)
        {
            Move(order.TargetPosition);
            Move(transform.position);
        }
        else
        {
            Move(order.TargetGameObject);
            Move(transform.position);
        }

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

    protected virtual void OnOrderResearch()
    {
        var order = Orders.First();
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
            MoveResources(this, order.TargetGameObject, order.Resources);
        }

        Orders.Pop();
    }

    void MoveResources(MyGameObject source, MyGameObject target, Dictionary<string, int> resources)
    {
        foreach (var i in resources)
        {
            source.Resources.Remove(i.Key, i.Value);
            target.Resources.Add(i.Key, i.Value);
        }
    }

    public Vector3 Entrance
    {
        get
        {
            var size = GetComponent<Collider>().bounds.size;

            return new Vector3(transform.position.x, transform.position.y, transform.position.z + size.x * 0.75f);
        }
    }

    public OrderContainer Orders { get; private set; }

    public Dictionary<OrderType, UnityAction> OrderHandlers { get; private set; }

    public ResourceContainer Resources { get; private set; }

    public List<Recipe> Recipes { get; private set; }

    public float Health { get; } = 100;

    public float Speed { get; } = 10;
}
