using UnityEngine;

public class OrderHandlerExplore : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        float range = myGameObject.GetComponent<Sight>().Range.Total;

        float x = Random.Range(myGameObject.Position.x - range, myGameObject.Position.x + range);
        float z = Random.Range(myGameObject.Position.z - range, myGameObject.Position.z + range);

        myGameObject.Move(new Vector3(x, 0.0f, z), 0); // TODO: Replace random with move towards unexplored sectors.
    }
}
