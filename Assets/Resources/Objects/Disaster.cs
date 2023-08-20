using System.Collections.Generic;
using UnityEngine;

public class Disaster : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Move);

        LiveTimer.Max = TimeToLive;
    }

    protected override void Update()
    {
        base.Update();

        if (LiveTimer.Update(Time.deltaTime))
        {
            Destroy(0);
        }

        if (DamagePerSecond > 0.0f)
        {
            foreach (KeyValuePair<MyGameObject, Timer> i in DamageTimer)
            {
                if (i.Key == null)
                {
                    continue;
                }

                if (i.Value.Update(Time.deltaTime))
                {
                    i.Value.Reset();

                    i.Key.OnDamage(DamagePerSecond);
                }
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        MyGameObject myGameObject = other.GetComponentInParent<MyGameObject>(); // TODO: Add collision with terrain.

        if (myGameObject == null)
        {
            return;
        }

        if (DamagePerSecond > 0.0f && DamageTimer.ContainsKey(myGameObject) == false)
        {
            DamageTimer[myGameObject] = new Timer(1.0f);
        }

        if (Damage > 0.0f && Damaged.Contains(myGameObject) == false)
        {
            Damaged.Add(myGameObject);

            myGameObject.OnDamage(Damage);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerStay(other);

        MyGameObject myGameObject = other.GetComponentInParent<MyGameObject>(); // TODO: Add collision with terrain.

        if (myGameObject == null)
        {
            return;
        }

        Damaged.Remove(myGameObject);
    }

    public override string GetInfo(bool ally)
    {
        return base.GetInfo(ally) + string.Format("\nDamage: {0:0.}\nDamage Per Second: {1:0.}\nTime: {2:0.}/{3:0.}", Damage, DamagePerSecond, LiveTimer.Current, LiveTimer.Max);
    }

    [field: SerializeField]
    public float FrequencyInSecondsMin = 120.0f;

    [field: SerializeField]
    public float FrequencyInSecondsMax = 180.0f;

    [field: SerializeField]
    public float Damage { get; set; } = 20.0f;

    [field: SerializeField]
    public float DamagePerSecond { get; set; } = 10.0f;

    [field: SerializeField]
    public float TimeToLive = 10.0f; // TODO: Move to upper class.

    private List<MyGameObject> Damaged = new List<MyGameObject>();

    private Dictionary<MyGameObject, Timer> DamageTimer = new Dictionary<MyGameObject, Timer>();

    private Timer LiveTimer = new Timer();
}
