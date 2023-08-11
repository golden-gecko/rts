using UnityEngine;

public class Damage : Skill
{
    public Damage(string name, float cooldown, float range, float value) : base(name, cooldown, range)
    {
        Value = value;
        Effect = "Effects/CFXR Flash";
    }

    public override void Execute(MyGameObject myGameObject)
    {
        foreach (MyGameObject target in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (target.IsAlly(myGameObject))
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
