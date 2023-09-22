using UnityEngine;

public class JobHandlerConstruct : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        float minDistance = float.MaxValue;
        MyGameObject closest = null;

        foreach (MyGameObject underConstruction in Object.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Refactor.
        {
            if (underConstruction.State != MyGameObjectState.UnderConstruction)
            {
                continue;
            }

            if (myGameObject.Is(underConstruction, DiplomacyState.Ally) == false)
            {
                continue;
            }

            float distance = myGameObject.DistanceTo(underConstruction);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = underConstruction;
            }
        }

        if (closest == null)
        {
            return null;
        }

        return Order.Construct(closest);
    }
}
