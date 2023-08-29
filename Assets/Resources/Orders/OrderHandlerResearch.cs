using UnityEngine;

public class OrderHandlerResearch : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.Technology.Length > 0;
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

        Technology technology = myGameObject.Player.TechnologyTree.Technologies[order.Technology];

        if (order.Timer == null)
        {
            order.Timer = new Timer(technology.Total / order.ResourceUsage);
        }

        if (HaveResources(myGameObject, technology) == false)
        {
            myGameObject.Wait(0);

            return;
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        MoveResources(myGameObject, technology);

        myGameObject.Player.TechnologyTree.Unlock(order.Technology);
        myGameObject.Stats.Add(Stats.TimeResearching, order.Timer.Max);
        myGameObject.Orders.Pop();
    }

    private bool HaveResources(MyGameObject myGameObject, Technology technology)
    {
        foreach (Resource i in technology.Cost.Items.Values)
        {
            if (myGameObject.GetComponent<Storage>().Resources.CanRemove(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveResources(MyGameObject myGameObject, Technology technology)
    {
        foreach (Resource i in technology.Cost.Items.Values)
        {
            myGameObject.GetComponent<Storage>().Resources.Remove(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesUsed, i.Max);
        }
    }
}
