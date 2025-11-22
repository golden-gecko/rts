using UnityEngine;

public class Gun : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Attack);

        Parent.OrderHandlers[OrderType.Attack] = new OrderHandlerAttack();
        Parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleAttacker();
    }

    protected override void Update()
    {
        base.Update();

        if (Parent.Operational == false)
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
    public GameObject MissilePrefab { get; private set; }

    [field: SerializeField]
    public Property Damage { get; private set; } = new Property();

    [field: SerializeField]
    public Property Range { get; private set; } = new Property();

    [field: SerializeField]
    public Timer Reload { get; private set; } = new Timer(10.0f, 10.0f);

    [field: SerializeField]
    public Counter Ammunition { get; } = new Counter(100, 100);
}
