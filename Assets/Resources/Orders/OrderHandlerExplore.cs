using UnityEngine;

public class OrderHandlerExplore : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        float range = myGameObject.GetComponent<Sight>().Range;

        float x = Random.Range(myGameObject.Position.x - range, myGameObject.Position.x + range);
        float z = Random.Range(myGameObject.Position.z - range, myGameObject.Position.z + range);

        myGameObject.Move(new Vector3(x, 0.0f, z)); // TODO: Replace random with move towards unexplored sectors.
        myGameObject.Wait();

        myGameObject.Orders.MoveToEnd();
    }
}
