using System.Collections.Generic;
using UnityEngine;

public class Gun : Part
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.AttackObject);
        Parent.Orders.AllowOrder(OrderType.AttackPosition);

        Parent.OrderHandlers[OrderType.AttackObject] = new OrderHandlerAttackObject();
        Parent.OrderHandlers[OrderType.AttackPosition] = new OrderHandlerAttackPosition();
        Parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleAttacker();
    }

    protected override void Update()
    {
        base.Update();

        if (Alive == false)
        {
            return;
        }

        Reload.Update(Time.deltaTime);
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Damage: {1:0.}, Range: {2:0.}, Reload: {3} Ammunition: {4}", base.GetInfo(), Damage.Total, Range.Total, Reload.GetInfo(), Ammunition.GetInfo());
    }

    public bool IsInRange(Vector3 position)
    {
        return Utils.IsInRange(GetComponentInParent<MyGameObject>().Position, position, Range.Total);
    }

    public virtual bool CanFire()
    {
        return Alive && Ammunition.CanDec() && Reload.Finished;
    }

    public virtual void Fire(MyGameObject myGameObject, Vector3 position)
    {
    }

    [field: SerializeField]
    public GameObject MissilePrefab { get; private set; }

    [field: SerializeField]
    public Property Damage { get; private set; } = new Property(10.0f);

    [field: SerializeField]
    public Property Range { get; private set; } = new Property(10.0f);

    [field: SerializeField]
    public Timer Reload { get; private set; } = new Timer(10.0f, 10.0f);

    [field: SerializeField]
    public Counter Ammunition { get; private set; } = new Counter(100, 100);

    [field: SerializeField]
    public List<DamageTypeItem> DamageType { get; private set; } = new List<DamageTypeItem>();

    [SerializeField]
    protected AudioClip AudioFire; // TODO: Refactor.
}
