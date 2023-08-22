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

    public bool ValidatePosition(MyGameObject myGameObject, Vector3 position, out Vector3 validated)
    {
        Ray ray = new Ray(position + Vector3.up * Config.TerrainMaxHeight, Vector3.down);
        int mask = LayerMask.GetMask("Terrain") | LayerMask.GetMask("Water");

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Config.RaycastMaxDistance, mask) == false)
        {
            validated = Vector3.zero;
            
            return false;
        }

        bool air = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Air);
        bool terrain = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Terrain);
        bool water = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Water);

        if (air)
        {
            validated = new Vector3(hitInfo.point.x, hitInfo.point.y + myGameObject.Altitude, hitInfo.point.z);
         
            return true;
        }

        if (terrain && water)
        {
            validated = hitInfo.point;

            return true;
        }

        if (terrain && hitInfo.transform.CompareTag("Terrain")) // TODO: Hardcoded.
        {
            validated = hitInfo.point;

            return true;
        }

        if (water && hitInfo.transform.CompareTag("Water")) // TODO: Hardcoded.
        {
            validated = hitInfo.point;

            return true;
        }

        validated = Vector3.zero;

        return false;
    }

    public ITerrainPosition CameraPositionHandler { get; } = new TerrainPositionAnywhere();

    public ITerrainPosition StructurePositionHandler { get; } = new TerrainPositionCenterGrid();
}
