using System.Collections.Generic;
using UnityEngine;

public class MyGameObject : MonoBehaviour
{
    public void OnDamage(float damage)
    {
        Health -= damage;

        Stats.Add(Stats.DamageTaken, damage);
    }

    public bool IsConstructed()
    {
        foreach (KeyValuePair<string, Resource> i in ConstructionResources.Items)
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

    public void MoveResources(MyGameObject source, MyGameObject target, Dictionary<string, int> resources)
    {
        foreach (KeyValuePair<string, int> i in resources)
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

    protected virtual void Awake()
    {
        Orders = new OrderContainer();
        Orders.AllowOrder(OrderType.Destroy);
        Orders.AllowOrder(OrderType.Idle);
        Orders.AllowOrder(OrderType.Stop);
        Orders.AllowOrder(OrderType.Wait);

        Resources = new ResourceContainer();
        Recipes = new RecipeContainer();
        Stats = new Stats();

        OrderHandlers = new Dictionary<OrderType, IOrderHandler>()
        {
            { OrderType.Attack, new OrderHandlerAttack() },
            { OrderType.Construct, new OrderHandlerConstruct() },
            { OrderType.Destroy, new OrderHandlerDestroy() },
            { OrderType.Follow, new OrderHandlerFollow() },
            { OrderType.Guard, new OrderHandlerGuard() },
            { OrderType.Load, new OrderHandlerLoad() },
            { OrderType.Move, new OrderHandlerMove() },
            { OrderType.Patrol, new OrderHandlerPatrol() },
            { OrderType.Produce, new OrderHandlerProduce() },
            { OrderType.Rally, new OrderHandlerRally() },
            { OrderType.Repair, new OrderHandlerRepair() },
            { OrderType.Research, new OrderHandlerResearch() },
            { OrderType.Stop, new OrderHandlerStop() },
            { OrderType.Transport, new OrderHandlerTransport() },
            { OrderType.Unload, new OrderHandlerUnload() },
            { OrderType.Wait, new OrderHandlerWait() },
        };

        ConstructionResources = new ResourceContainer();
        ConstructionResources.Add("Metal", 0, 30);

        ConstructionRecipies = new RecipeContainer();

        Recipe r1 = new Recipe();

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

    private void AlignPositionToTerrain()
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

    private void ProcessOrders()
    {
        if (Orders.Count > 0)
        {
            Order order = Orders.First();

            if (Orders.IsAllowed(order.Type) && OrderHandlers.ContainsKey(order.Type))
            {
                OrderHandlers[order.Type].OnExecute(this);
            }
            else
            {
                Orders.Pop();
            }
        }
        else if (OrderHandlers.ContainsKey(OrderType.Idle))
        {
            OrderHandlers[OrderType.Idle].OnExecute(this);
        }
    }

    void RaiseResourceFlags()
    {
        foreach (Recipe recipe in Recipes.Items)
        {
            foreach (RecipeComponent resource in recipe.ToConsume)
            {
                int capacity = Resources.Capacity(resource.Name);

                if (capacity > 0)
                {
                    Game.Instance.Consumers.Add(this, resource.Name, capacity);
                }
                else
                {
                    Game.Instance.Consumers.Remove(this, resource.Name);
                }
            }

            foreach (RecipeComponent resource in recipe.ToProduce)
            {
                int storage = Resources.Storage(resource.Name);

                if (storage > 0)
                {
                    Game.Instance.Producers.Add(this, resource.Name, storage);
                }
                else
                {
                    Game.Instance.Producers.Remove(this, resource.Name);
                }
            }
        }
    }

    void RaiseConstructionResourceFlags()
    {
        Game game = GameObject.Find("Game").GetComponent<Game>();

        foreach (Recipe recipe in ConstructionRecipies.Items)
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

    public bool IsCloseTo(Vector3 position, float radius = 1.0f)
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

    public float Damage { get; protected set; } = 10.0f;

    public float ConstructionTime { get; protected set; } = 10.0f;

    public float Health { get; protected set; } = 10.0f;

    public float MaxHealth { get; protected set; } = 10.0f;

    public float LoadTime { get; protected set; } = 10.0f;

    public float ProduceTime { get; protected set; } = 10.0f;

    public float Speed { get; protected set; } = 10.0f;

    public float UnloadTime { get; protected set; } = 10.0f;

    public float WaitTime { get; protected set; } = 10.0f;

    public string MissilePrefab { get; protected set; } = string.Empty;

    public float MissileRangeMax { get; protected set; } = 10.0f;

    public float MissileRangeMin { get; protected set; } = 10.0f; // TODO: Implement.

    public Vector3 Position { get => transform.position; }

    public OrderContainer Orders { get; private set; }

    public ResourceContainer Resources { get; private set; }

    public RecipeContainer Recipes { get; private set; }

    public Stats Stats { get; private set; }

    public MyGameObjectState State { get; set; } = MyGameObjectState.Operational;

    public ResourceContainer ConstructionResources { get; private set; }

    public RecipeContainer ConstructionRecipies { get; private set; }

    public Vector3 RallyPoint { get; set; }
    
    public Timer ReloadTimer { get; protected set; }

    public MyGameObject Parent { get; set; }

    protected Dictionary<OrderType, IOrderHandler> OrderHandlers { get; set; }
}
