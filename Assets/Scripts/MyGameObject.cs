using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class MyGameObject : MonoBehaviour
{
    protected virtual void Start()
    {
        Orders = new OrderContainer();
        Resources = new ResourceContainer();
        Recipes = new RecipeContainer();

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
            { OrderType.Rally, OnOrderRally },
            { OrderType.Research, OnOrderResearch },
            { OrderType.Stop, OnOrderStop },
            { OrderType.Transport, OnOrderTransport },
            { OrderType.Unload, OnOrderUnload },
        };

        Orders.AllowOrder(OrderType.Idle);
        Orders.AllowOrder(OrderType.Stop);
    }

    protected virtual void Update()
    {
        ProcessOrders();
        AlignPositionToTerrain();
        RaiseResourceFlags();
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
        // TODO: Refactor.
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

    protected virtual void OnOrderAttack()
    {
        var order = Orders.First();

        Orders.Pop();
    }

    protected virtual void OnOrderConstruct()
    {
        var order = Orders.First();

        Orders.Pop();
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

        var distanceToTarget = (target - position).magnitude;
        var distanceToTravel = Speed * Time.deltaTime;

        if (distanceToTarget > distanceToTravel)
        {
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
            transform.Translate(Vector3.forward * distanceToTravel);
        }
        else
        {
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

            foreach (var i in recipe.ToProduce)
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
                order.Timer.Update(Time.deltaTime);

                if (order.Timer.Finished)
                {
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

                        break;
                    }

                    order.Timer.Reset();

                    Orders.MoveToEnd();
                }
            }
        }
    }

    protected virtual void OnOrderRally()
    {
        var order = Orders.First();

        Orders.Pop();
    }

    protected virtual void OnOrderResearch()
    {
        var order = Orders.First();

        Orders.Pop();
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

    void AlignPositionToTerrain()
    {
        var hitInfo = new RaycastHit();
        var ray = new Ray(transform.position + new Vector3(0, 1000, 0), Vector3.down);

        if (Physics.Raycast(ray, out hitInfo, 2000, LayerMask.GetMask("Terrain")))
        {
            if (hitInfo.transform.tag == "Terrain")
            {
                transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
            }
        }
    }

    public string GetInfo()
    {
        return string.Format("ID: {0}\nName: {1}\nHP: {2}\nSpeed: {3}\nResources:{4}\nOrders: {5}", GetInstanceID(), name, Health, Speed, Resources.GetInfo(), Orders.GetInfo());
    }

    void MoveResources(MyGameObject source, MyGameObject target, Dictionary<string, int> resources)
    {
        foreach (var i in resources)
        {
            source.Resources.Remove(i.Key, i.Value);
            target.Resources.Add(i.Key, i.Value);
        }
    }

    void ProcessOrders()
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
            OnOrderIdle();
        }
    }

    void RaiseResourceFlags()
    {
        foreach (var i in Resources)
        {
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

    public RecipeContainer Recipes { get; private set; }

    public float Health { get; protected set; } = 100;

    public float Speed { get; protected set; } = 10;
}
