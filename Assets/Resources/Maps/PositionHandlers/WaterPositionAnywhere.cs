using UnityEngine;

public class WaterPositionAnywhere : ITerrainPosition
{
    public Vector3 GetPosition(Ray ray)
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Config.WaterMaxHeight, LayerMask.GetMask("Water")) == false)
        {
            return Vector3.zero;
        }

        return hitInfo.point;
    }

    public Vector3 GetPosition(Vector3 position)
    {
        return GetPosition(new Ray(position + Vector3.up * Config.WaterMaxHeight, Vector3.down));
    }
}
