using System.Collections.Generic;
using UnityEngine;

public class PassiveIncreaseDamage : Skill
{
    public override object Clone()
    {
        return new PassiveIncreaseDamage(Name, Range, Cooldown.Max, Value);
    }

    public PassiveIncreaseDamage(string name, float range, float cooldown, float value) : base(name, range, cooldown, "Effects/Skills/Green hit")
    {
        Value = value;
    }

    public override void Update(MyGameObject myGameObject)
    {
        base.Update(myGameObject);

        foreach (MyGameObject target in targets)
        {
            foreach (Gun gun in target.GetComponents<Gun>())
            {
                gun.Damage.Factor.Remove(myGameObject);
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
                gun.Damage.Factor.Add(myGameObject, Value);

            }

            targets.Add(target);
        }
    }

    public override void OnDestroy(MyGameObject myGameObject)
    {
        base.OnDestroy(myGameObject);

        foreach (MyGameObject target in targets)
        {
            foreach (Gun gun in target.GetComponents<Gun>())
            {
                gun.Damage.Factor.Remove(myGameObject);
            }
        }
    }

    public float Value { get; } = 0.0f;

    private HashSet<MyGameObject> targets = new HashSet<MyGameObject>();
}
