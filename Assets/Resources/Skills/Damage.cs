using UnityEngine;

public class Damage : Skill
{
    public Damage(string name, float cooldown, float range, float value) : base(name, cooldown, range)
    {
        Value = value;
        Effect = "Effects/Skills/Green hit"; // TODO: Get from property.
    }

    public override void Execute(MyGameObject myGameObject)
    {
        foreach (MyGameObject target in Object.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (target.Is(myGameObject, DiplomacyState.Ally))
            {
                continue;
            }

            if (target.IsInRange(myGameObject.Position, Range) == false)
            {
                continue;
            }

            target.OnDamage(Value);
        }

        Object.Instantiate(Resources.Load(Effect), myGameObject.Position, Quaternion.identity);
    }

    public float Value { get; } = 0.0f;
}
