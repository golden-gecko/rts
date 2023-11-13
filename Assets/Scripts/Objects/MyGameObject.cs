using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class MyGameObject : MonoBehaviour
{
    protected virtual void Awake()
    {
        Body = transform.Find("Body");

        SetupIndicators();
        SetupBase();

        Orders.AllowOrder(OrderType.Destroy); // TODO: Move to component.
        Orders.AllowOrder(OrderType.Disable);
        Orders.AllowOrder(OrderType.Enable);
        Orders.AllowOrder(OrderType.Idle);
        Orders.AllowOrder(OrderType.GuardObject);
        Orders.AllowOrder(OrderType.GuardPosition);
        Orders.AllowOrder(OrderType.Stop);
        Orders.AllowOrder(OrderType.UseSkill);
        Orders.AllowOrder(OrderType.Wait);

        OrderHandlers[OrderType.Destroy] = new OrderHandlerDestroy(); // TODO: Move to component.
        OrderHandlers[OrderType.Disable] = new OrderHandlerDisable();
        OrderHandlers[OrderType.Enable] = new OrderHandlerEnable();
        OrderHandlers[OrderType.GuardObject] = new OrderHandlerGuardObject();
        OrderHandlers[OrderType.GuardPosition] = new OrderHandlerGuardPosition();
        OrderHandlers[OrderType.Stop] = new OrderHandlerStop();
        OrderHandlers[OrderType.UseSkill] = new OrderHandlerUseSkill();
        OrderHandlers[OrderType.Wait] = new OrderHandlerWait();

        ConstructionResources.Init("Iron", 0, 30, ResourceDirection.In);

        Stats.Player = Player;
    }

    protected virtual void Start()
    {
        if (State == MyGameObjectState.Cursor || State == MyGameObjectState.Preview)
        {
            return;
        }

        CreateSkills();

        InitializePosition();

        UpdateSelection();
        UpdateVisibility();
    }

    protected virtual void Update()
    {
        if (State == MyGameObjectState.Cursor || State == MyGameObjectState.Preview)
        {
            return;
        }

        ExpirationTimer.Update(Time.deltaTime);

        switch (State)
        {
            case MyGameObjectState.Operational:
                if (Alive == false)
                {
                    Destroy(0);
                }

                if (Working)
                {
                    ProcessOrdersWhenEnabled();
                }
                else
                {
                    ProcessOrdersWhenDisabled();
                }
                break;

            case MyGameObjectState.UnderConstruction:
                if (Alive == false)
                {
                    Destroy(0);
                }

                RaiseConstructionResourceFlags();
                break;
        }

        UpdatePosition();
        UpdateSkills();
        UpdateVisibility();

        Experience.Set(Stats.Get(Stats.DamageDealt) * 0.1f + Stats.Get(Stats.TargetsDestroyed));
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
    }

    protected virtual void OnTriggerStay(Collider collider)
    {
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
    }

    #region Orders
    public void Assemble(string prefab)
    {
        Orders.Add(Order.Assemble(prefab));
    }

    public void AttackObject(MyGameObject myGameObject)
    {
        Orders.Add(Order.AttackObject(myGameObject));
    }

    public void AttackPosition(Vector3 position)
    {
        Orders.Add(Order.AttackPosition(position));
    }

    public void Construct(MyGameObject myGameObject)
    {
        Orders.Add(Order.Construct(myGameObject));
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
        Orders.Add(Order.Explore(Position));
    }

    public void Follow(MyGameObject myGameObject, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Follow(myGameObject));
        }
        else
        {
            Orders.Add(Order.Follow(myGameObject));
        }
    }

    public void GatherObject(MyGameObject myGameObject)
    {
        Orders.Add(Order.GatherObject(myGameObject));
    }

    public void GatherResource(string resource = "")
    {
        Orders.Add(Order.GatherResource(resource));
    }

    public void GuardObject(MyGameObject myGameObject)
    {
        Orders.Add(Order.GuardObject(myGameObject));
    }

    public void GuardPosition(Vector3 position)
    {
        Orders.Add(Order.GuardPosition(position));
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
            List<Order> path = PathFinder.Instance.GetPath(Position, position);

            for (int i = 0; i < path.Count; i++)
            {
                Orders.Insert(priority + i, path[i]);
            }

            // Orders.Insert(priority, Order.Move(position));
        }
        else
        {
            foreach (Order order in PathFinder.Instance.GetPath(Position, position))
            {
                Orders.Add(order);
            }

            // Orders.Add(Order.Move(position));
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

    public void Stock(MyGameObject myGameObject, string resource, int value, int priority = -1)
    {
        if (0 <= priority && priority < Orders.Count)
        {
            Orders.Insert(priority, Order.Stock(myGameObject, resource, value));
        }
        else
        {
            Orders.Add(Order.Stock(myGameObject, resource, value));
        }
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

    public void Turn(Vector3 target)
    {
        Orders.Add(Order.Turn(target));
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
    #endregion

    public void AssignWorker(MyGameObject myGameObject)
    {
        Workers.Add(myGameObject);
        myGameObject.Workplaces.Add(this);
    }

    public void UnassignWorker(MyGameObject myGameObject)
    {
        Workers.Remove(myGameObject);
        myGameObject.Workplaces.Remove(this);
    }

    public virtual string GetInfo(bool ally)
    {
        string info = string.Empty;

        switch (State)
        {
            case MyGameObjectState.Operational:
                info += string.Format("{0}\n\nHP: {1}", name, Health.GetInfo());

                Part[] myComponents = GetComponents<Part>();

                if (myComponents.Length > 0)
                {
                    info += "\n\nParts:";

                    foreach (Part myComponent in myComponents)
                    {
                        info += string.Format("\n  {0}", myComponent.GetInfo());
                    }
                }

                if (ally)
                {
                    string stats = Stats.GetInfo();

                    if (stats.Length > 0)
                    {
                        info += string.Format("\n\nStats: {0}", stats);
                    }

                    if (Skills.Count > 0)
                    {
                        info += "\nSkills:";

                        foreach (Skill skill in Skills.Values)
                        {
                            info += string.Format("\n  {0}", skill.GetInfo());
                        }
                    }

                    info += string.Format("\nExperience: {0}", Experience.GetInfo());
                    info += string.Format("\nPowered: {0}", Powered);
                    info += string.Format("\nWorking: {0}", Working);
                }
                break;

            case MyGameObjectState.UnderAssembly:
            case MyGameObjectState.UnderConstruction:
                info += string.Format("{0}\n\nResources: {1}", name, ConstructionResources.GetInfo());
                break;
        }

        return info;
    }

    public virtual string GetInfoOrders(bool ally)
    {
        if (ally)
        {
            switch (State)
            {
                case MyGameObjectState.Operational:
                    return Orders.GetInfo();
            }
        }

        return string.Empty;
    }

    public bool Is(MyGameObject myGameObject, DiplomacyState state)
    {
        return Player.Is(myGameObject.Player, state);
    }

    public bool Is(Player player, DiplomacyState state)
    {
        return Player.Is(player, state);
    }

    public float OnDamageHandler(List<DamageTypeItem> damageType, float damage)
    {
        float damageLeft = damage;

        if (TryGetComponent(out Shield shield))
        {
            foreach (DamageTypeItem damageTypeItem in damageType)
            {
                damageLeft -= shield.Absorb(damageTypeItem.Type, damageTypeItem.Ratio * damageLeft);
            }
        }

        if (TryGetComponent(out Armour armour))
        {
            foreach (DamageTypeItem damageTypeItem in damageType)
            {
                damageLeft -= armour.Absorb(damageTypeItem.Type, damageTypeItem.Ratio * damageLeft);
            }
        }

        damageLeft -= Health.Remove(damageLeft);

        Stats.Add(Stats.DamageTaken, damage - damageLeft);

        return damage - damageLeft;
    }

    public void OnDestroyHandler()
    {
        if (DestroyEffect != null)
        {
            Instantiate(DestroyEffect, Position, Quaternion.identity);
        }

        foreach (Part myComponent in GetComponents<Part>())
        {
            myComponent.OnDestroyHandler();
        }

        foreach (Skill skill in Skills.Values)
        {
            skill.OnDestroyHandler(this);
        }

        Destroy(gameObject);
    }

    public void OnMoveHandler(Vector3 position)
    {
        foreach (Skill skill in Skills.Values)
        {
            skill.OnMoveHandler(this, position);
        }

        Position = position;
    }

    public float OnRepairHandler(float health)
    {
        float healthLeft = health;

        healthLeft -= Health.Add(healthLeft);

        if (TryGetComponent(out Armour armour))
        {
            healthLeft -= armour.Repair(healthLeft);
        }

        if (TryGetComponent(out Shield shield))
        {
            healthLeft -= shield.Repair(healthLeft);
        }

        Stats.Add(Stats.DamageRepaired, health - healthLeft);

        return health - healthLeft;
    }

    public void Select(bool status)
    {
        if (Selectable == false)
        {
            return;
        }

        Indicators.OnSelect(status);
    }

    private void InitializePosition()
    {
        Map.Instance.SetOccupied(this, Position, 1);

        PreviousPosition = Position;
    }

    private void UpdatePosition()
    {
        if (Map.Instance.ValidatePosition(this, Position, out Vector3 validated))
        {
            if (SnapToGrid)
            {
                Position = Utils.SnapToCenter(validated, Config.Map.Scale);
            }
            else
            {
                Position = validated;
            }
        }

        if (Utils.ToGrid(PreviousPosition, Config.Map.Scale) != Utils.ToGrid(Position, Config.Map.Scale))
        {
            Map.Instance.SetOccupied(this, PreviousPosition, -1);
            Map.Instance.SetOccupied(this, Position, 1);

            PreviousPosition = Position;
        }
    }

    private void UpdateSelection()
    {
        if (Player != null && Indicators)
        {
            Indicators.OnPlayerChange(Player);
        }
    }

    private void ProcessOrdersWhenEnabled()
    {
        if (Orders.Count > 0)
        {
            Order order = Orders.First();

            if (Orders.IsAllowed(order.Type) && OrderHandlers.ContainsKey(order.Type))
            {
                OrderHandlers[order.Type].OnExecuteHandler(this);
            }
            else
            {
                Orders.Pop();
            }
        }
        else if (OrderHandlers.ContainsKey(OrderType.Idle))
        {
            OrderHandlers[OrderType.Idle].OnExecuteHandler(this);
        }
    }

    private void ProcessOrdersWhenDisabled()
    {
        if (Orders.Count > 0)
        {
            Order order = Orders.First();

            if (order.Type == OrderType.Enable && Orders.IsAllowed(order.Type) && OrderHandlers.ContainsKey(order.Type))
            {
                OrderHandlers[order.Type].OnExecuteHandler(this);
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
                    Player.RegisterConsumer(this, resource.Name, resource.Available, resource.Direction);
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
                    Player.RegisterProducer(this, resource.Name, resource.Current, resource.Direction);
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

    public void SetParent(MyGameObject myGameObject)
    {
        Parent = myGameObject;
    }

    public void SetState(MyGameObjectState state)
    {
        State = state;

        switch (state)
        {
            case MyGameObjectState.Cursor:
                ShowIndicators = false;
                ShowEntrance = false;
                ShowExit = false;

                EnableColliders(false);

                if (Indicators)
                {
                    Indicators.OnConstruction();
                }
                break;

            case MyGameObjectState.Preview:
                ShowIndicators = false;
                ShowEntrance = false;
                ShowExit = false;

                EnableColliders(false);
                break;

            case MyGameObjectState.Operational:
                RemoveConstructionResourceFlags();

                if (Indicators)
                {
                    Indicators.OnConstructionEnd();
                }
                break;

            case MyGameObjectState.UnderAssembly:
                if (Indicators)
                {
                    Indicators.OnConstruction();
                }
                break;

            case MyGameObjectState.UnderConstruction:
                RaiseConstructionResourceFlags();

                if (Indicators)
                {
                    Indicators.OnConstruction();
                }
                break;
        }
    }

    public bool HasCorrectPosition()
    {
        MyGameObjectMapLayer mapLayer;

        if (Map.Instance.GetPosition(this, Position, out _, out mapLayer) == false)
        {
            return false;
        }

        return (mapLayer == MyGameObjectMapLayer.Terrain && MapLayers.Contains(MyGameObjectMapLayer.Terrain))
            || (mapLayer == MyGameObjectMapLayer.Underwater && MapLayers.Contains(MyGameObjectMapLayer.Underwater))
            || (mapLayer == MyGameObjectMapLayer.Water && MapLayers.Contains(MyGameObjectMapLayer.Water));
    }

    public void ClearOrders()
    {
        Stats.Add(Stats.OrdersCancelled, Orders.Count);
        Orders.Clear();
    }

    private void SetupBase()
    {
        if (CreateBase == false)
        {
            return;
        }

        Vector3 size = Size;

        int x = Utils.MakeOdd(Mathf.CeilToInt(size.x / Config.Map.Scale));
        int z = Utils.MakeOdd(Mathf.CeilToInt(size.z / Config.Map.Scale));

        size.x = x * Config.Map.Scale;
        size.z = z * Config.Map.Scale;

        Body.transform.localPosition = new Vector3(0.0f, 0.25f, 0.0f);

        Indicators.transform.localPosition = new Vector3(0.0f, 0.25f, 0.0f);

        Base = Instantiate(Game.Instance.Config.Base, transform, false);
        Base.transform.localScale = new Vector3(size.x, 0.5f, size.z);
    }

    private void SetupIndicators()
    {
        Indicators = Instantiate(Game.Instance.Config.Indicators, transform, false).GetComponent<Indicators>();
    }

    private void CreateSkills()
    {
        foreach (string skillName in SkillsNames)
        {
            Skills[skillName] = Game.Instance.SkillManager.Get(skillName).Clone() as Skill;
            Skills[skillName].Start(this);
        }
    }

    private void UpdateSkills()
    {
        foreach (Skill skill in Skills.Values)
        {
            skill.Update(this);
        }
    }

    private void UpdateVisibility()
    {
        Player active = HUD.Instance.ActivePlayer;

        if (Player == active || Map.Instance.IsVisibleBySight(this.Position, active))
        {
            EnableRenderers(true);

            Indicators.OnShow();

            VisibilityState = MyGameObjectVisibilityState.Visible;
        }
        else if (Map.Instance.IsExplored(this, active))
        {
            EnableRenderers(true);

            Indicators.OnExploration();

            VisibilityState = MyGameObjectVisibilityState.Explored;
        }
        else if (Map.Instance.IsVisibleByRadar(this, active))
        {
            EnableRenderers(false);

            Indicators.OnRadar();

            VisibilityState = MyGameObjectVisibilityState.Radar;
        }
        else
        {
            EnableRenderers(false);

            Indicators.OnHide();

            VisibilityState = MyGameObjectVisibilityState.Hidden;
        }
    }

    private void EnableColliders(bool enabled)
    {
        foreach (Collider collider in Body.GetComponents<Collider>())
        {
            collider.enabled = enabled;
        }
    }

    private void EnableRenderers(bool enabled)
    {
        foreach (Renderer renderer in Body.GetComponentsInChildren<Renderer>(true))
        {
            renderer.enabled = enabled;
        }

        if (Base != null)
        {
            foreach (Renderer renderer in Base.GetComponentsInChildren<Renderer>(true))
            {
                renderer.enabled = enabled;
            }
        }
    }

    public Vector3 Center
    {
        get
        {
            return new Vector3(Position.x, Position.y + Size.y / 2.0f, Position.z);
        }
    }

    public Vector3 Direction { get => transform.forward; }

    public Vector3 Entrance { get => new Vector3(Position.x + Direction.x * Size.x, Position.y, Position.z + Direction.z * Size.z); }

    public Vector3 Exit { get => new Vector3(Position.x - Direction.x * Size.x, Position.y, Position.z - Direction.z * Size.z); }

    public float Radius { get => (Size.x + Size.y + Size.z) / 3.0f; }

    public Vector3 Size
    {
        get
        {
            Collider[] colliders = Body.GetComponentsInChildren<Collider>();

            if (colliders.Length <= 0)
            {
                return Vector3.zero;
            }

            Vector3 size = Vector3.zero;

            Quaternion rotation = Utils.ResetRotation(this);

            foreach (Collider collider in colliders)
            {
                size += collider.bounds.size;
            }

            Utils.RestoreRotation(this, rotation);

            return size / colliders.Length;
        }
    }

    public bool Alive { get => Health.Current > 0.0f && (ExpirationTimer.Active == false || ExpirationTimer.Finished == false); }

    public float Mass { get => GetComponents<Part>().Sum(x => x.Mass); }

    public bool Constructed { get => ConstructionResources.CurrentSum >= ConstructionResources.MaxSum; }

    public bool Powered
    {
        get
        {
            if (TryGetComponent(out PowerPlant powerPlant))
            {
                return powerPlant.PowerUpTime.Finished; // powerPlant.IsProducer || Map.Instance.IsVisibleByPower(this);
            }

            return false;
        }
    }

    public bool Working { get => Enabled && State == MyGameObjectState.Operational && (TryGetComponent(out PowerPlant _) == false || Powered); }

    [field: SerializeField]
    public Progress Experience { get; private set; } = new Progress(0.0f, 1000.0f);

    [field: SerializeField]
    public Player Player { get; private set; }

    [field: SerializeField]
    public bool Enabled { get; set; } = true;

    [field: SerializeField]
    public bool Gatherable { get; private set; } = false;

    [field: SerializeField]
    public bool Selectable { get; private set; } = true;

    [field: SerializeField]
    public Progress Health { get; private set; } = new Progress(100.0f, 100.0f); // TODO: Move to chassis.

    [field: SerializeField]
    public float EnableTime { get; private set; } = 2.0f;

    [field: SerializeField]
    public float WaitTime { get; private set; } = 2.0f;

    [field: SerializeField]
    public GameObject DestroyEffect { get; private set; }

    [field: SerializeField]
    public float Altitude { get; private set; } = -1.0f;

    [field: SerializeField]
    public float Depth { get; private set; } = -1.0f;

    [field: SerializeField]
    public List<MyGameObjectMapLayer> MapLayers { get; private set; } = new List<MyGameObjectMapLayer>();

    [field: SerializeField]
    public bool ShowIndicators { get; private set; } = true;

    [field: SerializeField]
    public Timer ExpirationTimer { get; private set; } = new Timer(-1.0f, -1.0f);

    [field: SerializeField]
    public List<string> SkillsNames { get; private set; } = new List<string>();

    [field: SerializeField]
    public MyGameObjectState State { get; private set; } = MyGameObjectState.Operational;

    [field: SerializeField]
    private bool CreateBase { get; set; } = false;

    [field: SerializeField]
    private bool SnapToGrid { get; set; } = false;

    [field: SerializeField]
    public MyGameObjectVisibilityState VisibilityState { get; private set; } = MyGameObjectVisibilityState.Visible;

    [field: SerializeField]
    public bool RotateTowardsTarget { get; private set; } = true;

    [field: SerializeField]
    public bool ShowEntrance { get; private set; } = false;

    [field: SerializeField]
    public bool ShowExit { get; private set; } = false;

    public Vector3 Position { get => transform.position; set => transform.position = value; }

    public Quaternion Rotation { get => transform.rotation; set => transform.rotation = value; }

    public Vector3 Scale { get => transform.localScale; }

    public OrderContainer Orders { get; } = new OrderContainer();

    public Stats Stats { get; } = new Stats();

    public ResourceContainer ConstructionResources { get; } = new ResourceContainer();

    public MyGameObject Parent { get; private set; }

    public Dictionary<string, Skill> Skills { get; } = new Dictionary<string, Skill>();

    public Dictionary<OrderType, OrderHandler> OrderHandlers { get; } = new Dictionary<OrderType, OrderHandler>();

    public Transform Body { get; private set; }

    public GameObject Base { get; private set; }

    public Indicators Indicators { get; private set; }

    private Vector3 PreviousPosition { get; set; }

    public HashSet<MyGameObject> Workers { get; } = new HashSet<MyGameObject>(); // TODO: Implement workers mechanic.

    public HashSet<MyGameObject> Workplaces { get; } = new HashSet<MyGameObject>(); // TODO: Implement workers mechanic.
}
