using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyGameObject : MonoBehaviour
{
    protected virtual void Awake()
    {
        // TODO: Move initialization of other objects to Awake method.

        Orders = new OrderContainer();
        Orders.AllowOrder(OrderType.Destroy);
        Orders.AllowOrder(OrderType.Idle);
        Orders.AllowOrder(OrderType.Stop);
        Orders.AllowOrder(OrderType.Wait);

        Resources = new ResourceContainer();
        Recipes = new RecipeContainer();
        Stats = new Stats();

        OrderHandlers = new Dictionary<OrderType, UnityAction>()
        {
            { OrderType.Attack, OnOrderAttack },
            { OrderType.Construct, OnOrderConstruct },
            { OrderType.Destroy, OnOrderDestroy },
            { OrderType.Follow, OnOrderFollow },
            { OrderType.Guard, OnOrderGuard },
            { OrderType.Idle, OnOrderIdle },
            { OrderType.Load, OnOrderLoad },
            { OrderType.Move, OnOrderMove },
            { OrderType.Patrol, OnOrderPatrol },
            { OrderType.Produce, OnOrderProduce },
            { OrderType.Rally, OnOrderRally },
            { OrderType.Repair, OnOrderRepair },
            { OrderType.Research, OnOrderResearch },
            { OrderType.Stop, OnOrderStop },
            { OrderType.Transport, OnOrderTransport },
            { OrderType.Unload, OnOrderUnload },
            { OrderType.Wait, OnOrderWait },
        };

        ConstructionResources = new ResourceContainer();
        ConstructionResources.Add("Metal", 0, 30);

        ConstructionRecipies = new RecipeContainer();

        var r1 = new Recipe();

        r1.Consume("Metal", 0);

        ConstructionRecipies.Add(r1);
    }

    protected virtual void Start()
    {
        RallyPoint = Exit;
    }

    protected virtual void Update()
    {
        if (Health <= 0.0f)
        {
            Destroy(0);
        }

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
        Reload();
    }

    public void OnDamage(float damage)
    {
        Health -= damage;

        Stats.Add(Stats.DamageTaken, damage);
    }

    public bool IsConstructed()
    {
        foreach (KeyValuePair<string, Resource> i in ConstructionResources)
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
        Transform selection = transform.Find("Selection");

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

    public void Destroy(int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, new Order(OrderType.Destroy));
        }
        else
        {
            Orders.Add(new Order(OrderType.Destroy));
        }
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
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, new Order(OrderType.Wait, WaitTime)); 
        }
        else
        {
            Orders.Add(new Order(OrderType.Wait, WaitTime));
        }
    }

    private Vector3 GetPositionToAttack(Vector3 target)
    {
        Vector3 direction = target - Position;

        direction.Normalize();

        return target - direction * MissileRangeMax * 0.9f;
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

        if (IsCloseTo(position, MissileRangeMax) == false) // TODO: Test by adding visual debug.
        {
            Move(GetPositionToAttack(position), 0); // TODO: Test by adding visual debug.
        }
        else
        {
            transform.LookAt(new Vector3(position.x, Position.y, position.z));

            if (ReloadTimer.Finished)
            {
                MyGameObject resource = UnityEngine.Resources.Load<MyGameObject>(MissilePrefab); // TODO: Remove name conflict.
                MyGameObject missile = Instantiate<MyGameObject>(resource, Position, Quaternion.identity);

                missile.Parent = this;
                missile.Player = Player;

                missile.Move(position);
                missile.Destroy();

                ReloadTimer.Reset();

                Orders.MoveToEnd();

                Stats.Add(Stats.MissilesFired, 1);
            }
        }
    }

    protected virtual void OnOrderConstruct()
    {
        Order order = Orders.First();

        switch (order.PrefabConstructionType)
        {
            case PrefabConstructionType.Structure:
                if (IsCloseTo(order.TargetPosition + new Vector3(0, 0, 1)) == false)
                {
                    Move(order.TargetPosition + new Vector3(0, 0, 1), 0); // TODO: Add offset based on object size.
                }
                else if (order.TargetGameObject == null)
                {
                    MyGameObject resource = UnityEngine.Resources.Load<MyGameObject>(order.Prefab); // TODO: Remove name conflict.

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
                    MyGameObject resource = UnityEngine.Resources.Load<MyGameObject>(order.Prefab); // TODO: Remove name conflict.

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

    protected virtual void OnOrderDestroy()
    {
        Object resource = UnityEngine.Resources.Load("Effects/WFXMR_ExplosiveSmoke"); // TODO: Remove name conflict.
        Object effect = Instantiate(resource, Position, Quaternion.identity);

        GameObject.Destroy(gameObject);

        Orders.Pop();
    }

    protected virtual void OnOrderFollow()
    {
        Order order = Orders.First();

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
        Order order = Orders.First();

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
        Order order = Orders.First();

        // Update timer.
        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished == false)
        {
            return;
        }

        // Check storage and capacity.
        Dictionary<string, int> resources = new Dictionary<string, int>();

        foreach (KeyValuePair<string, int> i in order.Resources)
        {
            int value = Mathf.Min(new int[] { i.Value, order.TargetGameObject.Resources.Storage(i.Key), Resources.Capacity(i.Key) });

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
        Order order = Orders.First();

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

        float distanceToTarget = (target - position).magnitude;
        float distanceToTravel = Speed * Time.deltaTime;

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
        Order order = Orders.First();

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
        Order order = Orders.First();

        foreach (var recipe in Recipes)
        {
            // Have all resources to consume.
            bool toConsume = true;

            foreach (RecipeComponent i in recipe.ToConsume)
            {
                if (Resources.CanRemove(i.Name, i.Count) == false)
                {
                    toConsume = false;
                    break;
                }
            }

            // Have all resources to produce.
            bool toProduce = true;

            foreach (RecipeComponent i in recipe.ToProduce)
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
                        foreach (RecipeComponent i in recipe.ToConsume)
                        {
                            Resources.Remove(i.Name, i.Count);
                        }

                        foreach (RecipeComponent i in recipe.ToProduce)
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
        Order order = Orders.First();

        RallyPoint = order.TargetPosition;

        Orders.Pop();
    }

    protected virtual void OnOrderRepair()
    {
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
        Order order = Orders.First();

        Move(order.SourceGameObject);
        Load(order.SourceGameObject, order.Resources);
        Move(order.TargetGameObject);
        Unload(order.TargetGameObject, order.Resources);

        Orders.Pop();
    }

    protected virtual void OnOrderUnload()
    {
        Order order = Orders.First();

        // Update timer.
        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished == false)
        {
            return;
        }

        // Check storage and capacity.
        Dictionary<string, int> resources = new Dictionary<string, int>();

        foreach (KeyValuePair<string, int> i in order.Resources)
        {
            if (order.TargetGameObject.State == MyGameObjectState.UnderConstruction)
            {
                int value = Mathf.Min(new int[] { i.Value, Resources.Storage(i.Key), order.TargetGameObject.ConstructionResources.Capacity(i.Key) });

                if (value > 0)
                {
                    resources[i.Key] = value;

                    Stats.Add(Stats.ResourcesTransported, value);
                }
            }
            else
            {
                int value = Mathf.Min(new int[] { i.Value, Resources.Storage(i.Key), order.TargetGameObject.Resources.Capacity(i.Key) });

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
        Order order = Orders.First();

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
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position + new Vector3(0, 1000, 0), Vector3.down);

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

        string info = string.Format("ID: {0}\nName: {1}\nHP: {2:0.}/{3:0.}", GetInstanceID(), name, Health, MaxHealth);

        if (Speed > 0)
        {
            info += string.Format("\nSpeed: {0:0.}", Speed);
        }

        if (ReloadTimer != null)
        {
            info += string.Format("\nReload: {0:0.}/{1:0.}", ReloadTimer.Value, ReloadTimer.Max);
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
            Order order = Orders.First();

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
        Game game = GameObject.Find("Game").GetComponent<Game>();

        foreach (Recipe recipe in Recipes)
        {
            foreach (RecipeComponent resource in recipe.ToConsume)
            {
                int capacity = Resources.Capacity(resource.Name);

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
                int storage = Resources.Storage(resource.Name);

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
        Game game = GameObject.Find("Game").GetComponent<Game>();

        foreach (Recipe recipe in ConstructionRecipies)
        {
            foreach (RecipeComponent resource in recipe.ToConsume)
            {
                int capacity = ConstructionResources.Capacity(resource.Name);

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
        foreach (KeyValuePair<string, Resource> i in order.TargetGameObject.ConstructionResources)
        {
            int capacity = i.Value.Capacity();
            int storage = Resources.Storage(i.Key);
            int value = Mathf.Min(new int[] { capacity, storage });

            if (value > 0)
            {
                Resources.Remove(i.Key, value);
                i.Value.Add(value);
            }
        }
    }

    protected bool IsCloseTo(Vector3 position, float radius = 1.0f)
    {
        Vector3 a = position;
        Vector3 b = transform.position;

        a.y = 0.0f;
        b.y = 0.0f;

        return (b - a).magnitude < radius;
    }

    protected void Reload()
    {
        if (ReloadTimer != null)
        {
            ReloadTimer.Update(Time.deltaTime);
        }
    }

    public Vector3 Entrance
    {
        get
        {
            Vector3 size = GetComponent<Collider>().bounds.size;

            return new Vector3(transform.position.x, transform.position.y, transform.position.z + size.z * 0.75f);
        }
    }

    public Vector3 Exit
    {
        get
        {
            Vector3 size = GetComponent<Collider>().bounds.size;

            return new Vector3(transform.position.x, transform.position.y, transform.position.z - size.z * 0.75f);
        }
    }

    [field: SerializeField]
    public Player Player { get; set; }

    public float ConstructionTime { get; protected set; } = 0.0f;

    public float Health { get; protected set; } = 0.0f;

    public float MaxHealth { get; protected set; } = 0.0f;

    public float LoadTime { get; protected set; } = 0.0f;

    public float ProduceTime { get; protected set; } = 0.0f;

    public float Speed { get; protected set; } = 0.0f;

    public float UnloadTime { get; protected set; } = 0.0f;

    public float WaitTime { get; protected set; } = 0.0f;

    public float Damage { get; protected set; } = 0.0f;

    public string MissilePrefab { get; protected set; } = string.Empty;

    public float MissileRangeMax { get; protected set; } = 0.0f;

    public float MissileRangeMin { get; protected set; } = 0.0f; // TODO: Implement.

    public Vector3 Position { get => transform.position; }

    public OrderContainer Orders { get; private set; }

    public Dictionary<OrderType, UnityAction> OrderHandlers { get; private set; }

    public ResourceContainer Resources { get; private set; }

    public RecipeContainer Recipes { get; private set; }

    public Stats Stats { get; private set; }

    public MyGameObjectState State { get; set; } = MyGameObjectState.Operational;

    public ResourceContainer ConstructionResources { get; private set; }

    public RecipeContainer ConstructionRecipies { get; private set; }

    public Vector3 RallyPoint { get; protected set; }
    
    public Timer ReloadTimer { get; protected set; }

    public MyGameObject Parent { get; protected set; }
}
