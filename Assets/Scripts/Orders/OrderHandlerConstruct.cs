using UnityEngine;

public class OrderHandlerConstruct : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.TargetGameObject == null)
        {
            Fail(myGameObject);

            return;
        }

        if (order.TargetGameObject.State != MyGameObjectState.UnderConstruction)
        {
            Fail(myGameObject);

            return;
        }

        ResourceContainer resourceContainer = order.TargetGameObject.ConstructionResources;

        if (order.Timer == null)
        {
            order.Timer = new Timer(Mathf.Ceil((float)resourceContainer.MaxSum / (float)myGameObject.GetComponentInChildren<Constructor>().ResourceUsage));
        }

        if (HaveResources(order, resourceContainer) == false)
        {
            foreach (Resource resource in order.TargetGameObject.ConstructionResources.Items)
            {
                myGameObject.Stock(order.TargetGameObject, resource.Name, resource.Available, 0);
            }

            return;
        }

        if (Utils.IsCloseTo(myGameObject.Position, order.TargetGameObject.Entrance) == false)
        {
            myGameObject.Move(order.TargetGameObject.Entrance, 0);

            return;
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        MoveResources(myGameObject, order, resourceContainer);

        order.TargetGameObject.SetState(MyGameObjectState.Operational);

        myGameObject.Stats.Inc(Stats.ObjectsConstructed);
        myGameObject.Stats.Add(Stats.TimeConstructing, order.Timer.Max);

        Success(myGameObject);
    }

    private bool HaveResources(Order order, ResourceContainer resourceContainer)
    {
        foreach (Resource i in resourceContainer.Items)
        {
            if (order.TargetGameObject.ConstructionResources.CanRemove(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveResources(MyGameObject myGameObject, Order order, ResourceContainer resourceContainer)
    {
        foreach (Resource i in resourceContainer.Items)
        {
            order.TargetGameObject.ConstructionResources.Remove(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesUsed, i.Max);
        }
    }
}
