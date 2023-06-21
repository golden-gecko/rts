using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum MyGameObjectState
{
    Operational,
    UnderAssembly,
    UnderConstruction,
}

public class MyGameObject : MonoBehaviour
{
    protected virtual void Start()
    {
        Orders = new OrderContainer();
        Resources = new ResourceContainer();
        Recipes = new RecipeContainer();
        Stats = new Stats();

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
            { OrderType.Wait, OnOrderWait },
        };

        Orders.AllowOrder(OrderType.Idle);
        Orders.AllowOrder(OrderType.Stop);
        Orders.AllowOrder(OrderType.Wait);

        ConstructionResources = new ResourceContainer();
        ConstructionResources.Add("Metal", 0, 30);

        ConstructionRecipies = new RecipeContainer();

        var r1 = new Recipe();

        r1.Consume("Metal", 0);

        ConstructionRecipies.Add(r1);

        RallyPoint = Exit;
    }

    protected virtual void Update()
    {
        switch (State)
        {
            case MyGameObjectState.Operational:
                ProcessOrders();
                RaiseResourceFlags();
                break;

            case MyGameObjectState.UnderAssembly:
                break;

            case MyGameObjectState.UnderConstruction:
                RaiseConstructionResourceFlags();
                break;
        }

        AlignPositionToTerrain();
    }

    public bool IsConstructed()
    {
        foreach (var i in ConstructionResources)
        {
            if (i.Value.Value < i.Value.Max)
            {
                return false;
            }
        }

        return true;
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

    public void Construct(string prefab, PrefabConstructionType prefabConstructionType)
    {
        Orders.Add(new Order(OrderType.Construct, prefab, prefabConstructionType, ConstructionTime));
    }

    public void Construct(string prefab, PrefabConstructionType prefabConstructionType, Vector3 target)
    {
        Orders.Add(new Order(OrderType.Construct, prefab, prefabConstructionType, target, ConstructionTime));
    }

    public void Guard(Vector3 target)
    {
        Orders.Add(new Order(OrderType.Guard, target));
    }

    public void Guard(MyGameObject target)
    {
        Orders.Add(new Order(OrderType.Guard, target));
    }

    public void Idle()
    {
        Orders.Add(new Order(OrderType.Idle));
    }

    public void Load(MyGameObject target, Dictionary<string, int> resources)
    {
        Orders.Add(new Order(OrderType.Load, target, resources, LoadTime, 3));
    }

    public void Move(Vector3 target, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, new Order(OrderType.Move, target, priority));
        }
        else
        {
            Orders.Add(new Order(OrderType.Move, target, priority));
        }
    }

    public void Move(MyGameObject target, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, new Order(OrderType.Move, target, priority));
        }
        else
        {
            Orders.Add(new Order(OrderType.Move, target, priority));
        }
    }

    public void Patrol(Vector3 target)
    {
        Orders.Add(new Order(OrderType.Patrol, target));
    }

    public void Patrol(MyGameObject target)
    {
        Orders.Add(new Order(OrderType.Patrol, target));
    }

    public void Rally(Vector3 target, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, new Order(OrderType.Rally, target));
        }
        else
        {
            Orders.Add(new Order(OrderType.Rally, target));
        }
    }

    public void Produce()
    {
        Orders.Add(new Order(OrderType.Produce, ProduceTime));
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
        Orders.Add(new Order(OrderType.Unload, target, resources, UnloadTime, 3));
    }

    public void Wait(int priority = -1)
    {
        // TODO: Test.
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, new Order(OrderType.Wait, WaitTime)); 
        }
        else
        {
            Orders.Add(new Order(OrderType.Wait, WaitTime));
        }
    }

    private void GetPositionToAttack(Vector3 target)
    {

    }

    protected virtual void OnOrderAttack()
    {
        Order order = Orders.First();

        Vector3 position;

        if (order.TargetGameObject == null)
        {
            position = order.TargetPosition;
        }
        else
        {
            position = order.TargetGameObject.Position;
        }

        GetPositionToAttack()

        Vector3 direction = position - Position;
        direction.Normalize();

        if ((position - Position).magnitude > MissileRange)
        {
            Move(position);
        }
        else
        {
            MyGameObject resource = UnityEngine.Resources.Load<MyGameObject>(order.Prefab); // TODO: Remove name conflict.
            MyGameObject missile = Instantiate<MyGameObject>(resource, Position, Quaternion.identity);

            missile.Move(position);

            Orders.Pop();
            // Orders.MoveToEnd();
        }
    }

    protected virtual void OnOrderConstruct()
    {
        var order = Orders.First();

        switch (order.PrefabConstructionType)
        {
            case PrefabConstructionType.Structure:
                if (IsCloseTo(order.TargetPosition + new Vector3(0, 0, 1)) == false)
                {
                    Move(order.TargetPosition + new Vector3(0, 0, 1), 0); // TODO: Add offset based on object size.
                }
                else if (order.TargetGameObject == null)
                {
                    var resource = UnityEngine.Resources.Load<MyGameObject>(order.Prefab); // TODO: Remove name conflict.

                    order.TargetGameObject = Instantiate<MyGameObject>(resource, order.TargetPosition, Quaternion.identity);
                    order.TargetGameObject.State = MyGameObjectState.UnderConstruction;
                }
                else if (order.TargetGameObject.IsConstructed())
                {
                    order.Timer.Update(Time.deltaTime);

                    if (order.Timer.Finished)
                    {
                        order.TargetGameObject.State = MyGameObjectState.Operational;
                        order.Timer.Reset();

                        Orders.Pop();

                        Stats.Add(Stats.OrdersExecuted, 1);
                        Stats.Add(Stats.TimeConstructing, order.Timer.Max);
                    }
                }
                else
                {
                    Wait(0);
                }

                break;

            case PrefabConstructionType.Unit:
                if (order.TargetGameObject == null)
                {
                    var resource = UnityEngine.Resources.Load<MyGameObject>(order.Prefab); // TODO: Remove name conflict.

                    order.TargetGameObject = Instantiate<MyGameObject>(resource, Exit, Quaternion.identity);
                    order.TargetGameObject.State = MyGameObjectState.UnderAssembly;
                }
                else if (order.TargetGameObject.IsConstructed() == false)
                {
                    MoveResourcesToUnit(order);
                }
                else if (order.TargetGameObject.IsConstructed())
                {
                    order.Timer.Update(Time.deltaTime);

                    if (order.Timer.Finished)
                    {
                        order.TargetGameObject.State = MyGameObjectState.Operational;
                        order.TargetGameObject.Move(RallyPoint);
                        order.Timer.Reset();

                        Orders.Pop();

                        Stats.Add(Stats.OrdersExecuted, 1);
                        Stats.Add(Stats.TimeConstructing, order.Timer.Max);
                    }
                }
                else
                {
                    Wait(0);
                }

                break;
        }
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

        // Update timer.
        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished == false)
        {
            return;
        }

        // Check storage and capacity.
        var resources = new Dictionary<string, int>();

        foreach (var i in order.Resources)
        {
            var value = Mathf.Min(new int[] { i.Value, order.TargetGameObject.Resources.Storage(i.Key), Resources.Capacity(i.Key) });

            if (value > 0)
            {
                resources[i.Key] = value;
            }
        }

        // Move resources or wait for them.
        if (resources.Count > 0)
        {
            MoveResources(order.TargetGameObject, this, resources);

            Orders.Pop();

            Stats.Add(Stats.OrdersExecuted, 1);
        }
        else
        {
            order.Retry();
            order.Timer.Reset();

            if (order.CanRetry)
            {
                Wait(0);
            }
            else
            {
                Orders.Pop();

                Stats.Add(Stats.OrdersFailed, 1);

                GameObject.Find("Canvas").GetComponentInChildren<InGameMenuController>().Log("Failed to execute load order");
            }
        }
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

            Stats.Add(Stats.DistanceDriven, distanceToTravel);
        }
        else
        {
            transform.position = target;

            Stats.Add(Stats.DistanceDriven, distanceToTarget);
            Stats.Add(Stats.OrdersExecuted, 1);

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

                            Stats.Add(Stats.ResourcesProduced, i.Count);
                        }
                    }

                    order.Timer.Reset();

                    Orders.MoveToEnd();

                    Stats.Add(Stats.OrdersExecuted, 1);
                    Stats.Add(Stats.TimeProducing, order.Timer.Max);
                }
            }
        }
    }

    protected virtual void OnOrderRally()
    {
        var order = Orders.First();

        RallyPoint = order.TargetPosition;

        Orders.Pop();
    }

    protected virtual void OnOrderResearch()
    {
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

        Orders.Pop();
    }

    protected virtual void OnOrderUnload()
    {
        var order = Orders.First();

        // Update timer.
        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished == false)
        {
            return;
        }

        // Check storage and capacity.
        var resources = new Dictionary<string, int>();

        foreach (var i in order.Resources)
        {
            if (order.TargetGameObject.State == MyGameObjectState.UnderConstruction)
            {
                var value = Mathf.Min(new int[] { i.Value, Resources.Storage(i.Key), order.TargetGameObject.ConstructionResources.Capacity(i.Key) });

                if (value > 0)
                {
                    resources[i.Key] = value;

                    Stats.Add(Stats.ResourcesTransported, value);
                }
            }
            else
            {
                var value = Mathf.Min(new int[] { i.Value, Resources.Storage(i.Key), order.TargetGameObject.Resources.Capacity(i.Key) });

                if (value > 0)
                {
                    resources[i.Key] = value;

                    Stats.Add(Stats.ResourcesTransported, value);
                }
            }
        }

        // Move resources or wait for them.
        if (resources.Count > 0)
        {
            MoveResources(this, order.TargetGameObject, resources);

            Orders.Pop();

            Stats.Add(Stats.OrdersExecuted, 1);
        }
        else
        {
            order.Retry();
            order.Timer.Reset();

            if (order.CanRetry)
            {
                Wait(0);
            }
            else
            {
                Orders.Pop();

                Stats.Add(Stats.OrdersFailed, 1);

                GameObject.Find("Canvas").GetComponentInChildren<InGameMenuController>().Log("Failed to execute load order");
            }
        }
    }

    protected virtual void OnOrderWait()
    {
        var order = Orders.First();

        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished)
        {
            order.Timer.Reset();

            Orders.Pop();

            // Stats.Add(Stats.OrdersExecuted, 1); // TODO: Should we count wait as order?
            Stats.Add(Stats.TimeWaiting, order.Timer.Max);
        }
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
        switch (State)
        {
            case MyGameObjectState.UnderAssembly:
            case MyGameObjectState.UnderConstruction:
                return string.Format("ID: {0}\nName: {1}\nResources:{2}", GetInstanceID(), name, ConstructionResources.GetInfo());
        }

        string info = string.Format("ID: {0}\nName: {1}\nHP: {2}", GetInstanceID(), name, Health);

        if (Speed > 0)
        {
            info += string.Format("\nSpeed: {0}", Speed);
        }

        info += string.Format("\nResources:{0}\nOrders: {1}\nStats: {2}", Resources.GetInfo(), Orders.GetInfo(), Stats.GetInfo());

        return info;
    }

    void MoveResources(MyGameObject source, MyGameObject target, Dictionary<string, int> resources)
    {
        foreach (var i in resources)
        {
            source.Resources.Remove(i.Key, i.Value);

            if (target.State == MyGameObjectState.UnderConstruction)
            {
                target.ConstructionResources.Add(i.Key, i.Value);
            }
            else
            {
                target.Resources.Add(i.Key, i.Value);
            }
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
        var game = GameObject.Find("Game").GetComponent<Game>();

        foreach (var recipe in Recipes)
        {
            foreach (var resource in recipe.ToConsume)
            {
                var capacity = Resources.Capacity(resource.Name);

                if (capacity > 0)
                {
                    game.Consumers.Add(this, resource.Name, capacity);
                }
                else
                {
                    game.Consumers.Remove(this, resource.Name);
                }
            }

            foreach (var resource in recipe.ToProduce)
            {
                var storage = Resources.Storage(resource.Name);

                if (storage > 0)
                {
                    game.Producers.Add(this, resource.Name, storage);
                }
                else
                {
                    game.Producers.Remove(this, resource.Name);
                }
            }
        }
    }

    void RaiseConstructionResourceFlags()
    {
        var game = GameObject.Find("Game").GetComponent<Game>();

        foreach (var recipe in ConstructionRecipies)
        {
            foreach (var resource in recipe.ToConsume)
            {
                var capacity = ConstructionResources.Capacity(resource.Name);

                if (capacity > 0)
                {
                    game.Consumers.Add(this, resource.Name, capacity);
                }
                else
                {
                    game.Consumers.Remove(this, resource.Name);
                }
            }
        }
    }

    void MoveResourcesToUnit(Order order)
    {
        foreach (var i in order.TargetGameObject.ConstructionResources)
        {
            var capacity = i.Value.Capacity();
            var storage = Resources.Storage(i.Key);

            var value = Mathf.Min(new int[] { capacity, storage });

            if (value > 0)
            {
                Resources.Remove(i.Key, value);
                i.Value.Add(value);
            }
        }
    }

    public bool IsCloseTo(Vector3 position)
    {
        var a = position;
        var b = transform.position;

        a.y = 0;
        b.y = 0;

        return (b - a).magnitude < 1;
    }

    public Vector3 Entrance
    {
        get
        {
            var size = GetComponent<Collider>().bounds.size;

            return new Vector3(transform.position.x, transform.position.y, transform.position.z + size.z * 0.75f);
        }
    }

    public Vector3 Exit
    {
        get
        {
            var size = GetComponent<Collider>().bounds.size;

            return new Vector3(transform.position.x, transform.position.y, transform.position.z - size.z * 0.75f);
        }
    }

    public Vector3 Position { get => transform.position; }

    public OrderContainer Orders { get; private set; }

    public Dictionary<OrderType, UnityAction> OrderHandlers { get; private set; }

    public ResourceContainer Resources { get; private set; }

    public RecipeContainer Recipes { get; private set; }

    [field: SerializeField]
    public float ConstructionTime { get; protected set; } = 10;

    [field: SerializeField]
    public float Health { get; protected set; } = 100;

    [field: SerializeField]
    public float MaxHealth { get; protected set; } = 100;

    [field: SerializeField]
    public float LoadTime { get; protected set; } = 2;

    [field: SerializeField]
    public float ProduceTime { get; protected set; } = 4;

    [field: SerializeField]
    public float Speed { get; protected set; } = 0;

    [field: SerializeField]
    public float UnloadTime { get; protected set; } = 2;

    [field: SerializeField]
    public float WaitTime { get; protected set; } = 2;

    public Stats Stats { get; private set; }

    public MyGameObjectState State { get; set; } = MyGameObjectState.Operational;

    public ResourceContainer ConstructionResources { get; private set; }

    public RecipeContainer ConstructionRecipies { get; private set; }

    public Vector3 RallyPoint { get; protected set; }

    [field: SerializeField]
    public Player Player { get; set; }
    
    [field: SerializeField]
    public float Damage { get; protected set; } = 0;

    [field: SerializeField]
    public string MissilePrefab { get; protected set; }

    [field: SerializeField]
    public float MissileRange { get; protected set; } = 0;

    [field: SerializeField]
    public float ReloadTime { get; protected set; } = 0;
}
