using UnityEngine;

public class Gun : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Attack);

        Reload.Max = CooldownTime;
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
        return string.Format("{0}, Reload: {1} Ammunition: {2}/{3}", base.GetInfo(), Reload.GetInfo(), Ammunition, AmmunitionMax);
    }

    public virtual bool CanFire()
    {
        return Ammunition > 0; // TODO: Add cooldown timer here.
    }

    public virtual void Fire(MyGameObject myGameObject, Vector3 position)
    {
    }

    [field: SerializeField]
    public float Damage { get; set; } = 10.0f;

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;

    [field: SerializeField]
    public float CooldownTime { get; set; } = 10.0f;

    [field: SerializeField]
    public GameObject MissilePrefab { get; set; }

    [field: SerializeField]
    public int Ammunition { get; set; } = 100;
    
    [field: SerializeField]
    public int AmmunitionMax { get; set; } = 100;

    public Timer Reload { get; } = new Timer();
}
