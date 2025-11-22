using UnityEngine;

public class TerrainPositionCenterGrid : ITerrainPosition
{
    public Vector3 GetPosition(Ray ray)
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Config.RaycastMaxDistance, LayerMask.GetMask("Terrain")) == false)
        {
            return Vector3.zero;
        }

        float scale = Config.TerrainConstructionScale;

        float x = Mathf.Floor(hitInfo.point.x / scale) * scale + scale / 2.0f;
        float z = Mathf.Floor(hitInfo.point.z / scale) * scale + scale / 2.0f;

        return new Vector3(x, hitInfo.point.y, z);
    }


    public Vector3 GetPosition(Vector3 position)
    {
        return GetPosition(new Ray(position + Vector3.up * Config.TerrainMaxHeight, Vector3.down));
    }

}
