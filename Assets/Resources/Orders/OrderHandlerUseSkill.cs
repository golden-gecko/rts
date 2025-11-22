public class OrderHandlerUseSkill : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.Skill.Length > 0;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            myGameObject.Stats.Inc(Stats.OrdersFailed);
            myGameObject.Orders.Pop();

            return;
        }

        if (myGameObject.Skills.ContainsKey(order.Skill) == false)
        {
            myGameObject.Stats.Inc(Stats.OrdersFailed);
            myGameObject.Orders.Pop();

            return;
        }

        if (myGameObject.Skills[order.Skill].Cooldown.Finished == false)
        {
            return;
        }

        myGameObject.Skills[order.Skill].Execute(myGameObject);
        myGameObject.Skills[order.Skill].Cooldown.Reset();

        myGameObject.Stats.Inc(Stats.OrdersCompleted);
        myGameObject.Stats.Inc(Stats.SkillsUsed);
        myGameObject.Orders.Pop();
    }
}
