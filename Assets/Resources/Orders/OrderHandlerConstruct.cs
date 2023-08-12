using System.Linq;
using UnityEngine;

public class OrderHandlerConstruct : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.TargetGameObject == null)
        {
            order.TargetGameObject = Game.Instance.CreateGameObject(order.Prefab, order.TargetPosition, myGameObject.Player, MyGameObjectState.UnderConstruction);
        }

        Recipe recipe = order.TargetGameObject.ConstructionRecipies.Items.First().Value;

        if (order.Timer == null)
        {
            order.Timer = new Timer(recipe.Total / order.ResourceUsage);
        }

        if (myGameObject.IsCloseTo(order.TargetGameObject.Entrance) == false)
        {
            myGameObject.Move(order.TargetGameObject.Entrance, 0);

            return;
        }

        if (HaveResources(myGameObject, recipe) == false)
        {
            myGameObject.Wait(0);

            return;
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        MoveResources(myGameObject, recipe);

        order.TargetGameObject.State = MyGameObjectState.Operational;

        myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
        myGameObject.Stats.Add(Stats.ObjectsConstructed, 1);
        myGameObject.Stats.Add(Stats.TimeConstructing, order.Timer.Max);
        myGameObject.Orders.Pop();
    }

    private bool HaveResources(MyGameObject myGameObject, Recipe recipe)
    {
        foreach (Resource i in recipe.ToConsume.Items.Values)
        {
            if (myGameObject.ConstructionResources.CanRemove(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        foreach (Resource i in recipe.ToProduce.Items.Values)
        {
            if (myGameObject.ConstructionResources.CanAdd(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveResources(MyGameObject myGameObject, Recipe recipe)
    {
        foreach (Resource i in recipe.ToConsume.Items.Values)
        {
            myGameObject.ConstructionResources.Remove(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesUsed, i.Max);
        }

        foreach (Resource i in recipe.ToProduce.Items.Values)
        {
            myGameObject.ConstructionResources.Add(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesProduced, i.Max);
        }
    }
}
