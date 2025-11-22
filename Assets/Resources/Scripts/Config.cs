using System.Collections.Generic;

public class Config
{
    public static float CameraMinHeight = 2.0f;
    public static float CameraMaxHeight = 100.0f;

    public static string DirectoryDisasters  = "Objects/Disasters";
    public static string DirectoryStructures = "Objects/Structures";
    public static string DirectoryUnits      = "Objects/Units";

    public static float DisasterDirection = 200.0f;
    public static float DisasterRange     = 20.0f;

    public static float RaycastMaxDistance = 5000.0f;

    public static List<string> Recipies = new List<string>() { "Metal using coal", "Metal using wood" };
    public static List<string> Skills   = new List<string>() { "Damage", "Repair" };

    public static float TerrainConstructionScale = 2.0f;
    public static float TerrainVisibilityScale   = 5.0f;
    public static int   TerrainVisibilitySize    = 100;
    public static float TerrainMaxHeight         = 2000.0f;

    public static float WaterConstructionScale = 2.0f;
    public static float WaterVisibilityScale   = 5.0f;
    public static int   WaterVisibilitySize    = 100;
    public static float WaterMaxHeight         = 2000.0f;
}
