using System.Collections.Generic;
using UnityEngine;

public class MyGameObject : MonoBehaviour
{
    protected virtual void Awake()
    {
        Orders.AllowOrder(OrderType.Destroy);
        Orders.AllowOrder(OrderType.Idle);
        Orders.AllowOrder(OrderType.Stop);
        Orders.AllowOrder(OrderType.Wait);

        OrderHandlers[OrderType.Assemble] = new OrderHandlerAssemble();
        OrderHandlers[OrderType.Construct] = new OrderHandlerConstruct();
        OrderHandlers[OrderType.Destroy] = new OrderHandlerDestroy();
        OrderHandlers[OrderType.Follow] = new OrderHandlerFollow();
        OrderHandlers[OrderType.Guard] = new OrderHandlerGuard();
        OrderHandlers[OrderType.Load] = new OrderHandlerLoad();
        OrderHandlers[OrderType.Move] = new OrderHandlerMove();
        OrderHandlers[OrderType.Patrol] = new OrderHandlerPatrol();
        OrderHandlers[OrderType.Produce] = new OrderHandlerProduce();
        OrderHandlers[OrderType.Rally] = new OrderHandlerRally();
        OrderHandlers[OrderType.Repair] = new OrderHandlerRepair();
        OrderHandlers[OrderType.Research] = new OrderHandlerResearch();
        OrderHandlers[OrderType.Stop] = new OrderHandlerStop();
        OrderHandlers[OrderType.Transport] = new OrderHandlerTransport();
        OrderHandlers[OrderType.Unload] = new OrderHandlerUnload();
        OrderHandlers[OrderType.Wait] = new OrderHandlerWait();

        ConstructionResources.Add("Metal", 0, 30);

        Recipe r1 = new Recipe();

        r1.Consume("Metal", 0);

        ConstructionRecipies.Add(r1);
    }

    protected virtual void Start()
    {
        RallyPoint = Exit;

        Transform visual = transform.Find("Visual");
        
        if (visual)
        {
            Transform selection = visual.Find("Selection");

            if (selection)
            {
                selection.GetComponent<SpriteRenderer>().sprite = Player.Selection;
            }
        }
    }

    protected virtual void Update()
    {
        if (Alive == false)
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

    public void Assemble(string prefab)
    {
        Orders.Add(Order.Assemble(prefab, ConstructionTime));
    }

    public void Attack(Vector3 position)
    {
        Orders.Add(Order.Attack(position));
    }

    public void Attack(MyGameObject target)
    {
        Orders.Add(Order.Attack(target));
    }

    public void Construct(string prefab, Vector3 position)
    {
        Orders.Add(Order.Construct(prefab, position, ConstructionTime));
    }

    public void Destroy(int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Destroy());
        }
        else
        {
            Orders.Add(Order.Destroy());
        }
    }

    public void Follow(MyGameObject myGameObject)
    {
        Orders.Add(Order.Follow(myGameObject));
    }

    public void Guard(Vector3 position)
    {
        Orders.Add(Order.Guard(position));
    }

    public void Guard(MyGameObject myGameObject)
    {
        Orders.Add(Order.Guard(myGameObject));
    }

    public void Load(MyGameObject target, Dictionary<string, int> resources)
    {
        Orders.Add(Order.Load(target, resources, LoadTime));
    }

    public void Move(Vector3 position, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Move(position));
        }
        else
        {
            Orders.Add(Order.Move(position));
        }
    }

    public void Patrol(Vector3 position)
    {
        Orders.Add(Order.Patrol(position));
    }

    public void Produce()
    {
        Orders.Add(Order.Produce(ProduceTime));
    }

    public void Rally(Vector3 target, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Rally(target));
        }
        else
        {
            Orders.Add(Order.Rally(target));
        }
    }

    public void Stop()
    {
        Orders.Insert(0, Order.Stop());
    }

    public void Transport(MyGameObject sourceGameObject, MyGameObject targetGameObject, Dictionary<string, int> resources)
    {
        Orders.Add(Order.Transport(sourceGameObject, targetGameObject, resources, LoadTime));
    }

    public void Unload(MyGameObject target, Dictionary<string, int> resources)
    {
        Orders.Add(Order.Unload(target, resources, LoadTime));
    }

    public void Wait(int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Wait(WaitTime));
        }
        else
        {
            Orders.Add(Order.Wait(WaitTime));
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
            info += string.Format("\nReload: {0:0.}/{1:0.}", ReloadTimer.Current, ReloadTimer.Max);
        }

        info += string.Format("\nResources:{0}\nOrders: {1}\nStats: {2}", Resources.GetInfo(), Orders.GetInfo(), Stats.GetInfo());

        return info;
    }

    public bool IsAlly(MyGameObject myGameObject)
    {
        return Game.Instance.Diplomacy[this.Player][myGameObject.Player] == DiplomacyState.Ally;
    }

    public bool IsEnemy(MyGameObject myGameObject)
    {
        return Game.Instance.Diplomacy[this.Player][myGameObject.Player] == DiplomacyState.Enemy;
    }

    public void OnDamage(float damage)
    {
        Health -= damage;

        Stats.Add(Stats.DamageTaken, damage);
    }

    public void Select(bool status)
    {
        Transform visual = transform.Find("Visual");

        if (visual)
        {
            Transform range = visual.transform.Find("Range");
            Transform selection = visual.transform.Find("Selection");

            if (range)
            {
                Vector3 scale = transform.localScale;

                range.gameObject.SetActive(status);
                range.localScale = new Vector3(MissileRangeMax * 2.0f / scale.x, MissileRangeMax * 2.0f / scale.z, 1.0f); // TODO: Why y and z are replaced?
            }

            if (selection)
            {
                Vector3 size = GetComponent<BoxCollider>().size; // TODO: Add check.

                selection.gameObject.SetActive(status);
                selection.localScale = new Vector3(size.x * 1.1f, size.z * 1.1f, 1.0f); // TODO: Why y and z are replaced?
            }
        }
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

    public bool IsCloseTo(Vector3 position)
    {
        return IsInRange(position, 0.0f, 1.0f);
    }

    public bool IsInRange(Vector3 position, float radiusMax)
    {
        return IsInRange(position, 0.0f, radiusMax);
    }

    public bool IsInRange(Vector3 position, float radiusMin, float radiusMax)
    {
        Vector3 a = position;
        Vector3 b = Position;

        a.y = 0.0f;
        b.y = 0.0f;

        float magnitude = (b - a).magnitude;

        return radiusMin <= magnitude && magnitude <= radiusMax;
    }

    private void Reload()
    {
        ReloadTimer?.Update(Time.deltaTime);
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

    private void RaiseConstructionResourceFlags()
    {
        foreach (Recipe recipe in ConstructionRecipies.Items)
        {
            foreach (RecipeComponent resource in recipe.ToConsume)
            {
                int capacity = ConstructionResources.Capacity(resource.Name);

                if (capacity > 0)
                {
                    Player.Consumers.Add(this, resource.Name, capacity);
                }
                else
                {
                    Player.Consumers.Remove(this, resource.Name);
                }
            }
        }
    }

    private void RaiseResourceFlags()
    {
        foreach (Recipe recipe in Recipes.Items)
        {
            foreach (RecipeComponent resource in recipe.ToConsume)
            {
                int capacity = Resources.Capacity(resource.Name);

                if (capacity > 0)
                {
                    Player.Consumers.Add(this, resource.Name, capacity);
                }
                else
                {
                    Player.Consumers.Remove(this, resource.Name);
                }
            }

            foreach (RecipeComponent resource in recipe.ToProduce)
            {
                int storage = Resources.Storage(resource.Name);

                if (storage > 0)
                {
                    Player.Producers.Add(this, resource.Name, storage);
                }
                else
                {
                    Player.Producers.Remove(this, resource.Name);
                }
            }
        }
    }

    public bool Constructed
    {
        get
        {
            foreach (KeyValuePair<string, Resource> i in ConstructionResources.Items)
            {
                if (i.Value.Current < i.Value.Max)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public Vector3 Entrance { get => new Vector3(transform.position.x, transform.position.y, transform.position.z + Size.z * 0.75f); }

    public Vector3 Exit { get => new Vector3(transform.position.x, transform.position.y, transform.position.z - Size.z * 0.75f); }

    public Vector3 Size { get => GetComponent<Collider>().bounds.size; }

    public bool Alive { get => Health > 0.0f; }

    [field: SerializeField]
    public Player Player;

    public float Damage { get; protected set; } = 0.0f;

    public float ConstructionTime { get; protected set; } = 10.0f;

    public float Health { get; protected set; } = 10.0f;

    public float MaxHealth { get; protected set; } = 10.0f;

    public float LoadTime { get; protected set; } = 10.0f;

    public float ProduceTime { get; protected set; } = 10.0f;

    public float Speed { get; protected set; } = 10.0f;

    public float WaitTime { get; protected set; } = 10.0f;

    public string MissilePrefab { get; protected set; } = string.Empty;

    public float MissileRangeMax { get; protected set; } = 0.0f;

    public float MissileRangeMin { get; protected set; } = 0.0f;

    public Vector3 Position { get => transform.position; set => transform.position = value; }

    public OrderContainer Orders { get; private set; } = new OrderContainer();

    public ResourceContainer Resources { get; private set; } = new ResourceContainer();

    public RecipeContainer Recipes { get; private set; } = new RecipeContainer();

    public Stats Stats { get; private set; } = new Stats();

    public MyGameObjectState State { get; set; } = MyGameObjectState.Operational;

    public ResourceContainer ConstructionResources { get; private set; } = new ResourceContainer();

    public RecipeContainer ConstructionRecipies { get; private set; } = new RecipeContainer();

    public Vector3 RallyPoint { get; set; }
    
    public Timer ReloadTimer { get; protected set; }

    public MyGameObject Parent { get; set; }

    protected Dictionary<OrderType, IOrderHandler> OrderHandlers { get; set; } = new Dictionary<OrderType, IOrderHandler>();
}
