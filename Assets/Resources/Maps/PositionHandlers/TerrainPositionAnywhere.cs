using UnityEngine;

public class TerrainPositionAnywhere : ITerrainPosition
{
    public Vector3 GetPosition(Ray ray)
    {
        RaycastHit hitInfo;
        int mask = LayerMask.GetMask("Terrain") | LayerMask.GetMask("Water");

        if (Physics.Raycast(ray, out hitInfo, Config.RaycastMaxDistance, mask) == false)
        {
            return Vector3.zero;
        }

        return hitInfo.point;
    }

    public Vector3 GetPosition(Vector3 position)
    {
        return GetPosition(new Ray(position + Vector3.up * Config.TerrainMaxHeight, Vector3.down));
    }
}
