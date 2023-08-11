using UnityEngine;

public class OrderHandlerExplore : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        float x = Random.Range(myGameObject.Position.x - myGameObject.VisibilityRange, myGameObject.Position.x + myGameObject.VisibilityRange);
        float z = Random.Range(myGameObject.Position.z - myGameObject.VisibilityRange, myGameObject.Position.z + myGameObject.VisibilityRange);

        myGameObject.Move(new Vector3(x, 0.0f, z)); // TODO: Replace random with move towards unexplored sectors.
        myGameObject.Wait();

        myGameObject.Orders.MoveToEnd();
    }
}
