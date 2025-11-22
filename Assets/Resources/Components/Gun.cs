using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Gun : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Attack);

        GetComponent<MyGameObject>().OrderHandlers[OrderType.Attack] = new OrderHandlerAttack();
        GetComponent<MyGameObject>().OrderHandlers[OrderType.Idle] = new OrderHandlerIdleAttacker();
    }

    protected override void Update()
    {
        base.Update();

        MyGameObject parent = GetComponent<MyGameObject>();

        if (parent.Working == false)
        {
            return;
        }

        Reload.Update(Time.deltaTime);
    }

    public override string GetInfo()
    {
        Storage storage = GetComponent<Storage>();

        return string.Format("{0}, Reload: {1} Ammunition: {2}/{3}", base.GetInfo(), Reload.GetInfo(), storage.Resources.Current("Ammunition"), storage.Resources.Max("Ammunition"));
    }

    public bool IsInRange(Vector3 position)
    {
        return Utils.IsInRange(GetComponent<MyGameObject>().Position, position, Range);
    }

    public virtual bool CanFire()
    {
        return GetComponent<Storage>().Resources.CanDec("Ammunition") && Reload.Finished;
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
}
