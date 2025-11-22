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

            foreach (Gun gun in target.GetComponents<Gun>())
            {
                gun.Range.Factor.Remove(myGameObject);
            }

            foreach (Radar radar in target.GetComponents<Radar>())
            {
                radar.Range.Factor.Remove(myGameObject);
            }

            foreach (Shield shield in target.GetComponents<Shield>())
            {
                shield.Range.Factor.Remove(myGameObject);
            }

            foreach (Sight sight in target.GetComponents<Sight>())
            {
                sight.Range.Factor.Remove(myGameObject);
            }
        }

        targets.Clear();

        foreach (RaycastHit hitInfo in Utils.SphereCastAll(myGameObject.Position, Range, LayerMask.GetMask("GameObject")))
        {
            MyGameObject target = Utils.GetGameObject(hitInfo);

            if (target.Is(myGameObject, DiplomacyState.Ally) == false)
            {
                continue;
            }

            foreach (Gun gun in target.GetComponents<Gun>())
            {
                gun.Range.Factor.Add(myGameObject, Value);
            }

            foreach (Radar radar in target.GetComponents<Radar>())
            {
                radar.Range.Factor.Add(myGameObject, Value);
            }

            foreach (Shield shield in target.GetComponents<Shield>())
            {
                shield.Range.Factor.Add(myGameObject, Value);
            }

            foreach (Sight sight in target.GetComponents<Sight>())
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

            foreach (Gun gun in target.GetComponents<Gun>())
            {
                gun.Range.Factor.Remove(myGameObject);
            }

            foreach (Radar radar in target.GetComponents<Radar>())
            {
                radar.Range.Factor.Remove(myGameObject);
            }

            foreach (Sight sight in target.GetComponents<Sight>())
            {
                sight.Range.Factor.Remove(myGameObject);
            }
        }
    }

    public float Value { get; } = 0.0f;

    private HashSet<MyGameObject> targets = new HashSet<MyGameObject>();
}
