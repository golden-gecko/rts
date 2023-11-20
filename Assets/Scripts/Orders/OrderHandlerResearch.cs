using UnityEngine;

public class OrderHandlerResearch : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        Technology technology = myGameObject.Player.TechnologyTree.Technologies[order.Technology];

        if (technology.Discovered)
        {
            Fail(myGameObject);

            return;
        }

        if (order.Timer == null)
        {
            order.Timer = new Timer(Mathf.Ceil((float)technology.MaxSum / (float)myGameObject.GetComponent<Researcher>().ResourceUsage));
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

        myGameObject.Player.TechnologyTree.Discover(order.Technology);
        myGameObject.Stats.Add(Stats.TimeResearching, order.Timer.Max);
        myGameObject.Orders.Pop();
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.Technology != null && order.Technology.Length > 0;
    }

    private bool HaveResources(MyGameObject myGameObject, Technology technology)
    {
        foreach (Resource i in technology.Cost.Items)
        {
            if (myGameObject.GetComponentInChildren<Storage>().Resources.CanRemove(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveResources(MyGameObject myGameObject, Technology technology)
    {
        foreach (Resource i in technology.Cost.Items)
        {
            myGameObject.GetComponentInChildren<Storage>().Resources.Remove(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesUsed, i.Max);
        }
    }
}
