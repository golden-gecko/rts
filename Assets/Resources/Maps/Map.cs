using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Instance { get; private set; }

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Vector3 ValidatePosition(Vector3 position)
    {
        Ray ray = new Ray(position + Vector3.up * Config.TerrainMaxHeight, Vector3.down);
        int mask = LayerMask.GetMask("Terrain") | LayerMask.GetMask("Water");

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Config.RaycastMaxDistance, mask) == false)
        {
            return Vector3.zero;
        }

        return Vector3.zero;
    }

    public Vector3 ValidatePosition(MyGameObject myGameObject)
    {
        Ray ray = new Ray(myGameObject.Position + Vector3.up * Config.TerrainMaxHeight, Vector3.down);
        int mask = LayerMask.GetMask("Terrain") | LayerMask.GetMask("Water");

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Config.RaycastMaxDistance, mask) == false)
        {
            return Vector3.zero;
        }

        bool air = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Air);
        bool terrain = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Terrain);
        bool water = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Water);

        if (air)
        {
            return new Vector3(hitInfo.point.x, hitInfo.point.y + myGameObject.Altitude, hitInfo.point.z);
        }

        if (terrain && water)
        {
            return hitInfo.point;
        }

        if (terrain && hitInfo.transform.name == "Terrain") // TODO: Hardcoded.
        {
            return hitInfo.point;
        }

        if (water && hitInfo.transform.name == "Water") // TODO: Hardcoded.
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }

    public ITerrainPosition CameraPositionHandler { get; } = new TerrainPositionAnywhere();

    public ITerrainPosition StructurePositionHandler { get; } = new TerrainPositionCenterGrid();
}
