using System.Collections.Generic;
using UnityEngine;

public class MyGameObject : MonoBehaviour
{
    protected virtual void Awake()
    {
        Transform visual = transform.Find("Visual");

        rangeMissile = visual.transform.Find("Range_Missile");
        rangeVisibility = visual.transform.Find("Range_Visibility");
        selection = visual.transform.Find("Selection");

        Orders.AllowOrder(OrderType.Destroy);
        Orders.AllowOrder(OrderType.Idle);
        Orders.AllowOrder(OrderType.Stop);
        Orders.AllowOrder(OrderType.Wait);

        OrderHandlers[OrderType.Assemble] = new OrderHandlerAssemble();
        OrderHandlers[OrderType.Construct] = new OrderHandlerConstruct();
        OrderHandlers[OrderType.Destroy] = new OrderHandlerDestroy();
        OrderHandlers[OrderType.Explore] = new OrderHandlerExplore();
        OrderHandlers[OrderType.Follow] = new OrderHandlerFollow();
        OrderHandlers[OrderType.Guard] = new OrderHandlerGuard();
        OrderHandlers[OrderType.Load] = new OrderHandlerLoad();
        OrderHandlers[OrderType.Move] = new OrderHandlerMove();
        OrderHandlers[OrderType.Patrol] = new OrderHandlerPatrol();
        OrderHandlers[OrderType.Produce] = new OrderHandlerProduce();
        OrderHandlers[OrderType.Rally] = new OrderHandlerRally();
        OrderHandlers[OrderType.Research] = new OrderHandlerResearch();
        OrderHandlers[OrderType.Stop] = new OrderHandlerStop();
        OrderHandlers[OrderType.Transport] = new OrderHandlerTransport();
        OrderHandlers[OrderType.Unload] = new OrderHandlerUnload();
        OrderHandlers[OrderType.Wait] = new OrderHandlerWait();

        ConstructionResources.Add("Metal", 0, 30);

        Recipe r1 = new Recipe("Metal");

        r1.Consume("Metal", 0);

        ConstructionRecipies.Add(r1);
    }

    protected virtual void Start()
    {
        RallyPoint = Exit;

        UpdateSelection();
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
    
    public void Explore()
    {
        Orders.Add(Order.Explore());
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

    public void Produce(string recipe = "")
    {
        Orders.Add(Order.Produce(recipe, ProduceTime));
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

    public void Research(string technology)
    {
        Orders.Add(Order.Research(technology, ResearchTime));
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

    public string GetInfo(bool ally)
    {
        string info = string.Empty;

        switch (State)
        {
            case MyGameObjectState.Operational:
                info += string.Format("ID: {0}\nName: {1}", GetInstanceID(), name);

                if (MaxHealth > 0.0f)
                {
                    info += string.Format("\nHP: {0:0.}/{1:0.}", Health, MaxHealth);
                }

                if (Speed > 0.0f)
                {
                    info += string.Format("\nSpeed: {0:0.}", Speed);
                }

                if (ReloadTimer != null)
                {
                    info += string.Format("\nReload: {0:0.}/{1:0.}", ReloadTimer.Current, ReloadTimer.Max);
                }

                string resources = Resources.GetInfo();

                if (resources.Length > 0)
                {
                    info += string.Format("\nResources:{0}", resources);
                }

                if (ally)
                {
                    string orders = Orders.GetInfo();
                    string stats = Stats.GetInfo();

                    if (orders.Length > 0)
                    {
                        info += string.Format("\nOrders: {0}", orders);
                    }

                    if (stats.Length > 0)
                    {
                        info += string.Format("\nStats: {0}", stats);
                    }
                }
                break;

            case MyGameObjectState.UnderAssembly:
            case MyGameObjectState.UnderConstruction:
                info += string.Format("ID: {0}\nName: {1}\nResources:{2}", GetInstanceID(), name, ConstructionResources.GetInfo());
                break;
        }

        return info;
    }

    public bool IsAlly(MyGameObject myGameObject)
    {
        return Player.IsAlly(myGameObject.Player);
    }

    public bool IsEnemy(MyGameObject myGameObject)
    {
        return Player.IsEnemy(myGameObject.Player);
    }

    public bool IsAlly(Player player)
    {
        return Player.IsAlly(player);
    }

    public bool IsEnemy(Player player)
    {
        return Player.IsEnemy(player);
    }

    public void OnDamage(float damage)
    {
        Health -= damage;

        Stats.Add(Stats.DamageTaken, damage);
    }

    public void Select(bool status)
    {
        Vector3 scale = transform.localScale;
        Vector3 size = GetComponent<BoxCollider>().size;

        rangeMissile.gameObject.SetActive(status);
        rangeMissile.localScale = new Vector3(MissileRange * 2.0f / scale.x, MissileRange * 2.0f / scale.z, 1.0f);

        rangeVisibility.gameObject.SetActive(status);
        rangeVisibility.localScale = new Vector3(VisibilityRange * 2.0f / scale.x, VisibilityRange * 2.0f / scale.z, 1.0f);

        selection.gameObject.SetActive(status);
        selection.localScale = new Vector3(size.x * 1.1f, size.z * 1.1f, 1.0f);
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

    public bool IsInAttackRange(Vector3 position)
    {
        return IsInRange(position, MissileRange);
    }

    public bool IsInVisibilityRange(Vector3 position)
    {
        return IsInRange(position, VisibilityRange);
    }

    private bool IsInRange(Vector3 position, float radiusMax)
    {
        return IsInRange(position, 0.0f, radiusMax);
    }

    private bool IsInRange(Vector3 position, float radiusMin, float radiusMax)
    {
        Vector3 a = position;
        Vector3 b = Position;

        a.y = 0.0f;
        b.y = 0.0f;

        float magnitude = (b - a).magnitude;

        return radiusMin <= magnitude && magnitude <= radiusMax;
    }

    public void UpdateSelection()
    {
        if (Player) // TODO: Fix.
        {
            selection.GetComponent<SpriteRenderer>().sprite = Player.SelectionSprite;
        }
    }

    private void Reload()
    {
        if (ReloadTimer != null)
        {
            ReloadTimer.Update(Time.deltaTime);
        }
    }

    protected virtual void AlignPositionToTerrain()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position + new Vector3(0, 1000, 0), Vector3.down);

        if (Physics.Raycast(ray, out hitInfo, 2000, LayerMask.GetMask("Terrain")))
        {
            if (hitInfo.transform.CompareTag("Terrain"))
            {
                Position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
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
        foreach (KeyValuePair<string, Recipe> recipe in ConstructionRecipies.Items)
        {
            foreach (RecipeComponent resource in recipe.Value.ToConsume)
            {
                int capacity = ConstructionResources.Capacity(resource.Name);

                if (capacity > 0)
                {
                    Game.Instance.RegisterConsumer(this, resource.Name, capacity);
                }
                else
                {
                    Game.Instance.UnregisterConsumer(this, resource.Name);
                }
            }
        }
    }

    private void RaiseResourceFlags()
    {
        foreach (KeyValuePair<string, Recipe> recipe in Recipes.Items)
        {
            foreach (RecipeComponent resource in recipe.Value.ToConsume)
            {
                int capacity = Resources.Capacity(resource.Name);

                if (capacity > 0)
                {
                    Game.Instance.RegisterConsumer(this, resource.Name, capacity);
                }
                else
                {
                    Game.Instance.UnregisterConsumer(this, resource.Name);
                }
            }

            foreach (RecipeComponent resource in recipe.Value.ToProduce)
            {
                int storage = Resources.Storage(resource.Name);

                if (storage > 0)
                {
                    Game.Instance.RegisterProducer(this, resource.Name, storage);
                }
                else
                {
                    Game.Instance.UnregisterProducer(this, resource.Name);
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

    public float DistanceTo(MyGameObject myGameObject)
    {
        return (Position - myGameObject.Position).magnitude;
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

    public float GatherTime { get; protected set; } = 2.0f;

    public float LoadTime { get; protected set; } = 2.0f;

    public float ProduceTime { get; protected set; } = 2.0f;

    public float ResearchTime { get; protected set; } = 2.0f;

    public float Speed { get; protected set; } = 10.0f;

    public float WaitTime { get; protected set; } = 2.0f;

    public string MissilePrefab { get; protected set; } = string.Empty;

    public float MissileRange { get; protected set; } = 0.0f;

    public float VisibilityRange { get; protected set; } = 10.0f;

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

    private Transform rangeMissile;
    private Transform rangeVisibility;
    private Transform selection;
}
