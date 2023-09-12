using UnityEngine;

public class PositionAnywhere : ITerrainPosition
{
    public Vector3 GetPosition(Ray ray, int layerMask = Physics.DefaultRaycastLayers)
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Config.TerrainMaxHeight, layerMask) == false)
        {
            return Vector3.zero;
        }

        return hitInfo.point;
    }

    public Vector3 GetPosition(Vector3 position, int layerMask = Physics.DefaultRaycastLayers)
    {
        return GetPosition(new Ray(position + Vector3.up * Config.TerrainMaxHeight, Vector3.down), layerMask);
    }
}
