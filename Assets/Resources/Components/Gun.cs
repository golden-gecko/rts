using UnityEngine;

public class Gun : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        parent.Orders.AllowOrder(OrderType.Attack);

        parent.OrderHandlers[OrderType.Attack] = new OrderHandlerAttack();
        parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleAttacker();
    }

    protected override void Update()
    {
        base.Update();

        if (parent.Working == false)
        {
            return;
        }

        Reload.Update(Time.deltaTime);
    }

    public override string GetInfo()
    {
        return string.Format("Gun: {0}, Damage: {1:0.}, Range: {2:0.}, Reload: {3} Ammunition: {4}", base.GetInfo(), Damage.Total, Range.Total, Reload.GetInfo(), Ammunition.GetInfo());
    }

    public bool IsInRange(Vector3 position)
    {
        return Utils.IsInRange(GetComponent<MyGameObject>().Position, position, Range.Total);
    }

    public virtual bool CanFire()
    {
        return Ammunition.CanDec() && Reload.Finished;
    }

    public virtual void Fire(MyGameObject myGameObject, Vector3 position)
    {
    }

    [field: SerializeField]
    public GameObject MissilePrefab { get; set; }

    [field: SerializeField]
    public Property Damage { get; private set; } = new Property();

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    [field: SerializeField]
    public Timer Reload { get; set; } = new Timer(10.0f, 10.0f);

    [field: SerializeField]
    public Counter Ammunition { get; } = new Counter(100, 100);
}
