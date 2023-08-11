using System.Collections.Generic;
using UnityEngine;

public class OrderHandlerProduce : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.Recipe.Length > 0)
        {
            if (myGameObject.Recipes.Items.ContainsKey(order.Recipe))
            {
                Produce(myGameObject, order, myGameObject.Recipes.Items[order.Recipe]);
            }
            else
            {
                myGameObject.Stats.Add(Stats.OrdersFailed, 1);
                myGameObject.Orders.Pop();
            }
        }
        else
        {
            foreach (KeyValuePair<string, Recipe> recipe in myGameObject.Recipes.Items)
            {
                if (Produce(myGameObject, order, recipe.Value))
                {
                    break;
                }
            }
        }
    }

    private bool Produce(MyGameObject myGameObject, Order order, Recipe recipe)
    {
        // Have storage to consume.
        bool toConsume = true;

        foreach (RecipeComponent i in recipe.ToConsume)
        {
            if (myGameObject.Resources.CanRemove(i.Name, i.Count) == false)
            {
                toConsume = false;
                break;
            }
        }

        // Have capacity to produce.
        bool toProduce = true;

        foreach (RecipeComponent i in recipe.ToProduce)
        {
            if (myGameObject.Resources.CanAdd(i.Name, i.Count) == false)
            {
                toProduce = false;
                break;
            }
        }

        if (toConsume == false || toProduce == false)
        {
            return false;
        }

        // Produce new resources.
        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished)
        {
            foreach (RecipeComponent i in recipe.ToConsume)
            {
                myGameObject.Resources.Remove(i.Name, i.Count);
            }

            foreach (RecipeComponent i in recipe.ToProduce)
            {
                myGameObject.Resources.Add(i.Name, i.Count);

                myGameObject.Stats.Add(Stats.ResourcesProduced, i.Count);
            }

            order.Timer.Reset();

            myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
            myGameObject.Stats.Add(Stats.TimeProducing, order.Timer.Max);
            myGameObject.Orders.Pop();
        }

        return true;
    }
}
