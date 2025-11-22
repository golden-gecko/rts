using UnityEngine;

public class Repair : Skill
{
    public override object Clone()
    {
        return new Repair(Name, Range, Cooldown.Max, Value);
    }

    public Repair(string name, float range, float cooldown, float value) : base(name, range, cooldown, "Effects/Skills/Healing", false)
    {
        Value = value;
    }

    public override void OnExecute(MyGameObject myGameObject)
    {
        base.OnExecute(myGameObject);

        RaycastHit[] hitInfos = Utils.SphereCastAll(myGameObject.Position, Range, Utils.GetGameObjectMask());

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
