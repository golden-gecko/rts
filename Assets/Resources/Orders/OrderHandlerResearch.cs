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
            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
            myGameObject.Orders.Pop();

            return;
        }

        if (CanMoveResource(myGameObject, order, myGameObject.Player.TechnologyTree.Technologies[order.Technology]) == false)
        {
            myGameObject.Wait(0);

            return;
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        order.Timer.Reset();

        MoveResource(myGameObject, order, myGameObject.Player.TechnologyTree.Technologies[order.Technology]);

        if (myGameObject.Player.TechnologyTree.Technologies[order.Technology].Researched)
        {
            myGameObject.Player.TechnologyTree.Unlock(order.Technology);
            myGameObject.Orders.Pop();
        }
    }

    private bool CanMoveResource(MyGameObject myGameObject, Order order, Technology technology)
    {
        foreach (Resource resource in technology.Cost.Items.Values)
        {
            int resouceToMove = Mathf.Min(new int[] { order.ResourceUsage, resource.Capacity, myGameObject.Resources.Storage(resource.Name) });

            if (resouceToMove <= 0)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveResource(MyGameObject myGameObject, Order order, Technology technology)
    {
        foreach (Resource resource in technology.Cost.Items.Values)
        {
            int resouceToMove = Mathf.Min(new int[] { order.ResourceUsage, resource.Capacity, myGameObject.Resources.Storage(resource.Name) });

            if (resouceToMove <= 0)
            {
                continue;
            }

            resource.Add(resouceToMove);

            myGameObject.Resources.Remove(resource.Name, resouceToMove);
            myGameObject.Stats.Add(Stats.ResourcesUsed, resouceToMove);

            break;
        }
    }
}
