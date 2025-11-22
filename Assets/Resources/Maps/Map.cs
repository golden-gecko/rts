using System.Collections.Generic;
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

    public void SetVisibleByRadar(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((position.x - range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z - range) / Config.TerrainVisibilityScale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((position.x + range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z + range) / Config.TerrainVisibilityScale));

        Vector3Int center = new Vector3Int(Mathf.FloorToInt(position.x / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt(position.z / Config.TerrainVisibilityScale));

        int radius = Mathf.FloorToInt(range / Config.TerrainVisibilityScale);

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (Utils.IsPointInRect(x, z, Config.TerrainVisibilitySize) == false)
                {
                    continue;
                }

                if (Utils.IsPointInCircle(x, z, center, radius) == false)
                {
                    continue;
                }

                SetVisible(Cells[x, z].VisibleByRadar, myGameObject.Player, value);
            }
        }
    }

    public void SetVisibleByAntiRadar(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((position.x - range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z - range) / Config.TerrainVisibilityScale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((position.x + range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z + range) / Config.TerrainVisibilityScale));

        Vector3Int center = new Vector3Int(Mathf.FloorToInt(position.x / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt(position.z / Config.TerrainVisibilityScale));

        int radius = Mathf.FloorToInt(range / Config.TerrainVisibilityScale);

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (Utils.IsPointInRect(x, z, Config.TerrainVisibilitySize) == false)
                {
                    continue;
                }

                if (Utils.IsPointInCircle(x, z, center, radius) == false)
                {
                    continue;
                }

                SetVisible(Cells[x, z].VisibleByAntiRadar, myGameObject.Player, value);
            }
        }
    }

    public void SetVisibleBySight(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((position.x - range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z - range) / Config.TerrainVisibilityScale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((position.x + range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z + range) / Config.TerrainVisibilityScale));

        Vector3Int center = new Vector3Int(Mathf.FloorToInt(position.x / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt(position.z / Config.TerrainVisibilityScale));

        int radius = Mathf.FloorToInt(range / Config.TerrainVisibilityScale);

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (Utils.IsPointInRect(x, z, Config.TerrainVisibilitySize) == false)
                {
                    continue;
                }

                if (Utils.IsPointInCircle(x, z, center, radius) == false)
                {
                    continue;
                }

                SetVisible(Cells[x, z].VisibleBySight, myGameObject.Player, value);
            }
        }
    }

    public void SetVisibleByPower(MyGameObject myGameObject, Vector3 position, float range, int value)
    {
        Vector3Int start = new Vector3Int(Mathf.FloorToInt((position.x - range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z - range) / Config.TerrainVisibilityScale));
        Vector3Int end = new Vector3Int(Mathf.FloorToInt((position.x + range) / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt((position.z + range) / Config.TerrainVisibilityScale));

        Vector3Int center = new Vector3Int(Mathf.FloorToInt(position.x / Config.TerrainVisibilityScale), 0, Mathf.FloorToInt(position.z / Config.TerrainVisibilityScale));

        int radius = Mathf.FloorToInt(range / Config.TerrainVisibilityScale);

        for (int x = start.x; x < end.x; x++)
        {
            for (int z = start.z; z < end.z; z++)
            {
                if (Utils.IsPointInRect(x, z, Config.TerrainVisibilitySize) == false)
                {
                    continue;
                }

                if (Utils.IsPointInCircle(x, z, center, radius) == false)
                {
                    continue;
                }

                SetVisible(Cells[x, z].VisibleByPower, myGameObject.Player, value);
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

    public bool GetPosition(MyGameObject myGameObject, Vector3 origin, out Vector3 position, out MyGameObjectMapLayer mapLayer)
    {
        RaycastHit[] hits = Utils.RaycastAllFromTop(origin, Utils.GetMapMask());

        if (hits.Length <= 0)
        {
            position = Vector3.zero;
            mapLayer = MyGameObjectMapLayer.None;

            return false;
        }

        Vector3 terrainPosition = Vector3.zero;
        Vector3 waterPosition = Vector3.zero;

        foreach (RaycastHit hitInfo in hits)
        {
            if (Utils.IsTerrain(hitInfo))
            {
                terrainPosition = hitInfo.point;
            }
            else if (Utils.IsWater(hitInfo))
            {
                waterPosition = hitInfo.point;
            }
        }

        bool underwater = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Underwater);

        if (terrainPosition.y > waterPosition.y)
        {
            position = terrainPosition;
            mapLayer = MyGameObjectMapLayer.Terrain;

            return true;
        }

        if (underwater)
        {
            position = terrainPosition;
            mapLayer = MyGameObjectMapLayer.Underwater;

            return true;
        }

        position = waterPosition;
        mapLayer = MyGameObjectMapLayer.Water;

        return true;
    }

    public bool GetPositionForCamera(Vector3 origin, out Vector3 position, out MyGameObjectMapLayer mapLayer)
    {
        RaycastHit[] hits = Utils.RaycastAllFromTop(origin, Utils.GetMapMask());

        if (hits.Length <= 0)
        {
            position = Vector3.zero;
            mapLayer = MyGameObjectMapLayer.None;

            return false;
        }

        Vector3 terrainPosition = Vector3.zero;
        Vector3 waterPosition = Vector3.zero;

        foreach (RaycastHit hitInfo in hits)
        {
            if (Utils.IsTerrain(hitInfo))
            {
                terrainPosition = hitInfo.point;
            }
            else if (Utils.IsWater(hitInfo))
            {
                waterPosition = hitInfo.point;
            }
        }

        if (terrainPosition.y > waterPosition.y)
        {
            position = terrainPosition;
            mapLayer = MyGameObjectMapLayer.Terrain;

            return true;
        }

        position = waterPosition;
        mapLayer = MyGameObjectMapLayer.Water;

        return true;
    }

    public bool MouseToPosition(MyGameObject myGameObject, out Vector3 position, out MyGameObjectMapLayer mapLayer)
    {
        RaycastHit[] hits = Utils.RaycastAllFromMouse(Utils.GetMapMask());

        if (hits.Length <= 0)
        {
            position = Vector3.zero;
            mapLayer = MyGameObjectMapLayer.None;

            return false;
        }

        Vector3 terrainPosition = Vector3.zero;
        Vector3 waterPosition = Vector3.zero;

        foreach (RaycastHit hitInfo in hits)
        {
            if (Utils.IsTerrain(hitInfo))
            {
                terrainPosition = hitInfo.point;
            }
            else if (Utils.IsWater(hitInfo))
            {
                waterPosition = hitInfo.point;
            }
        }

        bool underwater = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Underwater);

        if (terrainPosition.y > waterPosition.y)
        {
            position = terrainPosition;
            mapLayer = MyGameObjectMapLayer.Terrain;

            return true;
        }

        if (underwater)
        {
            position = terrainPosition;
            mapLayer = MyGameObjectMapLayer.Underwater;

            return true;
        }

        position = waterPosition;
        mapLayer = MyGameObjectMapLayer.Water;

        return true;
    }

    public bool ValidatePosition(MyGameObject myGameObject, Vector3 position, out Vector3 validated)
    {
        RaycastHit[] hits = Utils.RaycastAllFromTop(position, Utils.GetMapMask());

        if (hits.Length <= 0)
        {
            validated = Vector3.zero;

            return false;
        }

        Vector3 terrainPosition = Vector3.zero;
        Vector3 waterPosition = Vector3.zero;

        foreach (RaycastHit hitInfo in hits)
        {
            if (Utils.IsTerrain(hitInfo))
            {
                terrainPosition = hitInfo.point;
            }
            else if (Utils.IsWater(hitInfo))
            {
                waterPosition = hitInfo.point;
            }
        }

        bool air = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Air);
        bool hover = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Hover);
        bool submerged = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Submerged);
        bool terrain = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Terrain);
        bool underwater = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Underwater);
        bool water = myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Water);

        // Any position in air).
        if (air)
        {
            validated = position;

            return true;
        }

        // Position in air above terrain or water).
        if (hover)
        {
            if (terrainPosition.y > waterPosition.y)
            {
                validated = new Vector3(terrainPosition.x, terrainPosition.y + myGameObject.Altitude, terrainPosition.z);
            }
            else
            {
                validated = new Vector3(waterPosition.x, waterPosition.y + myGameObject.Altitude, waterPosition.z);
            }

            return true;
        }

        // Position under water surface.
        if (submerged)
        {
            if (terrainPosition.y > waterPosition.y)
            {
                validated = Vector3.zero;

                return false;
            }
            else
            {
                validated = new Vector3(waterPosition.x, waterPosition.y - myGameObject.Depth, waterPosition.z);

                return true;
            }
        }

        // Position on terrain or underwater.
        if (terrain && underwater)
        {
            validated = new Vector3(terrainPosition.x, terrainPosition.y, terrainPosition.z);

            return true;
        }

        // Position on terrain or water.
        if (terrain && water)
        {
            if (terrainPosition.y > waterPosition.y)
            {
                validated = new Vector3(terrainPosition.x, terrainPosition.y, terrainPosition.z);

                return true;
            }
            else
            {
                validated = new Vector3(waterPosition.x, waterPosition.y, waterPosition.z);

                return true;
            }
        }

        // Position on terrain.
        if (terrain)
        {
            if (terrainPosition.y > waterPosition.y)
            {
                validated = new Vector3(terrainPosition.x, terrainPosition.y, terrainPosition.z);

                return true;
            }
            else
            {
                validated = Vector3.zero;

                return false;
            }
        }

        // Position on water.
        if (water)
        {
            if (waterPosition.y > terrainPosition.y)
            {
                validated = new Vector3(waterPosition.x, waterPosition.y, waterPosition.z);

                return true;
            }
            else
            {
                validated = Vector3.zero;

                return false;
            }
        }

        validated = Vector3.zero;

        return false;
    }

    private void Clear()
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

    private void SetVisible(Dictionary<Player, int> layer, Player player, int value)
    {
        if (layer.ContainsKey(player) == false)
        {
            layer[player] = value;
        }
        else
        {
            layer[player] += value;
        }
    }

    private Cell[,] Cells = new Cell[Config.TerrainVisibilitySize, Config.TerrainVisibilitySize];
}
