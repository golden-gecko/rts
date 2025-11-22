using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        for (int x = 0; x < Config.TerrainVisibilitySize; x++)
        {
            for (int z = 0; z < Config.TerrainVisibilitySize; z++)
            {
                Cells[x, z] = new Cell();
            }
        }

        Clear();
    }

    public bool ValidatePosition(MyGameObject myGameObject, Vector3 position, out Vector3 validated)
    {
        if (myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Missile))
        {
            validated = position;

            return true;
        }

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

        if (terrain && IsTerrain(hitInfo))
        {
            validated = hitInfo.point;

            return true;
        }

        if (water && IsWater(hitInfo))
        {
            validated = hitInfo.point;

            return true;
        }

        validated = Vector3.zero;

        return false;
    }

    public void SetVisibleByRadar(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((position.x - range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z - range) / Config.TerrainVisibilityScale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((position.x + range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z + range) / Config.TerrainVisibilityScale));

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (x < 0 || x > Config.TerrainVisibilitySize - 1)
                {
                    continue;
                }

                if (z < 0 || z > Config.TerrainVisibilitySize - 1)
                {
                    continue;
                }

                if (Cells[x, z].VisibleByRadar.ContainsKey(myGameObject.Player) == false)
                {
                    Cells[x, z].VisibleByRadar[myGameObject.Player] = value;
                }
                else
                {
                    Cells[x, z].VisibleByRadar[myGameObject.Player] += value;
                }
            }
        }
    }

    public void SetVisibleByAntiRadar(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((position.x - range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z - range) / Config.TerrainVisibilityScale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((position.x + range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z + range) / Config.TerrainVisibilityScale));

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (x < 0 || x > Config.TerrainVisibilitySize - 1)
                {
                    continue;
                }

                if (z < 0 || z > Config.TerrainVisibilitySize - 1)
                {
                    continue;
                }

                if (Cells[x, z].VisibleByAntiRadar.ContainsKey(myGameObject.Player) == false)
                {
                    Cells[x, z].VisibleByAntiRadar[myGameObject.Player] = value;
                }
                else
                {
                    Cells[x, z].VisibleByAntiRadar[myGameObject.Player] += value;
                }
            }
        }
    }

    public void SetVisibleBySight(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((position.x - range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z - range) / Config.TerrainVisibilityScale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((position.x + range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z + range) / Config.TerrainVisibilityScale));

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (x < 0 || x > Config.TerrainVisibilitySize - 1)
                {
                    continue;
                }

                if (z < 0 || z > Config.TerrainVisibilitySize - 1)
                {
                    continue;
                }

                if (Cells[x, z].VisibleBySight.ContainsKey(myGameObject.Player) == false)
                {
                    Cells[x, z].VisibleBySight[myGameObject.Player] = value;
                }
                else
                {
                    Cells[x, z].VisibleBySight[myGameObject.Player] += value;
                }
            }
        }
    }

    public void SetVisibleByPower(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((position.x - range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z - range) / Config.TerrainVisibilityScale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((position.x + range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z + range) / Config.TerrainVisibilityScale));

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (x < 0 || x > Config.TerrainVisibilitySize - 1)
                {
                    continue;
                }

                if (z < 0 || z > Config.TerrainVisibilitySize - 1)
                {
                    continue;
                }

                if (Cells[x, z].VisibleByPower.ContainsKey(myGameObject.Player) == false)
                {
                    Cells[x, z].VisibleByPower[myGameObject.Player] = value;
                }
                else
                {
                    Cells[x, z].VisibleByPower[myGameObject.Player] += value;
                }
            }
        }
    }

    public bool IsVisibleByRadar(MyGameObject myGameObject, Player active)
    {
        Vector3Int position = new Vector3Int(Mathf.FloorToInt(myGameObject.Position.x / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt(myGameObject.Position.z / Config.TerrainVisibilityScale));

        int byRadar = Cells[position.x, position.z].VisibleByRadar.ContainsKey(active) ? Cells[position.x, position.z].VisibleByRadar[active] : 0;
        int byAntiRadar = Cells[position.x, position.z].VisibleByAntiRadar.ContainsKey(myGameObject.Player) ? Cells[position.x, position.z].VisibleByAntiRadar[myGameObject.Player] : 0;

        return byRadar > byAntiRadar;
    }

    public bool IsVisibleBySight(MyGameObject myGameObject, Player active)
    {
        Vector3Int position = new Vector3Int(Mathf.FloorToInt(myGameObject.Position.x / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt(myGameObject.Position.z / Config.TerrainVisibilityScale));

        return Cells[position.x, position.z].VisibleBySight.ContainsKey(active) && Cells[position.x, position.z].VisibleBySight[active] > 0;
    }

    public bool IsVisibleByPower(MyGameObject myGameObject, Player active)
    {
        Vector3Int position = new Vector3Int(Mathf.FloorToInt(myGameObject.Position.x / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt(myGameObject.Position.z / Config.TerrainVisibilityScale));

        return Cells[position.x, position.z].VisibleByPower.ContainsKey(active) && Cells[position.x, position.z].VisibleByPower[active] > 0;
    }

    public bool IsTerrain(RaycastHit hit)
    {
        return hit.transform.CompareTag("Terrain");
    }

    public bool IsWater(RaycastHit hit)
    {
        return hit.transform.CompareTag("Water");
    }
    protected void Clear()
    {
        for (int x = 0; x < Config.TerrainVisibilitySize; x++)
        {
            for (int z = 0; z < Config.TerrainVisibilitySize; z++)
            {
                Player[] players1 = Cells[x, z].VisibleByRadar.Keys.ToArray();

                foreach (Player player in players1)
                {
                    Cells[x, z].VisibleByRadar[player] = 0;
                }

                Player[] players2 = Cells[x, z].VisibleByAntiRadar.Keys.ToArray();

                foreach (Player player in players2)
                {
                    Cells[x, z].VisibleByAntiRadar[player] = 0;
                }

                Player[] players3 = Cells[x, z].VisibleBySight.Keys.ToArray();

                foreach (Player player in players3)
                {
                    Cells[x, z].VisibleBySight[player] = 0;
                }

                Player[] players4 = Cells[x, z].VisibleByPower.Keys.ToArray();

                foreach (Player player in players4)
                {
                    Cells[x, z].VisibleByPower[player] = 0;
                }
            }
        }
    }

    public ITerrainPosition CameraPositionHandler { get; } = new TerrainPositionAnywhere();

    public ITerrainPosition StructurePositionHandler { get; } = new TerrainPositionCenterGrid();

    private Cell[,] Cells = new Cell[Config.TerrainVisibilitySize, Config.TerrainVisibilitySize];
}
