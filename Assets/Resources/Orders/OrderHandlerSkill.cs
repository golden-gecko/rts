public class OrderHandlerSkill : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.Skill_.Length > 0;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
            myGameObject.Orders.Pop();

            return;
        }

        if (myGameObject.Skills.ContainsKey(order.Skill_))
        {
            if (myGameObject.Skills[order.Skill_].Cooldown.Finished)
            {
                myGameObject.Skills[order.Skill_].Execute(myGameObject);
                myGameObject.Skills[order.Skill_].Cooldown.Reset();

                myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
                myGameObject.Stats.Add(Stats.SkillsUsed, 1);
                myGameObject.Orders.Pop();
            }
        }
        else
        {
            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
            myGameObject.Orders.Pop();
        }
    }
}
