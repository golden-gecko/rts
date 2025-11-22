using System.Collections.Generic;
using UnityEngine;

public class PassiveIncreaseRange : Skill
{
    public override object Clone()
    {
        return new PassiveIncreaseRange(Name, Range, Cooldown.Max, Value);
    }

    public PassiveIncreaseRange(string name, float range, float cooldown, float value) : base(name, range, cooldown, null, true)
    {
        Value = value;
    }

    public override void Update(MyGameObject myGameObject)
    {
        base.Update(myGameObject);

        foreach (MyGameObject target in targets)
        {
            if (target == null)
            {
                continue;
            }

            if (target.TryGetComponent(out Gun gun))
            {
                gun.Range.Factor.Remove(myGameObject);
            }

            if (target.TryGetComponent(out Radar radar))
            {
                radar.Range.Factor.Remove(myGameObject);
            }

            if (target.TryGetComponent(out PowerPlant powerPlant))
            {
                powerPlant.Range.Factor.Remove(myGameObject);
            }

            if (target.TryGetComponent(out Shield shield))
            {
                shield.Range.Factor.Remove(myGameObject);
            }

            if (target.TryGetComponent(out Sight sight))
            {
                sight.Range.Factor.Remove(myGameObject);
            }
        }

        targets.Clear();

        foreach (RaycastHit hitInfo in Utils.SphereCastAll(myGameObject.Position, Range, Utils.GetGameObjectMask()))
        {
            MyGameObject target = Utils.GetGameObject(hitInfo);

            if (target.Is(myGameObject, DiplomacyState.Ally) == false)
            {
                continue;
            }

            if (target.TryGetComponent(out Gun gun))
            {
                gun.Range.Factor.Add(myGameObject, Value);
            }

            if (target.TryGetComponent(out Radar radar))
            {
                radar.Range.Factor.Add(myGameObject, Value);
            }

            if (target.TryGetComponent(out PowerPlant powerPlant))
            {
                powerPlant.Range.Factor.Add(myGameObject, Value);
            }

            if (target.TryGetComponent(out Shield shield))
            {
                shield.Range.Factor.Add(myGameObject, Value);
            }

            if (target.TryGetComponent(out Sight sight))
            {
                sight.Range.Factor.Add(myGameObject, Value);
            }

            targets.Add(target);
        }
    }

    public override void OnDestroy(MyGameObject myGameObject)
    {
        base.OnDestroy(myGameObject);

        foreach (MyGameObject target in targets)
        {
            if (target == null)
            {
                continue;
            }

            if (target.TryGetComponent(out Gun gun))
            {
                gun.Range.Factor.Remove(myGameObject);
            }

            if (target.TryGetComponent(out Radar radar))
            {
                radar.Range.Factor.Remove(myGameObject);
            }

            if (target.TryGetComponent(out PowerPlant powerPlant))
            {
                powerPlant.Range.Factor.Remove(myGameObject);
            }

            if (target.TryGetComponent(out Shield shield))
            {
                shield.Range.Factor.Remove(myGameObject);
            }

            if (target.TryGetComponent(out Sight sight))
            {
                sight.Range.Factor.Remove(myGameObject);
            }
        }
    }

    public float Value { get; } = 0.0f;

    private HashSet<MyGameObject> targets = new HashSet<MyGameObject>();
}
