public class OrderHandlerSkill : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.Prefab.Length > 0;
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

        if (myGameObject.Skills.ContainsKey(order.Prefab))
        {
            if (myGameObject.Skills[order.Prefab].Cooldown.Finished)
            {
                myGameObject.Skills[order.Prefab].Execute(myGameObject);
                myGameObject.Skills[order.Prefab].Cooldown.Reset();

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
