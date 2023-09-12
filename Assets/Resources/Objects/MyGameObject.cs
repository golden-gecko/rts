using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyGameObject : MonoBehaviour
{
    protected virtual void Awake()
    {
        body = transform.Find("Body");

        Orders.AllowOrder(OrderType.Destroy); // TODO: Move to component.
        Orders.AllowOrder(OrderType.Disable);
        Orders.AllowOrder(OrderType.Enable);
        Orders.AllowOrder(OrderType.Idle);
        Orders.AllowOrder(OrderType.UseSkill);
        Orders.AllowOrder(OrderType.Stop);
        Orders.AllowOrder(OrderType.Wait);

        OrderHandlers[OrderType.Destroy] = new OrderHandlerDestroy(); // TODO: Move to component.
        OrderHandlers[OrderType.Disable] = new OrderHandlerDisable();
        OrderHandlers[OrderType.Enable] = new OrderHandlerEnable();
        OrderHandlers[OrderType.Stop] = new OrderHandlerStop();
        OrderHandlers[OrderType.UseSkill] = new OrderHandlerUseSkill();
        OrderHandlers[OrderType.Wait] = new OrderHandlerWait();

        ConstructionResources.Init("Iron", 0, 30, ResourceDirection.In);

        Recipe r1 = new Recipe("Iron"); // TODO: Remove.
        r1.Consumes("Iron", 30);
        ConstructionRecipies.Add(r1);

        Stats.Player = Player;
    }

    protected virtual void Start()
    {
        UpdatePosition();
        UpdateSelection();
    }

    protected virtual void Update()
    {
        ExpirationTimer.Update(Time.deltaTime);

        switch (State)
        {
            case MyGameObjectState.Operational:
                if (Alive == false)
                {
                    OnDestroy_();
                }

                if (Working)
                {
                    ProcessOrders();
                    UpdateSkills();
                }
                else
                {
                    ProcessOrdersWhenInactive();
                }
                break;

            case MyGameObjectState.UnderConstruction:
                if (Alive == false)
                {
                    OnDestroy_();
                }

                RaiseConstructionResourceFlags();
                break;
        }

        UpdateVisibility();
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

    protected void UpdateVisibility()
    {
        Player active = HUD.Instance.ActivePlayer;

        if (Player == active || Map.Instance.IsVisibleBySight(this, active))
        {
            foreach (Renderer renderer in body.GetComponentsInChildren<Renderer>(true))
            {
                renderer.enabled = true;
            }

            GetComponentInChildren<Indicators>().OnShow();
        }
        else if (Map.Instance.IsVisibleByRadar(this, active))
        {
            foreach (Renderer renderer in body.GetComponentsInChildren<Renderer>(true))
            {
                renderer.enabled = false;
            }

            GetComponentInChildren<Indicators>().OnRadar();
        }
        else
        {
            foreach (Renderer renderer in body.GetComponentsInChildren<Renderer>(true))
            {
                renderer.enabled = false;
            }

            GetComponentInChildren<Indicators>().OnHide();
        }
    }

    public void Assemble(string prefab)
    {
        Orders.Add(Order.Assemble(prefab));
    }

    public void Attack(Vector3 position)
    {
        Orders.Add(Order.Attack(position));
    }

    public void Attack(MyGameObject myGameObject)
    {
        Orders.Add(Order.Attack(myGameObject));
    }

    public void Construct(MyGameObject myGameObject)
    {
        Orders.Add(Order.Construct(myGameObject));
    }

    public void Construct(string prefab, Vector3 position, Quaternion rotation)
    {
        Orders.Add(Order.Construct(prefab, position, rotation));
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

    public void Disable(int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Disable());
        }
        else
        {
            Orders.Add(Order.Disable());
        }
    }

    public void Enable(int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Enable());
        }
        else
        {
            Orders.Add(Order.Enable());
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

    public void Gather(MyGameObject myGameObject)
    {
        Orders.Add(Order.Gather(myGameObject));
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

    public void Load(MyGameObject myGameObject, string resource, int value, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Load(myGameObject, resource, value));
        }
        else
        {
            Orders.Add(Order.Load(myGameObject, resource, value));
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
        Orders.Add(Order.Produce(recipe));
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
        Orders.Add(Order.Research(technology));
    }

    public void Stop()
    {
        Orders.Insert(0, Order.Stop());
    }

    public void Transport(MyGameObject sourceGameObject, MyGameObject targetGameObject, string resource, int value, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Transport(sourceGameObject, targetGameObject, resource, value));
        }
        else
        {
            Orders.Add(Order.Transport(sourceGameObject, targetGameObject, resource, value));
        }
    }

    public void Unload(MyGameObject myGameObject, string resource, int value, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Unload(myGameObject, resource, value));
        }
        else
        {
            Orders.Add(Order.Unload(myGameObject, resource, value));
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
            Orders.Insert(priority, Order.Wait());
        }
        else
        {
            Orders.Add(Order.Wait());
        }
    }

    public virtual string GetInfo(bool ally)
    {
        string info = string.Empty;

        switch (State)
        {
            case MyGameObjectState.Operational:
                info += string.Format("ID: {0}\nName: {1}", GetInstanceID(), name);

                if (Health.Max > 0.0f)
                {
                    info += string.Format("\nHP: {0}", Health.GetInfo());
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

                    // TODO: Remove or uncomment.
                    // info += string.Format("\nEnabled: {0}", Enabled);
                    // info += string.Format("\nGatherable: {0}", Gatherable);
                    // info += string.Format("\nPowerable: {0}", Powerable);
                    // info += string.Format("\nSelectable: {0}", Selectable);
                    info += string.Format("\nPowered: {0}", Powered);
                    info += string.Format("\nWorking: {0}", Working);
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

    public float OnDamage(float damage)
    {
        float damageLeft = damage;

        foreach (Shield shield in GetComponents<Shield>())
        {
            damageLeft -= shield.Absorb(damageLeft);
        }

        foreach (Armour armour in GetComponents<Armour>())
        {
            damageLeft -= armour.Absorb(damageLeft);
        }

        damageLeft -= Health.Remove(damageLeft);

        Stats.Add(Stats.DamageTaken, damage - damageLeft);

        return damage - damageLeft;
    }

    public virtual void OnDestroy_() // TODO: Name collision with GameObject.OnDestroy.
    {
        if (DestroyEffect != null)
        {
            Instantiate(DestroyEffect, Position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void OnRepair(float value)
    {
        Stats.Add(Stats.DamageRepaired, Health.Add(value));
    }

    public void Select(bool status)
    {
        if (Selectable == false)
        {
            return;
        }

        GetComponentInChildren<Indicators>().OnSelect(status);
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
            GetComponentInChildren<Indicators>().OnPlayerChange(Player);
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

    private void ProcessOrdersWhenInactive() // TODO: Refactor.
    {
        if (Orders.Count > 0)
        {
            Order order = Orders.First();

            if (order.Type == OrderType.Enable && Orders.IsAllowed(order.Type) && OrderHandlers.ContainsKey(order.Type))
            {
                OrderHandlers[order.Type].OnExecute(this);
            }
            else
            {
                Orders.Pop();
            }
        }
    }

    public void RaiseConstructionResourceFlags()
    {
        foreach (Resource resource in ConstructionResources.Items)
        {
            if (resource.In)
            {
                if (resource.Full)
                {
                    Player.UnregisterConsumer(this, resource.Name);
                }
                else
                {
                    Player.RegisterConsumer(this, resource.Name, resource.Capacity, resource.Direction);
                }
            }

            if (resource.Out)
            {
                if (resource.Empty)
                {
                    Player.UnregisterProducer(this, resource.Name);
                }
                else
                {
                    Player.RegisterProducer(this, resource.Name, resource.Storage, resource.Direction);
                }
            }
        }
    }

    public void RemoveConstructionResourceFlags()
    {
        foreach (string name in ConstructionResources.Items.Select(x => x.Name))
        {
            Player.UnregisterConsumer(this, name);
            Player.UnregisterProducer(this, name);

        }
    }

    public float DistanceTo(MyGameObject myGameObject)
    {
        return (Position - myGameObject.Position).magnitude;
    }

    public void SetPlayer(Player player)
    {
        Player = player;
        Stats.Player = player;

        UpdateSelection();
    }

    public bool HasCorrectPosition()
    {
        Ray ray = new Ray(Position + Vector3.up * Config.TerrainMaxHeight, Vector3.down);
        int layerMask = LayerMask.GetMask("Terrain") | LayerMask.GetMask("Water");

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Config.RaycastMaxDistance, layerMask) == false)
        {
            return false;
        }

        return Map.Instance.IsTerrain(hitInfo) == MapLayers.Contains(MyGameObjectMapLayer.Terrain)
            || Map.Instance.IsWater(hitInfo) == MapLayers.Contains(MyGameObjectMapLayer.Water);
    }

    public Vector3 Center
    {
        get
        {
            Vector3 center = Vector3.zero;

            Collider[] colliders = GetComponentsInChildren<Collider>();

            foreach (Collider collider in colliders)
            {
                center += collider.bounds.center;
            }

            if (colliders.Length > 1)
            {
                center /= colliders.Length;
            }

            return center;
        }
    }

    public Vector3 Direction { get => transform.forward; }

    public Vector3 Entrance { get => Position + new Vector3(Direction.x * Size.x, Direction.y * Size.y, Direction.z * Size.z); }

    public Vector3 Exit { get => Position - new Vector3(Direction.x * Size.x, Direction.y * Size.y, Direction.z * Size.z); }

    public float Radius
    {
        get
        {
            Vector3 size = Size;

            return (size.x + size.y + size.z) / 3.0f;
        }
    }

    public Vector3 Size
    {
        get 
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                min.x = Mathf.Min(min.x, collider.bounds.min.x);
                min.y = Mathf.Min(min.y, collider.bounds.min.y);
                min.z = Mathf.Min(min.z, collider.bounds.min.z);

                max.x = Mathf.Max(max.x, collider.bounds.max.x);
                max.y = Mathf.Max(max.y, collider.bounds.max.y);
                max.z = Mathf.Max(max.z, collider.bounds.max.z);
            }

            return max - min;
        }
    }

    public bool Alive { get => Health.Current > 0.0f && (ExpirationTimer.Active == false || ExpirationTimer.Finished == false); }

    public float Mass
    {
        get
        {
            return GetComponents<MyComponent>().Sum(x => x.Mass);
        }
    }

    public bool Powered { get => Map.Instance.IsVisibleByPower(this, HUD.Instance.ActivePlayer); }

    public bool Working { get => Enabled && (Powerable == false || Powered); }

    [field: SerializeField]
    public Player Player { get; set; }

    [field: SerializeField]
    public bool Enabled { get; set; } = true;

    [field: SerializeField]
    public bool Powerable { get; set; } = false;

    [field: SerializeField]
    public bool Gatherable { get; set; } = false;

    [field: SerializeField]
    public bool Selectable { get; set; } = true;

    [field: SerializeField]
    public Progress Health { get; set; } = new Progress(100.0f, 100.0f);

    [field: SerializeField]
    public float EnableTime { get; set; } = 2.0f;

    [field: SerializeField]
    public float WaitTime { get; set; } = 2.0f;

    [field: SerializeField]
    public GameObject DestroyEffect { get; set; }

    [field: SerializeField]
    public float Altitude { get; set; } = 0.0f;

    [field: SerializeField]
    public List<MyGameObjectMapLayer> MapLayers { get; set; } = new List<MyGameObjectMapLayer>();

    [field: SerializeField]
    public bool ShowIndicators = true;

    [field: SerializeField]
    public Timer ExpirationTimer { get; set; } = new Timer(-1.0f, -1.0f);

    [field: SerializeField]
    public List<string> SkillsNames { get; set; } = new List<string>(); // TODO: Implement.

    public Vector3 Position { get => transform.position; set => transform.position = value; }

    public Quaternion Rotation { get => transform.rotation; }

    public Vector3 Scale { get => transform.localScale; }

    public OrderContainer Orders { get; } = new OrderContainer();

    public Stats Stats { get; } = new Stats();

    public MyGameObjectState State { get; set; } = MyGameObjectState.Operational;

    public ResourceContainer ConstructionResources { get; } = new ResourceContainer();

    public RecipeContainer ConstructionRecipies { get; } = new RecipeContainer(); // TODO: Remove.

    public MyGameObject Parent { get; set; } // TODO: Hide setter.

    public Dictionary<string, Skill> Skills { get; } = new Dictionary<string, Skill>();

    public Dictionary<OrderType, OrderHandler> OrderHandlers { get; } = new Dictionary<OrderType, OrderHandler>();

    private Transform body;
}
