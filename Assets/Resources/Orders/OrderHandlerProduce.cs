using UnityEngine;

public class OrderHandlerProduce : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        foreach (Recipe recipe in myGameObject.Recipes)
        {
            // Have all resources to consume.
            bool toConsume = true;

            foreach (RecipeComponent i in recipe.ToConsume)
            {
                if (myGameObject.Resources.CanRemove(i.Name, i.Count) == false)
                {
                    toConsume = false;
                    break;
                }
            }

            // Have all resources to produce.
            bool toProduce = true;

            foreach (RecipeComponent i in recipe.ToProduce)
            {
                if (myGameObject.Resources.CanAdd(i.Name, i.Count) == false)
                {
                    toProduce = false;
                    break;
                }
            }

            // Produce new resources.
            if (toConsume && toProduce)
            {
                order.Timer.Update(Time.deltaTime);

                if (order.Timer.Finished)
                {
                    if (toConsume && toProduce)
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
                    }

                    order.Timer.Reset();

                    myGameObject.Orders.MoveToEnd();

                    myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
                    myGameObject.Stats.Add(Stats.TimeProducing, order.Timer.Max);
                }
            }
        }
    }
}
