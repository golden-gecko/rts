using UnityEngine;

public class Repair : Skill
{
    public override object Clone()
    {
        return new Repair(Name, Range, Cooldown.Max, Value);
    }

    public Repair(string name, float range, float cooldown, float value) : base(name, range, cooldown, "Effects/Skills/Healing")
    {
        Value = value;
    }

    public override void Execute(MyGameObject myGameObject)
    {
        RaycastHit[] hitInfos = Utils.SphereCastAll(myGameObject.Position, Range, LayerMask.GetMask("GameObject"));

        foreach (RaycastHit hitInfo in hitInfos)
        {
            MyGameObject target = Utils.GetGameObject(hitInfo);

            if (target.Is(myGameObject, DiplomacyState.Ally) == false)
            {
                continue;
            }

            if (Utils.IsInRange(target.Position, myGameObject.Position, Range) == false)
            {
                continue;
            }

            target.OnRepair(Value);
        }

        Object.Instantiate(Resources.Load(Effect), myGameObject.Position, Quaternion.identity);
    }

    public float Value { get; } = 0.0f;
}
