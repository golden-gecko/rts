using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MyGameObject : MonoBehaviour
{
    protected virtual void Awake()
    {
        visual = transform.Find("Visual");
        rangeMissile = visual.transform.Find("Range_Missile");
        rangeVisibility = visual.transform.Find("Range_Visibility");
        selection = visual.transform.Find("Selection");

        Orders.AllowOrder(OrderType.Destroy);
        Orders.AllowOrder(OrderType.Idle);
        Orders.AllowOrder(OrderType.UseSkill);
        Orders.AllowOrder(OrderType.Stop);
        Orders.AllowOrder(OrderType.Wait);

        OrderHandlers[OrderType.Assemble] = new OrderHandlerAssemble();
        OrderHandlers[OrderType.Construct] = new OrderHandlerConstruct();
        OrderHandlers[OrderType.Destroy] = new OrderHandlerDestroy();
        OrderHandlers[OrderType.Explore] = new OrderHandlerExplore();
        OrderHandlers[OrderType.Follow] = new OrderHandlerFollow();
        OrderHandlers[OrderType.Gather] = new OrderHandlerGather();
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
        OrderHandlers[OrderType.UseSkill] = new OrderHandlerUseSkill();
        OrderHandlers[OrderType.Wait] = new OrderHandlerWait();

        ConstructionResources.Add("Iron", 0, 30);

        Recipe r1 = new Recipe("Iron");

        r1.Consumes("Iron", 30);

        ConstructionRecipies.Add(r1);
    }

    protected virtual void Start()
    {
        RallyPoint = Exit;

        UpdatePosition();
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

        UpdateSkills();
        UpdateSelectionPosition();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
    }

    protected virtual void OnTriggerStay(Collider other)
    {
    }

    protected virtual void OnTriggerExit(Collider other)
    {
    }

    protected void UpdateSkills()
    {
        foreach (Skill skill in Skills.Values)
        {
            skill.Update();
        }
    }

    protected void UpdateSelectionPosition()
    {
        Vector3 position = Map.Instance.CameraPositionHandler.GetPosition(Position);

        rangeMissile.position = position;
        rangeVisibility.position = position;
    }

    public void Assemble(string prefab)
    {
        Orders.Add(Order.Assemble(prefab, GetComponent<Constructor>().ResourceUsage));
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
        Orders.Add(Order.Construct(prefab, position, GetComponent<Constructor>().ResourceUsage));
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

    public void Gather(string resource = "")
    {
        Orders.Add(Order.Gather(resource));
    }

    public void Guard(Vector3 position)
    {
        Orders.Add(Order.Guard(position));
    }

    public void Guard(MyGameObject myGameObject)
    {
        Orders.Add(Order.Guard(myGameObject));
    }

    public void Load(MyGameObject target, Dictionary<string, int> resources, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Load(target, resources, LoadTime));
        }
        else
        {
            Orders.Add(Order.Load(target, resources, LoadTime));
        }
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
        Orders.Add(Order.Produce(recipe, GetComponent<Producer>().ResourceUsage));
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
        Orders.Add(Order.Research(technology, GetComponent<Researcher>().ResourceUsage));
    }

    public void Stop()
    {
        Orders.Insert(0, Order.Stop());
    }

    public void Transport(MyGameObject sourceGameObject, MyGameObject targetGameObject, Dictionary<string, int> resources, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Transport(sourceGameObject, targetGameObject, resources, LoadTime));
        }
        else
        {
            Orders.Add(Order.Transport(sourceGameObject, targetGameObject, resources, LoadTime));
        }
    }

    public void Unload(MyGameObject target, Dictionary<string, int> resources, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Unload(target, resources, LoadTime));
        }
        else
        {
            Orders.Add(Order.Unload(target, resources, LoadTime));
        }
    }
    public void UseSkill(string skill)
    {
        Orders.Add(Order.UseSkill(skill));
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

    public virtual string GetInfo(bool ally)
    {
        string info = string.Empty;

        switch (State)
        {
            case MyGameObjectState.Operational:
                info += string.Format("ID: {0}\nName: {1}", GetInstanceID(), name);

                if (HealthMax > 0.0f)
                {
                    info += string.Format("\nHP: {0:0.}/{1:0.}", Health, HealthMax);
                }

                MyComponent[] myComponents = GetComponents<MyComponent>();

                if (myComponents.Length > 0)
                {
                    info += "\nComponents:";

                    foreach (MyComponent myComponent in myComponents)
                    {
                        info += string.Format("\n  {0}", myComponent.GetInfo());
                    }
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

                    if (Skills.Count > 0)
                    {
                        info += "\nSkills:";

                        foreach (Skill skill in Skills.Values)
                        {
                            info += string.Format("\n  {0}", skill.GetInfo());
                        }
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

    public bool Is(MyGameObject myGameObject, DiplomacyState state)
    {
        return Player.Is(myGameObject.Player, state);
    }

    public bool Is(Player player, DiplomacyState state)
    {
        return Player.Is(player, state);
    }

    public float OnDamage(float value)
    {
        float damageToDeal;
        float damageDealt = 0.0f;
        float damageLeft = value;

        Armour armour = GetComponent<Armour>();

        if (armour != null)
        {
            damageToDeal = Mathf.Min(damageLeft, armour.Value);
            damageDealt += damageToDeal;
            damageLeft -= damageToDeal;

            armour.Value = Mathf.Clamp(armour.Value - damageToDeal, 0.0f, armour.ValueMax);
        }

        damageToDeal = Mathf.Min(damageLeft, Health);
        damageDealt += damageToDeal;

        Health = Mathf.Clamp(Health - damageToDeal, 0.0f, HealthMax);

        Stats.Add(Stats.DamageTaken, damageDealt);

        return damageDealt;
    }
    public void OnDestroy_() // TODO: Rename.
    {
        if (DestroyEffect != null && DestroyEffect.Length > 0)
        {
            Instantiate(UnityEngine.Resources.Load(DestroyEffect), Position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void OnRepair(float value)
    {
        Health = Mathf.Clamp(Health + value, 0.0f, HealthMax);

        Stats.Add(Stats.DamageRepaired, value);
    }

    public void Select(bool status)
    {
        if (Selectable == false)
        {
            return;
        }

        Vector3 scale = transform.localScale;
        Vector3 size = GetComponent<BoxCollider>().size;

        Gun gun = GetComponent<Gun>();

        if (gun != null)
        {
            rangeMissile.gameObject.SetActive(status);
            rangeMissile.localScale = new Vector3(gun.Range * 2.0f / scale.x, gun.Range * 2.0f / scale.z, 1.0f);
        }

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

    public bool IsInAttackRange(Vector3 position) // TODO: Remove?
    {
        return IsInRange(position, GetComponent<Gun>().Range);
    }

    public bool IsInVisibilityRange(Vector3 position)
    {
        return IsInRange(position, VisibilityRange);
    }

    public bool IsInRange(Vector3 position, float rangeMax)
    {
        return IsInRange(position, 0.0f, rangeMax);
    }

    public bool IsInRange(Vector3 position, float rangeMin, float rangeMax)
    {
        Vector3 a = position;
        Vector3 b = Position;

        a.y = 0.0f;
        b.y = 0.0f;

        float magnitude = (b - a).magnitude;

        return rangeMin <= magnitude && magnitude <= rangeMax;
    }

    protected virtual void UpdatePosition()
    {
        Vector3 validated;

        if (Map.Instance.ValidatePosition(this, Position, out validated))
        {
            Position = validated;
        }
    }

    public void UpdateSelection()
    {
        if (Player != null)
        {
            selection.GetComponent<SpriteRenderer>().sprite = Player.SelectionSprite;
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
        foreach (Recipe recipe in ConstructionRecipies.Items.Values)
        {
            foreach (Resource resource in recipe.ToConsume.Items.Values)
            {
                int capacity = ConstructionResources.Capacity(resource.Name);

                if (capacity > 0)
                {
                    Player.RegisterConsumer(this, resource.Name, capacity);
                }
                else
                {
                    Player.UnregisterConsumer(this, resource.Name);
                }
            }
        }
    }

    private void RaiseResourceFlags()
    {
        foreach (Recipe recipe in Recipes.Items.Values)
        {
            foreach (Resource resource in recipe.ToConsume.Items.Values)
            {
                int capacity = Resources.Capacity(resource.Name);

                if (capacity > 0)
                {
                    Player.RegisterConsumer(this, resource.Name, capacity);
                }
                else
                {
                    Player.UnregisterConsumer(this, resource.Name);
                }
            }

            foreach (Resource resource in recipe.ToProduce.Items.Values)
            {
                int storage = Resources.Storage(resource.Name);

                if (storage > 0)
                {
                    Player.RegisterProducer(this, resource.Name, storage);
                }
                else
                {
                    Player.UnregisterProducer(this, resource.Name);
                }
            }
        }
    }

    public bool Constructed
    {
        get
        {
            foreach (Resource resource in ConstructionResources.Items.Values)
            {
                if (resource.Full == false)
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

    public void SetPlayer(Player player)
    {
        Player = player;

        UpdateSelection();
    }

    public Vector3 Center
    {
        get
        {
            if (GetComponent<Collider>() != null)
            {
                return GetComponent<Collider>().bounds.center;
            }

            if (GetComponentInChildren<Collider>() != null) // TODO: Check if there are more than one.
            {
                return GetComponentInChildren<Collider>().bounds.center;
            }

            return Vector3.zero;
        }
    }

    public Vector3 Entrance { get => Position + new Vector3(0.0f, 0.0f, Size.z + 1.0f); }

    public Vector3 Exit { get => Position - new Vector3(0.0f, 0.0f, Size.z + 1.0f); }

    public Vector3 Size
    {
        get 
        {
            if (GetComponent<Collider>() != null)
            {
                return GetComponent<Collider>().bounds.size;
            }

            if (GetComponentInChildren<Collider>() != null) // TODO: Check if there are more than one.
            {
                return GetComponentInChildren<Collider>().bounds.size;
            }

            return Vector3.zero;
        } 
    }

    public bool Alive { get => Health > 0.0f; }

    public float Mass
    {
        get
        {
            float mass = 0.0f;

            foreach (MyComponent i in GetComponents<MyComponent>())
            {
                mass += i.Mass;
            }

            return mass;
        }
    }

    [field: SerializeField]
    public Player Player { get; set; }

    [field: SerializeField]
    public bool Gatherable { get; set; } = false;

    [field: SerializeField]
    public bool Selectable { get; set; } = true;

    [field: SerializeField]
    public float Health { get; set; } = 100.0f;

    [field: SerializeField]
    public float HealthMax { get; set; } = 100.0f;

    [field: SerializeField]
    public float VisibilityRange { get; set; } = 10.0f;

    [field: SerializeField]
    public float LoadTime { get; set; } = 2.0f;

    [field: SerializeField]
    public float WaitTime { get; set; } = 2.0f;

    [field: SerializeField]
    public string DestroyEffect { get; set; }

    [field: SerializeField]
    public float Altitude { get; set; } = 0.0f;

    [field: SerializeField]
    public List<MyGameObjectMapLayer> MapLayers { get; set; } = new List<MyGameObjectMapLayer>(); // TODO: Replace with HashSet.

    public Vector3 Position { get => transform.position; set => transform.position = value; }

    public OrderContainer Orders { get; } = new OrderContainer();

    public ResourceContainer Resources { get; } = new ResourceContainer();

    public RecipeContainer Recipes { get; } = new RecipeContainer();

    public Stats Stats { get; } = new Stats();

    public MyGameObjectState State { get; set; } = MyGameObjectState.Operational;

    public ResourceContainer ConstructionResources { get; } = new ResourceContainer();

    public RecipeContainer ConstructionRecipies { get; } = new RecipeContainer();

    public Vector3 RallyPoint { get; set; } = Vector3.zero;

    public MyGameObject Parent { get; set; } // TODO: Hide setter.

    public Dictionary<string, Skill> Skills { get; } = new Dictionary<string, Skill>();

    protected Dictionary<OrderType, IOrderHandler> OrderHandlers { get; } = new Dictionary<OrderType, IOrderHandler>();

    private Transform visual;
    private Transform rangeMissile;
    private Transform rangeVisibility;
    private Transform selection;
}
