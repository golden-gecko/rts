using UnityEngine;

public class Damage : Skill
{
    public override object Clone()
    {
        return new Damage(Name, Range, Cooldown.Max, Value);
    }

    public Damage(string name, float range, float cooldown, float value) : base(name, range, cooldown, "Effects/Skills/Green hit")
    {
        Value = value;
    }

    public override void Execute(MyGameObject myGameObject)
    {
        foreach (MyGameObject target in Object.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (target.Is(myGameObject, DiplomacyState.Ally))
            {
                continue;
            }

            if (Utils.IsInRange(target.Position, myGameObject.Position, Range) == false)
            {
                continue;
            }

            target.OnDamage(Value);
        }

        Object.Instantiate(Resources.Load(Effect), myGameObject.Position, Quaternion.identity);
    }

    public float Value { get; } = 0.0f;
}
