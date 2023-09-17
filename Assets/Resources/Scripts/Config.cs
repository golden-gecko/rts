using System.Collections.Generic;
using System.IO;

public class Config
{
    public static float CameraMinHeight = 2.0f;
    public static float CameraMaxHeight = 100.0f;

    public static float CursorRotateStep = 45.0f;

    public static string DirectoryDisasters = Path.Join("Objects", "Disasters");
    public static string DirectoryStructures = Path.Join("Objects", "Structures");
    public static string DirectoryUnits = Path.Join("Objects", "Units");

    public static float IndicatorMargin = 1.1f;
    public static float IndicatorTextOffset = 2.0f;

    public static float RaycastMaxDistance = 5000.0f;

    public static bool SnapToGrid = true;

    public static float TerrainConstructionScale = 2.0f;
    public static float TerrainVisibilityScale = 2.0f;
    public static int TerrainVisibilitySize = 250;
    public static float TerrainMaxHeight = 2000.0f;

    public static float WaterConstructionScale = 2.0f;
    public static float WaterVisibilityScale = 2.0f;
    public static int WaterVisibilitySize = 250;
    public static float WaterMaxHeight = 2000.0f;
}
