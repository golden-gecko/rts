using System.Linq;
using UnityEngine;

public class OrderHandlerConstruct : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.TargetGameObject == null)
        {
            order.TargetPosition = Map.Instance.StructurePositionHandler.GetPosition(order.TargetPosition);
            order.TargetGameObject = Game.Instance.CreateGameObject(order.Prefab, order.TargetPosition, myGameObject.Player, MyGameObjectState.UnderConstruction);
        }

        Recipe recipe = order.TargetGameObject.ConstructionRecipies.Items.First().Value;

        if (order.Timer == null)
        {
            order.Timer = new Timer(recipe.Sum / myGameObject.GetComponent<Constructor>().ResourceUsage);
        }

        if (Utils.IsCloseTo(myGameObject.Position, order.TargetGameObject.Entrance) == false)
        {
            myGameObject.Move(order.TargetGameObject.Entrance, 0);

            return;
        }

        if (HaveResources(myGameObject, order, recipe) == false)
        {
            myGameObject.Wait(0);

            return;
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        MoveResources(myGameObject, order, recipe);

        order.TargetGameObject.State = MyGameObjectState.Operational;

        myGameObject.Stats.Inc(Stats.OrdersCompleted);
        myGameObject.Stats.Inc(Stats.ObjectsConstructed);
        myGameObject.Stats.Add(Stats.TimeConstructing, order.Timer.Max);
        myGameObject.Orders.Pop();
    }

    private bool HaveResources(MyGameObject myGameObject, Order order, Recipe recipe)
    {
        foreach (Resource i in recipe.ToConsume.Items)
        {
            if (order.TargetGameObject.ConstructionResources.CanRemove(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveResources(MyGameObject myGameObject, Order order, Recipe recipe)
    {
        foreach (Resource i in recipe.ToConsume.Items)
        {
            order.TargetGameObject.ConstructionResources.Remove(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesUsed, i.Max);
        }
    }
}
