using UnityEngine;

public class Gun : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        MyGameObject parent = GetComponent<MyGameObject>();

        parent.Orders.AllowOrder(OrderType.Attack);

        parent.OrderHandlers[OrderType.Attack] = new OrderHandlerAttack();
        parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleAttacker();
    }

    protected override void Update()
    {
        base.Update();

        MyGameObject parent = GetComponent<MyGameObject>();

        if (parent.Working == false)
        {
            return;
        }

        // Reload gun.
        Reload.Update(Time.deltaTime);

        // Grab ammunition.
        // Storage storage = GetComponent<Storage>();
        // Ammunition.Add(storage.Resources.Remove("Ammunition", Ammunition.Max - Ammunition.Current));
    }

    public override string GetInfo()
    {
        // Storage storage = GetComponent<Storage>();
        // return string.Format("{0}, Reload: {1} Ammunition: {2}/{3}", base.GetInfo(), Reload.GetInfo(), storage.Resources.Current("Ammunition"), storage.Resources.Max("Ammunition"));
        return string.Format("Gun: {0}, Reload: {1} Ammunition: {2}", base.GetInfo(), Reload.GetInfo(), Ammunition.GetInfo());
    }

    public bool IsInRange(Vector3 position)
    {
        return Utils.IsInRange(GetComponent<MyGameObject>().Position, position, Range);
    }

    public virtual bool CanFire()
    {
        // return GetComponent<Storage>().Resources.CanDec("Ammunition") && Reload.Finished;
        return Ammunition.CanDec() && Reload.Finished;
    }

    public virtual void Fire(MyGameObject myGameObject, Vector3 position)
    {
    }

    [field: SerializeField]
    public GameObject MissilePrefab { get; set; }

    [field: SerializeField]
    public float Damage { get; set; } = 10.0f;

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;

    [field: SerializeField]
    public Timer Reload { get; set; } = new Timer(10.0f, 10.0f);

    [field: SerializeField]
    public Counter Ammunition { get; } = new Counter(100, 100);
}
