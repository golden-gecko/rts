using System.Collections.Generic;
using UnityEngine;

public class Disaster : MyGameObject
{
    protected override void Update()
    {
        base.Update();

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
                    i.Key.OnDamageHandler(DamageType, DamagePerSecond);
                }
            }
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        MyGameObject myGameObject = Utils.GetGameObject(collision);

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

            myGameObject.OnDamageHandler(DamageType, Damage);
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        MyGameObject myGameObject = Utils.GetGameObject(collision);

        if (myGameObject == null)
        {
            return;
        }

        Damaged.Remove(myGameObject);
    }

    public override string GetInfo(bool ally)
    {
        return base.GetInfo(ally) + string.Format("\nDamage: {0:0.}\nDamage Per Second: {1:0.}", Damage, DamagePerSecond);
    }

    [field: SerializeField]
    public float FrequencyInSecondsMin { get; private set; } = 120.0f;

    [field: SerializeField]
    public float FrequencyInSecondsMax { get; private set; } = 180.0f;

    [field: SerializeField]
    public float Damage { get; private set; } = 10.0f;

    [field: SerializeField]
    public float DamagePerSecond { get; private set; } = 10.0f;

    [field: SerializeField]
    public List<DamageTypeItem> DamageType { get; private set; } = new List<DamageTypeItem>();

    private List<MyGameObject> Damaged { get; } = new List<MyGameObject>();

    private Dictionary<MyGameObject, Timer> DamageTimer { get; } = new Dictionary<MyGameObject, Timer>();
}
