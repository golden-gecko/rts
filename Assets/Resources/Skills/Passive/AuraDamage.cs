using UnityEngine;

public class AuraDamage : Skill
{
    public override object Clone()
    {
        return new AuraDamage(Name, Range, Cooldown.Max, Value);
    }

    public AuraDamage(string name, float range, float cooldown, float value) : base(name, range, cooldown, "Effects/Skills/Green hit")
    {
        Value = value;
    }

    public override void Update(MyGameObject myGameObject)
    {
        base.Update(myGameObject);

        if (Cooldown.Finished == false)
        {
            return;
        }

        foreach (MyGameObject target in Targets)
        {
            foreach (Gun gun in target.GetComponents<Gun>())
            {
                gun.DamageFactor = 1.0f;
            }
        }

        Targets.Clear();

        foreach (RaycastHit hitInfo in Utils.SphereCastAll(myGameObject.Position, Range, LayerMask.GetMask("GameObject")))
        {
            MyGameObject target = Utils.GetGameObject(hitInfo);

            if (target.Is(myGameObject, DiplomacyState.Ally) == false)
            {
                continue;
            }

            foreach (Gun gun in target.GetComponents<Gun>())
            {
                gun.DamageFactor = 1.0f;
            }

            Targets.Add(target);
        }
    }

    public override void Execute(MyGameObject myGameObject)
    {
    }

    public float Value { get; } = 0.0f;
}
