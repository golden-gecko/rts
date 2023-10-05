public class OrderHandlerUseSkill : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (myGameObject.Skills.ContainsKey(order.Skill) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (myGameObject.Skills[order.Skill].Cooldown.Finished == false)
        {
            return;
        }

        myGameObject.Skills[order.Skill].OnExecute(myGameObject);
        myGameObject.Stats.Inc(Stats.SkillsUsed);

        Success(myGameObject);
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.Skill != null && order.Skill.Length > 0;
    }
}
