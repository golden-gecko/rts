using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Handles;

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

        PositionHandlers[PositionHandlerType.Camera] = new PositionAnywhere();
        PositionHandlers[PositionHandlerType.Plane] = new PositionAnywhere();
        PositionHandlers[PositionHandlerType.Ship] = new WaterPositionAnywhere();
        PositionHandlers[PositionHandlerType.Structure] = new TerrainPositionAnywhere();
        PositionHandlers[PositionHandlerType.Vehicle] = new TerrainPositionAnywhere();
    }

    public Dictionary<PositionHandlerType, ITerrainPosition> PositionHandlers = new Dictionary<PositionHandlerType, ITerrainPosition>();

    public ITerrainPosition CameraPositionHandler { get; } = new TerrainPositionAnywhere();

    public ITerrainPosition ShipPositionHandler { get; } = new WaterPositionAnywhere();

    public ITerrainPosition StructurePositionHandler { get; } = new TerrainPositionCenterGrid();

    public ITerrainPosition VehiclePositionHandler { get; } = new TerrainPositionAnywhere();
}
